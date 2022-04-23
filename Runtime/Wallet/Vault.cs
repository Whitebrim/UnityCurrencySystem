using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace CurrencySystem
{
    [Serializable]
    public class Vault : IVault
    {
        private byte[] _amount;
        private readonly string _currencyCode;
        [NonSerialized]
        private byte[] _key;

        public Vault(double amount, string currencyCode)
        {
            _currencyCode = currencyCode;
            InitKeys();
            Amount = amount;
        }

        public double Amount
        {
            get
            {
                return Decrypt(_amount, _key);
            }
            set
            {
                _amount = Encrypt(value, _key);
            }
        }

        internal void InitKeys()
        {
            _key = GenerateValidKeys(_currencyCode);
        }

        private byte[] GenerateValidKeys(string seed)
        {
            byte[] result = new byte[0];
            do
            {
                result = result.Concat(Encoding.ASCII.GetBytes(seed)).ToArray();
            } while (result.Length < 16);
            Array.Resize(ref result, 16);
            return result;
        }

        private double Decrypt(byte[] encryptedData, byte[] key)
        {
            byte[] bytesIv = new byte[16];
            Array.Copy(encryptedData, 0, bytesIv, 0, 16);
            byte[] data = new byte[encryptedData.Length - 16];
            Array.Copy(encryptedData, 16, data, 0, data.Length);

            double result;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = bytesIv;

                ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                    {
                        using (BinaryReader sr = new BinaryReader(cs))
                        {
                            result = sr.ReadDouble();
                        }
                    }
                }
            }            
            return result;
        }

        private byte[] Encrypt(double data, byte[] key)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter sw = new BinaryWriter(cs))
                        {
                            sw.Write(data);
                            //sw.Write(BitConverter.GetBytes(data));
                        }
                    }
                    encrypted = ms.ToArray();
                }
                encrypted = aes.IV.Concat(encrypted).ToArray();
            }
            return encrypted;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitKeys();
        }
    }
}
