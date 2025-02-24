using PandaAPI.Utils;

namespace PandaAPI.Test.Unit.Utils
{
    public class DateUtilsTests
    {
        [Fact]
        public void GetDuration_Should_Return_TimeSpan_For_Minutes_Only()
        {
            // Arrange
            string duration = "15m";
            int expectedHours = 0;
            int expectedMinutes = 15;

            // Act
            var result = DateUtils.GetDuration(duration);

            // Assert
            Assert.Equal(expectedHours, result.Hours);
            Assert.Equal(expectedMinutes, result.Minutes);
        }
        
        [Fact]
        public void GetDuration_Should_Return_TimeSpan_For_Hours_Only()
        {
            // Arrange
            string duration = "1h";
            int expectedHours = 1;
            int expectedMinutes = 0;

            // Act
            var result = DateUtils.GetDuration(duration);

            // Assert
            Assert.Equal(expectedHours, result.Hours);
            Assert.Equal(expectedMinutes, result.Minutes);
        }
        
        [Fact]
        public void GetDuration_Should_Return_TimeSpan_For_Hours_and_Minutes()
        {
            // Arrange
            string duration = "1h30m";
            int expectedHours = 1;
            int expectedMinutes = 30;

            // Act
            var result = DateUtils.GetDuration(duration);

            // Assert
            Assert.Equal(expectedHours, result.Hours);
            Assert.Equal(expectedMinutes, result.Minutes);
        }
    }
}