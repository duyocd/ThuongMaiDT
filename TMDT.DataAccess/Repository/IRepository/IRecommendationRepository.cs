using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDT.DataAccess.Repository.IRepository
{
    public interface IRecommendationRepository
    {
        List<int> GetRecommendedProducts(int productId);
    }
}
