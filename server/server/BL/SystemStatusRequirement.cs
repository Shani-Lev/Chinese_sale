using Microsoft.AspNetCore.Authorization;
using server.Models;
using server.Models.DTO;

namespace server.BL
{
    public class SystemStatusRequirement : IAuthorizationRequirement
    {
        public SystemStatus RequiredStatus { get; }

        public SystemStatusRequirement(SystemStatus requiredStatus)
        {
            RequiredStatus = requiredStatus;
        }
    }
}
