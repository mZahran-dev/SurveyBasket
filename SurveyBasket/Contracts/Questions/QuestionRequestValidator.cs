namespace SurveyBasket.Contracts.Questions;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Content).NotEmpty().Length(3, 1000);
        RuleFor(x => x.Answers).NotNull();

        RuleFor(x => x.Answers)
            .Must(x => x.Count() > 1)
            .WithMessage("At least two answers are required.");

        RuleFor(x => x.Answers)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You cannot add duplicated answers for the same question");




    }
}
