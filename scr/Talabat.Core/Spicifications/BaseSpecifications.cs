using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Spicifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
         public Expression<Func<T, bool>> Criteria { get; set; } = null;
         public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
         public Expression<Func<T, object>> OrderBy { get; set; } = null;
         public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagingEnabled { get; set; }

        public BaseSpecifications()
         {
           
         }

         public BaseSpecifications(Expression<Func<T, bool>> CriteriaExpression)
         {
            Criteria = CriteriaExpression;
         }

         public void AddOrderBy(Expression<Func<T, object>> OrderByEcpression)
         {
            OrderBy = OrderByEcpression;
         }

         public void AddOrderDesc(Expression<Func<T, object>> OrderByDescEcpression)
         {
            OrderByDesc = OrderByDescEcpression;
         }

        public void ApplyPagination(int skip,int take)
        {
            IsPagingEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
