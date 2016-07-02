using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

public class GitHubController : Controller
{
    public async Task<string> GitUser(string userId)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "aspnetcore-websample");
        string result = await client.GetStringAsync("https://api.github.com/users/" + userId);

        dynamic data = JsonConvert.DeserializeObject(result);
        string name = data.name;

        return "Hello " + name + "!!!1";
    }
}

public class Startup
{
    // Add Framework Services
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{action}/{userId}",
                defaults: new { controller = "GitHub", action="GitUser", userId="qubitron" });
        });
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();

        host.Run();
    }
}