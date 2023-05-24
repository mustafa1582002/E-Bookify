using Bookify.Web.Core.Consts;
using System.ComponentModel;

namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = Errors.MAxLength), Display(Name = "Author")]
        [Remote("AllowItem", null!, AdditionalFields  = "Id", ErrorMessage = Errors.Dublicated)]
        public string Name { get; set; } = null!;

    }
}


