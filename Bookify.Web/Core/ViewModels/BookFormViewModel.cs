using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(500,ErrorMessage=Errors.MAxLength)]
        public string Title { get; set; } = null!;
        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors  { get; set; }
        [MaxLength(200, ErrorMessage = Errors.MAxLength)]
        public string Publisher { get; set; } = null!;
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; }=DateTime.Now;
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        [MaxLength(50)]
        public string Hall { get; set; } = null!;
        [Display(Name = "Is Availabel For Rental?")]
        public bool IsAvailabelForRental { get; set; }
        public string Description { get; set; } = null!;
        public IList<int> SelectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }

    }
}
