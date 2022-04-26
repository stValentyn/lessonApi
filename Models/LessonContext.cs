using Microsoft.EntityFrameworkCore;


namespace lessonApi.Models
{
    public class LessonContext : DbContext
    {
        public LessonContext(DbContextOptions<LessonContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Lesson> Lessons { get; set; } 
    }
}