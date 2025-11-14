namespace SurveyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new Error("poll.NotFound", "no Poll was Found for the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DublicatedPoll =
        new Error("poll.DublicatedPoll", "Another Poll with the Same Title", StatusCodes.Status409Conflict);
}

