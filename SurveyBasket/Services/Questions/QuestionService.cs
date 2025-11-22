using SurveyBasket.Contracts.Answers;
using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services.Questions;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId,CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
        
        if (!pollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);


        var questions = await _context.Questions
            .Where(Q => Q.PollId == pollId)
            .Include(x => x.Answers)
            //.Select(q => new QuestionResponse(
            //     q.Id,
            //    q.Content,
            //    q.Answers.Select(a=> new AnswerResponse(a.Id,a.Content))
            //))
            .ProjectToType<QuestionResponse>() // better performance than Select
            .AsNoTracking()
            .ToListAsync(cancellationToken);



        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result<QuestionResponse>> GetAsync([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Where(Q => Q.PollId == pollId & Q.Id == questionId)
            .Include(x => x.Answers)
            .ProjectToType<QuestionResponse>() // better performance than Select
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if(question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success(question);
    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);
        if (hasVote)
            return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DublicatedVote);

        var PollIsExists = await _context.Polls
                .AnyAsync(p => p.Id == pollId && p.IsPublished 
                && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        if (!PollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);


        var question = await _context.Questions
            .Where(Q => Q.PollId == pollId && Q.IsActive)
            .Include(x => x.Answers).Select(q => new QuestionResponse(
                 q.Id,
                 q.Content,
                 q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
            )).AsNoTracking().ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionResponse>>(question);
    }
    public async Task<Result<QuestionResponse>> AddAsync([FromRoute] int pollId, [FromBody] QuestionRequest Request, CancellationToken cancellationToken)
    {
        var PollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId);

        if (!PollIsExists)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsExists = await _context.Questions
            .AnyAsync(Q => Q.Content == Request.Content && Q.PollId == pollId, cancellationToken);

        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);


        var question = Request.Adapt<Question>();
        question.PollId = pollId;

        await _context.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var questionIsExists = await _context.Questions
            .AnyAsync(x => x.PollId == pollId
                && x.Id != id
                && x.Content == request.Content,
                cancellationToken
            );

        if (questionIsExists)
            return Result.Failure(QuestionErrors.QuestionDublicatedContent);

        var question = await _context.Questions
            .Include(x => x.Answers)
            .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.Content = request.Content;

        //current answers
        var currentAnswers = question.Answers.Select(x => x.Content).ToList();

        //add new answer
        var newAnswers = request.Answers.Except(currentAnswers).ToList();

        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.SingleOrDefaultAsync(x => x.PollId == x.Id, cancellationToken: cancellationToken); ;

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
