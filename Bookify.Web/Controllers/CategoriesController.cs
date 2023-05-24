
namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.Select(c => new CategoryViewModel
            {
                Id= c.Id,
                Name = c.Name,
                IsDeleted= c.IsDeleted,
                CreatedOn= c.CreatedOn,
                LastUpdateOn= c.LastUpdateOn,
            }).AsNoTracking().ToList();
            
            return View(categories);
        }

        public IActionResult Create ()
        {
            return View("Form");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var cattegory = new Category { Name=model.Name};
            _context.Categories.Add(cattegory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);


            if (category is null)
                return NotFound();

            var viewmodel = new CategoryFormViewModel
            {
                Id= id,
                Name= category.Name
            };
            return View("Form", viewmodel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var category = _context.Categories.Find(model.Id);


            if (category is null)
                return NotFound();

          category.Name = model.Name;
          category.LastUpdateOn= DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is null)
                return NotFound();

            category.IsDeleted= !category.IsDeleted;
            category.LastUpdateOn = DateTime.Now;
            _context.SaveChanges();

            return Ok(category.LastUpdateOn.ToString());
        }
        public IActionResult Allowitem(CategoryFormViewModel model)
        { 
            var isExists=_context.Categories.Any(c=>c.Name==model.Name);
            return Json(!isExists);
        }

    }
}

