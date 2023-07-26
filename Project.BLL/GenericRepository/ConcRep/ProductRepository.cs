using Project.BLL.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.GenericRepository.ConcRep
{
    public class ProductRepository:BaseRepository<Product>
    {
        public override List<Product> GetActives()
        {
            return _db.Products.Where(x => x.Status != ENTITIES.Enums.DataStatus.Deleted && x.UnitsInStock >= 10).ToList();
        }
    }
}
