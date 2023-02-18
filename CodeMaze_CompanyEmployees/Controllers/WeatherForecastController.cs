using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CodeMaze_CompanyEmployees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        //private readonly ILogger<WeatherForecastController> _logger;
        private ILoggerManager _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        public WeatherForecastController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> GetLog()

        {
            _logger.LogInfo
                ("Here is info message from our values controller.");
            _logger.LogDebug
                ("Here is debug message from our values controller.");
            _logger.LogWarn
                ("Here is warn message from our values controller.");
            _logger.LogError
                ("Here is an error message from our values controller.");

            return new string[] { "value1", "value2" };
            /*
             * 2023-02-10 09:39:44.3316 Debug FileTarget(Name=logfile): Preparing for new file: 'C:\Users\SAGAWIN\source\repos\sagadavid\Api_CompanyEmployees\CodeMaze_CompanyEmployees\bin\Debug\net6.0\.\logs\2023-02-10_logfile.txt'
             * 2023-02-10 09:39:44.3316 Debug FileTarget(Name=logfile): Creating file appender: 'C:\Users\SAGAWIN\source\repos\sagadavid\Api_CompanyEmployees\CodeMaze_CompanyEmployees\bin\Debug\net6.0\.\logs\2023-02-10_logfile.txt'
             * 2023-02-10 09:39:44.3402 Trace FileTarget(Name=logfile): Opening C:\Users\SAGAWIN\source\repos\sagadavid\Api_CompanyEmployees\CodeMaze_CompanyEmployees\bin\Debug\net6.0\.\logs\2023-02-10_logfile.txt with allowFileSharedWriting=False
*/
        }

        [HttpGet("forecast")]
        public IEnumerable<WeatherForecast> GetForecast()
        {
            return Enumerable.Range(1, 5).Select(index => 
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}