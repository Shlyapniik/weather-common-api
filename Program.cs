using System.Text.Json;
using System.Globalization;
using System.Linq;

class WeatherResponse
{
    public CurrentWeather current_weather {get; set;}
}

class CurrentWeather
{
    public double temperature {get; set;}
    public double windspeed {get; set;}
}

class Program
{
    static async Task Main()
    {
        Console.WriteLine("\nChoose option:\n"+
                "1. Write latitude and logitude.\n"+
                "2. Exit program.\n");
        int input = int.Parse(Console.ReadLine());

        while (input != 0)
        {
            Console.Clear();
            Console.Write("Write latitude: ");
            double latitude;
            while (!double.TryParse(Console.ReadLine(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out latitude))
            {
                Console.Write("Invalid input. Use dot (e.g. 56.30): ");
            }

            Console.Write("Write longitude: ");
            double longitude;
            while (!double.TryParse(Console.ReadLine(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out longitude))
            {
                Console.Write("Invalid input. Use dot (e.g. 56.30): ");
            }

            await GetWeather(latitude, longitude); 

            Console.WriteLine("\nChoose option:\n"+
                "1. Write latitude and logitude.\n"+
                "2. Exit program.\n");
            input = int.Parse(Console.ReadLine());
        }

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

            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver()
            };

            var weatherList = JsonSerializer.Deserialize<List<WeatherResponse>>(response, options);
            var weather = weatherList?.FirstOrDefault();

            if (weather?.current_weather == null)
            {
                Console.WriteLine("No weather data.");
                return;
            }

            Console.WriteLine($"\nLocation: {latitude}; {longitude}");
            Console.WriteLine("Parsed data:");
            Console.WriteLine($"Temperature: {weather.current_weather.temperature}C");
            Console.WriteLine($"Wind speed: {weather.current_weather.windspeed}km/h");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}