using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo.UOW.Reoservices
{
    public class Repository<T> : IRepository<T> where T : class
    {
        TaskDbcontext _Dbc;
        public Repository(TaskDbcontext Dbc)
        {
            _Dbc = Dbc;
        }
        public IEnumerable<T> GetAll()
        {
            return _Dbc.Set<T>().ToList();
        }
        public T Get(int id)
        {
            return _Dbc.Set<T>().Find(id);
        }
        public void Insert(T entity)
        {
            _Dbc.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _Dbc.Set<T>().Update(entity);
        }
        public void Delbyid(int id)
        {
            var Entdel = _Dbc.Set<T>().Find(id);
            _Dbc.Entry(Entdel).State = EntityState.Deleted;
        }
    }
}
