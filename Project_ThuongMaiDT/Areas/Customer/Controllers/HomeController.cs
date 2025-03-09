using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TMDT.DataAccess.Repository.IRepository;
using TMDT.Models;
using TMDT.Models.ViewModels;
using TMDT.Utility;

namespace Project_ThuongMaiDT.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork; //thu�?c ti?nh cu?a class na?y 

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchTerm, string category, int pageNumber = 1, int pageSize = 12)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");

            // Filter by search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productList = productList.Where(p =>
                    (p.Title != null && p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Author != null && p.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                productList = productList.Where(p => p.Category.Name == category);
            }

            // Get the total number of products after filtering
            var totalProducts = productList.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Apply pagination
            var paginatedProducts = productList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get categories for the filter
            var categories = _unitOfWork.Category.GetAll().Select(c => c.Name).Distinct().ToList();

            // Store data for the view
            ViewData["CurrentFilter"] = searchTerm;
            ViewData["Categories"] = categories;
            ViewData["SelectedCategory"] = category;

            var viewModel = new ProductViewModel
            {
                Products = paginatedProducts,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            return View(viewModel);
        }




        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId,
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && 
            u.ProductId == shoppingCart.ProductId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                //_unitOfWork.ShoppingCart.Update(shoppingCart); ?? ?�y n�?u du?ng c�?y na?y se? th�m m�?t c?? s?? d?? li�?u m??i v??i ID m??i ta?i
                //vi? khi th�m 1 sa?n ph�?m shoppongcart se? t?? sinh kho?a va? ta?o m�?t csdl m??i nh?ng ta chi? mu�?n c�?p nh�?t m�?t csdl ta mu�?n c�?p nh�?t ch?? kh�ng c�?n th�m 
                //d?? li�?u m??i
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
                //vi?c kh�ng c?n g?i ph??ng th?c Update l� do Entity Framework t? ??ng theo d�i c�c thay ??i ??i v?i c�c ??i t??ng ?� n?p t? c? s? d? li?u
                //v� s? �p d?ng nh?ng thay ??i n�y khi b?n l?u v�o c? s? d? li?u b?ng c�ch g?i _unitOfWork.Save().
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult TopSellingBooks()
        {
            var topProducts = _unitOfWork.OrderDetail.GetTopSellingProducts(6);
            return View(topProducts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}