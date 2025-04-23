using AFSTranslator.Entities;
using AFSTranslator.Entities.Common;
using AFSTranslator.Entities.Responses;

namespace AFSTranslator.Tests
{
    public class FunTranslationServiceTests
    {
        private const string _mode = "LeetSpeak";
        private const string _originalText = "Hello there!";
        private const string _translatedText = "Th3r3, hello!";

        private readonly FunTranslationService _service;
        private readonly Mock<IConfiguration> _configMock = new();
        private readonly Mock<IRestService> _restServiceMock = new();
        private readonly Mock<IConfigurationSection> _sectionMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<ITranslationLogService> _translationLogServiceMock = new();

        public FunTranslationServiceTests()
        {
            _sectionMock.Setup(x => x.Value).Returns("https://api.funtranslations.com/translate/*");
            _configMock.Setup(c => c.GetSection("TranslationSettings:FunTranslation:BaseUrl")).Returns(_sectionMock.Object);

            List<Claim> claims = [new(ClaimTypes.PrimarySid, "1")];
            ClaimsIdentity identity = new(claims);
            ClaimsPrincipal principal = new(identity);
            DefaultHttpContext httpContext = new() { User = principal };

            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _service = new FunTranslationService(_restServiceMock.Object, _configMock.Object, _translationLogServiceMock.Object, _httpContextAccessorMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Translate_ShouldFail_WhenInputTextIsInvalid(string input)
        {
            var result = await _service.Translate("yoda", input);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Translate_ShouldReturnTranslatedText_AndLogTranslation()
        {
            var apiResult = new Result<FunTranslationResponse>
            {
                Content = new FunTranslationResponse
                {
                    Contents = new Contents
                    {
                        Translated = _translatedText,
                        Text = _originalText,
                        Translation = _mode
                    },
                    Success = new Success
                    {
                        Total = 1
                    }
                }
            };

            _restServiceMock.Setup(r => r.MakeRequest<FunTranslationResponse>(It.IsAny<string>(), ApiType.Post, It.IsAny<object>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(apiResult);

            var result = await _service.Translate(_mode, _originalText);

            result.IsSuccess.Should().BeTrue();
            result.Content.Should().Be(_translatedText);

            _translationLogServiceMock.Verify(t => t.LogTranslation(It.Is<TranslationLog>(log =>
                log.UserId == 1 &&
                log.Mode == _mode &&
                log.OriginalText == _originalText &&
                log.TranslatedText == _translatedText
            )), Times.Once);
        }

        [Fact]
        public async Task Translate_ShouldReturnError_WhenApiCallFails()
        {
            // Arrange
            var failedApiResult = new Result<FunTranslationResponse>
            {
                ErrorMessage = "An error occurred while processing the request"
            };

            _restServiceMock.Setup(r => r.MakeRequest<FunTranslationResponse>(
                It.IsAny<string>(), ApiType.Post, It.IsAny<object>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(failedApiResult);

            var result = await _service.Translate(_mode, _originalText);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("An error occurred while processing the request");
            result.Content.Should().BeNull();

            _translationLogServiceMock.Verify(t => t.LogTranslation(It.IsAny<TranslationLog>()), Times.Never);
        }
    }
}