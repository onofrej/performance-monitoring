namespace User.Api.Common;

[ExcludeFromCodeCoverage]
public readonly struct Error(string errorCode,
    string errorMessage,
    string? errorDetails = default) : IEquatable<Error>
{
    public string ErrorCode { get; } = errorCode;

    public string ErrorMessage { get; } = errorMessage;

    public string? ErrorDetails { get; } = errorDetails;

    public static bool operator !=(Error left, Error right)
    {
        return !(left == right);
    }

    public static bool operator ==(Error left, Error right)
    {
        return left.Equals(right);
    }

    public readonly bool Equals(Error other)
    {
        return ErrorCode == other.ErrorCode &&
            ErrorMessage == other.ErrorMessage;
    }

    public override bool Equals(object? obj)
    {
        return obj is Error error && Equals(error);
    }

    public override readonly int GetHashCode()
    {
        return ErrorCode.GetHashCode();
    }
}