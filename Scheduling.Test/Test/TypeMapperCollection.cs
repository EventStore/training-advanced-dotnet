using Xunit;

namespace Scheduling.Test.Test;

[CollectionDefinition("TypeMapper collection")]
public class DatabaseCollection : ICollectionFixture<TypeMapperFixture>
{
}