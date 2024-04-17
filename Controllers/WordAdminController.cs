using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordAdminController : ControllerBase
    {
        private readonly project4Context _context;

        public WordAdminController(project4Context context)
        {
            _context = context;
        }
        [HttpPost("addWord")]
        public async Task<IActionResult> addWord([FromBody] ModelViewWord model)
        {
            bool status = false;
            string tilte = "";

            try
            {
                var checkName = _context.Word.FirstOrDefault(x => x.IsDeleted == false && x.NameEN == model.nameEn && x.NameVN == model.nameVn);
                if (checkName == null)
                {
                    string dataFlutter = model.codeLesson;
                    string[] parts = dataFlutter.Split('_');
                    string CodeLesson = parts[1];

                    var check = _context.Word.OrderByDescending(x => x.ID).FirstOrDefault();

                    int id_max = (int)(check != null ? check.ID + 1 : 1);
                    Word word = new Word
                    {
                        Code = "W_" + CodeLesson + "_" + id_max,
                        LessonCode = model.codeLesson,
                        NameEN = model.nameEn,
                        NameVN = model.nameVn,
                        Avatar = model.avatar == "" ? "default.jpg" : model.avatar,
                        Count = model.count,
                        CreatedBy = "admin",
                        CreatedTime = DateTime.Now,
                        UpdatedBy = "",
                        DeletedBy = "",
                        IsDeleted = false,
                    };
                    _context.Word.Add(word);
                    _context.SaveChanges();
                    status = false;
                    tilte = "Đã thêm từ mới thành công.";
                }
                else
                {
                    status = true;
                    tilte = "Từ bạn thêm đã có trong bài học.";
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

        //[HttpPost("deleteWord")]
        //public async Task<IActionResult> deleteWord([FromBody] deleteodel word)
        //{
        //    bool status = false;
        //    string title = "";
        //    try
        //    {
        //        var codeWord = _context.Word.FirstOrDefault(x => x.IsDeleted == false && x.Code == word.code);
        //        if (codeWord == null)
        //        {
        //            status = true;
        //            title = "Không tìm thấy dữ liệu";
        //        }
        //        else
        //        {
        //            codeWord.IsDeleted = true;
        //            codeWord.DeletedBy = "admin";
        //            codeWord.DeletedTime = DateTime.Now;
        //            _context.SaveChanges();
        //            status = false;
        //            title = "Xoá từ mới thành công.";
        //        }
        //    }
        //    catch
        //    {
        //        status = true;
        //        title = "Đã có lỗi xảy ra vui lòng thử lại lần sau.";
        //    }
        //    var result = new
        //    {
        //        status = status,
        //        title = title,
        //    };
        //    return Ok(result);
        //}
    }
}
