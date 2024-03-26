using Fiona.Hosting.TestServer;
using NBomber.Contracts;
using NBomber.CSharp;

FionaTestServerStartup fionaServer = new FionaTestServerStartup(builder => { });

fionaServer.Run();

using HttpClient httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:7000/")
};

ScenarioProps? scenario = Scenario.Create("FionaServer", async context =>
    {
        var requestStep = await Step.Run("make_request", context, async () =>
        {
            HttpResponseMessage response = await httpClient.GetAsync("stress");
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        });

        return Response.Ok();
    }).WithoutWarmUp()
    .WithLoadSimulations(
        Simulation.Inject(
            10_000,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(60)
        )
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .WithScenarioCompletionTimeout(TimeSpan.FromMinutes(2))
    .Run();