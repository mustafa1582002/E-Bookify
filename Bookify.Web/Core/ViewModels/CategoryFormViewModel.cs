using Bookify.Web.Core.Consts;
using System.ComponentModel;

namespace Bookify.Web.Core.ViewModels
{
    [Index(nameof(Name), IsUnique =true)]
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage ="MAX Length cannot  be more than 5 character ")]
        [Remote("AllowItem",null,ErrorMessage = Errors.Dublicated)]
        public string Name { get; set; } = null!;

    }
}


