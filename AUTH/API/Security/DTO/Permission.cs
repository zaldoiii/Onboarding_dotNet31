using Microsoft.AspNetCore.Authorization;

namespace API.Security.DTO
{
    public class Permission : IAuthorizationRequirement
    {
        public Permission(string permissionName)
        {
            PermissionName = permissionName;
        }
        public string PermissionName { get; set; }
    }
}
