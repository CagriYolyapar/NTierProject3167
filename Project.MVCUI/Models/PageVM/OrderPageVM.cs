using Project.ENTITIES.Models;
using Project.MVCUI.PaymentAPIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.PageVM
{
    public class OrderPageVM
    {
        public Order Order { get; set; }
        public PaymentRequestModel PaymentRM { get; set; }
    }
}