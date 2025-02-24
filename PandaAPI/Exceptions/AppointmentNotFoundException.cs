namespace PandaAPI.Exceptions
{
    public class AppointmentNotFoundException(string message) : PandaApiException(message);
}