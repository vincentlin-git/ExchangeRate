namespace RateProviders.Exceptions;

public class RateProviderException : Exception
{
    public RateProviderException(string message) : base(message)
    {
    }

    public RateProviderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}