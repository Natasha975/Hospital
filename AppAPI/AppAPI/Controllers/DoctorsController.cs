using AppAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace AppAPI.Controllers
{
	/// <summary>
	/// API для управления врачами в системе больницы
	/// </summary>

	[RoutePrefix("api/doctors")]
	public class DoctorsController : ApiController
    {
		private БольницаEntities db = new БольницаEntities();

		/// <summary>
		/// Получение списка всех врачей
		/// </summary>
		/// <remarks>
		/// Возвращает полный список врачей с основной информацией
		/// </remarks>
		/// <returns>Список врачей</returns>
		/// <response code="200">Успешное выполнение запроса</response>
		/// <response code="500">Внутренняя ошибка сервера</response>

		[HttpGet]
		[Route("")]
		[ResponseType(typeof(List<DoctorResponse>))]
		public IHttpActionResult GetAllDoctors()
		{
			try
			{
				var doctors = db.Врач.ToList();
				var response = doctors.Select(d => new DoctorResponse
				{
					Id = d.НомерВрача,
					FullName = $"{d.Фамилия} {d.Имя} {d.Отчество}",
					Specialization = d.Специализация,
					InternalPhone = d.ВнутреннийТелефон
				}).ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}


		/// <summary>
		/// Получение информации о конкретном враче
		/// </summary>
		/// <param name="id">ID врача</param>
		/// <returns>Информация о враче</returns>
		
		[HttpGet]
		[Route("{id:int}", Name = "GetDoctorById")]
		[ResponseType(typeof(DoctorResponse))]
		public IHttpActionResult GetDoctor(int id)
		{
			try
			{
				var doctor = db.Врач.Find(id);
				if (doctor == null)
				{
					return NotFound();
				}

				var response = new DoctorResponse
				{
					Id = doctor.НомерВрача,
					FullName = $"{doctor.Фамилия} {doctor.Имя} {doctor.Отчество}",
					Specialization = doctor.Специализация,
					InternalPhone = doctor.ВнутреннийТелефон
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		/// <summary>
		/// Создание нового врача
		/// </summary>
		/// <param name="request">Данные нового врача</param>
		/// <returns>Созданный объект врача</returns>
		
		[HttpPost]
		[Route("")]
		[ResponseType(typeof(DoctorResponse))]
		public IHttpActionResult CreateDoctor([FromBody] DoctorRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var doctor = new Врач
				{
					Фамилия = request.LastName,
					Имя = request.FirstName,
					Отчество = request.MiddleName,
					Специализация = request.Specialization,
					ВнутреннийТелефон = request.InternalPhone
				};

				db.Врач.Add(doctor);
				db.SaveChanges();

				var response = new DoctorResponse
				{
					Id = doctor.НомерВрача,
					FullName = $"{doctor.Фамилия} {doctor.Имя} {doctor.Отчество}",
					Specialization = doctor.Специализация,
					InternalPhone = doctor.ВнутреннийТелефон
				};

				// Возвращаем ответ с кодом 201 и заголовком Location
				return CreatedAtRoute("GetDoctorById", new { id = doctor.НомерВрача }, response);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}

	/// <summary>
	/// Модель запроса для создания врача
	/// </summary>
	public class DoctorRequest
	{
		/// <summary>
		/// Фамилия врача
		/// </summary>
		[Required(ErrorMessage = "Фамилия обязательна для заполнения")]
		[StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
		public string LastName { get; set; }

		/// <summary>
		/// Имя врача
		/// </summary>
		[Required(ErrorMessage = "Имя обязательно для заполнения")]
		[StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
		public string FirstName { get; set; }

		/// <summary>
		/// Отчество врача
		/// </summary>
		[StringLength(50, ErrorMessage = "Отчество не должно превышать 50 символов")]
		public string MiddleName { get; set; }

		/// <summary>
		/// Специализация врача
		/// </summary>
		[StringLength(100, ErrorMessage = "Специализация не должна превышать 100 символов")]
		public string Specialization { get; set; }

		/// <summary>
		/// Внутренний телефонный номер
		/// </summary>
		[StringLength(20, ErrorMessage = "Внутренний телефон не должен превышать 20 символов")]
		public string InternalPhone { get; set; }
	}

	/// <summary>
	/// Модель ответа с информацией о враче
	/// </summary>
	public class DoctorResponse
	{
		/// <summary>
		/// Уникальный идентификатор врача
		/// </summary>
		/// <example>1</example>
		public int Id { get; set; }

		/// <summary>
		/// Полное имя врача (ФИО)
		/// </summary>
		/// <example>Иванов Сергей Петрович</example>
		public string FullName { get; set; }

		/// <summary>
		/// Специализация врача
		/// </summary>
		/// <example>Хирург</example>
		public string Specialization { get; set; }

		/// <summary>
		/// Внутренний телефонный номер
		/// </summary>
		/// <example>117</example>
		public string InternalPhone { get; set; }
	}
}