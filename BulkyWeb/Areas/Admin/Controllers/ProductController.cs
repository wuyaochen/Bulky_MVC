using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepositor;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                //update product
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webhostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //產生一個獨特的檔案名稱
                    var productPath = Path.Combine(wwwRootPath, @"images\product"); //設定上傳路徑

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) //如果產品已經有圖片了，刪除舊圖片
                    {
                        // delete old image
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\')); //取得舊圖片的完整路徑
                        if (System.IO.File.Exists(oldImagePath)) //如果舊圖片存在，刪除它
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) //建立一個新的檔案流，準備將新圖片寫入
                    {
                        file.CopyTo(fileStream); //將新圖片上傳到指定路徑
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName; //更新產品的圖片URL
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product); //表單按下送出後，將資料加入資料庫
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product); 
                }

                _unitOfWork.Save(); //save changes to database
                TempData["success"] = "Product created successfully"; //記錄這個動作成功
                return RedirectToAction("Index");
            }
            else
            {
                //如果驗證失敗，重新載入CategoryList
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(productVM);
            }

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                        Path.Combine(_webhostEnvironment.WebRootPath,
                        productToBeDeleted.ImageUrl.TrimStart('\\')); //取得舊圖片的完整路徑
            if (System.IO.File.Exists(oldImagePath)) //如果舊圖片存在，刪除它
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save(); //save changes to database

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
