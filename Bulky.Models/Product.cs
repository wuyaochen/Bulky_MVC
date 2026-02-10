using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class Product
    {
        [Key] // 表示這個是主鍵
        public int Id { get; set; }
        [Required] // 表示這個欄位是必填的
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [DisplayName("List Price")]
        [Range(1, 10000, ErrorMessage = "List Price must be between 1 and 10000")]
        public double ListPrice { get; set; }
        [Required]
        [DisplayName("Price for 1-50")]
        [Range(1, 10000, ErrorMessage = "List Price must be between 1 and 10000")]
        public double Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        [Range(1, 10000, ErrorMessage = "List Price must be between 1 and 10000")]
        public double Price50 { get; set; }
        [Required]
        [DisplayName("Price for 100+")]
        [Range(1, 10000, ErrorMessage = "List Price must be between 1 and 10000")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
