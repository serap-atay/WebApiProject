using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId);
    }
}
