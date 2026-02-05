using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? Id)
        {
            if (Id != null && Id != 0)
            {
                Category = _db.Categories.Find(Id);
            }
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category); //表單按下送出後，將資料加入資料庫
                _db.SaveChanges(); //save changes to database
                // TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
