namespace Bookify.Web.Core.ViewModels
{
    public class AuthorViewModel :BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}

  