using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TMDT.DataAccess.Data;
using TMDT.DataAccess.Repository.IRepository;
using TMDT.Models;

namespace TMDT.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db; 
        }

        public IEnumerable<Product> GetTopSellingProducts(int top)
        {
            var topProducts = _db.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(od => od.Count) // Tổng số lượng đã bán
                })
                .OrderByDescending(g => g.TotalSold)
                .Take(top)
                .ToList(); // Thực thi trên SQL Server

            // Lấy thông tin sản phẩm tương ứng
            var productIds = topProducts.Select(p => p.ProductId).ToList();

            return _db.products
                .Where(p => productIds.Contains(p.Id))
                .ToList();
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
