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
    public class TagController : ControllerBase
    {
        private readonly ReleaseasyContext context;

        public TagController(ReleaseasyContext context)
        {
            this.context = context;
        }

        // GET: api/Tag
        [HttpGet]
        public ActionResult<IEnumerable<Tag>> Get()
            => context.Tags;

        // GET: api/Tag/{id}
        [HttpGet("{id}")]
        public ActionResult<Tag> Get(int id)
        {
            return context.Tags.Where(x => x.Id == id).Include(x => x.Tasks).Single();
        }

        // POST: api/Tag
        [HttpPost]
        public ActionResult<int> Post([FromBody] Tag value)
        {
            if (context.Tags.Any(a => a.Name == value.Name))
                return -1;
            else
            {
                context.Add(value);
                context.SaveChanges();
                return 1;
            }
        }

        // PUT: api/Tag/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tag value)
        {
            var tag = context.Tags.Find(id);

            if (tag != null && value.Name != string.Empty)
                tag.Name = value.Name;

            context.Entry(tag).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // var
            Tag tag = context.Tags.Find(id);

            if (tag != null)
            {
                context.Remove(tag);
                context.SaveChanges();
            }
        }
    }
}
