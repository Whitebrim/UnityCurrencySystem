using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CurrencySystem
{
    public class FileSaver : IWalletSaver
    {
        public const string FILE_NAME = "wallet.dat";

        public bool Load(out Dictionary<string, CurrencyAccount> wallet)
        {
            wallet = new Dictionary<string, CurrencyAccount>();
            if (!File.Exists(Path.Combine(Application.persistentDataPath, FILE_NAME))) return false;
            var data = File.ReadAllText(Path.Combine(Application.persistentDataPath, FILE_NAME));
            if (string.IsNullOrEmpty(data)) return false;
            wallet = Serializer.Deserialize<Dictionary<string, CurrencyAccount>>(Convert.FromBase64String(data));
            return true;
        }

        public bool Save(Dictionary<string, CurrencyAccount> wallet)
        {
            File.WriteAllText(Path.Combine(Application.persistentDataPath, FILE_NAME), Convert.ToBase64String(Serializer.Serialize(wallet)));
            return true;
        }
    }
}
