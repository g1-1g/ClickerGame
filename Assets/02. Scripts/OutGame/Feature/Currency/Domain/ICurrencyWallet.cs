public interface ICurrencyWallet
{
    bool CanAfford(ECurrencyType type, Currency amount);
    bool TrySpend(ECurrencyType type, Currency amount);
}
