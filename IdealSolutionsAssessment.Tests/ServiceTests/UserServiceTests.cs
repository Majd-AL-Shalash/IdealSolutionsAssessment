using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Models;
using IdealSolutionsAssessment.Repositories.Interfaces;
using IdealSolutionsAssessment.Services.Implementations;
using Moq;

public class UserServiceTests
{
    [Fact]
    public async Task UpdateTaskAsync_ReturnsFalse_IfUserNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        var service = new UserService(mockRepo.Object);
        var id = Guid.NewGuid();
        var dto = new UpdateUserDTO();

        mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User)null);

        // Act
        var result = await service.UpdateAsync(id, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateTaskAsync_ReturnsTrue_IfUserFound()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        var service = new UserService(mockRepo.Object);
        var id = Guid.NewGuid();
        var dto = new UpdateUserDTO();

        mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new User());

        // Act
        var result = await service.UpdateAsync(id, dto);

        // Assert
        Assert.True(result);
    }
}