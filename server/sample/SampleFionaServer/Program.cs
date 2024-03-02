
using Fiona.Hosting;

var serviceBuilder = FionaHostBuilder.CreateHostBuilder();

using var host = serviceBuilder.Build();

Console.WriteLine();