namespace SurveyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error InvalidQuestion =
        new Error("Vote.InvalidQuestion", "Invalid Question", StatusCodes.Status400BadRequest);

    public static readonly Error DublicatedVote =
        new Error("Vote.DublicatedVote", "User Already Voted", StatusCodes.Status409Conflict);
}

