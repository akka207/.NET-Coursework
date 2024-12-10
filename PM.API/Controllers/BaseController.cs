using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonParamContainers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using PM.API.Data;
using StaffManagerModels;

namespace PM.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class BaseController : Controller
    {
        private const string UNKNOWN = "unknown";
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

            if (Request.Headers["RequestType"] == RequestHeader.PASSWORD.Format())
            {
                Param param = JsonConvert.DeserializeObject<Param>(body);
                return _context.Persons.Where(p => p.Login == param.Value).First()?.HashedPasword ?? UNKNOWN;
            }

            return UNKNOWN;
        }

        [HttpGet("object")]
        public async Task<object> GetObject()
        {
            string body;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }
            if (Request.Headers["RequestType"] == RequestHeader.STAFF.Format())
            {
                Param param = JsonConvert.DeserializeObject<Param>(body);
                Staff staff = await _context.Staffs.Where(s => s.Person.Login == param.Value).Include(s => s.Person).Include(s => s.Schedule).Include(s => s.Role).Include(s => s.Schedule.Events).FirstOrDefaultAsync();
                return staff;
            }

            return UNKNOWN;
        }

        [HttpGet("collection")]
        public async Task<IEnumerable<object>> GetCollection()
        {
            string body;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            if (Request.Headers["RequestType"] == RequestHeader.ALL_STAFF.Format())
            {
                return await _context.Staffs.Include(s => s.Person).Include(s => s.Schedule).Include(s => s.Role).ToListAsync();
            }

            return [UNKNOWN];
        }

        [HttpPost]
        public async Task<string> PostObject()
        {
            string body;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            if (Request.Headers["RequestType"] == RequestHeader.ADD_EVENT.Format())
            {
                StafEvent stafEventPair = JsonConvert.DeserializeObject<StafEvent>(body);
                stafEventPair.Event.ScheduleId = stafEventPair.Staff.ScheduleId;
                await _context.Events.AddAsync(stafEventPair.Event);
                await _context.SaveChangesAsync();

                return JsonConvert.SerializeObject(new Param(true.ToString()));
            }
            else if (Request.Headers["RequestType"] == RequestHeader.CH_PASSWORD.Format())
            {
                string[] values = JsonConvert.DeserializeObject<StringArray>(body).Values;

                Person? findedPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Login == values[0]);
                if (findedPerson != null)
                {
                    findedPerson.HashedPasword = values[1];
                    await _context.SaveChangesAsync();
                    return JsonConvert.SerializeObject(new Param(true.ToString()));
                }

                return JsonConvert.SerializeObject(new Param(false.ToString()));
            }
            else if (Request.Headers["RequestType"] == RequestHeader.CH_PASSWORD_SUBM.Format())
            {
                StringArray sarray = JsonConvert.DeserializeObject<StringArray>(body);

                Person? findedPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Login == sarray.Values[0]);
                if (findedPerson != null && findedPerson.HashedPasword == sarray.Values[1])
                {
                    findedPerson.HashedPasword = sarray.Values[2];
                    await _context.SaveChangesAsync();
                    return JsonConvert.SerializeObject(new Param(true.ToString()));
                }

                return JsonConvert.SerializeObject(new Param(false.ToString()));
            }
            else if (Request.Headers["RequestType"] == RequestHeader.CH_ROLE.Format())
            {
                StaffRole sr = JsonConvert.DeserializeObject<StaffRole>(body);

                Staff staff = await _context.Staffs.FirstOrDefaultAsync(s => s.Id == sr.StaffId);
                if (staff != null)
                {
                    staff.RoleId = sr.RoleId;
                    await _context.SaveChangesAsync();

                    return JsonConvert.SerializeObject(new Param(true.ToString()));
                }
            }
            else if (Request.Headers["RequestType"] == RequestHeader.EDT_PERSON.Format())
            {
                Person newPerson = JsonConvert.DeserializeObject<Person>(body);

                Person person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == newPerson.Id);
                if (person != null)
                {
                    if(person.Email != newPerson.Email)
                    {
                        person.Email = newPerson.Email;
                        await _context.SaveChangesAsync();
                    }
                    if (person.Phone != newPerson.Phone)
                    {
                        person.Phone = newPerson.Phone;
                        await _context.SaveChangesAsync();
                    }
                    return JsonConvert.SerializeObject(new Param(true.ToString()));
                }
            }
            else if (Request.Headers["RequestType"] == RequestHeader.REG_PERSON.Format())
            {
                Staff staff = JsonConvert.DeserializeObject<Staff>(body);


                var schedule = new Schedule();

                await _context.Persons.AddAsync(staff.Person);
                await _context.Schedules.AddAsync(schedule);
                await _context.SaveChangesAsync();

                staff.PersonId = staff.Person.Id;
                staff.ScheduleId = schedule.Id;
                staff.RoleId = staff.RoleId;

                await _context.Staffs.AddAsync(staff);
                await _context.SaveChangesAsync();

                return JsonConvert.SerializeObject(new Param(true.ToString()));

            }

            return UNKNOWN;
        }
    }
}
