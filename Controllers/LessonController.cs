using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using lessonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace lessonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        //private readonly List<Lesson> Lessons = new (); // todo later - serialise from file/db

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
        public async Task<IEnumerable<Lesson>> GetAll() => await db.Lessons.ToListAsync();


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetById(long id)
        {
            var Lesson = await db.Lessons.FindAsync(id);
            
            if ( Lesson == null )
                return NotFound();

            return Ok ( Lesson );
        }

        //POST  Add newLesson to repository
        [HttpPost]
        public async Task<ActionResult<Lesson>> CreateLesson(Lesson lesson)
        {
            db.Lessons.Add(lesson);
            await db.SaveChangesAsync();
            
            return CreatedAtAction(nameof(lesson), new { id = lesson.Id }, lesson );  
        }

        //PUT
        [HttpPut("{id")]
        public async Task<ActionResult<Lesson>> Put(Lesson lesson)
        {

            db.Entry(lesson).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok (lesson);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lesson>> Delete(long item)
        {
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == item);
            
            if (lesson == null) return Ok();

            db.Lessons.Remove(lesson);

            await db.SaveChangesAsync();

            return Ok();
        }

    }

}
