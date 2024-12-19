using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Business.Abstract
{
    public interface IGenericService<T>
    {
        void TAdd(T t);
        void TDelete(int id);
        void TUpdate(T t);
        List<T> TGetList();
        T TGetByID(int id);
    }
}
