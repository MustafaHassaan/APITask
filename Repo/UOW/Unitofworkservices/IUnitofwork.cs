using Core.Models;
using Repo.UOW.Reoservices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.UOW.Unitofworkservices
{
    public interface IUnitofwork : IDisposable
    {
        IRepository<Products> Products { get; }
        int Complete();
    }
}
