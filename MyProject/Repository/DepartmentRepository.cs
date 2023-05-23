using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;

namespace MyProject.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext myContext;
        public DepartmentRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public IEnumerable<Department> Get()
        {
            return myContext.Departments.ToList();
        }
        public Department Get(int Id)
        {
            return myContext.Departments.Find(Id);
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
        public int Insert(Department department)
        {
            myContext.Departments.Add(department);
            var save = myContext.SaveChanges();
            return save;
            
        }

        public int Update(Department department)
        {
            myContext.Entry(department).State = EntityState.Modified;
            return myContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var dataDepart = myContext.Departments.Find(Id);
            if (dataDepart != null)
            {
                myContext.Departments.Remove(dataDepart);
                var save = myContext.SaveChanges();
                return save;
            }
            return 0;
        }

    }
}
