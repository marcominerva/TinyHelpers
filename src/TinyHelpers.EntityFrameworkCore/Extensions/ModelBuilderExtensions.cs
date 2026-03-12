using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ModelBuilder"/> to apply query filters and retrieve entity types.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies a global query filter to all entity types assignable to <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The base type or interface to match entity types against.</typeparam>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to apply the filter to.</param>
    /// <param name="expression">The filter expression to apply.</param>
    public static void ApplyQueryFilter<TEntity>(this ModelBuilder modelBuilder, Expression<Func<TEntity, bool>> expression)
    {
        foreach (var clrType in modelBuilder.GetEntityTypes<TEntity>())
        {
            var parameter = Expression.Parameter(clrType);
            var body = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameter, expression.Body);
            modelBuilder.Entity(clrType).HasQueryFilter(Expression.Lambda(body, parameter));
        }
    }

    /// <summary>
    /// Applies a global query filter to all entity types that have a property with the specified name and type.
    /// </summary>
    /// <typeparam name="TType">The type of the property to filter on.</typeparam>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to apply the filter to.</param>
    /// <param name="propertyName">The name of the property to filter on.</param>
    /// <param name="value">The value to compare the property against.</param>
    public static void ApplyQueryFilter<TType>(this ModelBuilder modelBuilder, string propertyName, TType value)
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
    /// Applies a named query filter to all entity types assignable to <typeparamref name="TEntity"/>.
    /// Named query filters can be selectively disabled at query time using <c>IgnoreQueryFilters</c>.
    /// </summary>
    /// <typeparam name="TEntity">The base type or interface to match entity types against.</typeparam>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to apply the filter to.</param>
    /// <param name="filterName">The name to assign to the query filter.</param>
    /// <param name="expression">The filter expression to apply.</param>
    /// <remarks>
    /// This feature requires .NET 10 or greater. Named query filters allow multiple filters per entity type
    /// and selective disabling via <c>IgnoreQueryFilters(["filterName"])</c>.
    /// See <see href="https://learn.microsoft.com/ef/core/querying/filters">EF Core Query Filters</see> for more information.
    /// </remarks>
    public static void ApplyQueryFilter<TEntity>(this ModelBuilder modelBuilder, string filterName, Expression<Func<TEntity, bool>> expression)
    {
        foreach (var clrType in modelBuilder.GetEntityTypes<TEntity>())
        {
            var parameter = Expression.Parameter(clrType);
            var body = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameter, expression.Body);
            modelBuilder.Entity(clrType).HasQueryFilter(filterName, Expression.Lambda(body, parameter));
        }
    }

    /// <summary>
    /// Applies a named query filter to all entity types that have a property with the specified name and type.
    /// Named query filters can be selectively disabled at query time using <c>IgnoreQueryFilters</c>.
    /// </summary>
    /// <typeparam name="TType">The type of the property to filter on.</typeparam>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to apply the filter to.</param>
    /// <param name="filterName">The name to assign to the query filter.</param>
    /// <param name="propertyName">The name of the property to filter on.</param>
    /// <param name="value">The value to compare the property against.</param>
    /// <remarks>
    /// This feature requires .NET 10 or greater. Named query filters allow multiple filters per entity type
    /// and selective disabling via <c>IgnoreQueryFilters(["filterName"])</c>.
    /// See <see href="https://learn.microsoft.com/ef/core/querying/filters">EF Core Query Filters</see> for more information.
    /// </remarks>
    public static void ApplyQueryFilter<TType>(this ModelBuilder modelBuilder, string filterName, string propertyName, TType value)
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
    /// Gets all entity types in the model that are assignable to <typeparamref name="TType"/>.
    /// </summary>
    /// <typeparam name="TType">The base type or interface to match entity types against.</typeparam>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to query.</param>
    /// <returns>An enumerable of CLR types that are assignable to <typeparamref name="TType"/>.</returns>
    public static IEnumerable<Type> GetEntityTypes<TType>(this ModelBuilder modelBuilder)
        => GetEntityTypes(modelBuilder, typeof(TType));

    /// <summary>
    /// Gets all entity types in the model that are assignable to the specified <paramref name="baseType"/>.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to query.</param>
    /// <param name="baseType">The base type or interface to match entity types against.</param>
    /// <returns>An enumerable of CLR types that are assignable to <paramref name="baseType"/>.</returns>
    public static IEnumerable<Type> GetEntityTypes(this ModelBuilder modelBuilder, Type baseType)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(baseType))
            .ToList();

        return entityTypes.Select(t => t.ClrType);
    }
}
