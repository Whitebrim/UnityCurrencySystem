using System;
using System.Security.Cryptography;

namespace CurrencySystem
{
    [Serializable]
    public class CurrencyAccount : ICurrency
    {
        private readonly string _currencyCode;
        private float _multiplier = 1;
        private double _amount;

        [field: NonSerialized]
        public event ICurrency.CurrencyEvent OnCurrencyChange;

        private double Amount
        {
            get { return _amount; }  // TODO decrypt
            set { _amount = value; } // TODO encrypt
        }

        public string CurrencyCode => _currencyCode;

        /// <param name="currencyCode">Currency name used for accesing currency in the wallet and en/decryption currency</param>
        /// <param name="amount">Initial money amount</param>
        /// <param name="multiplier">Booster for income. By default it's 1.0f</param>
        public CurrencyAccount(string currencyCode, double amount, float multiplier = 1)
        {
            _currencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
            _amount = amount;
            _multiplier = multiplier;
        }

        private void NotifyEvent()
        {
            Aes.Create();
            OnCurrencyChange?.Invoke(Amount);
        }

        public void Add(double amount)
        {
            Amount += amount * _multiplier;
            NotifyEvent();
        }

        public void Clear()
        {
            Amount = 0;
            NotifyEvent();
        }

        public double Get()
        {
            return Amount;
        }

        public void Subtract(double amount)
        {
            Amount -= amount;
            NotifyEvent();
        }

        public bool TrySubtract(double amount)
        {
            if (Amount - amount >= 0)
            {
                Amount -= amount;
                NotifyEvent();
                return true;
            }
            return false;
        }

        public static CurrencyAccount operator +(CurrencyAccount vault, double income)
        {
            vault.Add(income);
            return vault;
        }

        public static CurrencyAccount operator +(CurrencyAccount vault, CurrencyAccount vault2)
        {
            vault.Add(vault2.Get());
            return vault;
        }

        public static CurrencyAccount operator ++(CurrencyAccount vault)
        {
            vault.Add(1);
            return vault;
        }

        public static CurrencyAccount operator -(CurrencyAccount vault, double toll)
        {
            vault.Subtract(toll);
            return vault;
        }

        public static CurrencyAccount operator -(CurrencyAccount vault, CurrencyAccount vault2)
        {
            vault.Subtract(vault2.Get());
            return vault;
        }

        public static CurrencyAccount operator --(CurrencyAccount vault)
        {
            vault.Subtract(1);
            return vault;
        }

        public static implicit operator double(CurrencyAccount vault) => vault.Amount;

        public override bool Equals(object obj)
        {
            return Amount.Equals(obj);
        }

        public override string ToString()
        {
            return Amount.ToString();
        }
    }
}
