using User.Api.Common;

namespace User.Api.Features.User;

[ExcludeFromCodeCoverage]
internal static class Errors
{
    internal static Error ReturnInvalidEntriesError(string errorDetails) => new(errorCode: "US001",
        errorMessage: "Invalid entries", errorDetails);

    internal static Error ReturnUserNotFoundError() => new(errorCode: "US002",
        errorMessage: "User not found");

    internal static Error ReturnTransactionError(string errorDetails) => new(errorCode: "US003",
       errorMessage: "Transaction error", errorDetails);
}