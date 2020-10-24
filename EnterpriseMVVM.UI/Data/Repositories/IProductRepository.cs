using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;

namespace EnterpriseMVVM.UI.Data.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void ReloadCategoryAsync(int productId);
    }
}