using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcore.Contexts;
using efcore.Entities;
using efcore.Entities.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> Get()
        {            
            var instructors = await _context.Instructors
                                    .Include(i => i.OfficeAssignment)
                                    .Include(i => i.CourseAssignments)
                                        .ThenInclude(c => c.Course)
                                            .ThenInclude(c => c.Enrollments)
                                                .ThenInclude(e => e.Student)
                                    .Include(i => i.CourseAssignments)
                                        .ThenInclude(c => c.Course)
                                            .ThenInclude(c => c.Department)
                                    .AsNoTracking()
                                    .OrderBy(i => i.LastName)
                                    .ToListAsync();
            
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructorById(int id)
        {
            var instructor = await _context.Instructors.Where(i => i.ID == id).SingleOrDefaultAsync();
            
            if(instructor is null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }

        [HttpGet("{id}/eager/courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesEargerly(int id)
        {
            var instructors = await _context.Instructors
                                    .Include(i => i.OfficeAssignment)
                                    .Include(i => i.CourseAssignments)
                                        .ThenInclude(c => c.Course)
                                            .ThenInclude(c => c.Enrollments)
                                                .ThenInclude(e => e.Student)
                                    .Include(i => i.CourseAssignments)
                                        .ThenInclude(c => c.Course)
                                            .ThenInclude(c => c.Department)
                                    .AsNoTracking()
                                    .OrderBy(i => i.LastName)
                                    .ToListAsync();

            var instructor = instructors.Where(i => i.ID == id).Single();
            var courses = instructor.CourseAssignments.Select(c => c.Course);

            return Ok(courses);
        }

        [HttpGet("{id}/explicit/courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetOfficeAssignmentExplicit(int id)
        {
            var instructors = await _context.Instructors.ToListAsync();

            var instructor = instructors.Where(i => i.ID == id).Single();
            await _context.Entry(instructor).Collection(x => x.CourseAssignments).LoadAsync();
            foreach(CourseAssignment assignment in instructor.CourseAssignments)
            {
                await _context.Entry(assignment).Reference(c => c.Course).LoadAsync();
            }

            var courses = instructor.CourseAssignments.Select(c => c.Course);
            return Ok(courses);
        }

        [HttpGet("{id}/office-assignment")]
        public async  Task<ActionResult<OfficeAssignment>> GetOfficeAssignment(int id)
        {
            var instructor = await _context.Instructors.Include(i => i.OfficeAssignment).AsNoTracking().SingleOrDefaultAsync(i => i.ID == id);
            return Ok(instructor.OfficeAssignment);

        }

        [HttpPut("{id}/office-assignment")]
        public async Task<ActionResult<Instructor>> UpdateInstructor(int id, Instructor updatedInstructor)
        {
            var instructorToUpdate = await _context.Instructors.Include(i => i.OfficeAssignment).FirstOrDefaultAsync(i => i.ID == id);
            if(await TryUpdateModelAsync<Instructor>(
                updatedInstructor,
                "",
                i => i.FirstName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment
            ))
            {
                if(String.IsNullOrWhiteSpace(updatedInstructor.OfficeAssignment?.Location))
                {
                    updatedInstructor.OfficeAssignment = null;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(updatedInstructor);
        }

    }
}