using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PM.API.Data;
using StaffManagerModels;

namespace PM.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class BaseController : Controller
    {
        private readonly PMAPIContext _context;

        public BaseController(PMAPIContext context)
        {
            _context = context;
        }

        [HttpGet("string")]
        public async Task<string> GetString()
        {
            string body;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            if (Request.Headers["Request"] == RequestHeader.PASSWORD.Format())
            {
                return _context.Persons.Where(p => p.Login == body).First()?.HashedPasword ?? "unknown";
            }

            return "NotFound";
        }

        [HttpGet("collection")]
        public async Task<IEnumerable<object>> GetCollection()
        {
            string body;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            if (Request.Headers["Request"] == RequestHeader.ALL_STAFF.Format())
            {
                return _context.Staffs;
            }

            return null;
        }

    }
}
