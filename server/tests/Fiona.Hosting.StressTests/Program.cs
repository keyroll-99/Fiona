
using Fiona.Hosting.TestServer;
using NBomber.CSharp;

FionaTestServerStartup fionaServer = new FionaTestServerStartup(builder =>
{
    
});

fionaServer.Run();

using var httpClient = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:7000/")
}; 

var scenario = Scenario.Create("FionaServer", async context =>
{
    var response = await httpClient.GetAsync("status-code/200");
    return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
}).WithLoadSimulations(
    Simulation.KeepConstant(40_000, TimeSpan.FromSeconds(30))
);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithScenarioCompletionTimeout(TimeSpan.FromMinutes(2))
    .Run();