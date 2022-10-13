using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GojoMarket.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]  
        public String? Name { get; set; }
        [DisplayName("Products")]
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Num of Orders in Category must be Greater than 0")]
        public int NumOfOrders { get; set; }
    }
}
