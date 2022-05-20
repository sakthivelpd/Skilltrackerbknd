




namespace SkillTracker.Profile.API;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddApplicationServices(Configuration)
            .AddCustomMvc()
            .AddEventBus(Configuration)
            .AddCosmosDbContext(Configuration)
            .AddCustomSwagger(Configuration)
            .AddHealthChecks(Configuration);


    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        var pathBase = Configuration["PATH_BASE"];
        if (!string.IsNullOrEmpty(pathBase))
        {
            loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            app.UsePathBase(pathBase);
        }

        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Profile.API V1");
            });

        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
        });
        ConfigureAuth(app);

    }
    protected virtual void ConfigureAuth(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}

static class CustomExtensionsMethods
{

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        })
         .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
       // services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
        hcBuilder
                .AddRabbitMQ(
                    $"amqp://{configuration["EventBusConnection"]}",
                    name: "ordering-rabbitmqbus-check",
                    tags: new string[] { "rabbitmqbus" });

        return services;
    }
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        //services.AddScoped(typeof(ISearchService), typeof(SearchService));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BusinessBehavior<,>));
        return services;
    }
    public static IServiceCollection AddCosmosDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var EndpointUri= configuration["CosmosDB:EndpointUri"];
        var Key = configuration["CosmosDB:Key"];
        var ConnectionString = configuration["CosmosDB:ConnectionString"];
        var Database = configuration["CosmosDB:Database"];
        var Container = configuration["CosmosDB:Container"];
        CosmosClient cosmosClient = new CosmosClient(EndpointUri, Key);
        services.AddSingleton<CosmosClient>(cosmosClient);
        Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(Database).Result;
        database.CreateContainerIfNotExistsAsync(Container, "/empId");
        services.AddDbContext<SkillTrackerContext>(option => option.UseCosmos(ConnectionString, Database));

        services.AddScoped<IProfileRepository, ProfileRepository>();
        return services;
    }
    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
       // services.Configure<OrderingSettings>(configuration);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });

        return services;
    }
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SkillTracker - Profile HTTP API",
                Version = "v1",
                Description = "The Profile Service HTTP API"
            });
        });

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"], c =>
                {
                    c.Username(configuration["EventBusSettings:username"]);
                    c.Password(configuration["EventBusSettings:password"]);
                });
            });
        });
        services.AddMassTransitHostedService();
        return services;
    }

}