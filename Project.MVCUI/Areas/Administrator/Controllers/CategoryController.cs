using Project.BLL.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Areas.Administrator.Data.AdminPageVms;
using Project.MVCUI.Areas.Administrator.Data.AdminRequestModels;
using Project.MVCUI.Areas.Administrator.Data.AdminResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Administrator.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRepository _cRep;

        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }

        // GET: Administrator/Category

        //CategoryResponseModel 
        //List<CategoryResponseModel>

        //_cRep.Select(x=> new CategoryResponseModel{}).ToList();
        public ActionResult ListCategories()
        {
            //Dikkat ederseniz Domain Entity ile Client tarafındaki direkt iletişimi kestik
            ListCategoriesPageVM lcVm = new ListCategoriesPageVM
            {
                Categories = _cRep.Where(x=>x.Status != ENTITIES.Enums.DataStatus.Deleted).Select(x => new CategoryResponseModel
                {
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    Status = x.Status
                }).ToList()
            };
            return View(lcVm);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryRequestModel category)
        {
            Category c = new Category
            {
                CategoryName = category.CategoryName,
                Description = category.Description
            };
            _cRep.Add(c);
            return RedirectToAction("ListCategories");
        }

        public ActionResult UpdateCategory(int id)
        {
            return View(_cRep.Find(id));
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            _cRep.Update(category);
            return RedirectToAction("ListCategories");
        }

        public ActionResult DeleteCategory(int id)
        {
            _cRep.Delete(_cRep.Find(id));
            return RedirectToAction("ListCategories");
        }
    }
}