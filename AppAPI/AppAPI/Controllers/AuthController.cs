using AppAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace AppAPI.Controllers
{
	[RoutePrefix("api/auth")]
	public class AuthController : ApiController
    {
		private readonly БольницаEntities db = new БольницаEntities();

		/// <summary>
		/// Аутентификация пользователя и получение JWT токена
		/// </summary>
		/// <param name="login">Данные для входа (логин и пароль)</param>
		/// <returns>JWT токен и информация о пользователе</returns>
		/// <response code="200">Возвращает JWT токен</response>
		/// <response code="400">Неверные параметры запроса</response>
		/// <response code="401">Неверные учетные данные</response>
		[HttpPost]
		[Route("login")]
		[AllowAnonymous]
		public IHttpActionResult Login(LoginModel login)
		{
			if (!ModelState.IsValid)
				return BadRequest("Неверные учетные данные");

			var user = AuthenticateUser(login.Username, login.Password);

			if (user == null)
				return Unauthorized();

			var token = GenerateJwtToken(user);

			return Ok(new
			{
				Token = token,
				UserId = user.НомерЗаписи,
				Role = user.Роль1.Ниаменование,
				FullName = $"{user.Фамилия} {user.Имя} {user.Отчество}"
			});
		}

		private Пользователи AuthenticateUser(string username, string password)
		{
			var user = db.Пользователи
						.Include("Роль1")
						.FirstOrDefault(u => u.Логин == username);

			if (user == null)
				return null;

			// Простая проверка пароля (в реальном проекте используйте хеширование)
			if (user.Пароль != password)
				return null;

			return user;
		}

		private string GenerateJwtToken(Пользователи user)
		{
			var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
			var audience = ConfigurationManager.AppSettings["JwtAudience"];
			var secret = ConfigurationManager.AppSettings["JwtSecret"];
			var expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["JwtExpireDays"]);

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.НомерЗаписи.ToString()),
				new Claim(ClaimTypes.Name, user.Логин),
				new Claim(ClaimTypes.Role, user.Роль1.Ниаменование)
			};

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.Now.AddDays(expireDays),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

	/// <summary>
	/// Модель информации о пользователе
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		/// Логин пользователя
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; }
	}
}