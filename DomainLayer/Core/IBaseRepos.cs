using DomainLayer.Entities;
using DomainLayer.SpecificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Core
{
    public interface IBaseRepos<T> where T : BaseEntities
    {
        Task<IList<T>> ListAllAsync();

        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);


        Task<IList<T>> ListAsynccheck(ISpecification<T> spec);
        Task<T?> FindOne(ISpecification<T?> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
