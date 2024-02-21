using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfCare.Models;

namespace SelfCare.Pages.Practitioner
{
    public class GPsModel : PageModel
    {
        public List<Gp> Gps { get; set; }

        private readonly SelfCareContext _selfcareContext;
        public GPsModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public IActionResult OnGet()
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyLoggedIn) ?? "";
            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;

            if ((loggedInStatus != "true") || (userType != (int)Infrastructure.Enums.UserType.Practitioner))
            {

                return RedirectToPage("/login");
            }

            Gps = _selfcareContext.Gps.ToList();

            return Page();
        }
        public IActionResult OnPostDeleteGp([FromForm] int Gpid, [FromForm] string Gpname) 
        {
            
            _selfcareContext.Gps.Where(u => u.Gpid == Gpid).ExecuteDelete();
            Gps = _selfcareContext.Gps.ToList();
            ViewData["Message"] = $"Successfully Deleted {Gpname}";
            return Page();  
        }

    }
}
