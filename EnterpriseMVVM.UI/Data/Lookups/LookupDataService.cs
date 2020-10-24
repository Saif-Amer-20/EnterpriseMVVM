using EnterpriseMVVM.DataAccess;
using EnterpriseMVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.Data.Lookups
{
    public class LookupDataService : ICategoryLookupDataService,
                                     IProductLookupDataService
    {
        private Func<EnterpriseMVVMDbContext> _contextCreator;

        public LookupDataService(Func<EnterpriseMVVMDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetCategoryLookupAsync(ListSortDirection sortDirection, string findText = "")
        {
            using (var ctx = _contextCreator())
            {
                var orderByAscending = await ctx.Categories.AsNoTracking()
                    .Select(c => new LookupItem { Id = c.CategoryId, DisplayMember = c.Name })
                    .Where(c => c.DisplayMember.Contains(findText)).OrderBy(o => o.DisplayMember)
                    .ToListAsync();
                var orderByDescending = await ctx.Categories.AsNoTracking()
                    .Select(c => new LookupItem { Id = c.CategoryId, DisplayMember = c.Name })
                    .Where(c => c.DisplayMember.Contains(findText)).OrderByDescending(o => o.DisplayMember)
                    .ToListAsync();
                switch (sortDirection)
                {
                    case ListSortDirection.Ascending:
                        return orderByAscending;
                    case ListSortDirection.Descending:
                        return orderByDescending;
                    default:
                        return orderByAscending;
                }
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProductLookupAsync(ListSortDirection sortDirection, string findText = "")
        {
            using (var ctx = _contextCreator())
            {
                var orderByAscending = await ctx.Products.AsNoTracking()
                    .Select(c => new LookupItem { Id = c.ProductId, DisplayMember = c.Name })
                    .Where(c => c.DisplayMember.Contains(findText)).OrderBy(o => o.DisplayMember)
                    .ToListAsync();
                var orderByDescending = await ctx.Products.AsNoTracking()
                    .Select(c => new LookupItem { Id = c.ProductId, DisplayMember = c.Name })
                    .Where(c => c.DisplayMember.Contains(findText)).OrderByDescending(o => o.DisplayMember)
                    .ToListAsync();
                switch (sortDirection)
                {
                    case ListSortDirection.Ascending:
                        return orderByAscending;
                    case ListSortDirection.Descending:
                        return orderByDescending;
                    default:
                        return orderByAscending;
                }
            }
        }
    }
}
