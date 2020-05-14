using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Releaseasy.Model;
using Microsoft.EntityFrameworkCore;

namespace Releaseasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ReleaseasyContext context;

        public TaskController(ReleaseasyContext context)
        {
            this.context = context;
        }
        // GET: api/Task
        [HttpGet]
        public ActionResult<IEnumerable<Task>>Get()
        {
            return context.Tasks.ToArray();
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {

                return context.Tasks.Find(id);
        }


        // POST: api/Task
        [HttpPost]
        public void Post([FromBody] Task value)
        {
            if (value.Name.Length < 3)
            {
                throw new InvalidOperationException("Task name must be at least 3 characters long");
            }
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
        [HttpPost("AddTag")]
        public void AddTag([FromBody] AddTagParameters inc)
        {

            Tag tag = context.Tags.Where(t => t.Id == inc.TagId).Include(t => t.Tasks).Single();
            Task task = context.Tasks.Where(tt => tt.Id == inc.TaskId).Include(t => t.TaskTags).Single();

            if (tag != null && task != null)
            {
                var tasktag = new TaskTag
                {
                    TagId = tag.Id,
                    TaskId = task.Id
                };

                task.TaskTags.Add(tasktag);
                tag.Tasks.Add(tasktag);
                context.SaveChanges();

            }
        }
       
        // PUT: api/Task/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Task value)
        {
            Task task;


            task = context.Tasks.Find(id);

            if (task != null)
            {
            if (value.Description != null)
                task.Description = value.Description;
            if (value.Name != null)
                task.Name = value.Name;
            }
            context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();


        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Task task;
            task = context.Tasks.Find(id);

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

            if(tag !=null)
            {
                foreach(var connection in tag.Tasks)
                {
                    if(inc.TaskId == connection.TaskId)
                    {
                        Task taskToDeleteFromTag = context.Tasks.Where(tt => tt.Id == inc.TaskId).Include(tt => tt.TaskTags).Single();
                        tag.Tasks.Remove(connection);
                        taskToDeleteFromTag.TaskTags.Remove(connection);
                    }
                    else
                    {
                        throw new InvalidOperationException("Selected Tag is not a member of selected Task");
                    }
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
