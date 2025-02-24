namespace PandaAPI.Exceptions
{
    public class AppointmentExistsException(string message) : PandaApiException(message);
}