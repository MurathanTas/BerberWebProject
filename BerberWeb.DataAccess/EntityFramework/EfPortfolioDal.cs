using BerberWeb.DataAccess.Abstract;
using BerberWeb.DataAccess.Context;
using BerberWeb.DataAccess.Repository;
using BerberWeb.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.DataAccess.EntityFramework
{
    public class EfPortfolioDal : GenericRepository<Portfolio>, IPortfolioDal
    {
        public EfPortfolioDal(BerberWebDbContext context) : base(context)
        {
        }
    }
}
