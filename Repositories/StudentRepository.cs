using System.Collections.Generic;
using System.Threading.Tasks;
using efcore.Contexts;
using efcore.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace efcore.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Student>> Get(string sortParams, string filterParams, int? pageSize, int? pageNumber)
        {
            var students = from s in _context.Students select s;
            if(filterParams is not null)
            {
                students = students.Where(s => s.FirstName.Contains(filterParams) || s.LastName.Contains(filterParams));
            }
            
            switch(sortParams)
            {
                case "firstName":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "lastName":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "enrollmentDate":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
            }
            var pNumber = pageNumber ?? 1;
            var pSize = pageSize ?? 100;

            
            var count = await students.CountAsync();            
            return await students.Skip((pNumber -1) * pSize).Take(pSize).AsNoTracking().ToListAsync();
        }
    }
}