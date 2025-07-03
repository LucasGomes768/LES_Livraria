using E_CommerceLivraria.Helpers;
using System.Linq.Expressions;

namespace E_CommerceLivraria.Specifications.OperatorsSpecs
{
    public class AndSpecification<T> : BaseSpecification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(
            CombineCriteria(left, right),  // Critério combinado
            CombineIncludes(left, right),  // Includes combinados
            CombineIncludeStrings(left, right),  // Include strings combinados
            right.OrderBy ?? left.OrderBy,  // Ordenação
            right.OrderByDescending ?? left.OrderByDescending  // Ordenação descendente
        )
        { }

        private static Expression<Func<T, bool>>? CombineCriteria(
            ISpecification<T> left,
            ISpecification<T> right)
        {
            if (left.Criteria != null && right.Criteria != null)
                return PredicateBuilder.And(left.Criteria, right.Criteria);

            return left.Criteria ?? right.Criteria;
        }

        private static List<Expression<Func<T, object>>> CombineIncludes(
            ISpecification<T> left,
            ISpecification<T> right)
        {
            var includes = new List<Expression<Func<T, object>>>();
            includes.AddRange(left.Includes);
            includes.AddRange(right.Includes.Where(include =>
                !left.Includes.Any(li => li.ToString() == include.ToString())));

            return includes;
        }

        private static List<string> CombineIncludeStrings(
        ISpecification<T> left,
        ISpecification<T> right)
        {
            var includeStrings = new List<string>();
            includeStrings.AddRange(left.IncludeStrings);
            includeStrings.AddRange(right.IncludeStrings.Where(include =>
                !left.IncludeStrings.Contains(include)));

            return includeStrings;
        }
    }
}
