using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TinyNetHelpers.EntityFrameworkCore.Extensions
{
    public static class DbContextExtensions
    {
        public static Task ExecuteTransactionAsync(this DbContext context, Func<Task> action)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

                await action.Invoke().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
            });
        }
    }
}
