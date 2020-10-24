using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using EnterpriseMVVM.Model;
namespace EnterpriseMVVM.UI.Data.Lookups
{
    public interface IProductLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetProductLookupAsync(ListSortDirection sortDirection, string findText = "");
    }
}