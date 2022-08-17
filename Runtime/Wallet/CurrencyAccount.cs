using System;

namespace CurrencySystem
{
    [Serializable]
    public sealed class CurrencyAccount : ICurrency
    {
        private readonly string _currencyCode;
        private float _multiplier = 1;
        private IVault _vault;

        /// <summary>
        /// Передает общее кол-во денег на счету
        /// </summary>
        [field: NonSerialized]

        public event ICurrency.CurrencyEvent OnNewAmount;
        /// <summary>
        /// Передает сумму, на которую счет изменился
        /// </summary>
        [field: NonSerialized]
        public event ICurrency.CurrencyEvent OnChange;

        private double Amount
        {
            get { return _vault.Amount; }
            set
            {
                OnChange?.Invoke(value - _vault.Amount);
                _vault.Amount = value;
                OnNewAmount?.Invoke(value);
            }
        }

        public string CurrencyCode => _currencyCode;

        /// <param name="currencyCode">Currency name used for accesing currency in the wallet and en/decryption currency</param>
        /// <param name="amount">Initial money amount</param>
        /// <param name="multiplier">Booster for income. By default it's 1.0f</param>
        public CurrencyAccount(string currencyCode, double amount, float multiplier = 1)
        {
            _currencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
            _multiplier = multiplier;
            _vault = new Vault(amount, currencyCode);
        }

        public void Add(double amount)
        {
            Amount += amount * _multiplier;
        }

        public void Clear()
        {
            Amount = 0;
        }

        public double Get()
        {
            return Amount;
        }

        public void Subtract(double amount)
        {
            Amount -= amount;
        }

        public bool TrySubtract(double amount)
        {
            if (Amount - amount >= 0)
            {
                Amount -= amount;
                return true;
            }
            return false;
        }

        public static CurrencyAccount operator +(CurrencyAccount account, double income)
        {
            account.Add(income);
            return account;
        }

        public static CurrencyAccount operator +(CurrencyAccount account, CurrencyAccount account2)
        {
            account.Add(account2.Get());
            return account;
        }

        public static CurrencyAccount operator ++(CurrencyAccount account)
        {
            account.Add(1);
            return account;
        }

        public static CurrencyAccount operator -(CurrencyAccount account, double toll)
        {
            account.Subtract(toll);
            return account;
        }

        public static CurrencyAccount operator -(CurrencyAccount account, CurrencyAccount account2)
        {
            account.Subtract(account2.Get());
            return account;
        }

        public static CurrencyAccount operator --(CurrencyAccount account)
        {
            account.Subtract(1);
            return account;
        }

        public static implicit operator double(CurrencyAccount account) => account.Amount;

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
