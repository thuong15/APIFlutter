using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Question_adminController : ControllerBase
    {
        private readonly project4Context _context;
        public Question_adminController(project4Context context)
        {
            _context = context;
        }


        [HttpPost("GetAllqwa")]
        public async Task<IActionResult> GetQuestionsAndAnswers([FromBody] Item model)
        {

            var data = (from a in _context.Question.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
                        join b in _context.Answer.Where(x => !x.IsDeleted) on a.Code equals b.QuestionCode into e
                        from b in e.DefaultIfEmpty()
                        select new
                        {
                            QuestionCode = a.Code,
                            QuestionName = a.Name,
                            IsAnsquestion = b != null,
                            orderByDecending = b != null ? 1 : 0,
                            AnswerName = b != null ? b.Name : "",
                            AnswerCode = b != null ? b.Code : "",
                            IsTrue = b != null ? b.IsTrue : false,
                        } into c
                        group c by c.QuestionCode into d
                        select new
                        {
                            QuestionCode = d.Key,
                            QuestionName = d.First().QuestionName,
                            order = d.First().orderByDecending,
                            Answers = d.Select(x => new { x.AnswerCode, x.AnswerName, x.IsTrue })
                        }).OrderByDescending(x => x.order);
            var result = new
            {
                answerQ = data,
            };

            return Ok(result);
        }
        [HttpPost("addQuestion")]
        public async Task<IActionResult> addQuestion([FromBody] AddQuestion model)
        {
            bool status = false;
            string title = "";
            var question = _context.Question.FirstOrDefault(x => x.IsDeleted == false && x.Name == model.Question);
            if (question == null)
            {
                string dataFlutter = model.LessonCode;
                string[] parts = dataFlutter.Split('_');
                string CodeLesson = parts[1];

                var check = _context.Question.OrderByDescending(x => x.ID).FirstOrDefault();

                int id_max = (int)(check != null ? check.ID + 1 : 1);
                Question addquestion = new Question
                {
                    Code = "Q_" + CodeLesson + "_" + id_max,
                    LessonCode = model.LessonCode,
                    Avatar = model.Avatar,
                    Name = model.Question,
                    Description = "",
                    CreatedBy = "admin",
                    UpdatedBy = "",
                    DeletedBy = "",
                    CreatedTime = DateTime.Now,
                    IsDeleted = false,
                };
                _context.Question.Add(addquestion);
                var checkAs = _context.Answer.OrderByDescending(x => x.ID).FirstOrDefault();

                int id_maAsx = (int)(check != null ? check.ID + 1 : 1);
                int count = 0;
                foreach (var name in model.ListAnswer)
                {
                    if (name != "")
                    {
                        Answer answer = new Answer
                        {
                            Code = "A_" + CodeLesson + "_" + id_maAsx,
                            QuestionCode = "Q_" + CodeLesson + "_" + id_max,
                            Name = name,
                            IsTrue = count == 0 ? true : false,
                            CreatedBy = "admin",
                            UpdatedBy = "",
                            DeletedBy = "",
                            CreatedTime = DateTime.Now,
                        };
                        count++;
                        _context.Answer.Add(answer);
                    }

                }
                _context.SaveChanges();
                status = false;
                title = "Thêm câu hỏi thành công.";
            }
            else
            {
                status = true;
                title = "Câu hỏi đã có trong danh sách.";
            }
            var result = new
            {
                status = status,
                title = title,
            };
            return Ok(result);
        }
    }
}
