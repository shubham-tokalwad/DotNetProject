using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.DataAccessLibraray.Interface
{
    public interface IProductOptionRepository
    {
        Task<IEnumerable<ProductOption>> GetOptionsAsync(Guid productId);
        Task<ProductOption> GetOptionAsync(Guid id);
        Task CreateOrUpdateOptionAsync(ProductOption option);
        Task DeleteOptionAsync(Guid id);
    }
}
