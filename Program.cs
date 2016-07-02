using System.Net.Http;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class Startup
{
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "NetCore-Sample-App");
            string result = await client.GetStringAsync("https://api.github.com/users/qubitron");

            await context.Response.WriteAsync(result);
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