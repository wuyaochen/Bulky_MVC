using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepositor;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }
        public IActionResult Create()
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
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product); //表單按下送出後，將資料加入資料庫
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

        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            { 
                return NotFound(); 
            }
            Product? productFromDb = _unitOfWork.Product.Get(u=>u.Id==id);
            // Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            // Category? categoryFromDb2 = _db.Categories.Find(id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj); //表單按下送出後，將資料加入資料庫
                _unitOfWork.Save(); //save changes to database
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save(); //save changes to database
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
