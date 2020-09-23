using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Releaseasy.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Releaseasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ReleaseasyContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskController(ReleaseasyContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// GET: api/Task
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Task>> Get()
          => context.Tasks.ToArray();

        /// <summary>
        /// GET: api/Task/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var task = context.Tasks.Where(a => a.Id == id).Include(x => x.Tags).Include(x => x.Creator).Single();

            return Ok(new
                {
                    Creator = task.Creator.Id,
                    task.Description,
                    task.Name,
                    task.Id,
                    task.EndTime,
                    task.StartTime,
                    task.Tags
                }
            );
        }

        /// <summary>
        /// POST: api/Task
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public ActionResult<Task> Post([FromBody] Task value)
        {
            if (value.Name.Length < 3)
                throw new InvalidOperationException("Task name must be at least 3 characters long");

            var x = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (x != null)
                value.Creator = context.Users
                    .Where(u => u.UserName == x)
                    .Include(u => u.Projects)
                    .Include(x => x.CreatedProjects)
                    .Single();

            context.Add(value);
            context.SaveChanges();

            return Ok(new {
                value.Id,
                value.Name,
                value.Description,
                value.StartTime,
                value.EndTime,
                value.Status,
                value.Tags,
                Creator = value.Creator.Id
            });
        }

        [HttpPost("AddTag")]
        public void AddTag([FromBody] AddTagParameters inc)
        {
            var tag = context.Tags.Where(t => t.Id == inc.TagId).Include(t => t.Tasks).Single();
            var task = context.Tasks.Where(tt => tt.Id == inc.TaskId).Include(t => t.Tags).Single();

            if (tag != null && task != null)
            {
                var tasktag = new TaskTag
                {
                    TagId = tag.Id,
                    TaskId = task.Id
                };

                task.Tags.Add(tasktag);
                tag.Tasks.Add(tasktag);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// PUT: api/Task/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Task value)
        {
            var task = context.Tasks.Find(id);

            if (task != null)
            {
                if (value.Description != null)
                    task.Description = value.Description;

                if (value.Name != null)
                    task.Name = value.Name;

                if (value.Status != null)
                    task.Status = value.Status;
            }

            context.Entry(task).State = EntityState.Modified;
            context.SaveChanges();
        }

        /// <summary>
        /// DELETE: api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var task = context.Tasks.Find(id);

            if (task != null)
            {
                context.Remove(task);
                context.SaveChanges();
            }
        }

        [HttpPost("RemoveTag")]
        public void RemoveTag([FromBody] AddTagParameters inc)
        {
            Tag tag = context.Tags.Where(t => t.Id == inc.TagId).Include(t => t.Tasks).Single();

            if (tag != null)
            {
                foreach (var connection in tag.Tasks)
                {
                    if (inc.TaskId == connection.TaskId)
                    {
                        Task taskToDeleteFromTag = context.Tasks.Where(tt => tt.Id == inc.TaskId).Include(tt => tt.Tags).Single();
                        tag.Tasks.Remove(connection);
                        taskToDeleteFromTag.Tags.Remove(connection);
                    }
                    else
                        throw new InvalidOperationException("Selected Tag is not a member of selected Task");
                }
            }
        }

        public class AddTagParameters
        {
            public int TagId { get; set; }
            public int TaskId { get; set; }
        }
        public class AddTeamParameters
        {
            public int TeamId { get; set; }
            public int TaskId { get; set; }
        }
    }
}
