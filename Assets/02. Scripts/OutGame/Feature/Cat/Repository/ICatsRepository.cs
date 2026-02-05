using Cysharp.Threading.Tasks;

public interface ICatsRepository
{
    public UniTask Save(OwnedCatsSaveData data);

    public UniTask<OwnedCatsSaveData> Load();
}