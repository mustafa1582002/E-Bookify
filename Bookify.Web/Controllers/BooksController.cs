using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Bookify.Web.Core.Consts;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _allowedExtension = new() { ".jpeg", ".jpg", ".png" };
        private int _maxAlowedSize = 2097152;
        private readonly IMapper _mapper;
        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            
            return View("Form", PopulateViewModel());
        }
       
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", PopulateViewModel(model));
            }
            var book = _mapper.Map<Books>(model);
            if(model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtension.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedExt);
                    return View("Form", model);
                }
                if(model.Image.Length > _maxAlowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaximgSize);
                    return View("Form", model);
                }   
                var imageName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books",imageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                book.ImageUrl = imageName;
            }
            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = category });
            _context.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(b =>b.Categories).SingleOrDefault(b =>b.Id ==id);

            if (book is null)
                return NotFound();

            var model = _mapper.Map<BookFormViewModel>(book);
            var viewmodel = PopulateViewModel(model);

            viewmodel.SelectedCategories=book.Categories.Select(c => c.CategoryId).ToList();
            return View("Form", viewmodel);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));
            
            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == model.Id);

            if (book is null)
                return NotFound();
             
            if (model.Image is not null)
            {
                if(!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", book.ImageUrl);
                    if(System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtension.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedExt);
                    return View("Form", model);
                }
                if (model.Image.Length > _maxAlowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaximgSize);
                    return View("Form", model);
                }
                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", imageName);

                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);

                model.ImageUrl = imageName;
            }
            else if(model.Image is null && !string.IsNullOrEmpty(book.ImageUrl))
                model.ImageUrl= book.ImageUrl;
            
             book =_mapper.Map(model, book);
            book.LastUpdateOn= DateTime.Now;

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = category });

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        private BookFormViewModel PopulateViewModel(BookFormViewModel? model = null)
        {
            BookFormViewModel viewModel = model is null? new BookFormViewModel() : model;   
            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _context.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return viewModel;
        }
    }
}
