using System;
using System.ComponentModel.DataAnnotations;

namespace lessonApi.Models
{
    public class Lesson
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string Name { get; set; }

        public Uri RecordLink { get; set; }

        public string TaskDescription { get; set; }

        public bool IsTaskComplete { get; set; }

        public string Notes { get; set; }

    }
}
