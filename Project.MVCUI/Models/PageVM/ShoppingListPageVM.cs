using PagedList;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.PageVM
{
    public class ShoppingListPageVM
    {
        //Todo : IPagedList<ProductVM> yapmayı lütfen unutmayın
        public IPagedList<Product> PagedProducts { get; set; }
        public List<Category> Categories { get; set; }

    }
}