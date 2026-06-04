using Microsoft.AspNetCore.Authorization;

namespace Advanced.Authorizations
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    {
        public string Permission { get; }
        public PermissionAttribute(string permission) => Permission = permission;

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }
    }
}
