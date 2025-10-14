
using buildingBlock.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pizza_shop.gateway.Middleware
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly JwtSettings _jwtSettings;

        public AuthMiddleware(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(!context.Request.Path.Value!.Contains("signup") && !context.Request.Path.Value.Contains("login") && !context.Request.Path.Value.Contains("catalog/products"))
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    await UnAuthorisedError(context);
                    return;
                }

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (!ValidateToken(token))
                {
                    await UnAuthorisedError(context);
                    return;
                }

            }
            await next(context);
            
        }

        private bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,

                    ValidateAudience = false,
                    //ValidAudience = _jwtSettings.Audience,

                    //ValidAudiences = [],
                    

                    ValidateLifetime = false, // Disable lifetime validation
                    ClockSkew = TimeSpan.Zero
                };
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task UnAuthorisedError(HttpContext context)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorised",
                Detail = "Unauthorised request",
                Type = ""
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}