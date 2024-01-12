using DomainLayer.Core;
using DomainLayer.Entities;
using DomainLayer.SpecificationPattern;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class Repository<T> : IBaseRepos<T> where T : BaseEntities
    {
        private readonly AppDbContext _db;

        public Repository(AppDbContext db)
        {
            _db = db;
        }

        public void Delete(T entity)
        {
           _db.Remove(entity);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<IList<T>> ListAllAsync()
        {
           return await _db.Set<T>().ToListAsync();
        }

        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<IList<T>> ListAsynccheck(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T?> FindOne(ISpecification<T?> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationQueryBuilder<T>.GetQuery(_db.Set<T>().AsQueryable(), spec);
        }
    }
}
