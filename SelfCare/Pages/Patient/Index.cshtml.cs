using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SelfCare.Pages.Patient
{
    public class PatientModel : PageModel
    {
        public IActionResult OnGet()
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyLoggedIn) ?? "";

            if ((loggedInStatus != "true") || (userType != (int)Infrastructure.Enums.UserType.Patient))
            {

                return RedirectToPage("/login");
            }

            return Page();
        }
    }
}
