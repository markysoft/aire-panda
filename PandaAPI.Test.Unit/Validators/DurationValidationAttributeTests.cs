using System.ComponentModel.DataAnnotations;
using PandaAPI.Validators;

namespace PandaAPI.Test.Unit.Validators
{
    public class DurationValidationAttributeTests
    {
        private readonly DurationValidationAttribute durationValidationAttribute = new DurationValidationAttribute();

        [Theory]
        [InlineData("1h", true)]
        [InlineData("15m", true)]
        [InlineData("1h15m", true)]
        [InlineData("59m", true)]
        [InlineData("60m", false)]
        [InlineData("1h60m", false)]
        [InlineData("1hour", false)]
        [InlineData("1h15", false)]
        [InlineData("h15m", false)]
        [InlineData("1hm", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void DurationValidationAttribute_ShouldValidateTimesCorrectly(string duration, bool expectedIsValid)
        {
            // Arrange
            var validationContext = new ValidationContext(new { });

            // Act
            var result = durationValidationAttribute.GetValidationResult(duration, validationContext);

            // Assert
            if (expectedIsValid)
            {
                Assert.Equal(ValidationResult.Success, result);
            }
            else
            {
                Assert.NotEqual(ValidationResult.Success, result);
            }
        }
    }
}