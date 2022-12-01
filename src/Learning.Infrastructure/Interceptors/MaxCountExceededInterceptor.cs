using System.Data.Common;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Learning.Infrastructure.Interceptors;

public sealed class MaxCountExceededInterceptor : IDbCommandInterceptor
{
    private readonly int _maxCount;

    public MaxCountExceededInterceptor(int maxCount = 100)
        => _maxCount = maxCount;

    public ValueTask<InterceptionResult> DataReaderClosingAsync(DbCommand command, DataReaderClosingEventData eventData, 
        InterceptionResult result)
    {
        LogWarningWhenExceedMaxLength(eventData);
        return ValueTask.FromResult(result);
    }

    public InterceptionResult DataReaderClosing(DbCommand command, DataReaderClosingEventData eventData,
        InterceptionResult result)
    {
        LogWarningWhenExceedMaxLength(eventData);
        return result;
    }

    private void LogWarningWhenExceedMaxLength(DataReaderClosingEventData eventData)
    {
        // Should be eventData.ReadCount - 1
        if (eventData.ReadCount > _maxCount)
        {
            var factory = eventData.Context!.Database.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<MaxCountExceededInterceptor>();
            logger.LogWarning("Result count exceeded max of {MaxCount} with query {CommandText}",
                _maxCount,
                eventData.Command.CommandText
            );
        }
    }
}
