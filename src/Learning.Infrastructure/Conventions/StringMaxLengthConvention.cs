using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Learning.Infrastructure.Conventions;

internal sealed class StringMaxLengthConvention : IModelFinalizingConvention
{
    private readonly int _maxLength;

    public StringMaxLengthConvention(int maxLength) 
        => _maxLength = maxLength;

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var property in modelBuilder.Metadata.GetEntityTypes()
                     .SelectMany(e => e.GetDeclaredProperties().Where(p => p.ClrType == typeof(string))))
        {
            property.Builder.HasMaxLength(_maxLength);
        }
    }
}
