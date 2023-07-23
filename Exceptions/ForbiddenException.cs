namespace shareme_backend.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message, Exception ex)
        : base(message, ex)
    {
    }
}