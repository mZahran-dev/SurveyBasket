using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services.Questions;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest Request, CancellationToken cancellationToken);
    Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId,CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> GetAsync(int pollId,int questionId,CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest Request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);
}
