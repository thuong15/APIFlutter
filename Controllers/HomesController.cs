using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;
using System.Globalization;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HomesController : ControllerBase
	{
		private readonly project4Context _context;

		public HomesController(project4Context context)
		{
			_context = context;
		}

		[HttpPost("GetSttTopic")]
		public async Task<IActionResult> GetSttTopic([FromBody] Item model)
		{
			var topic = _context.Topic.FirstOrDefault(x => !x.IsDeleted && x.Code == model.Code);
			var data = (from a in _context.Topic.Where(x => x.IsDeleted == false && x.ID < topic.ID)
						select new
						{
							Name = a.Name
						}).ToList();

			var result = new
			{
				stt = data.Count() + 1
			};

			return Ok(result);
		}
		[HttpPost("GetListLesson")]
		public async Task<IActionResult> GetListLesson([FromBody] ItemGetListLesson model)
		{
			var checkDefault = from a in _context.Lesson.Where(x => !x.IsDeleted && x.TopicCode == model.CodeTopic)
							   select new
							   {
								   LessonCode = a.Code,
								   TopicCode = a.TopicCode,
								   Count = _context.Word.Count(b => b.LessonCode == a.Code && !b.IsDeleted) + _context.Question.Count(b => b.LessonCode == a.Code && !b.IsDeleted)
							   };

			var checkHistory = from a in _context.Topic.Where(x => !x.IsDeleted && x.Code == model.CodeTopic)
							   join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
							   join c in _context.History.Where(x => !x.IsDeleted && x.IsCorrect && x.UserCode == model.CodeUser) on b.Code equals c.LessonCode into k
							   from c in k.DefaultIfEmpty()
							   select new
							   {
								   code = b.Code,
								   check = c != null ? true : false,
								   wCode = c.WordCode,
								   qCode = c.QuestionCode
							   }
							   into e
							   group e by new { e.code, e.wCode, e.qCode } into h
							   select new
							   {
								   code = h.Key.code,
								   check = h.First().check
							   }
							   into d
							   group d by d.code into groupdata
							   select new
							   {
								   code = groupdata.Key,
								   Count = !groupdata.First().check ? 0 : groupdata.Count()
							   }
							   ;

			var data = from a in checkDefault
					   join b in checkHistory on a.LessonCode equals b.code
					   select new
					   {
						   a.LessonCode,
						   totalStar = b.Count == 0 ? 0 : a.Count <= b.Count ? 3 : ((b.Count / a.Count) * 100) > 60 ? 2 : 1,
					   };

			var result = from a in _context.Topic.Where(x => !x.IsDeleted && x.Code == model.CodeTopic)
						 join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
						 join c in data on b.Code equals c.LessonCode
						 select new
						 {
							 id = b.ID,
							 code = b.Code,
							 title = b.Name,
							 img = b.Avatar,
							 totalStar = c.totalStar
						 };


			return Ok(result);
		}
		[HttpPost("GetDay")]
		public async Task<IActionResult> GetDay([FromBody] Item model)
		{
			var data = (from a in _context.History.Where(x => !x.IsDeleted && x.UserCode == model.Code)
						select new
						{
							CodeHistory = a.Code,
							Date = a.CreatedTime.Value.ToString("dd/MM/yyyy")
						})
						.ToList()
						.GroupBy(x => x.Date)
						.Select(g => new
						{
							Date = g.Key,
							u = g.First().CodeHistory
						})
						.ToList();
			int consecutiveDays = 1;
			if (data.Count() > 0)
			{
				DateTime date = DateTime.ParseExact(data.LastOrDefault().Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
				DateTime currentDate = DateTime.Now;

				double check = (currentDate.Day - date.Day);

				if (check <= 1)
				{
					for (int i = 0; i < data.Count - 1; i++)
					{
						currentDate = DateTime.ParseExact(data[i].Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
						DateTime nextDate = DateTime.ParseExact(data[i + 1].Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

						check = (nextDate.Day - currentDate.Day);

						if ((nextDate.Day - currentDate.Day) == 1 && (nextDate.Month == currentDate.Month))
						{
							consecutiveDays++;
						}
						else
						{
							consecutiveDays = 1;
						}
					}
				}
				if (check == 1)
				{
					consecutiveDays++;
				}
			}

			var result = new
			{
				totalDay = consecutiveDays.ToString()
			};

			return Ok(result);
		}

		[HttpPost("GetCups")]
		public async Task<IActionResult> GetCups([FromBody] Item model)
		{
			string code = "viet_1";
			var data = (from a in _context.History.Where(x => !x.IsDeleted
						&& x.CreatedTime.Value.Month < DateTime.Now.Month
						)
						select new
						{
							CodeHistory = a.Code,
							Date = a.CreatedTime.Value.Month
						}).ToList();
			var data1 = (from a in _context.Account.Where(x => x.IsDeleted == false)
						 join b in _context.History.Where(x => x.IsDeleted == false && x.IsCorrect && x.CreatedTime.Value.Month < DateTime.Now.Month)
						 on a.Code equals b.UserCode
						 select new
						 {
							 a.Code,
							 month = b.CreatedTime.Value.Month
						 } into c
						 group c by new { c.Code, c.month } into groupedData
						 select new
						 {
							 groupedData.Key.Code,
							 totalscore = groupedData.Count(),
							 month = groupedData.Key.month
						 });

			var check = (from a in data1
						 group a by a.month into b
						 select new
						 {
							 b.Key
						 }).ToList();

			List<ItemResult> listResult = new List<ItemResult>();

			for (int i = 0; i < check.Count; i++)
			{
				var temp = (from a in data1.Where(x => x.month == check[i].Key)
							select a).OrderByDescending(x => x.totalscore).ToList();
				int index = temp.FindIndex(x => x.Code == code);
				int checkExits = listResult.FindIndex(x => x.position == index + 1);
				if (checkExits == -1 && index < 3)
				{
					ItemResult item = new ItemResult()
					{
						month = check[i].Key.ToString(),
						position = index + 1
					};
					listResult.Add(item);
				}
			}

			return Ok(listResult);
		}
		[HttpPost("DeleteItem")]
		public async Task<IActionResult> DeleteItem([FromBody] ItemDelete model)
		{
			bool status = false;
			string title = "";
			if (model.Type == "account")
			{
				var data = _context.Account.FirstOrDefault(x => x.Code == model.Code);
				if (data != null)
				{
					data.IsDeleted = true;
					_context.SaveChanges();
					status = true;
					title = "Xóa tài khoản thành công!";
				}
			}

			var result = new
			{
				status = status,
				title = title
			};

			return Ok(result);
		}

	}
	class ItemResult
	{
		public int position { get; set; }
		public string month { get; set; }
	}
}
