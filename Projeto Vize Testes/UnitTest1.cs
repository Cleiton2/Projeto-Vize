using Projeto_Vize.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Projeto_Vize_Testes
{
    public class UnitTest1
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7133/api/Produtos") };

        [Fact]
        public async Task GivenARequest_WhenCallingGetBooks_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange.
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedContent = new ProdutoModel();
            var stopwatch = Stopwatch.StartNew();
            // Act.
            var response = await _httpClient.GetAsync("/ObtenhaProdutos");
            // Assert.
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }
    }

    public static class TestHelpers
    {
        private static readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7133") };

        private const string _jsonMediaType = "application/json";
        private const int _expectedMaxElapsedMilliseconds = 1000;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public static async Task AssertResponseWithContentAsync<T>(Stopwatch stopwatch,
    HttpResponseMessage response, HttpStatusCode expectedStatusCode,
    T expectedContent)
        {
            AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
            Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(expectedContent, await JsonSerializer.DeserializeAsync<T?>(
                await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions));
        }

        private static void AssertCommonResponseParts(Stopwatch stopwatch,
            HttpResponseMessage response, HttpStatusCode expectedStatusCode)
        {
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds);
        }

        public static StringContent GetJsonStringContent<T>(T model)
            => new(JsonSerializer.Serialize(model), Encoding.UTF8, _jsonMediaType);

        public static void Dispose()
        {
            _httpClient.DeleteAsync("/state").GetAwaiter().GetResult();
        }
    }
}