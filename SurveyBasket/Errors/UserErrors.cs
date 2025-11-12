using SurveyBasket.Abstractions;

namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials = new Error("User.InvalidCredentials", "Invalid Email/Password");
    public static readonly Error InvalidJwtToken =
    new("User.InvalidJwtToken", "Invalid Jwt token");

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token");
}

