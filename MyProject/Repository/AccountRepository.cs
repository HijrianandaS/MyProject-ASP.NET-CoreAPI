using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;
using MyProject.ViewModels; 
using System.Net;

namespace MyProject.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext myContext;
        public AccountRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public IEnumerable<Account> Get()
        {
            return myContext.Accounts.ToList();
        }
        public Account Get(string NIK, string password)
        {
            return myContext.Accounts.Find(NIK, password);
            //return myContext.Accounts.Find(password);
        }

        public Account Login(LoginVM loginVM)
        {
            Account cekAccount = myContext.Accounts.SingleOrDefault(a => a.Employee.Email == loginVM.Email);
            var pass = BCrypt.Net.BCrypt.Verify(loginVM.Password, cekAccount.Password);

            var result = (from a in myContext.Accounts
                          join b in myContext.Employees
                          on a.NIK equals b.NIK
                          where (loginVM.Email == b.Email && pass == true)
                          select new Account
                          {
                              NIK = a.NIK,
                              Password = a.Password,
                              Employee = a.Employee
                          });

            if (result.Count() != 0)
            {
                return result.First();
            }

            return null;
        }

        /*public Account Login(string Email, string Password)
        {
            Account cekAccount = myContext.Accounts.SingleOrDefault(a => a.Employee.Email==Email);
            var pass = BCrypt.Net.BCrypt.Verify(Password, cekAccount.Password);

                var result = (from a in myContext.Accounts
                          join b in myContext.Employees
                          on a.NIK equals b.NIK 
                          where (Email==b.Email && pass==true)
                          select new Account
                            {
                                NIK = a.NIK,
                                Password = a.Password,
                                Employee = a.Employee
                            });

            if (result.Count() != 0)
            {
                return result.First();
            }

            return null;
        }*/

        public int Register(RegisterVM registerVM)
        {
            var NIK = DateTime.Today.ToString("ddMMyyyy") + (myContext.Employees.Count() + 1).ToString("D3");
            Employee employee = new Employee
            {
                NIK = NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Gender = (Gender)registerVM.Gender,
                Phone = registerVM.Phone,
                Salary = registerVM.Salary,
                BirthDate = registerVM.BirthDate,
                Email = registerVM.Email,
                Department = new Department { Id = registerVM.Department_Id }
            };
            myContext.Entry(employee).State = EntityState.Added;
            Account account = new Account
            {
                NIK = employee.NIK,
                Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password)
            };
            myContext.Accounts.Add(account);
            return myContext.SaveChanges();
        }
        public bool CheckIfPhoneOrEmailExist(string phone, string email)
        {
            return myContext.Employees.Any(e => e.Phone == phone || e.Email == email);
        }

        /*public int Insert(Department department)
        {
            var cekDepart = myContext.Departments.Where(d => d.Name == department.Name).FirstOrDefault();
            if (cekDepart != null)
            {
                return 0;
            }
            else
            {
                var lastDepart = myContext.Departments.OrderByDescending(d => d.Id).FirstOrDefault();
                int newID = 1;
                if (lastDepart != null)
                {
                    newID = lastDepart.Id + 1;
                }
                department.Id = newID;
                myContext.Departments.Add(department);
                var save = myContext.SaveChanges();
                return save;
            }
        }*/
        public int Insert(Account account)
        {
            myContext.Entry(account).State = EntityState.Added;
            var save = myContext.SaveChanges();
            return save;

        }

        /*public int Update(Account account)
        {
            myContext.Entry(account).State = EntityState.Modified;
            return myContext.SaveChanges();
        }*/

        /*public int Delete(string NIK, string password)
        {
            var dataAccount = myContext.Accounts.Find(NIK, password);
            if (dataAccount != null)
            {
                myContext.Accounts.Remove(dataAccount);
                var save = myContext.SaveChanges();
                return save;
            }
            return 0;
        }*/

    }
}
