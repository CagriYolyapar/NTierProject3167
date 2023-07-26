using Project.MVCUI.Areas.Administrator.Data.AdminResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Administrator.Data.AdminPageVms
{
    public class ListCategoriesPageVM
    {
        public List<CategoryResponseModel> Categories { get; set; }
    }
}