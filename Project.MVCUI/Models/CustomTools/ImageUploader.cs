using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.CustomTools
{
    public static class ImageUploader
    {

        //Geriye string deger döndüren metodumuz resmin yolunu döndürecek veya resim yükleme ile ilgili bir sorun varsa onun kodunu döndürecek "C:/Images".... "1" , "2"

        //HttpPostedFileBase
        public static string UploadImage(string serverPath,HttpPostedFileBase file,string name)
        {

            if (file != null)
            {
                Guid uniqueName = Guid.NewGuid(); //Unique(eşi benzeri olmayan) bir deger yaratmaktır...

               

               string[] fileArray =   file.FileName.Split('.'); //c://ProgramFiles/Resimler/Bilim_Kurgu_Resimleri/UzayGemisi.png ..BUrada split metodu sayesinde ilgili yapının uzantısının da iceride bulundugu bir string dizisi almıs olduk...Split metodu belirttiginiz char karakterinden metni bölerek size bir array sunar...

                string extension = fileArray[fileArray.Length - 1].ToLower(); //Dosya uzantısını yakalayarak kücük harflere cevirdik...

                string fileName = $"{uniqueName}.{name}.{extension}"; //Normal şartlarda biz burada GUid kullandıgımız icin asla bir dosya ismi aynı olamyacaktır...Lakin siz GUid kullanmazsanız (sadece kullanıcıya yüklemek istedigi dosyanın ismini girdirmek isterseniz) böyle bir durumda aynı isimde dosya upload'i mümkün hale gelecektir...Dolayısıyla öyle bir durumda ek olarak bir kontrol yapmanız gerekecektir...Tabii ki böyle bir senaryo olsun veya olmasın önce extension kontrol edilmelidir...Bahsettigimiz ek kontrol daha sonra yapılmalıdır...

                if(extension =="jpg" ||extension == "jpeg" || extension =="gif" || extension == "png")
                {
                    //Eger dosya ismi zaten varsa
                    if (File.Exists(HttpContext.Current.Server.MapPath(serverPath + fileName)))
                    {
                        return "1"; //Bu dosya isminde zaten bir dosya var kodu...Ancak Guid kullandıgımız icin bu acıdan zaten güvendeyiz (Dosya ismi zaten var kodu)
                    }
                    else
                    {
                        string filePath = HttpContext.Current.Server.MapPath(serverPath + fileName);
                        file.SaveAs(filePath);
                        return $"{serverPath}{fileName}";
                    }


                }
                else
                {
                    return "2"; //SEcilen dosya bir resim degildir kodu
                }



            }
            else
            {
                return "3"; //Dosya bos kodu
            }

          
        }
    }
}