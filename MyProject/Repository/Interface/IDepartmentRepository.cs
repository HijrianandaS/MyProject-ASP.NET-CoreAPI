using MyProject.Models;

namespace MyProject.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Department Get(int Id);
        int Insert(Department department);
        int Update(Department department);
        int Delete(int ID);
    }
}
