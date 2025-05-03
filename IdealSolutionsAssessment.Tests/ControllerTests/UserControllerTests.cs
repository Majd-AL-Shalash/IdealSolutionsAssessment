using Xunit;
using Moq;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdealSolutionsAssessment.Controllers;
using IdealSolutionsAssessment.Services.Interfaces;
using IdealSolutionsAssessment.DTOs;

public class UserControllerTests
{
    private static UserController CreateUserController(Mock<IUserService> mockService, ClaimsPrincipal? user = null)
    {
        var controller = new UserController(mockService.Object);
        
        var context = new DefaultHttpContext();
        if (user != null)
        {
            context.User = user;
        }
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };
        return controller;
    }

    private static ClaimsPrincipal CreateUser(bool isAdmin)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        }, "mock"));
    }

    [Fact]
    public async Task UpdateUserByAdmin_ReturnsNotFound_IfUserMissing()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDTO>()))
                   .ReturnsAsync(false);

        var adminUser = CreateUser(isAdmin: true);
        var controller = CreateUserController(mockService, adminUser);

        // Act
        var result = await controller.UpdateUserByAdmin(Guid.NewGuid(), new UpdateUserDTO());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateUserByAdmin_ReturnsNoContent_IfSuccess()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDTO>()))
                   .ReturnsAsync(true);

        var adminUser = CreateUser(isAdmin: true);
        var controller = CreateUserController(mockService, adminUser);

        // Act
        var result = await controller.UpdateUserByAdmin(Guid.NewGuid(), new UpdateUserDTO());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}