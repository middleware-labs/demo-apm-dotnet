using Grpc.Core;
using WeatherService;

namespace MW_WebApp_Nuget.Services
{
    public class WeatherServiceImplGrpc: WeatherService.WeatherService.WeatherServiceBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Sunny with a chance of rain.", "Cloudy with chance of rain.", "Sunny", "Cloudy", "Haze", "Foggy"
        };
        public override Task<WeatherReply> GetWeatherGrpc(WeatherRequest request, ServerCallContext context)
        {
            return Task.FromResult(new WeatherReply
            {
                Forecast = Summaries[Random.Shared.Next(Summaries.Length)].ToString(),
            });
        }

    }
}
