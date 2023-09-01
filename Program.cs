using SenitronPrintHandlerService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;


// create host objects
IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "SenitronPrintHandlerService";
    })
    //.ConfigureAppConfiguration(
    //(hostContext, config) =>
    //{
    //    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    //    //config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
    //}
    //)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();





//IHostBuilder builder = Host.CreateDefaultBuilder(args)
//    .UseWindowsService(options =>
//    {
//        options.ServiceName = "SenitronPrintHandlerService";
//    })
//    .ConfigureServices((context, services) =>
//    {
//        LoggerProviderOptions.RegisterProviderOptions<
//            EventLogSettings, EventLogLoggerProvider>(services);

//        services.AddSingleton<Worker>();
//        services.AddHostedService<Worker>();

//        // See: https://github.com/dotnet/runtime/issues/47303
//        services.AddLogging(builder =>
//        {
//            builder.AddConfiguration(
//                context.Configuration.GetSection("Logging"));
//        });
//    });

//IHost host = builder.Build();
//host.Run();

