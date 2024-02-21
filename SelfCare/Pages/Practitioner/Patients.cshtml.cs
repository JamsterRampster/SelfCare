using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfCare.Models;

namespace SelfCare.Pages.Practitioner
{
    public class PatientsModel : PageModel
    {
        public List<Models.Patient> Patients { get; set; }

        private readonly SelfCareContext _selfcareContext;
        public PatientsModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public IActionResult OnGet()
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyLoggedIn) ?? "";

            if ((loggedInStatus != "true") || (userType != (int)Infrastructure.Enums.UserType.Practitioner))
            {

                return RedirectToPage("/login");
            }

            PopulatePatients();

            return Page();
        }

        private void PopulatePatients()
        {
            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;
            Models.Practitioner practitioner = _selfcareContext.Practitioners.Where(x => x.UserId == userId).FirstOrDefault();
            Patients = _selfcareContext.Patients.Include(p => p.Gp).Where(u => u.PractitionerId == practitioner.PractitionerId).ToList();
        }

        public IActionResult OnPostDeletePatient([FromForm] int patientId, [FromForm] string patientName)
        {

            _selfcareContext.Patients.Where(u => u.PatientId == patientId).ExecuteDelete();
            PopulatePatients();
            ViewData["Message"] = $"Successfully Deleted {patientName}";
            return Page();
        }

    }
}