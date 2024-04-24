using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
/*using System.Data;*/
using Microsoft.AspNetCore.Http;

namespace Datateam.Security
{
    public class IAMService : IIAMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
/*        private readonly SecurityDbContext _securityDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;*/
        private readonly IConfiguration _config;

        public IAMService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            /*_securityDbContext = securityDbContext;
            _httpContextAccessor = httpContextAccessor;*/
            _config = config;
        }

        public async Task<bool> RegisterUser(RegisterUser user)
        {
            if(user is null)
            {
                var superSecurityAdminRole = new IdentityRole("SuperSecurityAdmin");
                var securityAdminRole = new IdentityRole("SecurityAdmin");
                var superTenantsAdminRole = new IdentityRole("SuperTenantsAdmin");
                var tenantsAdminRole = new IdentityRole("TenantsAdmin");
                var superOrgsAdminRole = new IdentityRole("SuperOrgsAdmin");
                var orgsAdminRole = new IdentityRole("OrgsAdmin");

                await _roleManager.CreateAsync(superSecurityAdminRole);
                await _roleManager.CreateAsync(securityAdminRole);
                await _roleManager.CreateAsync(superTenantsAdminRole);
                await _roleManager.CreateAsync(tenantsAdminRole);
                await _roleManager.CreateAsync(superOrgsAdminRole);
                await _roleManager.CreateAsync(orgsAdminRole);

                var identitySuperSecurityAdmin = new ApplicationUser
                {
                    UserName = "SuperSecurityAdmin@Datateam.com",
                    Email = "SuperSecurityAdmin@Datateam.com",
                    TenantId = Guid.Empty,
                    OrganisationId = Guid.Empty
                };
                var identitySecurityAdmin = new ApplicationUser
                {
                    UserName = "SecurityAdmin@Datateam.com",
                    Email = "SecurityAdmin@Datateam.com",
                    TenantId = Guid.Empty,
                    OrganisationId = Guid.Empty
                };

                var resultSuperSecurityAdmin = await _userManager.CreateAsync(identitySuperSecurityAdmin, "SuperSecret1!");
                var resultSecurityAdmin = await _userManager.CreateAsync(identitySecurityAdmin, "SuperSecret1!");

                await _userManager.AddToRoleAsync(identitySuperSecurityAdmin, "SuperSecurityAdmin");
                await _userManager.AddToRoleAsync(identitySecurityAdmin, "SecurityAdmin");

                return resultSuperSecurityAdmin.Succeeded && resultSecurityAdmin.Succeeded;
            } else
            {
                var identitySuperTenantAdmin = new ApplicationUser
                {
                    UserName = user.Name + "SuperTenantAdmin@Datateam.com",
                    Email = user.Name + "SuperTenantAdmin@Datateam.com",
                    TenantId = user.TenantId,
                    OrganisationId = new Guid("00000000-0000-0000-0000-000000000000")
                };
                var identityTenantAdmin = new ApplicationUser
                {
                    UserName = user.Name + "TenantAdmin@Datateam.com",
                    Email = user.Name + "TenantAdmin@Datateam.com",
                    TenantId = user.TenantId,
                    OrganisationId = new Guid("00000000-0000-0000-0000-000000000000")
                };
                var resultSuperTenantAdmin = await _userManager.CreateAsync(identitySuperTenantAdmin, "SuperSecret1!");
                var resultTenantAdmin = await _userManager.CreateAsync(identityTenantAdmin, "SuperSecret1!");

                await _userManager.AddToRoleAsync(identitySuperTenantAdmin, "SuperTenantsAdmin");
                await _userManager.AddToRoleAsync(identityTenantAdmin, "TenantsAdmin");

                return resultSuperTenantAdmin.Succeeded && resultTenantAdmin.Succeeded;
            }
            

            /*var currentUserRole = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            if (currentUserRole == "Superadmin" && (user.Role == "Orgadmin" || user.Role == "Superadmin"))
            {
                var identityUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.UserName
                };

                if (user.Role == "Orgadmin") {
                    if (user.OrganisationName is not null || user.OrganisationName != string.Empty)
                    {
                        var organisation = _securityDbContext.Organisations.Where(o => o.OrganisationName == user.OrganisationName).FirstOrDefault();
                        if (organisation is null)
                        {
                            Organisation newOrganisation = new();
                            newOrganisation.OrganisationName = user.OrganisationName;
                            _securityDbContext.Organisations.Add(newOrganisation);
                            await _securityDbContext.SaveChangesAsync();
                            organisation = _securityDbContext.Organisations.Where(o => o.OrganisationName == user.OrganisationName).FirstOrDefault();

                        }
                        identityUser.Organisation = organisation;
                    }
                }

                var result = await _userManager.CreateAsync(identityUser, user.Password);
                await _userManager.AddToRoleAsync(identityUser, user.Role);
                return result.Succeeded;
            }
            
            return false;*/
        }

        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);
            if (identityUser is null)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(identityUser);

            foreach (var role in roles)
            {
                user.Role = role;
                break;
            }


            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public string GenerateTokenString(LoginUser user)
        {
            var claims = new List<Claim>();

            if (user.UserName is not null)
            {
                Claim EmailClaim = new Claim(ClaimTypes.Email, user.UserName);
                claims.Add(EmailClaim);
            }
            
            if (user.Role is not null)
            {
                Claim RoleClaim = new Claim(ClaimTypes.Role, user.Role);
                claims.Add(RoleClaim);
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection
                ("Jwt:Key").Value));

            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            SecurityToken securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        /*public async Task<ClaimsPrincipal> GenerateCookie(LoginUser user) {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuthenticationScheme");
            var principal = new ClaimsPrincipal(identity);
            return principal;

        }*/
    }
}
