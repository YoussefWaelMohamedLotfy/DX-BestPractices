using System.Data.Common;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Learning.Infrastructure.Interceptors;

public sealed class SuppressDeletesWithoutWhereInterceptor : IDbCommandInterceptor
{
    public ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        result = LogCriticalAndSuppress(command, eventData, result);

        return ValueTask.FromResult(result);
    }

    public InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData,
    InterceptionResult<int> result)
    {
        result = LogCriticalAndSuppress(command, eventData, result);

        return result;
    }

    private static InterceptionResult<int> LogCriticalAndSuppress(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        if (command.CommandText.StartsWith("DELETE", StringComparison.Ordinal)
            && !command.CommandText.Contains("WHERE", StringComparison.Ordinal))
        {
            var factory = eventData.Context!.Database.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<SuppressDeletesWithoutWhereInterceptor>();
            logger.LogCritical("DELETE without WHERE Caught with query {CommandText}",
                command.CommandText
            );

            return InterceptionResult<int>.SuppressWithResult(0);
        }

        return result;
    }
}
