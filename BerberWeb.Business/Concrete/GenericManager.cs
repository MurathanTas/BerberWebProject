using BerberWeb.Business.Abstract;
using BerberWeb.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Business.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IGenericDal<T> _repository;

        public GenericManager(IGenericDal<T> repository)
        {
            _repository = repository;
        }

        public void TAdd(T t)
        {
            _repository.Insert(t);
        }

        public void TDelete(int id)
        {
            _repository.Delete(id);
        }

        public T TGetByID(int id)
        {
            return _repository.GetByID(id);
        }

        public List<T> TGetList()
        {
            return _repository.GetList();
        }

        public void TUpdate(T t)
        {
            _repository.Update(t);
        }
    }
}
