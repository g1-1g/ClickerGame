
using Cysharp.Threading.Tasks;

public interface IAccountRepository 
{
    UniTask<bool> IsEmailAvailable(string email);
    UniTask<AccountResult> Register(string email, string password);
    UniTask<AccountResult> Login(string email, string password);
    void Logout();
}