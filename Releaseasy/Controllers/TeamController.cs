using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Releaseasy.Model;


namespace Releaseasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ReleaseasyContext context;

        public TeamController(ReleaseasyContext context)
        {
            this.context = context;
        }
        // GET: api/Team
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Team/5
        [HttpGet("{id}")]
        public ActionResult<Team> Get(int id)
        {
            return context.Teams.Find(id);
        }

        // POST: api/Team
        [HttpPost]
        public void Post([FromBody] Team value)
        {
            try
            {
                context.Add(value);
                context.SaveChanges();
            
            }
            catch(ValidationException ex)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        // PUT: api/Team/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Team value)
        {
            Team team;

            team = context.Teams.Find(id);

            if(team!=null)
            {
                if (value.Name != null)
                    team.Name = value.Name;
            }
            context.Entry(team).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Team team;
            team = context.Teams.Find(id);

            if(team!=null)
            {
                context.Remove(team);
                context.SaveChanges();
            }
        }
    }
}
