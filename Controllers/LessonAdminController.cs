using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonAdminController : ControllerBase
    {
        private readonly project4Context _context;

        public LessonAdminController(project4Context context)
        {
            _context = context;
        }
        [HttpGet("ListLesson")]
        public async Task<IActionResult> ListLesson()
        {
            var data = _context.Lesson.Where(x => x.IsDeleted == false).Select(x => new
            {
                x.Code,
                x.Name,
                x.Description,
                x.Avatar,
                topic = _context.Topic.FirstOrDefault(a => a.IsDeleted == false && a.Code == x.TopicCode).Name,
            });
            return Ok(data);
        }

        [HttpGet("ListTopic")]
        public async Task<IActionResult> ListTopic()
        {
            var data = _context.Topic.Where(x => x.IsDeleted == false).Select(x => new
            {
                code = x.Code,
                name = x.Name,
            });
            return Ok(data);
        }
        [HttpPost("addLesson")]
        public async Task<IActionResult> addLesson([FromBody] addLesson add)
        {
            bool status = false;
            string title = "";
            try
            {
                var nameLesson = _context.Lesson.FirstOrDefault(x => x.IsDeleted == false && x.TopicCode == add.codeTopic && x.Name == add.name);
                if (nameLesson != null)
                {
                    status = true;
                    title = "Tên bài học đã tồn tại";
                }
                else
                {
                    var check = _context.Lesson.OrderByDescending(x => x.ID).FirstOrDefault();

                    int id_max = (int)(check != null ? check.ID + 1 : 1);
                    Lesson lesson = new Lesson
                    {
                        Code = "L_" + id_max,
                        TopicCode = add.codeTopic,
                        Avatar = add.avatar,
                        Name = add.name,
                        Description = add.description,
                        CreatedBy = "admin",
                        DeletedBy = "",
                        UpdatedBy = "",
                        CreatedTime = DateTime.Now,
                        IsDeleted = false,
                    };
                    _context.Lesson.Add(lesson);
                    _context.SaveChanges();

                    status = false;
                    title = "Thêm bài học thành công.";
                }
            }
            catch (Exception ex)
            {
                status = true;
                title = "Lỗi không thêm được bài học.";
            }
            var result = new
            {
                status = status,
                title = title,
            };
            return Ok(result);
        }
        [HttpPost("ListWord")]
        public async Task<IActionResult> ListWord([FromBody] ModelGet model)
        {
            var data = _context.Word.Where(x => x.IsDeleted == false && x.LessonCode == model.codeLesson).Select(x => new
            {
                code = x.Code,
                nameEn = x.NameEN,
                nameVn = x.NameVN,
                avatar = x.Avatar,
            });
            return Ok(data);
        }
        [HttpGet("GetWord")]
        public async Task<IActionResult> GetWord()
        {
            var result = _context.Word.Where(w => w.IsDeleted == false).Select(w => new
            {
                ID = w.ID,
                NameVN = w.NameVN,
                NameEN = w.NameEN,
                Count = w.Count,
                CreateTime = w.CreatedTime,

            });
            return Ok(result);
        }
    }
}
