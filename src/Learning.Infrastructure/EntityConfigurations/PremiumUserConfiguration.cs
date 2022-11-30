using Learning.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.EntityConfigurations;

internal sealed class PremiumUserConfiguration : IEntityTypeConfiguration<PremiumUser>
{
    public void Configure(EntityTypeBuilder<PremiumUser> builder)
    {
        builder.Property(u => u.Bio).HasMaxLength(100);
    }
}
