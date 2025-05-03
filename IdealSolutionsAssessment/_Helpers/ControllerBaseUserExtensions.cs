using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdealSolutionsAssessment.Helpers;

public static class ControllerBaseUserExtensions
{
    public static Guid GetUserId(this ControllerBase controller)
    {
        var userIdClaim = controller.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim?.Value, out var userId) ? userId : throw new Exception("controller.GetUserId");
    }

    public static bool IsAdmin(this ControllerBase controller)
    {
        return controller.User.IsInRole("Admin") ;
    }
}
