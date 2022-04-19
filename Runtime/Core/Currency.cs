namespace CurrencySystem
{
    public class Currency : ICurrency
    {
        private readonly string _currencyCode;
        private double _amount;

        public event ICurrency.CurrencyEvent OnCurrencyChange;

        private double Amount
        {
            get { return _amount; }  // TODO decrypt
            set { _amount = value; } // TODO encrypt
        }

        public string CurrencyCode => _currencyCode;

        private void NotifyEvent()
        {
            OnCurrencyChange?.Invoke(Amount);
        }

        public void Add(double amount)
        {
            Amount += amount;
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

        public static Currency operator+ (Currency vault, double income)
        {
            vault.Add(income);
            return vault;
        }

        public static Currency operator- (Currency vault, double toll)
        {
            vault.Subtract(toll);
            return vault;
        }
    }
}
