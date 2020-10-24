using EnterpriseMVVM.DataAccess;
using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category, EnterpriseMVVMDbContext>, ICategoryRepository
    {

        public CategoryRepository(EnterpriseMVVMDbContext context) : base(context)
        {

        }

        public async Task<bool> IsReferencedByProductAsync(int categoryId)
        {
            return await Context.Products.AsNoTracking().AnyAsync(p => p.CategoryId == categoryId);
        }
    }
}
