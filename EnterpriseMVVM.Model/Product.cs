using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EnterpriseMVVM.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public double UnitPrice { get; set; } = 0;
    }
}
