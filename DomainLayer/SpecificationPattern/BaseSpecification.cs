using DomainLayer.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.SpecificationPattern
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public BaseSpecification(){}

        public Expression<Func<T, bool>> Criteria { get; set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; } 
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public Expression<Func<T, object>> GroupBy { get; set; }
        public int Take { get; set;  }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; }

        public BaseSpecification<T> AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes)
        {
            this.Includes = Includes;
            return this;
        }

        public virtual BaseSpecification<T> ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            return this;
        }

        public virtual BaseSpecification<T> ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            return this;
        }

        public virtual BaseSpecification<T> ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
            return this;
        }

        public virtual BaseSpecification<T> ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
            return this;
        }
    }
}
