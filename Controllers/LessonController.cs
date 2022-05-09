using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using lessonApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace lessonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly LessonContext db;

        public LessonController(LessonContext context)
        { 
            db = context;
            
            if (!db.Lessons.Any())
            {
                db.Lessons.AddRange(new Lesson { Name = "07 api", TaskDescription = "Made WEB API project", IsTaskComplete = false });
                db.Lessons.Add(new Lesson { Name = "08 sched.c, многозадачность", TaskDescription = "get ready to async", IsTaskComplete = false });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<Lesson>> GetAll(CancellationToken token) 
          { 
            // try
            // {
                return await db.Lessons.ToListAsync(token);
            // }
            // catch (TaskCanceledException tokenException)
            // {
            //    throw;
            // }
          }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetById(long id, CancellationToken token)
        {
            var Lesson = await db.Lessons.FindAsync(id, token);
            
            if ( Lesson == null )
                return NotFound();

            return Ok ( Lesson );
        }

        //POST  Add newLesson to repository
        [HttpPost]
        public async Task<ActionResult<Lesson>> CreateLesson(Lesson lesson, CancellationToken token)
        {
            db.Lessons.Add(lesson);
            await db.SaveChangesAsync(token);
            
            return CreatedAtAction(nameof(lesson), new { id = lesson.Id }, lesson );  
        }

        //PUT
        [HttpPut("{id")]
        public async Task<ActionResult<Lesson>> Put(Lesson lesson, CancellationToken token)
        {

            db.Entry(lesson).State = EntityState.Modified;
            await db.SaveChangesAsync(token);

            return Ok (lesson);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lesson>> Delete(long item, CancellationToken token)
        {
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == item);
            
            if (lesson == null) return Ok();

            db.Lessons.Remove(lesson);

            await db.SaveChangesAsync(token);

            return Ok();
        }

    }

}
