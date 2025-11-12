namespace SurveyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound = new Error("poll.NotFound", "no Poll was Found for the given ID"); 
}

