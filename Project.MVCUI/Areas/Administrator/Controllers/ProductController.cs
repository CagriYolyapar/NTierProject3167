using Project.BLL.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Areas.Administrator.Data.AdminPageVms;

using Project.MVCUI.Models.CustomTools;
using Project.MVCUI.Models.PageVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Administrator.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository _pRep;
        CategoryRepository _cRep;

        public ProductController()
        {
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();   
        }
        // GET: Administrator/Product
        public ActionResult ListProducts(int? id) //burada demek istedigimiz bu Action'i id degeri gelmeyebilir(nullable) deger gelmezse bütün ürünler (Aktif olanlar) gelsin...Deger gelirse de o kategori id'sine sahip olan ürünler gelsin...
        {
            return View(id==null? _pRep.GetActives():_pRep.Where(x=>x.CategoryID == id));
        }

        public ActionResult AddProduct()
        {
            AddUpdateProductPageVM aupvm = new AddUpdateProductPageVM
            {
                Categories = _cRep.GetActives()
            };
            return View(aupvm);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product,HttpPostedFileBase image,string fileName)
        {
            product.ImagePath = ImageUploader.UploadImage("/Pictures/", image, fileName);
            _pRep.Add(product);
            return RedirectToAction("ListProducts");
        }

        public ActionResult UpdateProduct(int id)
        {
            AddUpdateProductPageVM aupvm = new AddUpdateProductPageVM
            {
                Categories = _cRep.GetActives(),
                Product = _pRep.Find(id)
            };
            return View(aupvm);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            _pRep.Update(product);
            return RedirectToAction("ListProducts");
        }

        public ActionResult DeleteProduct(int id)
        {
            _pRep.Delete(_pRep.Find(id));
            return RedirectToAction("ListProducts");
        }
    }
}