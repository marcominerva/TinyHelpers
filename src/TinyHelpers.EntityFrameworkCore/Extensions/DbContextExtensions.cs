using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

public static class DbContextExtensions
{
    public static Task ExecuteTransactionAsync(this DbContext context, Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);

            await action.Invoke().ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        });
    }

    public static Task ExecuteTransactionAsync<TResult>(this DbContext context, Func<Task<TResult>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);

            var result = await action.Invoke().ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

            return result;
        });
    }

    public static Task ExecuteTransactionAsync(this DbContext context, Func<IDbContextTransaction, Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            await action.Invoke(transaction).ConfigureAwait(false);
        });
    }

    public static Task<TResult> ExecuteTransactionAsync<TResult>(this DbContext context, Func<IDbContextTransaction, Task<TResult>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            var result = await action.Invoke(transaction).ConfigureAwait(false);

            return result;
        });
    }
}
