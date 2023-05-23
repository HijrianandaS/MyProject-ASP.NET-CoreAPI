using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;
using MyProject.ViewModels;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository accountRepository;
        private readonly EmployeeRepository employeeRepository;
        private readonly IConfiguration _configuration;
        public static Employee employee = new Employee();
        private string NIK;
        private string Password;
        private string Email;

        public AccountsController(AccountRepository accountRepository, EmployeeRepository employeeRepository, IConfiguration configuration)
        {
            this.accountRepository = accountRepository;
            this.employeeRepository = employeeRepository;
            this._configuration = configuration;
        }


        /*[HttpPost("Register")]
        public ActionResult Insert(Account account)
        {
            var myAccount = accountRepository.Insert(account);
            if (myAccount != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Anda Berhasil Mendaftar, Silahkan Login", Data = account });
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Yang Anda Masukkan Tidak Sesuai!", Data = account });
        }*/


        //[HttpGet("Login")]
        //public ActionResult Get(string NIK, string password)
        //{
        //    var get = accountRepository.Get(NIK, password);
        //    if (get != null)
        //    {
        //        return StatusCode(200, new { HttpStatusCode.OK, message = "Anda Berhasil Login", Data = get });
        //    }
        //    else
        //    {
        //        return StatusCode(404, new { HttpStatusCode.NotFound, message = "Account Anda Tidak Ditemukan", Data = get });
        //    }
        //}

        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (accountRepository.CheckIfPhoneOrEmailExist(registerVM.Phone, registerVM.Email))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Phone or Email already exists");
            }
            if (registerVM.Email.Contains(" ") || registerVM.Password.Contains(" "))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Email or Password cannot contain spaces");
            }
            var get = accountRepository.Register(registerVM);
            if (get != 0)
            {
                return StatusCode(200, new { HttpStatusCode.OK, message = "Anda Berhasil Register, Silahkan Login", Data = get });
            }
            else
            {
                return StatusCode(404, new { HttpStatusCode.NotFound, message = "Gagal Register", Data = get });
            }
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginVM loginVM)
        {
            if (string.IsNullOrWhiteSpace(loginVM.Email) || string.IsNullOrWhiteSpace(loginVM.Password))
            {
                return StatusCode(400, new { HttpStatusCode.BadRequest, message = "Email dan password harus diisi." });
            }
            else if (loginVM.Email.Contains(" "))
            {
                return StatusCode(400, new { HttpStatusCode.BadRequest, message = "Email tidak boleh mengandung spasi." });
            }
            else
            {
                var get = accountRepository.Login(loginVM);
                if (get != null)
                {
                    var token = IMakeToken(get.Employee);
                    return StatusCode(200, new { HttpStatusCode.OK, message = "Data Account Ditemukan", Data = get, Token = token });
                }
                else
                {
                    return StatusCode(404, new { HttpStatusCode.NotFound, message = "Data Account Tidak Ditemukan", Data = get });
                }
            }
        }

        private string IMakeToken(Employee employee)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("FirstName", employee.FirstName),
                new Claim("NIK", employee.NIK),
                new Claim("Phone", employee.Phone),
                new Claim("Email", employee.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        /*[HttpPost("Login")]
        public ActionResult Login(string Email, string Password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                return StatusCode(400, new { HttpStatusCode.BadRequest, message = "Email dan password harus diisi." });
            }
            else if (Email.Contains(" "))
            {
                return StatusCode(400, new { HttpStatusCode.BadRequest, message = "Email tidak boleh mengandung spasi." });
            }
            else
            {
                var get = accountRepository.Login(Email, Password);
                if (get != null)
                {
                    return StatusCode(200, new { HttpStatusCode.OK, message = "Data Account Ditemukan", Data = get });
                }
                else
                {
                    return StatusCode(404, new { HttpStatusCode.NotFound, message = "Data Account Tidak Ditemukan", Data = get });
                }
            }
        }*/



        [HttpGet]
        public ActionResult Get()
        {
            var get = accountRepository.Get();
            if (get != null)
            {
                return StatusCode(200, new { HttpStatusCode.OK, message = "Data Account Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { HttpStatusCode.NotFound, message = "Data Account Tidak Ditemukan", Data = get });
            }
        }


        /*[HttpPut]
        public ActionResult Update(Account account)
        {

            var existingAccount = accountRepository.Get(account.NIK);
            if (existingAccount != null)
            {
                existingAccount.Password = existingAccount.Password;

                accountRepository.Update(existingAccount);
                return Ok();
            }
            return NotFound();
        }*/


        /*[HttpDelete("{DeleteAccount}")]
        public ActionResult Delete(string NIK, string password)
        {
            var getAccount = accountRepository.Get(NIK, password);
            if (getAccount != null)
            {
                accountRepository.Delete(NIK, password);
                return Ok("Data berhasil dihapus");
            }
            return NotFound();
        }*/

    }
}
