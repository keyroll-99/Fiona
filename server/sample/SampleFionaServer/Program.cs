
using Fiona.Hosting;

var serviceBuilder = FionaHostBuilder.CreateHost();

using var host = serviceBuilder.Build();
Console.WriteLine();