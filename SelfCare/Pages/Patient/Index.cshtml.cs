using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SelfCare.Pages.Patient
{
    public class PatientModel : PageModel
    {
        public IActionResult OnGet()
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyUserType) ?? "";

            if ((loggedInStatus != "true") || (userType != 2))
            {

                return RedirectToPage("/login");
            }

            return Page();
        }
    }
}
