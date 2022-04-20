using System;
using System.Collections.Generic;
using UnityEngine;

namespace CurrencySystem
{
    public class PlayerPrefsSaver : IWalletSaver
    {
        public const string PLAYER_PREFS_WALLET_KEY = "wallet";

        public bool Load(out Dictionary<string, CurrencyAccount> wallet)
        {
            wallet = new Dictionary<string, CurrencyAccount>();
            var data = PlayerPrefs.GetString(PLAYER_PREFS_WALLET_KEY, "");
            if (string.IsNullOrEmpty(data)) return false;
            wallet = Serializer.Deserialize<Dictionary<string, CurrencyAccount>>(Convert.FromBase64String(data));
            return true;
        }

        public bool Save(Dictionary<string, CurrencyAccount> wallet)
        {
            PlayerPrefs.SetString(PLAYER_PREFS_WALLET_KEY, Convert.ToBase64String(Serializer.Serialize(wallet)));
            return true;
        }
    }
}
