namespace SurveyBasket.Errors;

public static class VoteErrors
{
    //public static readonly Error VoteNotFound =
    //    new Error("poll.NotFound", "no Poll was Found for the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DublicatedVote =
        new Error("Vote.DublicatedVote", "User Already Voted", StatusCodes.Status409Conflict);
}

