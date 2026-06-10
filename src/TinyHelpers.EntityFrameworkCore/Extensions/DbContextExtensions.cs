using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

/// <summary>
/// Provides transaction helper methods for <see cref="DbContext" /> instances.
/// </summary>
/// <remarks>
/// These helpers wrap execution strategies and explicit transactions so callers can run application work with
/// retry-aware database semantics without repeating the same boilerplate in every repository or service.
/// </remarks>
public static class DbContextExtensions
{
    extension(DbContext context)
    {
        /// <summary>
        /// Executes database work inside a transaction and retries the operation when the provider execution
        /// strategy considers the failure transient.
        /// </summary>
        /// <param name="action">The asynchronous unit of work to run inside the transaction.</param>
        /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <remarks>
        /// Use this overload when the operation does not need direct access to the active transaction object.
        /// </remarks>
        public Task ExecuteTransactionAsync(Func<CancellationToken, Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);

                await action.Invoke(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// Executes database work inside a transaction and returns a computed result while preserving retry
        /// semantics for transient failures.
        /// </summary>
        /// <typeparam name="TResult">The result type returned by the work item.</typeparam>
        /// <param name="action">The asynchronous unit of work to run inside the transaction.</param>
        /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <remarks>
        /// Use this overload when the operation needs a transactional boundary and return value, but does not need
        /// direct access to the active <see cref="IDbContextTransaction" /> instance.
        /// </remarks>
        public Task ExecuteTransactionAsync<TResult>(Func<CancellationToken, Task<TResult>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);

                var result = await action.Invoke(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

                return result;
            });
        }

        /// <summary>
        /// Executes database work inside a transaction while exposing the active transaction to the callback.
        /// </summary>
        /// <param name="action">The asynchronous unit of work that receives the active transaction.</param>
        /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <remarks>
        /// This overload is useful when the caller needs transaction metadata or must pass the transaction to
        /// lower-level APIs while still preserving the retry semantics of the execution strategy.
        /// </remarks>
        public Task ExecuteTransactionAsync(Func<IDbContextTransaction, CancellationToken, Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
                await action.Invoke(transaction, cancellationToken).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// Executes database work inside a transaction, exposes the active transaction, and returns a result.
        /// </summary>
        /// <typeparam name="TResult">The result type returned by the work item.</typeparam>
        /// <param name="action">The asynchronous unit of work that receives the active transaction.</param>
        /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
        /// <param name="cancellationToken">The token used to cancel the operation.</param>
        /// <remarks>
        /// This overload is the most flexible option when both transaction access and a computed result are
        /// required.
        /// </remarks>
        public Task<TResult> ExecuteTransactionAsync<TResult>(Func<IDbContextTransaction, CancellationToken, Task<TResult>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
                var result = await action.Invoke(transaction, cancellationToken).ConfigureAwait(false);

                return result;
            });
        }
    }
}
