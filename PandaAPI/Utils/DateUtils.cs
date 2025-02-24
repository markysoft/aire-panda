namespace PandaAPI.Utils
{
    public class DateUtils
    {
        public static TimeSpan GetDuration(string duration)
        {
            var hours = 0;
            var minutes = 0;
            
            if (duration.Contains("h") && duration.Contains("m"))
            {
                var durationArray = duration.Split('h', 'm');
                hours = int.Parse(durationArray[0]);
                minutes = int.Parse(durationArray[1]);
            }
            else if (duration.Contains("h"))
            {
                hours = int.Parse(duration.Split('h')[0]);
            }
            else if (duration.Contains("m"))
            {
                minutes = int.Parse(duration.Split('m')[0]);
            }
            return new TimeSpan(hours, minutes, 0);
        }

        public static string FormatDuration(TimeSpan duration)
        {
            var hours = duration.Hours;
            var minutes = duration.Minutes;

            if (hours > 0 && minutes > 0)
            {
                return $"{hours}h{minutes}m";
            }

            if (hours > 0)
            {
                return $"{hours}h";
            }

            if (minutes > 0)
            {
                return $"{minutes}m";
            }

            return "0m";
        }
    }
}
