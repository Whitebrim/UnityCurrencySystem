using System;
using System.Collections.Generic;
using UnityEngine;

namespace CurrencySystem
{
    public class Wallet
    {
        public IWalletSaver _saveLoad;

        private Dictionary<string, CurrencyAccount> _wallet = new Dictionary<string, CurrencyAccount>();

        public Wallet(IWalletSaver saveLoad, Dictionary<string, CurrencyAccount> wallet = null)
        {
            _saveLoad = saveLoad ?? throw new ArgumentNullException(nameof(saveLoad));
            if (wallet == null) Load();
            else _wallet = wallet;
        }

        public CurrencyAccount GetAccount(string currencyCode)
        {
            if (_wallet.TryGetValue(currencyCode, out var account))
            {
                return account;
            }
            return null;
        }

        public bool AddAccount(CurrencyAccount newAccount)
        {
            if (_wallet.ContainsKey(newAccount.CurrencyCode))
            {
                Debug.LogError($"Currency account with code {newAccount.CurrencyCode} already exist.");
                return false;
            }
            _wallet.Add(newAccount.CurrencyCode, newAccount);
            return true;
        }

        public bool Load()
        {
            if (_saveLoad.Load(out var newWallet))
            {
                _wallet = newWallet;
                return true;
            }
            return false;
        }

        public bool Save()
        {
            return _saveLoad.Save(_wallet);
        }
    }
}
