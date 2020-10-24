using EnterpriseMVVM.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EnterpriseMVVM.DataAccess
{
    public class EnterpriseMVVMDbContext : DbContext
    {
        public EnterpriseMVVMDbContext() : base("EnterpriseMVVMConnectionString")
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
