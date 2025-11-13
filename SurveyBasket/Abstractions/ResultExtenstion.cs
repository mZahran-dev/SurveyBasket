using System.Runtime.CompilerServices;

namespace SurveyBasket.Abstractions;

public static class ResultExtenstion
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("cannot convert Success result to Problem");

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "error", new[] {
                    new
                    {
                        result.Error.Code,
                        result.Error.Description,
                    }
                }
            }
        };
        return new ObjectResult(problemDetails);
    }
}
