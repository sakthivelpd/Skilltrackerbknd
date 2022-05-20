namespace SkillTracker.Services.Profile.API.Application.Behaviors;
public class BusinessBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is AddProfileCommand)
            {
                var commnd = request as AddProfileCommand;
                if (!commnd.EmpId.ToUpper().StartsWith("CTS"))
                {
                    commnd.EmpId = "CTS" + commnd.EmpId;
                }
            }
        if (request is UpdateProfileCommand)
        {
            var commnd = request as UpdateProfileCommand;
            if (!commnd.EmpId.ToUpper().StartsWith("CTS"))
            {
                commnd.EmpId = "CTS" + commnd.EmpId;
            }
        }
        return await next();
        }
    }
