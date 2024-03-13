using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfCare.Models;

namespace SelfCare.Pages.Practitioner
{
    public class PatientViewModel : PageModel
    {
        private readonly SelfCareContext _selfcareContext;
        public List<Models.Note> notes { get; set; }
        public PatientViewModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public IActionResult OnGet(int patientId)
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyLoggedIn) ?? "";
            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;

            Models.Practitioner practitioner = _selfcareContext.Practitioners.Where(u => u.UserId == userId).FirstOrDefault();

            if ((loggedInStatus != "true") || (userType != (int)Infrastructure.Enums.UserType.Practitioner) || (practitioner == null))
            {

                return RedirectToPage("/login");
            }

            Models.Patient patient = _selfcareContext.Patients.Where(u => u.PatientId == patientId && u.PractitionerId == practitioner.PractitionerId).FirstOrDefault();

            if (patient == null) {
                return RedirectToPage("/practitioner/Patients");
            }
            
            notes = _selfcareContext.Notes.Where(u => u.PatientId == patient.PatientId).ToList();

            return Page();
        }
    }
}
