using CurrencySystem;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIMediator : MonoBehaviour
{
    [SerializeField] Text _text;
    private Wallet _wallet;

    public void AssignWallet(Wallet wallet)
    {
        _wallet = wallet;

        if (_wallet.GetAccount("RUB") is null)
        {
            Debug.Log("Creating new currency");
            _wallet.AddAccount(new CurrencyAccount("RUB", 1000));
            _wallet.Save();
        }

        _wallet.GetAccount("RUB").OnCurrencyChange += UpdateUI;
        UpdateUI(_wallet.GetAccount("RUB").Get());
    }

    private void UpdateUI(double text)
    {
        _text.text = text.ToString();
    }

    public void Add9()
    {
        _wallet.GetAccount("RUB").Add(9);
    }

    public void Subtract4()
    {
        _wallet.GetAccount("RUB").Subtract(4);
    }

    public void Double()
    {
        var account = _wallet.GetAccount("RUB");
        account = account + account;
    }

    public void Clear()
    {
        _wallet.GetAccount("RUB").Clear();
    }

    public void SwitchToPlayerPrefs()
    {
        AssignWallet(new Wallet(new PlayerPrefsSaver()));
    }

    public void SwitchToFileSystem()
    {
        AssignWallet(new Wallet(new FileSaver()));
    }

    public void Save()
    {
        _wallet.Save();
    }

    public void Load()
    {
        if (_wallet.Load()) AssignWallet(_wallet);
        else Debug.LogError("No save");
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsSaver.PLAYER_PREFS_WALLET_KEY);
        File.Delete(Path.Combine(Application.persistentDataPath, FileSaver.FILE_NAME));
    }
}
