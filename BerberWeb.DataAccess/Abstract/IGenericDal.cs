using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.DataAccess.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        void Insert(T t);
        void Delete(int id);
        void Update(T t);
        List<T> GetList();
        T GetByID(int id);
        List<T> GetbyFilter(Expression<Func<T, bool>> filter);
    }
}
