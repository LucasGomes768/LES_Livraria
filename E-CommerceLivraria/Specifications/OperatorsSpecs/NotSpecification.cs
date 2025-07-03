using System.Linq.Expressions;

namespace E_CommerceLivraria.Specifications.OperatorsSpecs
{
    public class NotSpecification<T> : BaseSpecification<T>
    {
        public NotSpecification(ISpecification<T> specification) : base(
                DefineCriteria(specification),
                specification.Includes,
                specification.IncludeStrings,
                specification.OrderBy,
                specification.OrderByDescending
            )
        { }

        private static Expression<Func<T, bool>>? DefineCriteria(ISpecification<T> spec)
        {
            return spec.Criteria != null
                ? Expression.Lambda<Func<T, bool>>(
                        Expression.Not(spec.Criteria.Body),
                        spec.Criteria.Parameters)
                : null;
        }
    }
}
