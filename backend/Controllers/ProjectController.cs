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
        private readonly ReleaseasyContext context;

        public ProjectController(ReleaseasyContext context)
        {
            this.context = context;
        }


        // GET: api/Project
        [HttpGet]
        public ActionResult<IEnumerable<Project>> Get()
        {
            return context.Projects.ToArray();
         
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public ActionResult<Project> Get(int id)
        {
           return context.Projects.Find(id);
        }

        // POST: api/Project
        [HttpPost]
        public ActionResult<Project> Post([FromBody] Project value)
        {  
            try
            {
                value.StartTime = DateTime.Now;

                context.Add(value);
                context.SaveChanges();

                return value;
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
        [HttpPost("AddUser")]
        public void AddUser([FromBody] UserProjectPair inc )
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

        [HttpPost("RemoveUser")]
        public void RemoveUser([FromBody] UserProjectPair inc)
        {
            User user = context.Users.Where(u => u.Id == inc.UserId).Include(u => u.Projects).Single();

            if (user != null)
            {
                foreach (var connection in user.Projects)
                {
                    if (inc.ProjectId == connection.ProjectId)
                    {
                        Project projectToDeleteUserFrom = context.Projects.Where(p => p.Id == inc.ProjectId).Include(p => p.Users).Single();
                        user.Projects.Remove(connection);
                        projectToDeleteUserFrom.Users.Remove(connection);
                    }
                    else
                    {
                        throw new InvalidOperationException("Selected user is not a member of selected project.");
                    }
                }
            }
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Project value)
        {
            Project project;
            
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Project project;
            
            project = context.Projects.Find(id);

            if (project != null)
            {
                context.Remove(project);  
                context.SaveChanges();
            }
        }

        public class UserProjectPair
        {
            public int UserId { get; set; }
            public int ProjectId { get; set; }
        }
    }
}
