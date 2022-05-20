namespace SkillTracker.Services.Profile.API.Application.Behaviors;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
   
        _logger.LogInformation("[START] {CommandName} ({@Command})", request.GetType().Name, request);
        var response = await next();
           _logger.LogInformation("[END] {CommandName} handled - response: {@Response}", request.GetType().Name, response);

            return response;
        }
    }
