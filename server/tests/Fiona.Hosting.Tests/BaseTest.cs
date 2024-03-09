using Fiona.Hosting.Tests.Utils;

namespace Fiona.Hosting.Tests;

[CollectionDefinition("FionaTests")]
public class BaseTest() : ICollectionFixture<FionaTestServerBuilder>
{
}