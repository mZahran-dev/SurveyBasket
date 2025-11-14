using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services.Questions;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest Request, CancellationToken cancellationToken);
}
