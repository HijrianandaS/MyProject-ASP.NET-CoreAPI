using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;
using MyProject.ViewModels;
using BCrypt.Net;

namespace MyProject.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        /*public IEnumerable<Employee> Get()  
        {
            return myContext.Employees.ToList();
        }*/
        public IEnumerable<Employee> Get()
        {
            return myContext.Employees.ToList();
        }


        public Employee Get(string NIK)
        {
            /*return myContext.Employees.Find(NIK)*/;
            //return myContext.Employees.FirstOrDefault(e => e.NIK == NIK);
            //return myContext.Employees.Where(e => e.NIK == NIK).FirstOrDefault();
            return myContext.Employees.Where(e => e.NIK == NIK).SingleOrDefault();
        }


        public int Insert(Employee employee)
        {
            //employee.NIK = GenerateId();
            string generatedNIK = DateTime.Today.ToString("ddMMyyyy") + (myContext.Employees.Count() + 1).ToString("D3");
            employee.NIK = generatedNIK;

            myContext.Entry(employee).State = EntityState.Added;
            var save = myContext.SaveChanges();
            return save;
        }
        public bool CheckIfPhoneOrEmailExist(string phone, string email)
        {
            return myContext.Employees.Any(e => e.Phone == phone || e.Email == email);
        }


        /*public int Insert(Employee employee)
        {
            if (IsPhoneExists(employee.Phone) || IsEmailExists(employee.Email))
            {
                throw new Exception("Phone or email already exists.");
            }

            string lastId = myContext.Employees
                            .OrderByDescending(e => e.NIK)
                            .Select(e => e.NIK)
                            .FirstOrDefault();

            string newId = GenerateNewId(lastId);

            employee.NIK = newId;

            myContext.Employees.Add(employee);
            var save = myContext.SaveChanges();
            return save;
        }

        private bool IsPhoneExists(string phone)
        {
            return myContext.Employees.Any(e => e.Phone == phone);
        }

        private bool IsEmailExists(string email)
        {
            return myContext.Employees.Any(e => e.Email == email);
        }


        private string GenerateNewId(string lastId)
        {
            if (string.IsNullOrEmpty(lastId))
            {
                return $"{DateTime.Now:ddMMyyyy}001";
            }

            int lastNumber = int.Parse(lastId.Substring(8));
            if (DateTime.Now.ToString("ddMMyyyy") == lastId.Substring(0, 8))
            {
                return $"{DateTime.Now:ddMMyyyy}{(lastNumber + 1).ToString("000")}";
            }

            return $"{DateTime.Now:ddMMyyyy}001";
        }*/




        /*public int Update(Employee employee)
        {
            myContext.Employees.Update(employee);
            var save = myContext.SaveChanges();
            return save;
        }*/

        public int Update(Employee employee)
        {
            myContext.Entry(employee).State = EntityState.Modified;
            return myContext.SaveChanges();
        }

        public int Delete(string NIK)
        {
            var dataEmployee = myContext.Employees.Find(NIK);
            if (dataEmployee != null)
            {
                myContext.Employees.Remove(dataEmployee);
                var save = myContext.SaveChanges();
                return save;
            }
            return 0;
        }

        public IEnumerable<object> Combinated()
        {
            var result = myContext.Employees.Include(e => e.Department)
                .Select(e => new
                {
                    EmployeeID = e.NIK,
                    FullName = e.FirstName + " " + e.LastName,
                    DepartmentName = e.Department.Name
                }).ToList();
            return result;  
        }

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
    }
}
