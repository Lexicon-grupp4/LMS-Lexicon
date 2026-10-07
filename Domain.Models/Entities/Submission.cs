using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models.Entities
{
    public class Submission
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public SubmissionStatus Status { get; set; } 
    }
}
