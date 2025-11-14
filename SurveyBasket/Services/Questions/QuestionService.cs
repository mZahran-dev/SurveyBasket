using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services.Questions;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest Request, CancellationToken cancellationToken)
    {
        var PollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId);

        if (!PollIsExists)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsExists = await _context.Questions
            .AnyAsync(Q => Q.Content == Request.Content && Q.PollId == pollId, cancellationToken);

        if (!questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);


        var question = Request.Adapt<Question>();
        question.PollId = pollId;

        Request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer}));

        await _context.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }
}
