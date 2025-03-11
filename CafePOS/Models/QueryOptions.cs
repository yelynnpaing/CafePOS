using System.Linq.Expressions;

namespace CafePOS.Models
{
    public class QueryOptions<T> where T : class
    {
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, bool>> Where { get; set; } = null;
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}