using BerberWeb.DataAccess.Abstract;
using BerberWeb.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.DataAccess.Repository
{
    public class GenericRepository<T>(BerberWebDbContext _context) : IGenericDal<T> where T : class
    {
      

        public DbSet<T> Table { get => _context.Set<T>(); }

        public void Delete(int id)
        {
            var entity = Table.Find(id);
            Table.Remove(entity);
            //_context.Remove(t);
            _context.SaveChanges();
        }

        public List<T> GetbyFilter(Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>().Where(filter).ToList();
        }

        public T GetByID(int id)
        {
            return Table.Find(id);
        }

        public List<T> GetList()
        {
            return _context.Set<T>().ToList();
        }

        public void Insert(T t)
        {
            Table.Add(t);
            _context.SaveChanges();
        }

        public void Update(T t)
        {
            Table.Update(t);
            _context.SaveChanges();
        }
    }
}
