using Moq;
using UrlShortener.BLL.Services.UrlService;
using UrlShortener.DAL.Common.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.UnitOfWork.Interfaces;

namespace UrlShortener.Tests.Services;

public class UrlServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<Url, string>> _urlRepositoryMock;
    private readonly UrlService _urlService;
    public static TheoryData<string, Url> UrlTestData()
    {
        return new TheoryData<string, Url>
        {
            { "testId", new Url { Id = "testId", LongUrl = "https://example.com" } },
            { "invalidId", null }
        };
    }
    public UrlServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _urlRepositoryMock = new Mock<IRepository<Url, string>>();

        _unitOfWorkMock.Setup(uow => uow.GetRepository<Url, string>()).Returns(_urlRepositoryMock.Object);
        _urlService = new UrlService(_unitOfWorkMock.Object);
    }

    [Theory]
    [MemberData(nameof(UrlTestData))]
    public async Task GetUrl_ReturnsExpectedResult(string urlId, Url expectedUrl)
    {
        // Arrange
        var urlService = new UrlService(_unitOfWorkMock.Object);

        _urlRepositoryMock.Setup(repo => repo.GetByIdAsync(urlId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await urlService.GetUrl(urlId);

        // Assert
        Assert.Equal(expectedUrl, result);
    }

    [Theory]
    [InlineData("abc123", "https://example.com")]
    public async Task GetLongUrl_ValidId_ReturnsLongUrl(string urlId, string expectedLongUrl)
    {
        // Arrange

        _urlRepositoryMock.Setup(repo => repo.GetByIdAsync(urlId))
            .ReturnsAsync(new Url { Id = urlId, LongUrl = expectedLongUrl });

        // Act
        var result = await _urlService.GetLongUrl(urlId);

        // Assert
        Assert.Equal(expectedLongUrl, result);
    }

    [Theory]
    [InlineData("abc123", true)]
    [InlineData("nonExistentId", false)]
    public async Task DeleteAsync_ValidUrlId_ReturnsExpectedResult(string urlId, bool expectedResult)
    {
        // Arrange
        var urlService = new UrlService(_unitOfWorkMock.Object);

        if (expectedResult)
            _urlRepositoryMock.Setup(repo => repo.GetByIdAsync(urlId)).ReturnsAsync(new Url { Id = urlId });

        else
            _urlRepositoryMock.Setup(repo => repo.GetByIdAsync(urlId)).ReturnsAsync((Url)null);

        // Act
        var result = await urlService.DeleteAsync(urlId);

        // Assert
        Assert.Equal(expectedResult, result);

        if (expectedResult)
        {
            _urlRepositoryMock.Verify(repo => repo.DeleteAsync(urlId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
        else
        {
            // Verify that neither DeleteAsync nor SaveAsync were called
            _urlRepositoryMock.Verify(repo => repo.DeleteAsync(urlId), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
        }
    }

    [Theory]
    [InlineData("https://example.com", 1, "http://short.com/")]
    public async Task CreateAsync_ReturnsExpectedResult(string longUrl, int userId, string baseUrl)
    {
        // Act
        var result = await _urlService.CreateAsync(longUrl, userId, baseUrl);

        // Assert
        Assert.True(result);
        _urlRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Url>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_UrlExists_DeletesAndReturnsTrue()
    {
        // Arrange
        var existingUrlId = "existingId";
        var existingUrl = new Url { Id = existingUrlId, LongUrl = "https://example.com", ShortUrl = "http://short.com/example", CreatedAt = DateTime.Now };

        _urlRepositoryMock.Setup(repo => repo.GetByIdAsync(existingUrlId)).ReturnsAsync(existingUrl);

        // Act
        var result = await _urlService.DeleteAsync(existingUrlId);

        // Assert
        Assert.True(result);
        _urlRepositoryMock.Verify(repo => repo.DeleteAsync(existingUrlId), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}