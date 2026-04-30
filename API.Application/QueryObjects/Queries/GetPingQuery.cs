using MediatR;

namespace API.Web.CommandQueryObjects.Queries
{
    public class GetPingQuery : IRequest<string>
    {
        public GetPingQuery() 
        { 
        }
    }

    public class GetPingHandler : IRequestHandler<GetPingQuery, string>
    {
        public Task<string> Handle(GetPingQuery request, CancellationToken cancellationToken)
        {
            string result = "Pong";
            return Task.FromResult(result);
        }
    }
}
