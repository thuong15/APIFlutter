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
    public class ListVocabularysController : ControllerBase
    {
        private readonly project4Context _context;

        public ListVocabularysController(project4Context context)
        {
            _context = context;
        }

        [HttpPost("ListWords")]
        public async Task<IActionResult> ListWords([FromBody] ModelItem modelItem)
        {
            var data = from a in _context.Word.Where(x => !x.IsDeleted)
                       select new
                       {
                           a.Code,
                           a.Count,
                           a.NameEN,
                       };
            var check = from a in _context.History.Where(x => !x.IsDeleted && x.IsCorrect && x.QuestionCode == "" && x.UserCode == modelItem.CodeUser)
                        group a by a.WordCode into b
                        select new
                        {
                            b.First().Code,
                            WCode = b.Key,
                            Count = b.Count()
                        };

            var result = (from a in data
                         join b in check on a.Code equals b.WCode
                         select new
                         {
                             a.Code,
                             wordTotalCount = a.Count,
                             wordCount = b.Count,
                             IsRemomerize = a.Count == b.Count,
                             NameEN = char.ToUpper(a.NameEN.FirstOrDefault()) + a.NameEN.Substring(1)
                         } into c
                         where c.IsRemomerize == modelItem.IsRemomerize
                         select new
                         {
                             c.Code,
                             c.NameEN,
                             c.wordTotalCount,
                             c.wordCount
                         }).ToList();

            // Action InfoTotalWord using
            return Ok(result);
        }
        [HttpPost("InfoTotalWord")]
        public async Task<IActionResult> InfoTotalWord([FromBody] ItemInfoTotalWord modelItem)
        {
            dynamic listWordLearning = await ListWords(new ModelItem { CodeUser = modelItem.CodeUser, IsRemomerize = false });
            dynamic listWordLearned = await ListWords(new ModelItem { CodeUser = modelItem.CodeUser, IsRemomerize = true });

            int countListWordLearning = 0;
            if (listWordLearning != null && listWordLearning.Value != null)
            {
                countListWordLearning = listWordLearning.Value.Count;
            }

            int countListWordLearned = 0;
            if (listWordLearned != null && listWordLearned.Value != null)
            {
                countListWordLearned = listWordLearned.Value.Count;
            }

            var data = from a in _context.History.Where(x => !x.IsDeleted && x.UserCode == modelItem.CodeUser && x.WordCode != "")
                       group a by a.WordCode into b
                       select new
                       {
                           b.Key,
                       };

            var result = new
            {
                totalNumber = data.Count(),
                wordsLearned = countListWordLearning,
                wordsBeingStudied = countListWordLearned
            };

            return Ok(result);
        }

    }
}
