namespace Innowise.Clinic.Offices.Services.Exceptions;

public class BlobServiceException : ApplicationException
{
    public BlobServiceException(string message, bool isCritical = false) : base(message)
    {
        IsCritical = isCritical;
    }

    public bool IsCritical { get; }
}