using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;

namespace EnterpriseMVVM.UI.Wrapper
{
    public partial class CategoryWrapper : ModelWrapper<Category>
    {
        public CategoryWrapper(Category model) : base(model)
        {
        }

        public System.Int32 CategoryId
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }
        public System.Int32 CategoryIdOriginalValue => GetOriginalValue<System.Int32>(nameof(CategoryId));
        public bool CategoryIdIsChanged => GetIsChanged(nameof(CategoryId));


        public System.String Name
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }
        public System.String NameOriginalValue => GetOriginalValue<System.String>(nameof(Name));
        public bool NameIsChanged => GetIsChanged(nameof(Name));


        public System.String Description
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }
        public System.String DescriptionOriginalValue => GetOriginalValue<System.String>(nameof(Description));
        public bool DescriptionIsChanged => GetIsChanged(nameof(Description));

    }
}
