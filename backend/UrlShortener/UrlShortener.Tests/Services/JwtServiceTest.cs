using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using UrlShortener.BLL.Services.JwtService;

namespace UrlShortener.Tests.Services;

public class JwtServiceTest
{
    public class JwtServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly JwtService _jwtService;

        public JwtServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();

            _jwtService = new JwtService(_configurationMock.Object);
        }

        [Theory]
        [InlineData(123, "testuser", "user", "your-secret-key-test-1234567-890", "your-issuer", "your-audience")]
        public void GetToken_ShouldGenerateValidJwtToken(int userId, string username, string role, string key, string issuer, string audience)
        {
            // Arrange
            _configurationMock.SetupGet(x => x["Jwt:Key"]).Returns(key);
            _configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns(issuer);
            _configurationMock.SetupGet(x => x["Jwt:Audience"]).Returns(audience);

            // Act
            var token = _jwtService.GetToken(userId, username, role);

            // Assert
            Assert.NotNull(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            Assert.NotNull(jwtToken);
            Assert.Equal(issuer, jwtToken.Issuer);
            Assert.Equal(audience, jwtToken.Audiences.FirstOrDefault());

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            Assert.NotNull(userIdClaim);
            Assert.Equal(userId.ToString(), userIdClaim.Value);

            Assert.NotNull(usernameClaim);
            Assert.Equal(username, usernameClaim.Value);
        }
    }
}