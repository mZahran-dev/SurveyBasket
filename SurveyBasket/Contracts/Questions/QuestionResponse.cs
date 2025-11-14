using SurveyBasket.Contracts.Answers;

namespace SurveyBasket.Contracts.Questions;

public record QuestionResponse
(
    string Content,
    int Id,
    IEnumerable<AnswerResponse> Answers
);