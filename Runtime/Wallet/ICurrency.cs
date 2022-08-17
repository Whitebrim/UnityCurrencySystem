namespace CurrencySystem
{
    public interface ICurrency
    {
        delegate void CurrencyEvent(double amount);
        event CurrencyEvent OnNewAmount;
        event CurrencyEvent OnChange;
        public string CurrencyCode { get; }
        public double Get();
        public void Add(double amount);
        public void Subtract(double amount);
        public bool TrySubtract(double amount);
        public void Clear();
    }
}
