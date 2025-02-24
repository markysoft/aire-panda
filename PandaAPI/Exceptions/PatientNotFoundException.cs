namespace PandaAPI.Exceptions
{
    public class PatientNotFoundException(string message) : PandaApiException(message);
}