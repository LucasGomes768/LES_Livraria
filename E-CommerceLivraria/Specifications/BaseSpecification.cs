using System.Linq.Expressions;

namespace E_CommerceLivraria.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public List<string> IncludeStrings { get; } = new();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected BaseSpecification() { }

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification(Expression<Func<T, bool>> criteria, List<Expression<Func<T, object>>> includes, List<string> includeStrings, Expression<Func<T, object>> orderBy, Expression<Func<T, object>> orderByDescending)
        {
            Criteria = criteria;
            Includes = includes;
            IncludeStrings = includeStrings;
            OrderBy = orderBy;
            OrderByDescending = orderByDescending;
        }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeExpression)
        {
            IncludeStrings.Add(includeExpression);
        }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByExpression)
        {
            OrderByDescending = orderByExpression;
        }
    }
}
