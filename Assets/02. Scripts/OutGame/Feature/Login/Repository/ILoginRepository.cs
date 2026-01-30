
public interface ILoginRepository 
{
    public void Save(CurrencySaveData saveData);

    public string Load();
}