﻿using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.GenericRepository.IntRep
{
    public interface IRepository<T> where T: BaseEntity
    {
        


        //List Commands
        List<T> GetAll();
        List<T> GetActives();
        List<T> GetPassives();
        List<T> GetModifieds();


        //Modify Commands

        void Add(T item);
        void AddRange(List<T> list);
        void Update(T item);
        void UpdateRange(List<T> list);
        void Delete(T item);
        void DeleteRange(List<T> list);
        void Destroy(T item);
        void DestroyRange(List<T> list);    


        //Linq

        List<T> Where(Expression<Func<T, bool>> exp); 
        T FirstOrDefault(Expression<Func<T, bool>> exp);
        bool Any(Expression<Func<T, bool>> exp);
        IQueryable<X> Select<X>(Expression<Func<T, X>> exp);




        //Find Command

        T Find(int id);

        //Last Data

        List<T> GetFirstData(int count);

        //First Data
        List<T> GetLastData(int count);

        //Get Counted Datas
        List<T> GetCountedDatas(int count);
    }
}
