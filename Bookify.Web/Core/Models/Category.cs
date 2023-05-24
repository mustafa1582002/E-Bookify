namespace Bookify.Web.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category 
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum Length is 100")]
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? LastUpdateOn { get; set; }
        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();
    }
}
