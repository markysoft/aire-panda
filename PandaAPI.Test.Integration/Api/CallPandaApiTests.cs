using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PandaAPI.Dtos;

namespace PandaAPI.Test.Integration.Api
{
    public class CallPandaApiTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient client;

        public CallPandaApiTests(WebApplicationFactory<Program> factory)
        {
            client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // add your mocks here
                });
                builder.UseEnvironment("Test");
            }).CreateClient();
        }

        [Fact]
        public async Task CAllApiWithValidPatient()
        {
            // Arrange
            var validRequest = new PatientDto()
            {
                NhsNumber = "0091845289",
                Name = "Mr Café Mötorhead",
                DateOfBirth = "2000-03-17",
                Postcode = "SW1A 1AA"
            };

            // Act
            var response = await client.PostAsync("/api/patient", ToJsonContent(validRequest));
            var responseText = await response.Content.ReadAsStringAsync();
            var responsePatient = JsonConvert.DeserializeObject<PatientDto>(responseText);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(responsePatient);
            Assert.Equal(validRequest.NhsNumber, responsePatient.NhsNumber);
            Assert.Equal(validRequest.Name, responsePatient.Name);
            Assert.Equal(validRequest.DateOfBirth, responsePatient.DateOfBirth);
            Assert.Equal(validRequest.Postcode, responsePatient.Postcode);
        }

        private StringContent ToJsonContent(Object validRequest)
        {
            return new StringContent(JsonConvert.SerializeObject(validRequest),
                  Encoding.UTF8, "application/json");
        }
    }
}
