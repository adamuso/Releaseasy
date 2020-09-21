using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public ActionResult<IEnumerable<Project>> Get()
            => context.Projects.ToArray();

        // GET: api/Project/5
        [HttpGet("{id}")]
        public ActionResult<Project> Get(int id)
            => context.Projects.Find(id);

        // POST: api/Project
        [HttpPost]
        public ActionResult<Project> Post([FromBody] Project value)
        {
            if (value.Name?.Length < 3)
                throw new InvalidOperationException("Project name must be at least 3 characters long");

            value.StartTime = DateTime.Now;

            context.Add(value);
            context.SaveChanges();

            return value;
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
            var Project = context.Projects.Where(p => p.Id == ath.ProjectId).Single();
            var TaskToAdd = context.Tasks.Where(t => t.Id == ath.Task.Id).Single();

            Project.Tasks.Add(TaskToAdd);
            context.SaveChanges();
        }

        [HttpPost("RemoveTask")]
        public void RemoveTask([FromBody] TaskProjectPair tpp)
        {
            Project project = context.Projects.Where(p => p.Id == tpp.ProjectId).Include(t => t.Tasks).Single();

            if (project != null)
            {
                foreach (var connection in project.Tasks)
                {
                    if (tpp.TaskId == connection.Id)
                        project.Tasks.Remove(connection);
                    else
                        throw new InvalidOperationException("Selected Project doesn't have specified task!");
                }
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
                        throw new InvalidOperationException("Selected user is not a member of selected project.");
                }
            }
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Project value)
        {
            var project = context.Projects.Find(id);

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

        public class UserProjectPair
        {
            public string UserId { get; set; }
            public int ProjectId { get; set; }
        }

        public class AddTaskHelper
        {
            public int ProjectId { get; set; }
            public Model.Task Task { get; set; }
        }

        public class TaskProjectPair
        {
            public int ProjectId { get; set; }
            public int TaskId { get; set; }
        }
    }
}
