using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Releaseasy.Model;

namespace Releaseasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ReleaseasyContext context;

        public ProjectController(ReleaseasyContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        private User ReturnLoggedUser()
        {
            Model.User actualUser = null;
            try
            {
                var userID = userManager.GetUserId(User);
                actualUser = context.Users.Where(u => u.Id == userID).Single();
            }
            catch (Exception ex)
            {
                Console.Write("SOMEONE NOT LOGGED TRIED TO PERFORM AN ACTION: ");
                Console.WriteLine(ex);
            }
            return actualUser;
        }

        // GET: api/Project
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Project>> Get()
            => context.Projects.ToArray();

        // GET: api/Project/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Project> Get(int id)
        {
            var project = context.Projects.Where(p => p.Id == id).Include(p => p.Creator).Include(p => p.Tasks).Single();

             return Ok(new
                {
                    Creator = project.Creator.Id,
                    project.Description,
                    project.Name,
                    project.Id,
                    project.EndTime,
                    project.StartTime,
                    Tasks = project.Tasks.Select(t => new {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        Status = t.Status,
                        Creator = t.Creator.Id
                    }).ToArray()
                }
            );
        }

        // POST: api/Project
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Project>> Post([FromBody] Project value)
        {
            var user = await userManager.GetUserAsync(User);

            if (value.Name.Length < 3)
            {
                throw new InvalidOperationException("Project name must be at least 3 characters long");
            }

            try
            {
                value.Creator = user;
                value.StartTime = DateTime.Now;

                context.Add(value);
                context.SaveChanges();

                return Ok(new
                    {
                        value.Description,
                        value.Name,
                        value.Id,
                        value.EndTime,
                        value.StartTime
                    }
                );
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
        public void AddUser([FromBody] UserProjectPair inc)
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

        [HttpPost("AddTask")]
        public void AddTask([FromBody] AddTaskHelper ath)
        {
            var Project = context.Projects.Where(p => p.Id == ath.ProjectId).Include(p => p.Tasks).Single();
            var TaskToAdd = context.Tasks.Where(t => t.Id == ath.TaskId).Single();

            Project.Tasks.Add(TaskToAdd);
            context.SaveChanges();
        }

        [HttpPost("RemoveTask")]
        public void RemoveTask([FromBody] TaskProjectPair tpp)
        {
            Project project = context.Projects.Where(p => p.Id == tpp.ProjectId).Include(t => t.Tasks).Single();

            if (project == null)
            {
                throw new InvalidOperationException("Specified Project doesn't exist!");
            }

            Model.Task taskToRemove = null;

            foreach (var connection in project.Tasks)
            {
                if (tpp.TaskId == connection.Id)
                {
                    taskToRemove = connection;
                }
            }

            if (taskToRemove == null)
            {
                throw new InvalidOperationException("Selected Project doesn't have specified task!");
            }

            project.Tasks.Remove(taskToRemove);
            context.SaveChanges();
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
                        throw new InvalidOperationException("Selected user is not a member of selected project.");
                }
            }
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Project value)
        {
            Project project;

            project = context.Projects.Where(p => p.Id == id).Single();

            if (project != null)
            {
                if (value.Description != null)
                    project.Description = value.Description;

                if (value.Name != null)
                    project.Name = value.Name;

                if (value.StartTime != null)
                    project.StartTime = value.StartTime;

                if (value.EndTime != null)
                    project.EndTime = value.EndTime;

                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var project = context.Projects.Find(id);

            if (project != null)
            {
                context.Remove(project);
                context.SaveChanges();
            }
        }

        [HttpGet("LastCreatedProjects")]
        [Authorize]
        public async Task<ActionResult<ICollection<object>>> GetLastCreatedProjects()
        {
            var projects = context.Projects.OrderByDescending(p => p.StartTime);

            return Ok(projects.Select(p => new
            {
                p.Description,
                p.EndTime,
                p.Id,
                p.Name,
                p.StartTime
            }));
        }

        public class UserProjectPair
        {
            public string UserId { get; set; }
            public int ProjectId { get; set; }
        }

        public class AddTaskHelper
        {
            public int ProjectId { get; set; }
            public int TaskId { get; set; }
        }

        public class TaskProjectPair
        {
            public int ProjectId { get; set; }
            public int TaskId { get; set; }
        }
    }
}
