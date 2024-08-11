using System.Net;
using Microsoft.AspNetCore.Mvc;
using ResultsPatternMinimalApis.Models.ResultsTypes;

namespace ResultsPatternMinimalApis;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static T Match<T>(
        this FluentResults.Result result,
        Func<T> onSuccess,
        Func<T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure();
    }
    
    

}