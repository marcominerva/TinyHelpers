using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

public static class DbContextExtensions
{
    extension(DbContext context)
    {
        public Task ExecuteTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);

                await action.Invoke().ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            });
        }

        public Task ExecuteTransactionAsync<TResult>(Func<Task<TResult>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
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

        public Task ExecuteTransactionAsync(Func<IDbContextTransaction, Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
                await action.Invoke(transaction).ConfigureAwait(false);
            });
        }

        public Task<TResult> ExecuteTransactionAsync<TResult>(Func<IDbContextTransaction, Task<TResult>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
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
}
