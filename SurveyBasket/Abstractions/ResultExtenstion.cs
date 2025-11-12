using System.Runtime.CompilerServices;

namespace SurveyBasket.Abstractions;

public static class ResultExtenstion
{
    public static ObjectResult ToProblem(this Result result, int statusCode)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("cannot convert Success result to Problem");

        var problem = Results.Problem(statusCode: statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "error", new[] { result.Error}
            }
        };
        return new ObjectResult(problemDetails);
    }
}
