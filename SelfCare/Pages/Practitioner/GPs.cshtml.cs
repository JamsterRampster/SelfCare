using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfCare.Models;

namespace SelfCare.Pages.Practitioner
{
    public class GPsModel : PageModel
    {
        public List<Gp> Gps { get; set; } //Generate an empty list of the class GP

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
            
            _selfcareContext.Gps.Where(u => u.Gpid == Gpid).ExecuteDelete(); //Find a GP with the same Id as the one provided by the form and delete it 
            Gps = _selfcareContext.Gps.ToList(); //Repopulate the GP list so the page doesnt break when the onpost handler is sent back
            ViewData["Message"] = $"Successfully Deleted {Gpname}";
            return Page();  
        }

    }
}
