using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.Extensions.NETCore.Setup;
using API.AutoMapper;
using DotNetEnv;
using API.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        Env.Load();
        // Retrieve credentials and region from environment
        var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        var awsRegion = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");

        Console.WriteLine(awsRegion);
        Console.WriteLine(awsSecretKey);
        Console.WriteLine(awsAccessKey);

        if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(awsRegion))
        {
            throw new InvalidOperationException("AWS credentials or region are missing.");
        }

        // Configure AWS options
        var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
        var region = RegionEndpoint.GetBySystemName(awsRegion);

        builder.Services.AddDefaultAWSOptions(new AWSOptions
        {
            Credentials = credentials,
            Region = region
        });

        builder.Services.AddAWSService<IAmazonDynamoDB>();
        builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        builder.Services.AddControllers();


        // Register the IStateRepository with its implementation
        builder.Services.AddScoped<IStateRepository, StateRepository>();

        // Register AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));

     
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

        app.MapControllers();

        app.Run();
    }
}

