using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymManagement.Infrastructure.Common.Persistence.Converters; // Add this line if the class exists in this namespace

namespace GymManagement.Infrastructure.Common.Persistence;

public static class FluentApiExtensions
{
    // FYI: SQLite doesn't support JSON columns yet. Otherwise, we'd prefer calling .ToJson() on the owned entity instead.
    public static PropertyBuilder<T> HasValueJsonConverter<T>(this PropertyBuilder<T> propertyBuilder)
    {
        return propertyBuilder.HasConversion(
            new ValueJsonConverter<T>(),
            new ValueJsonComparer<T>());
    }

    public static PropertyBuilder<T> HasListOfIdsConverter<T>(this PropertyBuilder<T> propertyBuilder)
    {
        return propertyBuilder.HasConversion(
            new ListOfIdsConverter(),
            new ListOfIdsComparer());
    }
}