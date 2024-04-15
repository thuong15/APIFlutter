using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CombineSentencesController : ControllerBase
	{
		private readonly project4Context _context;

		public CombineSentencesController(project4Context context)
		{
			_context = context;
		}
		[HttpPost("GetListCombine")]
		public async Task<IActionResult> GetListCombine([FromBody] Item model)
		{
			Random rand = new Random();
			var check = (from a in _context.Word
						 select new
						 {
							 a.NameEN
						 }).ToList();

			var data = (from a in _context.Question.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
						join b in _context.Answer.Where(x => !x.IsDeleted) on a.Code equals b.QuestionCode into c
						from b in c.DefaultIfEmpty()
						where b == null
						select new
						{
							a.Code,
							a.Name,
							a.Description
						} into d
						group d by d.Code into b
						select new ResultCombineSentences
						{
							Code = b.Key,
							Name = b.First().Name,
							Description = b.First().Description,
							ListAnswer = b.First().Name.SplitBySpace()
						}).ToList();
			for (var i = 0; i < data.Count(); i++)
			{
				List<int> listIndex = randomAnswer(check.Count()).Result;
				var item = data[i];
				for (int j = 0; j < listIndex.Count(); j++)
				{
					item.ListAnswer.Add(check[listIndex[j]].NameEN);
				}
				for (int j = 0; j < item.ListAnswer.Count() - 2; j++)
				{
					int k = rand.Next(0, item.ListAnswer.Count() - 1);
					string temp = item.ListAnswer[j];
					item.ListAnswer[j] = item.ListAnswer[k];
					item.ListAnswer[k] = temp;
				}
			}
			var result = from a in data
						 select new
						 {
							 code = a.Code,
							 name = a.Name,
							 description = a.Description,
							 listAnswer = a.ListAnswer.Select(x => new {
								 nameEn = x,
								 isShowText = true,
							 })
						 };
			return Ok(result);
		}
		[HttpPost("randomAnswer")]
		public async Task<List<int>> randomAnswer(int max)
		{
			int totalAnswer = 3;
			Random rand = new Random();

			List<int> numbers = Enumerable.Range(0, max).ToList();

			for (int i = 0; i < max - 2; i++)
			{
				int j = rand.Next(0, max - 1);
				int temp = numbers[i];
				numbers[i] = numbers[j];
				numbers[j] = temp;
			}

			List<int> result = numbers.GetRange(0, totalAnswer);

			result = result.OrderBy(x => rand.Next()).ToList();

			return result;
		}
	}
}
public static class StringExtensions
{
	public static List<string> SplitBySpace(this string input)
	{
		return input.Split(' ').ToList();
	}
}
