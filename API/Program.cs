using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.Extensions.NETCore.Setup;
using API.AutoMapper;
using DotNetEnv;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Load environment variables from .env file
        Env.Load();

        //Retrieve AWS credentials from environment variables
        var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        var awsRegion = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");

        //Validate credentials
        if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(awsRegion))
        {
            throw new InvalidOperationException("AWS credentials are missing. Please check your .env file.");
        }

        //Configure AWS credentials
        var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
        var awsRegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion);

        //Register AWS SDK services
        builder.Services.AddSingleton(credentials);
        builder.Services.AddSingleton(awsRegionEndpoint);

        // Add DynamoDB
        builder.Services.AddAWSService<IAmazonDynamoDB>();
        builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        var summaries = new[]
        {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

        app.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        app.Run();
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
