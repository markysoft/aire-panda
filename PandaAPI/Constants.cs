namespace PandaAPI
{
    public static class Constants
    {
        public const string Cancelled = "cancelled";
        public const string Active = "active";
        public const string Attended = "attended";
        public const string Missed = "missed";
        public const string StatusRegex = $"^({Active}|{Attended}|{Cancelled})$";
        public const string StatusErrorMessage = $"Status must be '{Active}', '{Attended}', or '{Cancelled}'.";
    }
}
