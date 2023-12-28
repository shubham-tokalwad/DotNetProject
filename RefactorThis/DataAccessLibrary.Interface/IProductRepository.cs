
using refactor_me.Models;
//using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.DataAccessLibraray.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> LoadProductsAsync(string name);
        Task<Product> GetProductByIdAsync(Guid? id);
        Task SaveOrUpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }
}
