using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        // 配置基本架構，並且繼承自 Entity Framework 的 DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> MyProperty { get; set; }
        // 這個是用來對應 Category 這個 Model 的資料表，且會自動建立資料表
    }
}
