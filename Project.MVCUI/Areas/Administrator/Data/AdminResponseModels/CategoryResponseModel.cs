using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Administrator.Data.AdminResponseModels
{
    public class CategoryResponseModel
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DataStatus Status { get; set; }

    }
}