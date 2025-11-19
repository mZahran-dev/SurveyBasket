    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SurveyBasket.Services.Questions;
using System.Security.Claims;

namespace SurveyBasket.Controllers;

[Route("api/polls/{pollId}/vote/[controller]")]
[ApiController]
public class VotesController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet("")]
    public async Task<IActionResult> Start(int pollId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _questionService.GetAvailableAsync(pollId, userId!, cancellationToken);

        if(result.IsSuccess)
            return Ok(result.Value);

        return result.Error.Equals(VoteErrors.DublicatedVote)
            ? result.ToProblem()
            : result.ToProblem();
    }
}
