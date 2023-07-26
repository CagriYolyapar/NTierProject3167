using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Administrator.Data.AdminPageVms
{
    public class AddUpdateProductPageVM
    {
        public List<Category> Categories { get; set; }
        public Product Product { get; set; }

    }
}