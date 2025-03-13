using Microsoft.AspNetCore.Mvc;
using TMDT.DataAccess.Data;

namespace Project_ThuongMaiDT.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Results(string query)
        {
            var results = _context.products
                .Where(p => p.Title.Contains(query) || p.Description.Contains(query))
                .ToList();

            return View(results);
        }
    }
}
