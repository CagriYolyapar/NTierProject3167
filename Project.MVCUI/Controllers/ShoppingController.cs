using PagedList;
using Project.BLL.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.PageVM;
using Project.MVCUI.Models.ShoppingTools;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {

        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;

        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
            _odRep = new OrderDetailRepository();
        }


        // GET: Shopping
        public ActionResult ShoppingList(int? page, int? categoryID)
        {

            #region  Notlar
            //nullable int vermemizin sebebi aslında buradaki int'in kacıncı sayfada oldugumuzu temsil edecek olmasıdır...Ancak birisi direkt alısveriş sayfasına ulastıgında sayfa bilgisi (page argümanı) gönderilmeyecektir yani bu parametre null bir deger de alabilecektir ki Action , argüman gönderilmedigi zaman da destek verebilsin...

            //Aynı zamanda bu Action , Category'e göre de ürün getirebilsin diye bir de categoryID isminde int? tipinde bir parametre daha verdik...

            #endregion

            #region OnemliNotlar

            //string a = "Mehmet";

            //string b = a ?? "Çağrı"; //a null ise b'ye Cagrı degerini at...Ama a'nın degeri null degilse b'ye a'nın degerini at...

            //page??1

            //page?? ifadesi page null ise demektir...


            #endregion

            ShoppingListPageVM slvm = new ShoppingListPageVM
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives()
            };

            if (categoryID != null) TempData["catID"] = categoryID;


            return View(slvm);
        }

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            Product eklenecekUrun = _pRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }

        public ActionResult CartPage()
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                return View(c);
            }
            TempData["bos"] = "Sepetinizde ürün bulunmamaktadır";
            return RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenCikar(id);
                if (c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizdeki tüm ürünler bosaltılmıstır";
                    return RedirectToAction("ShoppingList");
                }
            }

            return RedirectToAction("CartPage");
        }

        public ActionResult ConfirmOrder()
        {
            AppUser currentUser;

            if (Session["member"] != null)
            {
                currentUser = Session["member"] as AppUser;
            }

            return View();
        }

        //http://localhost:65360/api/Payment/ReceivePayment

        [HttpPost]
        public async Task<ActionResult> ConfirmOrder(OrderPageVM ovm)
        {
            bool sonuc;
            Cart sepet = Session["scart"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentRM.ShoppingPrice = Convert.ToDecimal(sepet.TotalPrice);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65360/api/");

                HttpResponseMessage postTask = await client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentRM);

                HttpResponseMessage result;

                try
                {
                    result = postTask;
                }
                catch (Exception ex)
                {

                    TempData["baglantiRed"] = "Banka baglantıyı reddetti";
                    return RedirectToAction("ShoppingList");
                }

                if (result.IsSuccessStatusCode) sonuc = true;
                else sonuc = false;

                if (sonuc)
                {
                    if (Session["member"] != null)
                    {
                        AppUser kullanici = Session["member"] as AppUser;
                        ovm.Order.AppUserID = kullanici.ID;
                    }

                    _oRep.Add(ovm.Order); //OrderRepository bu noktada Order'i eklerken onun ID'sini olusturur...

                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = Convert.ToDecimal(item.SubTotal);
                        od.Quantity = item.Amount;
                        _odRep.Add(od);

                        //Stoktan da düsürelim
                        Product stoktanDusurulecek = _pRep.Find(item.ID);
                        stoktanDusurulecek.UnitsInStock -= item.Amount;
                        _pRep.Update(stoktanDusurulecek);


                        //Todo: Algoritma Odevi: Eger stoktan düsürüldügünde stokta kalmayacak bir şekilde item varsa onun Amount'i sepette asılamayacak bir hale gelsin

                    }

                    //database'e gidip OrderID'si 

                    //List<OrderDetail> siparisDetaylari = _odRep.Where(x => x.OrderID == ovm.Order.ID).ToList();

                    //
                    /*
                     

                    string detay = "";
                     
                     foreach(OrderDetail item in sipariDetalari)
                    {
                    

                    detay += $"{ item.Product.ProductName} ,   \n";
                      
                    item.Product.UnitPrice
                    item.Amount
                    item.SubTotal



                    }
                     
                     
                     
                     
                     
                     
                     */


                    TempData["odeme"] = "Siparişiniz bize ulasmıstır...TEsekkür ederiz";




                    if (Session["member"] != null) MailService.Send(ovm.Order.AppUser.Email, body: $"Siparişiniz basarıyla alındı:{ovm.Order.TotalPrice}");
                    else MailService.Send(ovm.Order.NonMemberEmail, body: $"Siparişiniz basarıyla alındı {ovm.Order.TotalPrice}");

                    Session.Remove("scart");
                    return RedirectToAction("ShoppingList");


                }
                else
                {
                    Task<string> mesaj = result.Content.ReadAsStringAsync();
                    TempData["sorun"] = mesaj;
                    return RedirectToAction("ShoppingList");
                }
            }



        }
    }
}