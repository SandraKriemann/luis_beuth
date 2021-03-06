using Microsoft.AspNetCore.Mvc;
using luis_beuth.Models.Data;
using luis_beuth.Data;
using luis_beuth.Models.ApiStudentModels;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;


namespace luis_beuth.Controllers
{
    [Route("api/student")]
    public class ApiStudentController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<ApiStudentController> _logger;


        public ApiStudentController (ApplicationDbContext context, ILogger<ApiStudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 
        // GET: /Student/
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _context.Student.ToList();
        }

        // 
        // GET: /Student/{Id}/ 
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var found = _context.Student.FirstOrDefault(p => p.Id == id);
            if (found == null)
            {
                return NotFound();
            }

            return new ObjectResult(found);
        }

       
        // POST /api/student
        [HttpPost]
        public IActionResult Post([FromBody]Student newStudent)
        {
            if (string.IsNullOrWhiteSpace(newStudent.Name) || newStudent.MatriculationNumber <= 0 )
            {
                return BadRequest();
            }
            _context.Student.Add(newStudent);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT api/student/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Student student)
        {
            if (student == null /*|| student.id != id*/) // optional: Prüfung ob id des Studenten der übermittelt wird gleich der id in der Route ist
            {
                return BadRequest();
            }
            
            var studentToUpdate = _context.Student.FirstOrDefault(p => p.Id == id);
            if (studentToUpdate == null)
            {
                return NotFound();
            }
            
            // studentToUpdate.Id is created by database
            studentToUpdate.Name = student.Name;
            studentToUpdate.MatriculationNumber = student.MatriculationNumber;
            studentToUpdate.Approved = false; // only via Web Interface

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/student/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var studentToDelete = _context.Student.FirstOrDefault(p => p.Id == id);
            if (studentToDelete == null)
            {
                return NotFound();
            }
            _context.Student.Remove(studentToDelete);
            _context.SaveChanges();

            return NoContent();
        }
    }
}