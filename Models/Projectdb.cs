using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace portfoliomanager.Models
{
    public class Projectdb
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        //public string ProjectImage { get; set; } = string.Empty;
        //public string ProjectLink { get; set; } = string.Empty;
        public DateTime ProjectDate { get; set; } = DateTime.Now;
        public string ProjectCategory { get; set; } = string.Empty;
        public string ProjectOwnerId { get; set; } = string.Empty;
        public bool ProjectIsReviewed { get; set; } = false;
        
    }
}