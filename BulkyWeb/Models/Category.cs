using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    // 這個是用來建立資料庫的Row名稱
    public class Category
    {
        [Key] // 表示這個是主鍵
        public int Id { get; set; }
        [Required] // 表示這個欄位是必填的
        public string Name { get; set; }
        public string DisplayOrder { get; set; }
    }
}
