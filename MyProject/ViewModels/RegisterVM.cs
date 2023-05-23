using MyProject.Models;

namespace MyProject.ViewModels
{
    public class RegisterVM  //menggunakan atribut yg hanya ingin digunakan dr tbl model
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public int Department_Id { get; set; }
        public string Password { get; set; }
    }
}
