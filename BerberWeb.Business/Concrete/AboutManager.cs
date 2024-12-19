using BerberWeb.Business.Abstract;
using BerberWeb.DataAccess.Abstract;
using BerberWeb.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Business.Concrete
{
    public class AboutManager : GenericManager<About>, IAboutService
    {
        //IAboutDal _aboutDal;

        //public AboutManager(IAboutDal aboutDal)
        //{
        //    _aboutDal = aboutDal;
        //}

        //public List<About> TGetList()
        //{
        //    return _aboutDal.GetList();
        //}

        //public void TAdd(About t)
        //{
        //    _aboutDal.Insert(t);
        //}

        //public void TDelete(int id)
        //{
        //    _aboutDal.Delete(id);
        //}

        //public About TGetByID(int id)
        //{
        //    return _aboutDal.GetByID(id);
        //}

        //public void TUpdate(About t)
        //{
        //    _aboutDal.Update(t);
        //}
        public AboutManager(IGenericDal<About> repository) : base(repository)
        {
        }
    }
}
