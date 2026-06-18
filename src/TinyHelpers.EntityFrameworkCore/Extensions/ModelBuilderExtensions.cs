using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

/// <summary>
/// Provides <see cref="ModelBuilder" /> helpers for applying shared query filters and discovering mapped CLR entity types.
/// </summary>
/// <remarks>
/// These helpers centralize model-wide conventions, such as soft-delete or tenant filters, so each entity type does not
/// need duplicate configuration when it shares a common base type, interface, or property.
/// </remarks>
public static class ModelBuilderExtensions
{
    extension(ModelBuilder modelBuilder)
    {
        /// <summary>
        /// Applies a global query filter to every mapped entity type assignable to <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">The base type or interface to match entity types against.</typeparam>
        /// <param name="expression">The filter expression to apply.</param>
        /// <remarks>
        /// Use this for cross-cutting filters such as soft delete, tenant isolation, or visibility rules that should
        /// apply consistently to every mapped subtype.
        /// </remarks>
        public void ApplyQueryFilter<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            foreach (var clrType in modelBuilder.GetEntityTypes<TEntity>())
            {
                var parameter = Expression.Parameter(clrType);
                var body = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameter, expression.Body);
                modelBuilder.Entity(clrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }
        }

        /// <summary>
        /// Applies a global query filter to every mapped entity type that exposes a property with the specified name and type.
        /// </summary>
        /// <typeparam name="TType">The type of the property to filter on.</typeparam>
        /// <param name="propertyName">The name of the property to filter on.</param>
        /// <param name="value">The value to compare the property against.</param>
        /// <remarks>
        /// Use this when entities do not share an interface or base type but still expose the same shadow or CLR
        /// property that must be constrained globally.
        /// </remarks>
        public void ApplyQueryFilter<TType>(string propertyName, TType value)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var property = entityType.FindProperty(propertyName);
                if (property?.ClrType == typeof(TType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var propertyAccess = Expression.Call(typeof(EF), nameof(EF.Property), [typeof(TType)], parameter, Expression.Constant(propertyName));
                    var filter = Expression.Lambda(Expression.Equal(propertyAccess, Expression.Constant(value, typeof(TType))), parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }

#if NET10_0_OR_GREATER
    /// <summary>
    /// Applies a named query filter to every mapped entity type assignable to <typeparamref name="TEntity" />.
    /// </summary>
    /// <typeparam name="TEntity">The base type or interface to match entity types against.</typeparam>
    /// <param name="filterName">The name to assign to the query filter.</param>
    /// <param name="expression">The filter expression to apply.</param>
    /// <remarks>
    /// Named filters allow multiple filters per entity type and selective disabling via <c>IgnoreQueryFilters(["filterName"])</c>.
    /// Use this for filters that callers may need to disable independently, such as soft delete without disabling tenant isolation.
    /// See <see href="https://learn.microsoft.com/ef/core/querying/filters">Entity Framework Core Query Filters</see> for more information.
    /// </remarks>
    public void ApplyQueryFilter<TEntity>(string filterName, Expression<Func<TEntity, bool>> expression)
    {
        foreach (var clrType in modelBuilder.GetEntityTypes<TEntity>())
        {
            var parameter = Expression.Parameter(clrType);
            var body = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameter, expression.Body);
            modelBuilder.Entity(clrType).HasQueryFilter(filterName, Expression.Lambda(body, parameter));
        }
    }

    /// <summary>
    /// Applies a named query filter to every mapped entity type that exposes a property with the specified name and type.
    /// </summary>
    /// <typeparam name="TType">The type of the property to filter on.</typeparam>
    /// <param name="filterName">The name to assign to the query filter.</param>
    /// <param name="propertyName">The name of the property to filter on.</param>
    /// <param name="value">The value to compare the property against.</param>
    /// <remarks>
    /// Named filters allow multiple filters per entity type and selective disabling via <c>IgnoreQueryFilters(["filterName"])</c>.
    /// Use this when the filter is driven by a shared property and callers may need to disable it independently.
    /// See <see href="https://learn.microsoft.com/ef/core/querying/filters">Entity Framework Core Query Filters</see> for more information.
    /// </remarks>
    public void ApplyQueryFilter<TType>(string filterName, string propertyName, TType value)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var property = entityType.FindProperty(propertyName);
            if (property?.ClrType == typeof(TType))
            {
                var parameter = Expression.Parameter(entityType.ClrType);
                var propertyAccess = Expression.Call(typeof(EF), nameof(EF.Property), [typeof(TType)], parameter, Expression.Constant(propertyName));
                var filter = Expression.Lambda(Expression.Equal(propertyAccess, Expression.Constant(value, typeof(TType))), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filterName, filter);
            }
        }
    }
#endif

        /// <summary>
        /// Gets the mapped CLR entity types that are assignable to <typeparamref name="TType" />.
        /// </summary>
        /// <typeparam name="TType">The base type or interface to match entity types against.</typeparam>
        /// <returns>An enumerable of CLR types that are assignable to <typeparamref name="TType"/>.</returns>
        public IEnumerable<Type> GetEntityTypes<TType>()
            => modelBuilder.GetEntityTypes(typeof(TType));

        /// <summary>
        /// Gets the mapped CLR entity types that are assignable to the specified runtime type.
        /// </summary>
        /// <param name="baseType">The base type or interface to match entity types against.</param>
        /// <returns>An enumerable of CLR types that are assignable to <paramref name="baseType"/>.</returns>
        public IEnumerable<Type> GetEntityTypes(Type baseType)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.IsAssignableTo(baseType))
                .ToList();

            return entityTypes.Select(t => t.ClrType);
        }
    }
}
