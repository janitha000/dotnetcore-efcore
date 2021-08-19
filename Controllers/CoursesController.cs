using System.Collections.Generic;
using System.Threading.Tasks;
using efcore.Contexts;
using efcore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _context.Courses.Include(c => c.Department).AsNoTracking().ToListAsync();
            return Ok(courses);
        }
        
    }
}