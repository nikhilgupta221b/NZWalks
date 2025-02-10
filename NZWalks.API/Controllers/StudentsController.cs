using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult getStudents()
        {
            string[] studentsName = new string[] { "John", "Jane" };

            return Ok(studentsName); 
        }
    }
}
