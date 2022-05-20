namespace SkillTracker.Services.Admin.API.Application.Behaviors;
public class BusinessBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
          where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is SearchProfileQuery)
            {
                var query = request as SearchProfileQuery;
                if (!string.IsNullOrEmpty(query.EmpId) &&
                    !query.EmpId.ToUpper().StartsWith("CTS"))
                {
                    query.EmpId = "CTS" + query.EmpId;
                }
            }
            return await next();
        }
    }

