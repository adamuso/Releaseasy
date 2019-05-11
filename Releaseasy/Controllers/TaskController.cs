﻿using System;
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
        // GET: api/Task
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            using (var context = new ReleaseasyContext())
                return context.Tasks.Find(id);
        }


        // POST: api/Task
        [HttpPost]
        public void Post([FromBody] Task value)
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
        [HttpPost("AddTag")]
        public void AddTag([FromBody] AddTagParameters inc)
        {
            using (var context = new ReleaseasyContext())
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


        }
        // PUT: api/Task/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Task value)
        {
            Task task;

            using (var context = new ReleaseasyContext())
            {
                task = context.Tasks.Find(id);

                if (task != null)
                {
                    if (value.Description != null)
                        task.Description = value.Description;
                    if (value.Name != null)
                        task.Name = value.Name;
                    //if (value.Group != null)
                    //    task.Group = value.Group;
                    //if (value.Status != null)
                    //    task.Status = value.Status;

                }
                context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Task task;

            using (var context = new ReleaseasyContext())
            {
                task = context.Tasks.Find(id);

                if (task != null)
                {
                    context.Remove(task);
                    context.SaveChanges();
                }


            }
        }
        public class AddTagParameters
        {
            public int TagId { get; set; }
            public int TaskId { get; set; }
        }
    }
}
