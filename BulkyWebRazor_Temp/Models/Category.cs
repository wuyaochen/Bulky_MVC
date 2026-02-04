using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWebRazor_Temp.Models
{
    public class Category
    {
        public class Category
        {
            [Key] // 表示這個是主鍵
            public int Id { get; set; }
            [Required] // 表示這個欄位是必填的
            [MaxLength(30)]
            [DisplayName("Category Name")] // 這個是用來顯示在畫面上的名稱s
            public string Name { get; set; }
            [DisplayName("Display Order")]
            [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
            public int DisplayOrder { get; set; }
        }
    }
}
