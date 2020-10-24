using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;

namespace EnterpriseMVVM.UI.Wrapper
{
    public partial class ProductWrapper : ModelWrapper<Product>
    {
        public ProductWrapper(Product model) : base(model)
        {
        }

        public System.Int32 ProductId
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }
        public System.Int32 ProductIdOriginalValue => GetOriginalValue<System.Int32>(nameof(ProductId));
        public bool ProductIdIsChanged => GetIsChanged(nameof(ProductId));


        public System.String Name
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }
        public System.String NameOriginalValue => GetOriginalValue<System.String>(nameof(Name));
        public bool NameIsChanged => GetIsChanged(nameof(Name));


        public System.Int32 CategoryId
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }
        public System.Int32 CategoryIdOriginalValue => GetOriginalValue<System.Int32>(nameof(CategoryId));
        public bool CategoryIdIsChanged => GetIsChanged(nameof(CategoryId));


        public System.Double UnitPrice
        {
            get { return GetValue<System.Double>(); }
            set { SetValue(value); }
        }
        public System.Double UnitPriceOriginalValue => GetOriginalValue<System.Double>(nameof(UnitPrice));
        public bool UnitPriceIsChanged => GetIsChanged(nameof(UnitPrice));

    }
}
