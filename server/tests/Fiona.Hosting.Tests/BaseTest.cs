using Fiona.Hosting.Tests.Utils;

namespace Fiona.Hosting.Tests;

[CollectionDefinition("FionaTests")]
public class BaseTest(FionaTestServerBuilder testBuilder) : ICollectionFixture<FionaTestServerBuilder>
{
    protected readonly FionaTestServerBuilder TestBuilder = testBuilder;
}