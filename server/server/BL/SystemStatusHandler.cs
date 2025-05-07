using Microsoft.AspNetCore.Authorization;
using server.DAL;
using server.Models.DTO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace server.BL
{
    public class SystemStatusHandler : AuthorizationHandler<SystemStatusRequirement>
    {
        private readonly PDbContext _context;

        public SystemStatusHandler(PDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SystemStatusRequirement requirement)
        {
            var userStatus = await _context.Statuse
                .OrderByDescending(s => s.UpdateTo)
                .FirstOrDefaultAsync();

            var userStatusText = userStatus?.Text;

            if (Enum.TryParse(typeof(SystemStatus), userStatusText, true, out var status))
            {
                if ((SystemStatus)status == requirement.RequiredStatus)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
