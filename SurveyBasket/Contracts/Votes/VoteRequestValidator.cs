namespace SurveyBasket.Contracts.Votes;

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(x => x.VoteAnswers).NotEmpty();

        RuleForEach(x => x.VoteAnswers).SetInheritanceValidator(V => V.Add(new VoteAnswerRequestValidator()));
    }
}
