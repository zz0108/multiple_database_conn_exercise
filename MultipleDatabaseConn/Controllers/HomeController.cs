using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleDatabaseConn.Models;
using MultipleDatabaseConn.Repositories;

namespace MultipleDatabaseConn.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IMemberRepository memberRepository)
        : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateMembers(int count)
        {
            try
            {
                // �ͦ����|�����
                var members = MemberFaker.GenerateMembers(count);

                // �[�J���Ʈw
                await memberRepository.MultipleCreate(members);

                return Json(new { success = true, count = members.Count });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "�ͦ��|����Ʈɵo�Ϳ��~");
                return Json(new { success = false, message = "�ͦ��|����Ʈɵo�Ϳ��~" });
            }
        }

        [HttpGet("Home/GetMember/{id}")]
        public async Task<IActionResult> GetMember(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid memberId))
                {
                    return Json(new { success = false, message = "�L�Ī��|�� ID" });
                }

                var member = await memberRepository.GetById(memberId);

                if (member == null)
                {
                    return Json(new { success = false, message = "�䤣�즹�|��" });
                }

                return Json(new { success = true, member });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> AllMember(int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await memberRepository.GetAll(pageNumber, pageSize);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_MemberList", pagedResult);
            }

            return View(pagedResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
