using AppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace AppAPI.Controllers
{
	/// <summary>
	/// API для работы с пациентами больницы
	/// </summary>

	[RoutePrefix("api/patients")]
	public class PatientsController : ApiController
    {
		private БольницаEntities db = new БольницаEntities();

		/// <summary>
		/// Получение списка всех пациентов
		/// </summary>
		/// <remarks>
		/// Возвращает полный список пациентов с основной информацией, включая адрес и документы
		/// </remarks>
		/// <returns>Список пациентов</returns>
		/// <response code="200">Успешное выполнение запроса</response>
		/// <response code="500">Внутренняя ошибка сервера</response>
		// GET: api/patients
		[HttpGet]
		[Route("")]
		[ResponseType(typeof(List<PatientResponse>))]
		public IHttpActionResult GetPatients()
		{
			try
			{
				var patients = db.Пациент.ToList();
				var response = patients.Select(p => new PatientResponse(p, db)).ToList();
				return Ok(response);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		/// <summary>
		/// Получение информации о конкретном пациенте
		/// </summary>
		/// <param name="id">Идентификатор пациента</param>
		/// <returns>Информация о пациенте</returns>
		/// <response code="200">Пациент найден</response>
		/// <response code="404">Пациент не найден</response>
		/// <response code="500">Внутренняя ошибка сервера</response>
		// GET: api/patients/5
		[HttpGet]
		[Route("{id:int}")]
		[ResponseType(typeof(PatientResponse))]
		public IHttpActionResult GetPatient(int id)
		{
			var patient = db.Пациент.FirstOrDefault(p => p.НомерПациента == id);
			if (patient == null)
			{
				return NotFound();
			}

			return Ok(new PatientResponse(patient, db));
		}
	}

	/// <summary>
	/// Модель ответа с информацией о пациенте
	/// </summary>
	public class PatientResponse
	{
		/// <summary>
		/// Уникальный идентификатор пациента
		/// </summary>
		/// <example>1</example>
		public int Id { get; set; }

		/// <summary>
		/// Полное имя пациента (ФИО)
		/// </summary>
		/// <example>Иванов Иван Иванович</example>
		public string FullName { get; set; }

		/// <summary>
		/// Контактный телефон пациента
		/// </summary>
		/// <example>+79001234567</example>
		public string Phone { get; set; }

		/// <summary>
		/// Полный адрес пациента
		/// </summary>
		/// <example>г. Москва, ул. Ленина, д. 10</example>
		public string Address { get; set; }

		/// <summary>
		/// Информация о документе пациента
		/// </summary>
		/// <example>Паспорт 4501 123456</example>
		public string DocumentInfo { get; set; }

		/// <summary>
		/// Конструктор модели ответа
		/// </summary>
		/// <param name="patient">Объект пациента из БД</param>
		/// <param name="context">Контекст базы данных</param>
		public PatientResponse(Пациент patient, БольницаEntities context)
		{
			Id = patient.НомерПациента;
			FullName = $"{patient.Фамилия} {patient.Имя} {patient.Отчество}";
			Phone = patient.Телефон;

			// Получаем адрес из контекста
			var address = patient.Адрес.HasValue ?
				context.Адрес.Find(patient.Адрес.Value) :
				null;

			Address = address != null ?
				$"{address.Город}, {address.Улица} {address.Дом}" :
				string.Empty;

			// Аналогично для документа
			var document = patient.НомерДокумента.HasValue ?
				context.Документ.Find(patient.НомерДокумента.Value) :
				null;

			DocumentInfo = document != null ?
				$"{document.НаименованиеДокумента} {document.Серия} {document.Номер}" :
				string.Empty;
		}
	}
}