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
    public class TagController : ControllerBase
    {
        private readonly ReleaseasyContext context;

        public TagController(ReleaseasyContext context)
        {
            this.context = context;
        }
        // GET: api/Tag
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Tag/5
        [HttpGet("{id}")]
        public ActionResult<Tag> Get(int id)
        {
            
                return context.Tags.Find(id);
        }

        // POST: api/Tag
        [HttpPost]
        public void Post([FromBody] Tag value)
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

        // PUT: api/Tag/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tag value)
        {

            Tag tag;

           tag = context.Tags.Find(id);

                if (tag != null)
                {
                    if (value.Name != null)
                        tag.Name = value.Name;

                }
                context.Entry(tag).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Tag tag;

                tag = context.Tags.Find(id);

                if (tag != null)
                {
                    context.Remove(tag);
                    context.SaveChanges();
                }
            
        }
    }
}
