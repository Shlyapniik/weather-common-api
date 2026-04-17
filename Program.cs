using System.Text.Json;

class WeatherResponse
{
    public CurrentWeather current_weather {get; set;}
}

class CurrentWeather
{
    public double temperature {get; set;}
    public double windSpeed {get; set;}
}

class Program
{
    static async Task Main()
    {
        Console.Write("Write latitude: ");
        double latitude = double.Parse(Console.ReadLine());
        Console.Write("Write longitude: ");
        double longitude = double.Parse(Console.ReadLine());

        await GetWeather(latitude, longitude);
    }

    public static async Task GetWeather(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        using HttpClient client = new HttpClient();

        try
        {
            string response = await client.GetStringAsync(url);

            Console.WriteLine("\nRaw JSON");
            Console.WriteLine(response);

            var optinons = new JsonSerializerOptions
            {
                TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver()
            };

            var weather = JsonSerializer.Deserialize<WeatherResponse>(response, optinons);

            if (weather?.current_weather == null)
            {
                Console.WriteLine("No weather data.");
                return;
            }

            Console.WriteLine("\nParsed data:");
            Console.WriteLine($"Temperature: {weather.current_weather.temperature}C");
            Console.WriteLine($"Wind speed: {weather.current_weather.windSpeed}km/h");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}