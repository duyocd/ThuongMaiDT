using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDT.DataAccess.Data;
using TMDT.DataAccess.Repository.IRepository;
using Accord.MachineLearning.Rules;

using Microsoft.EntityFrameworkCore;
using TMDT.Utility;
namespace TMDT.DataAccess.Repository
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly ApplicationDbContext _db;

        public RecommendationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<int> GetRecommendedProducts(int productId)
        {
            // Truy vấn chỉ lấy OrderHeaderId và danh sách sản phẩm (tránh Include không cần thiết)
            var transactions = _db.OrderDetails
                .Where(od => od.OrderHeaderId != null && od.OrderHeader.OrderStatus == SD.StatusApproved && od.OrderHeader.PaymentStatus == SD.PaymentStatusApproved ) // Đảm bảo không có OrderHeader null
                .GroupBy(od => od.OrderHeaderId)
                .Where(g => g.Select(od => od.ProductId).Distinct().Count() > 1) // Chỉ lấy Order có nhiều hơn 1 sản phẩm
                .Select(g => g.Select(od => od.ProductId).Distinct().ToArray()) // Chuyển mỗi nhóm thành 1 mảng sản phẩm
                .ToList(); // Thực thi truy vấn trên SQL Server

            if (transactions.Count == 0)
            {
                return new List<int>(); // Tránh lỗi nếu không có dữ liệu
            }

            // Chuyển đổi dữ liệu thành kiểu int[][] để phù hợp với Apriori
            int[][] transactionsArray = transactions.ToArray();

            // Khởi tạo thuật toán Apriori với ngưỡng tối thiểu (support) và confidence
            var apriori = new Apriori<int>(threshold: 2, confidence: 0.5);
            var model = apriori.Learn(transactionsArray);

            // Lấy danh sách các luật kết hợp (association rules)
            AssociationRule<int>[] rules = model.Rules;

            // Danh sách sản phẩm được đề xuất
            var recommendations = new HashSet<int>(); // Dùng HashSet tránh trùng lặp

            foreach (var rule in rules)
            {
                if (rule.X.Contains(productId)) // Nếu sản phẩm đầu vào nằm trong tập X
                {
                    foreach (var item in rule.Y)
                    {
                        recommendations.Add(item); // Thêm sản phẩm vào danh sách gợi ý
                    }
                }
            }

            return recommendations.ToList(); // Chuyển HashSet thành List trước khi trả về
        }


    }
}
