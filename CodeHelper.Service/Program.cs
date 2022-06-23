using CodeHelper.Service;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "CodeHelper.Service";
     })
    .ConfigureServices(services =>
    {
        //services.AddSingleton<JokeService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
