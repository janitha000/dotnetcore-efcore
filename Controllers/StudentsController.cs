using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcore.Contexts;
using efcore.Dtos;
using efcore.Entities;
using efcore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace efcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentRepository _repository;
        public StudentsController(SchoolContext context, IStudentRepository repo, ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
            _repository = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
            [FromQuery(Name ="sort_by")] string sortParam, 
            [FromQuery(Name ="filterBy")] string filterParam,
            [FromQuery(Name ="pageNumber")] int? pageNumber,
            [FromQuery(Name ="pageSize")] int? pageSize
            )
        {
            var students = await _repository.Get(sortParam, filterParam, pageSize, pageNumber);
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.Where(student => student.ID == id).SingleOrDefaultAsync();
            if(student is null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<Student>> GetStudentDetailsByIdAsync(int id)
        {
            var student = await _context.Students.Include(s => s.Enrollments)
                                        .ThenInclude(e => e.Course)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.ID == id);

            if(student is null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudentAsync(StudentDto studentDto)
        {
            Student student = new ()
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                EnrollmentDate = studentDto.EnrollmentDate
            };

            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetStudentByIdAsync), new {Id = student.ID}, student);
                }
                return NoContent();
                
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"Error when adding student to the database. ex: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, StudentDto studentDto)
        {
            // var studentToUpdate = _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            // if(studentToUpdate is null)
            // {
            //     return NotFound();
            // }

            Student studentToUpdate = new() 
            {
                ID = id,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                EnrollmentDate = studentDto.EnrollmentDate
            };

            _context.Entry(studentToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"Error when updating student to the database. ex: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            if(student is null)
            {
                return NotFound();
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"Error when updating student to the database. ex: ${ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("enrollments")]
        public async Task<ActionResult<IEnumerable<EnrollmentDateGroup>>> GetEnrollments()
        {
            var data = from student in _context.Students
                        group student by student.EnrollmentDate into dateGroup
                        select new EnrollmentDateGroup()
                        {
                            EnrollmentDate = dateGroup.Key,
                            StudentCount = dateGroup.Count()
                        };
            var enrollmentDates = await data.AsNoTracking().ToListAsync();
            return Ok(enrollmentDates);
        }

    }

    
}