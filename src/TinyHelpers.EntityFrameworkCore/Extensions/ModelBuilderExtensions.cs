using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyQueryFilter<TEntity>(this ModelBuilder modelBuilder, Expression<Func<TEntity, bool>> expression)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(TEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameter, expression.Body);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }
        }
    }

    public static void ApplyQueryFilter<TType>(this ModelBuilder modelBuilder, string propertyName, TType value)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var property = entityType.FindProperty(propertyName);
            if (property?.ClrType == typeof(TType))
            {
                var parameter = Expression.Parameter(entityType.ClrType);
                var filter = Expression.Lambda(Expression.Equal(Expression.Property(parameter, propertyName), Expression.Constant(value)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}
