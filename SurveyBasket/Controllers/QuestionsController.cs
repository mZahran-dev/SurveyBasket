using SurveyBasket.Contracts.Questions;
using SurveyBasket.Services.Questions;

namespace SurveyBasket.Controllers;

[Route("api/polls/{PollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int PollId, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAllAsync(PollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] int PollId, [FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAsync(PollId, Id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int PollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(PollId, request, cancellationToken);

        return result.IsSuccess
               ? CreatedAtAction(nameof(Get), new { PollId, result.Value.Id }, result.Value)
               : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/toggleStatus")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
