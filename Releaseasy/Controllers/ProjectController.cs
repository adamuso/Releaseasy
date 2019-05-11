using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Releaseasy.Model;

namespace Releaseasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        // GET: api/Project
        [HttpGet]
        public ActionResult<IEnumerable<Project>> Get()
        {
            using (var context = new ReleaseasyContext())
            {
                return context.Projects.ToArray();
            }
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public ActionResult<Project> Get(int id)
        {
            using (var context = new ReleaseasyContext())
            {
                return context.Projects.Find(id);
            }
        }

        // POST: api/Project
        [HttpPost]
        public void Post([FromBody] Project value)
        {
            using (var context = new ReleaseasyContext())
            {
                try
                {
                    context.Add(value);
                    context.SaveChanges();
                }
                catch (ValidationException ex)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        [HttpPost("AddUser")]
        public void AddUser([FromBody] AddUserParameters inc )
        {     
            using (var context = new ReleaseasyContext())
            {
                User user = context.Users.Where(u => u.Id == inc.UserId).Include(u => u.Projects).Single();
                Project project = context.Projects.Where(p => p.Id == inc.ProjectId).Include(u => u.Users).Single();

                if (user != null && project != null)
                {
                    var projectUser = new ProjectUser
                    {
                        UserId = user.Id,
                        ProjectId = project.Id
                    };

                    project.Users.Add(projectUser);
                    user.Projects.Add(projectUser);
                    context.SaveChanges();
                }
            }


        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Project value)
        {
            Project project;

            using (var context = new ReleaseasyContext())
            {
                project = context.Projects.Find(id);
            
                if (project != null)
                {
                    if (value.Description != null)
                    {
                        project.Description = value.Description;
                    }
                    if (value.Name != null)
                    {
                        project.Name = value.Name;
                    }
                    if (value.StartTime != null)
                    {
                        project.StartTime = value.StartTime;
                    }
                    if (value.EndTime != null)
                    {
                        project.EndTime = value.EndTime;
                    }


                    context.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.SaveChanges();
                }
              
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Project project;

            using (var context = new ReleaseasyContext())
            {
                project = context.Projects.Find(id);

                if (project != null)
                {
                    context.Remove(project);  
                    context.SaveChanges();
                }

            }

        }

        public class AddUserParameters
        {
            public int UserId { get; set; }
            public int ProjectId { get; set; }
        }
    }
}
