using System.Threading.Tasks;
using efcore.Contexts;
using efcore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [ApiController]
    [Route("controller")]
    public class DepartmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        
    }
}