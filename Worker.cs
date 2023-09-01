namespace SenitronPrintHandlerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
    

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            // start init the config
            // get the config information
            string ServiceRootPath = _configuration.GetSection("SenitronPrintHandlerServiceConfig:ServiceRootPath").Value;
            //string settingFilePath = Path.Combine(Path.GetFullPath(ServiceRootPath),"setup.ini");
            string pdfFromPath = _configuration.GetSection("SenitronPrintHandlerServiceConfig:PDFFromPath").Value;
            string pdfToPath = _configuration.GetSection("SenitronPrintHandlerServiceConfig:PDFToPath").Value;
            string logFilePath = _configuration.GetSection("SenitronPrintHandlerServiceConfig:LogFilePath").Value;

            // start init the config
            PdfFileWatcher pdfWatcher = new PdfFileWatcher(ServiceRootPath, pdfFromPath, pdfToPath, logFilePath);
            //PdfProcessor pdfProcessor = new PdfProcessor();
            try
            {
                pdfWatcher.StartPdfWatcher();
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        pdfWatcher.StopPdfWatcher();
                        _logger.LogInformation("SenitronPrintHandlerService stoping !!!!!!!!!!!!");
                        break;
                    }
                    await Task.Delay(TimeSpan.FromMinutes(0.1), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
                
            }
            catch (Exception ex)
            {
                pdfWatcher.StopPdfWatcher();
                _logger.LogError(ex, "{Message}", ex.Message);
                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                _logger.LogInformation("SenitronPrintHandlerService stopped !!!!!!!!!!!!");
                Environment.Exit(1);
            }

        }
    }
}