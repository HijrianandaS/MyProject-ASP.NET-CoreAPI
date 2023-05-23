using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;
using System.Linq;
using System.Net;

namespace MyProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentRepository departmentRepository;
        private int Id;

        public DepartmentsController(DepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        [HttpPost]
        public ActionResult Insert(Department department)
        {
            var myDepart = departmentRepository.Insert(department);
            if (myDepart != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Input", Data = department });
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data gagal Berhasil di Input", Data = department });
        }
        /*
                [HttpPost]
                public ActionResult Insert(Department department)
                {
                    if (departmentRepository.IsDepIdExists(department.Id))
                    {
                        return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Data Already Exists in Database", Data = department });
                    }

                    departmentRepository.Insert(department);
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Tambahkan ke Database", Data = department });
                }*/


        [HttpGet]
        public ActionResult Get()
        {
            var get = departmentRepository.Get();
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", Data = get });
            }
        }

        [HttpGet("{Id}")]
        public ActionResult Get(int Id)
        {
            var get = departmentRepository.Get(Id);
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", Data = get });
            }
        }

        [HttpPut]
        public ActionResult Update(Department department)
        {

            var existingDepart = departmentRepository.Get(department.Id);
            if (existingDepart != null)
            {
                existingDepart.Name = department.Name;

                departmentRepository.Update(existingDepart);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Update", Data = existingDepart });
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Gagal di Update", Data = existingDepart });
        }


        [HttpDelete("{Id}")]
        public ActionResult Delete(int Id)
        {
            var getDepart = departmentRepository.Get(Id);
            if (getDepart != null)
            {
                departmentRepository.Delete(Id);
                return StatusCode(200, new {status = HttpStatusCode.OK, message = "Data Berhasil di Hapus"});
            }
            return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Gagal di Hapus" });
        }

    }
}
