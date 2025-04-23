namespace AFSTranslator.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<HttpContext> _httpContextMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<IAuthenticationService> _authenticationServiceMock = new();

        public AuthServiceTests()
        {   
            _authService = new AuthService(_userRepoMock.Object, _tokenServiceMock.Object, _httpContextAccessorMock.Object, _passwordHasherMock.Object);
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("username", "")]
        [InlineData(" ", "password")]
        [InlineData("username", " ")]
        [InlineData(null, "password")]
        [InlineData("username", null)]
        public async Task Register_ShouldFail_ForInvalidInputs(string username, string password)
        {   
            var result = await _authService.Register(username, password);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Register_ShouldFail_WhenUsernameAlreadyExists()
        {
            _userRepoMock.Setup(x => x.GetByUsernameAsync("existingUser")).ReturnsAsync(new User());

            var result = await _authService.Register("existingUser", "securePassword");

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Username taken");
        }

        [Fact]
        public async Task Register_ShouldSucceed_WithValidInputs()
        {
            _userRepoMock.Setup(x => x.GetByUsernameAsync("newUser")).ReturnsAsync(default(User));

            _passwordHasherMock.Setup(x => x.Hash("password")).Returns("hashedPassword");

            _userRepoMock.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(true);

            var result = await _authService.Register("newUser", "password");

            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Contain("Successful request");
        }

        [Fact]
        public async Task Login_ShouldFail_IfUserNotFound()
        {
            _userRepoMock.Setup(x => x.GetByUsernameAsync("missingUser")).ReturnsAsync(default(User));

            var result = await _authService.Login("missingUser", "anyPassword");

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Invalid username or password");
        }

        [Fact]
        public async Task Login_ShouldFail_IfPasswordIsIncorrect()
        {
            _passwordHasherMock.Setup(x => x.Verify("hashed", "wrongPassword")).Returns(false);

            _userRepoMock.Setup(x => x.GetByUsernameAsync("validUser")).ReturnsAsync(new User { Username = "validUser", PasswordHash = "hashed" });

            var result = await _authService.Login("validUser", "wrongPassword");

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Invalid username or password");
        }

        [Fact]
        public async Task Login_ShouldSucceed_WithValidCredentials()
        {
            User user = new () { Username = "validUser", PasswordHash = "hashedPassword" };

            _userRepoMock.Setup(x => x.GetByUsernameAsync("validUser")).ReturnsAsync(user);

            _passwordHasherMock.Setup(x => x.Verify("hashedPassword", "password")).Returns(true);

            _httpContextMock.Setup(x => x.RequestServices.GetService(typeof(IAuthenticationService))).Returns(_authenticationServiceMock.Object);

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(_httpContextMock.Object);

            _authenticationServiceMock.Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(),
                It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);

            var result = await _authService.Login("validUser", "password");

            result.IsSuccess.Should().BeTrue();
            result.Content.Should().Be("Login successful");
            result.Message.Should().Be("Successful request");
        }

        [Fact] 
        public async Task Logout_ShouldSucceed_AndClearCookie()
        {
            var responseMock = new Mock<HttpResponse>();
            var cookiesMock = new Mock<IResponseCookies>();

            responseMock.Setup(r => r.Cookies).Returns(cookiesMock.Object);
            _httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);
            _httpContextMock.Setup(c => c.RequestServices.GetService(typeof(IAuthenticationService)))
                        .Returns(_authenticationServiceMock.Object);

            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(_httpContextMock.Object);

            _authenticationServiceMock.Setup(x => x.SignOutAsync(_httpContextMock.Object, "MyCookieAuth", null)).Returns(Task.CompletedTask);

            var result = await _authService.Logout();

            result.IsSuccess.Should().BeTrue();
            _authenticationServiceMock.Verify(x => x.SignOutAsync(_httpContextMock.Object, "MyCookieAuth", null), Times.Once);
        }
    }
}