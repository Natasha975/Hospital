using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(AppAPI.Startup))]
namespace AppAPI
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
			var audience = ConfigurationManager.AppSettings["JwtAudience"];
			var secret = ConfigurationManager.AppSettings["JwtSecret"];

			app.UseJwtBearerAuthentication(
				new JwtBearerAuthenticationOptions
				{
					AuthenticationMode = AuthenticationMode.Active,
					AllowedAudiences = new[] { audience },
					TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = issuer,
						ValidateAudience = true,
						ValidAudience = audience,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
						ValidateLifetime = true
					}
				});
		}
	}
}