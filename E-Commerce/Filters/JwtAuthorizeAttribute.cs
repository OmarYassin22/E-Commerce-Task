using System.Web.Http.Filters;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using System.Web.Http.Controllers;
using System.Linq;
using NLog;

public class JwtAuthorizeAttribute : AuthorizationFilterAttribute
{
    private readonly string _secretKey = "R&hO6]0=::JC*Yeiww>eYB2[ql>]lFK@4FT>oFNWMy^|d`+Ds%2aZ^~vyF(1G(;";
    private  ILogger _logger { get; set; }
    public JwtAuthorizeAttribute()
    {
        _logger = LogManager.GetCurrentClassLogger(); 

    }

    public override void OnAuthorization(HttpActionContext actionContext)
    {
        var token = actionContext.Request.Headers.Authorization?.Parameter;
        if (string.IsNullOrEmpty(token))
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            return;
        }

        var principal = ValidateToken(token);
        if (principal != null)
        {
            actionContext.RequestContext.Principal = principal;
        }
        else
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = "E-Commerice",
                ValidateAudience = true,
                ValidAudience = "E-Commerice Users",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var subClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var nameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var trr = principal.Claims.ToList();

            if (string.IsNullOrEmpty(subClaim))
            {
                _logger.Error("Sub claim is missing. Token is invalid.");

                return null;
            }

            return principal;
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.Error($"Token has expired: {ex.Message}");
        }
        catch (SecurityTokenValidationException ex)
        {
            _logger.Error($"Token validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Unexpected error during token validation: {ex.Message}");

        }

        return null;
    }

}
