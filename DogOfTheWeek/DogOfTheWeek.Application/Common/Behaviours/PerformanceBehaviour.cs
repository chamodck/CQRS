using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    //private readonly ICurrentUserService _currentUserService;

    public PerformanceBehaviour(
        ILogger<TRequest> logger
        //, ICurrentUserService currentUserService
        )
    {
        this._timer = new Stopwatch();

        this._logger = logger;
        //this._currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = ""; //_currentUserService.UserId ?? string.Empty;
            var userName = "";// _currentUserService.UserId ?? string.Empty;


            _logger.LogWarning("ALFI API Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
    requestName, elapsedMilliseconds, userId, userName, request);
        }

        return response;
    }
}
