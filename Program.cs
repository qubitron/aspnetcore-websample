using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

public class GitHubController : Controller
{
    public async Task<IActionResult> GitUser(string userId)
    {
        // Get json string containing user's github profile
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "aspnetcore-websample");
        string result = await client.GetStringAsync("https://api.github.com/users/" + userId);

        // Deserialize into a dynamic object that can be accessed by the view
        dynamic data = JsonConvert.DeserializeObject(result);
        ViewData["profile"] = data;

        // Renders /Views/GitHub/GitUser.cshtml and returns it to the browser
        return View();
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add MVC to the app
        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // Show exception details in the browser when requests fail
        if (env.IsDevelopment()) 
        {
            app.UseDeveloperExceptionPage();
        }

        // Serve contents of the wwwroot folder as static files
        app.UseStaticFiles();

        // Set up MVC Routes
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
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .Build();

        host.Run();
    }
}