namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new Error("Question.NotFound", "no Question was Found for the given ID", StatusCodes.Status404NotFound);

    public static readonly Error QuestionDublicatedContent =
        new Error("Question.DublicatedContent", "Another Question with the Same Content", StatusCodes.Status409Conflict);
}

