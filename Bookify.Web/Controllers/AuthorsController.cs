using Bookify.Web.Data.Migrations;

namespace Bookify.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var authors= _context.Authors.Select(c => new AuthorViewModel
            {
                Id = c.Id,
                Name = c.Name,
                IsDeleted = c.IsDeleted,
                CreatedOn = c.CreatedOn,
                LastUpdateOn = c.LastUpdateOn,
            }).AsNoTracking().ToList();
            
            return View(authors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var Author = new Author { Name = model.Name };
            _context.Authors.Add(Author);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Author = _context.Authors.Find(id);


            if (Author is null)
                return NotFound();

            var viewmodel = new AuthorFormViewModel
            {
                Id = id,
                Name = Author.Name!
            };
            return View("Form", viewmodel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var Author = _context.Authors.Find(model.Id);


            if (Author is null)
                return NotFound();

            Author.Name = model.Name;
            Author.LastUpdateOn = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var Author = _context.Authors.Find(id);

            if (Author is null)
                return NotFound();

            _context.Remove(Author);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult Allowitem(AuthorFormViewModel model)
        {
            var isExists = _context.Authors.Any(c => c.Name == model.Name);
            return Json(!isExists);
        }

    }
}
