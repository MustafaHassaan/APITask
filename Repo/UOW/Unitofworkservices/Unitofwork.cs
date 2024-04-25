using Core.Models;
using Repo.UOW.Reoservices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.UOW.Unitofworkservices
{
    public class Unitofwork : IUnitofwork
    {
        TaskDbcontext _Dbc;
        public IRepository<Products> Products { get; private set; }
        public Unitofwork(TaskDbcontext Dbc)
        {
            _Dbc = Dbc;
            Products = new Repository<Products>(_Dbc);
        }
        public int Complete()
        {
            return _Dbc.SaveChanges();
        }
        public void Dispose()
        {
            _Dbc.Dispose();
        }
    }
}
