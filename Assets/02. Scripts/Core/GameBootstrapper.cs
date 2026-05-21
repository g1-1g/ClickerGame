using Cysharp.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private CatManager _catManager;

    private async void Start()
    {
        if (!TryResolveGameManagers())
        {
            Debug.LogError("[GameBootstrapper] Game managers are missing in this scene.");
            return;
        }

        if (AccountManager.Instance == null || string.IsNullOrEmpty(AccountManager.Instance.Email))
        {
            Debug.LogError("[GameBootstrapper] AccountManager is not ready.");
            return;
        }

        string userId = AccountManager.Instance.Email;
        await _currencyManager.Initialize(new FirebaseCurrencyRepository(userId));
        await _itemManager.Initialize(new FirebaseItemLevelRepository(userId));
        await _catManager.Initialize(new FirebaseCatsRepository(userId));
    }

    private bool TryResolveGameManagers()
    {
        if (_currencyManager == null)
        {
            _currencyManager = CurrencyManager.Instance;
        }

        if (_itemManager == null)
        {
            _itemManager = ItemManager.Instance;
        }

        if (_catManager == null)
        {
            _catManager = CatManager.Instance;
        }

        return _currencyManager != null &&
               _itemManager != null &&
               _catManager != null;
    }
}
