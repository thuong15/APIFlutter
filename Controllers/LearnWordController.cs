using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LearnWordController : ControllerBase
	{
		private readonly project4Context _context;

		public LearnWordController(project4Context context)
		{
			_context = context;
		}
		[HttpPost("getDataQuestion")]
		public async Task<IActionResult> getDataQuestion([FromBody] ItemGetDataQuestion model)
		{
			var data = (from a in _context.Word.Where(x => !x.IsDeleted)
						select new ItemAnswer
                        {
							Code = a.Code,
                            NameEN = a.NameEN,
                            NameVN = a.NameVN,
							IsCorrect = false
						}).ToList();

			data[model.Stt].IsCorrect = true;
            List<int> test = randomAnswer(data.Count(), model.Stt).Result;

            List<ItemAnswer> listAnswer = new List<ItemAnswer>();

			foreach(var i in test)
			{
				listAnswer.Add(data[i]);
            };

			var result = (from a in _context.Word.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
						 select new
						 {
							 a.NameEN,
                             listAnswer = listAnswer,
							 a.Avatar,
						 }).ToList()[model.Stt];

			return Ok(result);
        }
        [HttpPost("randomAnswer")]
        public async Task<List<int>> randomAnswer(int max, int indexCorrect)
        {
            Random rand = new Random();

            List<int> numbers = Enumerable.Range(0, max).Where(x => x != indexCorrect).ToList();

            for (int i = 0; i < max - 2; i++)
            {
                int j = rand.Next(i, max - 1);
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
            }

            List<int> result = numbers.GetRange(0, 2);

            int index = rand.Next(0, 3);
            result.Insert(index, indexCorrect);

            result = result.OrderBy(x => rand.Next()).ToList();

            return result;
		}
	}
}
