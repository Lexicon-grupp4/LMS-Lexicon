using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public int? ModuleId { get; set; }
        public Module? Module { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DateTime UploadAt { get; set; }

        [ForeignKey(nameof(UploadedBy))]
        public string UploadedById { get; set; } = null!;
        public ApplicationUser UploadedBy { get; set; } = null!;
    }
}
