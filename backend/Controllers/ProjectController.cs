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
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;

        private readonly ReleaseasyContext context;

        public ProjectController(ReleaseasyContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        // GET: api/Project
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Project>> Get()
        {
            return context.Projects.ToArray();

        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Project> Get(int id)
        {
            return context.Projects.Find(id);
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
        public void AddUser([FromBody] UserProjectPair inc)
        {
            User user = context.Users.Where(u => u.Id == inc.UserId.ToString()).Include(u => u.Projects).Single();
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


        // { "projectId" : "2",
        //   "task"' : {
        //      "name" : "nazwa",
        //      "descritpion" : "descriptionTaska"
        //    }}
        [HttpPost("AddTask")]
        public void AddTask([FromBody] AddTaskHelper ath)
        {
            /*var user = ReturnLoggedUser();
            if (user == null)
            {
                throw new InvalidOperationException("You must be logged in to add Task!");
            }*/
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
                    {
                        Model.Task taskToDeleteFromProject = context.Tasks.Where(tt => tt.Id == tpp.TaskId).Include(tt => tt.TaskTags).Single();
                        project.Tasks.Remove(connection);
                        //taskToDeleteFromProject.TaskTags.Remove(connection);
                    }
                    else
                    {
                        throw new InvalidOperationException("Selected Project does not have that Task!");
                    }

                }
            }


        }


        [HttpPost("RemoveUser")]
        public void RemoveUser([FromBody] UserProjectPair inc)
        {
            User user = context.Users.Where(u => u.Id == inc.UserId.ToString()).Include(u => u.Projects).Single();

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
