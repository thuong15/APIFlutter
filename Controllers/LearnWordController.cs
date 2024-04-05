using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;
using System.Linq;

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
            int totalAnswer = 2;

            var check = (from a in _context.Word.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
                         select new
                         {
                             a.Code
                         }).ToList();
            var data = (from a in _context.Word.Where(x => !x.IsDeleted)
                        select new ItemAnswer
                        {
                            Code = a.Code,
                            NameEN = a.NameEN,
                            NameVN = a.NameVN,
                            IsCorrect = false,
                            IsChoose = false
                        }).ToList();

            List<ItemAnswer> listAnswer = new List<ItemAnswer>();

            foreach (var item in check)
            {
                int index = data.FindIndex(x => x.Code == item.Code);
                data[index].IsCorrect = true;
                List<int> test = randomAnswer(data.Count(), index, totalAnswer).Result;

                foreach (var i in test)
                {
                    listAnswer.Add(data[i]);
                };
                //data[index].IsCorrect = false;
            }

            var result = (from a in _context.Word.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
                          select new ResultGetDataQuestion
                          {
                              NameEN = a.NameEN,
                              listAnswer = listAnswer,
                              Avatar = a.Avatar,
                          }).ToList();
            for (int i = 0; i < (listAnswer.Count() / totalAnswer); i++ )
            {
                result[i].listAnswer = listAnswer.Skip(totalAnswer * i).Take(totalAnswer).ToList();
            }

            return Ok(result);
        }
        [HttpPost("randomAnswer")]
        public async Task<List<int>> randomAnswer(int max, int indexCorrect, int totalAnswer)
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

            List<int> result = numbers.GetRange(0, (totalAnswer - 1));

            int index = rand.Next(0, totalAnswer);
            result.Insert(index, indexCorrect);

            result = result.OrderBy(x => rand.Next()).ToList();

            return result;
        }
    }
}
