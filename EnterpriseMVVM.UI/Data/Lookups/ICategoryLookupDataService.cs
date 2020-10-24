using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using EnterpriseMVVM.Model;

namespace EnterpriseMVVM.UI.Data.Lookups
{
    public interface ICategoryLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetCategoryLookupAsync(ListSortDirection sortDirection, string findText = "");
    }
}