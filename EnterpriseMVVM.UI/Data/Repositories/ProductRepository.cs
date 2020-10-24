using EnterpriseMVVM.DataAccess;
using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product, EnterpriseMVVMDbContext>, IProductRepository
    {

        public ProductRepository(EnterpriseMVVMDbContext context) : base(context)
        {
        }

        public async override Task<Product> GetByIdAsync(int id)
        {
            return await Context.Products.SingleAsync(c => c.ProductId == id);
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Context.Products.AsNoTracking().Include(c => c.Category).OrderBy(c => c.CategoryId).ToListAsync();
        }

        public async void ReloadCategoryAsync(int productId)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Product>().SingleOrDefault(db => db.Entity.ProductId == productId);
            if (dbEntityEntry != null)
            {
                if (dbEntityEntry.State != EntityState.Added)
                {
                    await dbEntityEntry.ReloadAsync();
                }
            }
        }
    }
}
