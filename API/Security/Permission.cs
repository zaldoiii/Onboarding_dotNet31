using Microsoft.AspNetCore.Authorization;

namespace API.Security
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
