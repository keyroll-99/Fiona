using Fiona.Hosting.TestServer;
using NBomber.CSharp;

FionaTestServerStartup fionaServer = new FionaTestServerStartup(builder => { });

fionaServer.Run();

using var httpClient = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:7000/")
};

var scenario = Scenario.Create("FionaServer", async context =>
    {
        var requestStep = await Step.Run("make_request", context, async () =>
        {
            var response = await httpClient.GetAsync("stress");
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        });
        
        return Response.Ok();
    }).WithoutWarmUp()
    .WithLoadSimulations(
        Simulation.Inject(
            rate: 10_000,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromSeconds(60)
        )
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .WithScenarioCompletionTimeout(TimeSpan.FromMinutes(2))
    .Run();