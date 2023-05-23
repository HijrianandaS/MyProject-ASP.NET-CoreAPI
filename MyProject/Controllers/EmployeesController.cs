using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;
using MyProject.ViewModels;
using System.Linq;
using System.Net;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository employeeRepository;
        private string NIK;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (employeeRepository.CheckIfPhoneOrEmailExist(registerVM.Phone, registerVM.Email))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Phone or Email already exists");
            }
            if (registerVM.Email.Contains(" ") || registerVM.Password.Contains(" "))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Email or password cannot contain spaces");
            }
            var get = employeeRepository.Register(registerVM);
            if (get != 0)
            {
                return StatusCode(200, new { HttpStatusCode.OK, message = "Anda Berhasil Register, Silahkan Login", Data = get });
            }
            else
            {
                return StatusCode(404, new { HttpStatusCode.NotFound, message = "Gagal Register", Data = get });
            }
        }

        [HttpPost]
        public ActionResult Insert(Employee employee)
        {
            try
            {
                if (employeeRepository.CheckIfPhoneOrEmailExist(employee.Phone, employee.Email))
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Phone or Email already exists");
                }

                employeeRepository.Insert(employee);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Input", Data = employee });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, ex.Message);
            }

        }



        /*[HttpGet]
        public ActionResult Get()
        {
            try
            {
                var get = employeeRepository.Get();
                return Ok(get);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/

        // Controller get mengambil data dari 2 table

        [HttpGet]
        public ActionResult Get()
        {

            var get = employeeRepository.Get();
            if (get != null)
            {
                return StatusCode(200, new { HttpStatusCode.OK, message = "Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", Data = get });
            }
        }


        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var get = employeeRepository.Get(NIK);
            if (get != null)
            {
                return StatusCode(200, new { HttpStatusCode.OK, message = "Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", Data = get });
            }
        }


        /*[HttpPut("{NIK}")]
        public ActionResult Update(Employee employee)
        {
            var get = employeeRepository.Get(NIK);
            employeeRepository.Update(employee);
            return Ok("Data Berhasil di Update");
        }*/
        //[HttpPut("{NIK}")]

        [HttpPut]
        public ActionResult Update(Employee employee)
        {

            var existingEmployee = employeeRepository.Get(employee.NIK);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.BirthDate = employee.BirthDate;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.Email = employee.Email;
                existingEmployee.Gender = employee.Gender;
                employeeRepository.Update(existingEmployee);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Update", Data = employee });
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak di temukan", Data = employee });
        }


        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            var getEmp = employeeRepository.Get(NIK);
            if (getEmp != null)
            {
                employeeRepository.Delete(NIK);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Hapus"});
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak di temukan"});
        }

        [HttpGet("CombinatedEmp&Dept")]
        public ActionResult Combinated()
        {
            var get = employeeRepository.Combinated();
            if(get == null)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data tidak di temukan" });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Di Tampilkan", data = get });
        }

        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil");
        }
        
    }
}
