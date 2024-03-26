using Fiona.Hosting;
using Fiona.Hosting.Abstractions;

IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();

using IFionaHost host = serviceBuilder.Build();

Console.WriteLine();