using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailLessonController : ControllerBase
    {
        private readonly project4Context _context;

        public DetailLessonController(project4Context context)
        {
            _context = context;
        }
        [HttpPost("GetDetailLesson")]
        public async Task<IActionResult> GetDetailLesson([FromBody] ModelViewDetailLesson modelViewDetailLesson)
        {
            var title = _context.Lesson.FirstOrDefault(x => x.IsDeleted == false && x.Code == modelViewDetailLesson.LessonCode).Name;

            var data = _context.Question.Where(x => x.IsDeleted == false && x.LessonCode == modelViewDetailLesson.LessonCode).Select(x => new
            {

                question = x.Name,
                avatar = x.Avatar,
                listAnswer = _context.Answer.Where(h => h.IsDeleted == false && h.QuestionCode == x.Code).Select(x => new
                {
                    answer = x.Name,
                    isTrue = x.IsTrue,
                }).ToList()
            });
            var total = _context.Question.Where(x => x.IsDeleted == false && x.LessonCode == modelViewDetailLesson.LessonCode).Count();
            var listData = new
            {
                title = title,
                data = data,
                total= total
            };
            return Ok(listData);
        }
        [HttpPost("AddCombine")]
        public async Task<IActionResult> AddCombine([FromBody] ItemAddSentence model)
		{
			bool status = false;
			string tilte = "";

			try
			{
				var checkName = _context.Question.FirstOrDefault(x => x.IsDeleted == false && x.Name == model.Sentences);
				if (checkName == null)
				{
					string dataFlutter = model.LessonCode;
					string[] parts = dataFlutter.Split('_');
					string CodeLesson = parts[1];

					var test = _context.Question.Where(x => !x.IsDeleted).ToList();

					var check = _context.Question.OrderByDescending(x => x.ID).FirstOrDefault();

					int id_max = (int)(check != null ? check.ID + 1 : 1);
					Question item = new Question
					{
						Code = "Q_" + CodeLesson + "_" + id_max,
						LessonCode = model.LessonCode,
						Name = model.Sentences,
						Avatar = "",
						Description = model.Description,
						CreatedBy = "admin",
						CreatedTime = DateTime.Now,
						UpdatedBy = "",
						DeletedBy = "",
						IsDeleted = false,
					};
					_context.Question.Add(item);
					_context.SaveChanges();
					status = false;
					tilte = "Đã thêm câu mới thành công.";
				}
				else
				{
					status = true;
					tilte = "Câu bạn thêm đã có trong bài học.";
				}
			}
			catch (Exception ex)
			{
				status = true;
				tilte = "Đã có lỗi xảy ra vui lòng thử lại lần sau.";
			}
			var result = new
			{
				status = status,
				title = tilte
			};
			return Ok(result);
		}
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] ItemAddQuestion model)
		{
			bool status = false;
			string tilte = "";

			try
			{
				var checkName = _context.Question.FirstOrDefault(x => x.IsDeleted == false && x.Name == model.Question);
				if (checkName == null)
				{
					string dataFlutter = model.LessonCode;
					string[] parts = dataFlutter.Split('_');
					string CodeLesson = parts[1];

					string[] listAnswer = model.ListAnswer.Split('_');

					var check = _context.Question.OrderByDescending(x => x.ID).FirstOrDefault();

					int id_max = (int)(check != null ? check.ID + 1 : 1);
					Question item = new Question
					{
						Code = "Q_" + CodeLesson + "_" + id_max,
						LessonCode = model.LessonCode,
						Name = model.Question,
						Description = "",
						Avatar = model.Avatar,
						CreatedBy = "admin",
						CreatedTime = DateTime.Now,
						UpdatedBy = "",
						DeletedBy = "",
						IsDeleted = false,
					};
					_context.Question.Add(item);
					_context.SaveChanges();
					status = false;
					tilte = "Đã thêm câu mới thành công.";
				}
				else
				{
					status = true;
					tilte = "Câu bạn thêm đã có trong bài học.";
				}
			}
			catch (Exception ex)
			{
				status = true;
				tilte = "Đã có lỗi xảy ra vui lòng thử lại lần sau.";
			}
			var result = new
			{
				status = status,
				title = tilte
			};
			return Ok(result);
		}
    }
}
