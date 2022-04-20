using System.Collections.Generic;

namespace CurrencySystem
{
    public interface IWalletSaver
    {
        bool Save(Dictionary<string, CurrencyAccount> wallet);
        bool Load(out Dictionary<string, CurrencyAccount> wallet);
    }
}
