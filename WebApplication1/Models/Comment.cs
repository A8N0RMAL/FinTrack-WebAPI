using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        // EF will set the relationship 1-M automatically...
        public int? StockId { get; set; }
        public Stock? Stock { get; set; } // To avoid Circular Reference
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
