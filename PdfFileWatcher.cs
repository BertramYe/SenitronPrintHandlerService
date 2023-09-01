using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SenitronPrintHandlerService
{
    internal class PdfFileWatcher
    {
        FileSystemWatcher _fileWatcher;
        //private ILogger<Worker> _logger;
        private string _settingFilePath;
        private string _pdfFromPath;
        private string _pdfToPath;
        private string _logFilePath;
        private LogHelper _serviceLogs;
        private PdfProcessor _pdfprocessor;

        public PdfFileWatcher(string settingFilePath,string pdfFromPath,string pdfToPath, string logFilePath)
        {
            initial(settingFilePath, pdfFromPath, pdfToPath, logFilePath);
 
        }


        private void initial(string settingFilePath, string pdfFromPath, string pdfToPath, string logFilePath)
        {
            try
            {

                string[] path_list = new string[3];
                path_list[0] = pdfFromPath;
                path_list[1] = pdfToPath;
                path_list[2] = logFilePath;
                for (int i = 0; i < path_list.Length; i++)
                {
                    if (!Directory.Exists(path_list[i]))
                    {
                        Directory.CreateDirectory(path_list[i]);
                    }
                }

                LogHelper serviceLogs = new LogHelper(logFilePath, "serviceRunnings");
                serviceLogs.WriteLog("start the service!!!");
                FileSystemWatcher watcher = new FileSystemWatcher(pdfFromPath);
                PdfProcessor pdfprocessor = new PdfProcessor();
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size;
                //watcher.NotifyFilter = NotifyFilters.FileName;

                // watcher event
                watcher.Created += OnFileCreated;
                //watcher.Changed += OnFileChanged;
                //watcher.Deleted += OnFileDeleted;

                _fileWatcher = watcher;
                _settingFilePath = settingFilePath;
                _pdfFromPath = pdfFromPath;
                _pdfToPath = pdfToPath;
                _serviceLogs = serviceLogs;
                _pdfprocessor = pdfprocessor;
                _logFilePath = logFilePath;

            }
            catch (Exception ex)
            {
                _serviceLogs.WriteLog($"Error1: {ex.Message},{ex}");
            }
          
        }

        public void StartPdfWatcher()
        {
            _fileWatcher.EnableRaisingEvents = true;
            //_logger = logger;
        }

        public void StopPdfWatcher()
        {
            _fileWatcher.EnableRaisingEvents = false;


            // release resource
            _fileWatcher.Dispose();
        }

      
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            int currentFileCount = Directory.GetFiles(_pdfFromPath).Length;
            //_logger.LogInformation($"file numbers {currentFileCount}");
            if (currentFileCount > 0 && _pdfprocessor != null)
            {
                try
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(_pdfFromPath);
                 
                    //_logger.LogInformation("Files in the folder:");
                    foreach (string file in files)
                    {
                        _serviceLogs.WriteLog($"find pdf file : {file} ");
                        _serviceLogs.WriteLog($"bgin to handle pdf file : {file} ");
                        _pdfprocessor.Process(file, _settingFilePath, _logFilePath);
                        string fileName = Path.GetFileName(file);
                        string tomovepath = Path.Combine(_pdfToPath, fileName);
                        _serviceLogs.WriteLog($"end handle pdf file : {file} ");
                        File.Move(file, tomovepath);
                        _serviceLogs.WriteLog($"moved pdf file from path: {file}  to path {tomovepath}");
                       
                    }
                }
                catch (Exception ex)
                {
                   
                    //_logger.LogInformation($"Error: {ex.Message},{ex}");
                    _serviceLogs.WriteLog($"Error: {ex.Message},{ex}");
                }
                //_logger.LogInformation($"result, here: pdf files is here ");
                
            }

        }


    }
}
