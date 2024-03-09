using Fiona.Hosting.Tests.FionaServer;

namespace Fiona.Hosting.Tests;

[CollectionDefinition("FionaTests")]
public class BaseTest() : ICollectionFixture<FionaTestServerBuilder>
{
}