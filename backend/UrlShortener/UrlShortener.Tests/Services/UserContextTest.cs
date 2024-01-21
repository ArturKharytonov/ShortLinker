using System.Security.Claims;
using UrlShortener.BLL.Services.UserContext;

namespace UrlShortener.Tests.Services;

public class UserContextTest
{
    public static IEnumerable<object[]> UserIdTestData()
    {
        yield return new object[] { new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.NameIdentifier, "123") })) };
        yield return new object[] { new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.NameIdentifier, "456") })) };
        yield return new object[] { new ClaimsPrincipal(new ClaimsIdentity()) }; // For the case when there's no NameIdentifier claim
    }
    public static IEnumerable<object[]> UserRoleTestData()
    {
        yield return new object[] { new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, "admin") })) };
        yield return new object[] { new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, "user") })) };
        yield return new object[] { new ClaimsPrincipal(new ClaimsIdentity()) }; // For the case when there's no Role claim
    }

    [Theory]
    [MemberData(nameof(UserIdTestData))]
    public void GetUserId_ShouldReturnUserId_WhenUserHasNameIdentifierClaim(ClaimsPrincipal user)
    {
        // Arrange
        var userContext = new UserContext(user);

        // Act
        var result = userContext.GetUserId();

        // Assert
        if (user.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            Assert.Equal(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, result);

        else
            Assert.Null(result);

    }

    [Theory]
    [MemberData(nameof(UserRoleTestData))]
    public void GetUserRole_ShouldReturnUserRole(ClaimsPrincipal user)
    {
        // Arrange
        var userContext = new UserContext(user);

        // Act
        var result = userContext.GetUserRole();

        // Assert
        if (user.HasClaim(c => c.Type == ClaimTypes.Role))
            Assert.Equal(user.FindFirst(ClaimTypes.Role)?.Value, result);

        else
            Assert.Null(result);

    }
}