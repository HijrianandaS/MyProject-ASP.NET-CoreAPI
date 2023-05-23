using MyProject.Models;

namespace MyProject.Repository.Interface
{
    public interface IAccountRepository
    {
        IEnumerable<Account> Get();
        Account Get(string Email, string password);
        int Insert(Account account);
        //int Update(Account account);
        //int Delete(string NIK, string password);
    }
}
