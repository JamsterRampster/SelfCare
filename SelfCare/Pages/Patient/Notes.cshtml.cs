using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SelfCare.Models;

namespace SelfCare.Pages.Patient
{
    [BindProperties]
    public class NotesModel : PageModel
    {
        public List<Models.Note> Notes { get; set; } //List the notes are populated into

        public int editNoteId; //A variable linked to a hidden input on the screen

        private readonly SelfCareContext _selfcareContext;
        public NotesModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }

        public IActionResult OnGet()
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyLoggedIn) ?? "";

            if ((loggedInStatus != "true") || (userType != (int)Infrastructure.Enums.UserType.Patient))
            {

                return RedirectToPage("/login");
            }

            PopulateNote();

            return Page();
        }

        private void PopulateNote()
        {
            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0; //Get the userid from the session
            Models.Patient patient = _selfcareContext.Patients.Where(x => x.UserId == userId).FirstOrDefault(); //Get the patient from the userId
            Notes = _selfcareContext.Notes.Where(u => u.PatientId == patient.PatientId).ToList(); //Get the notes that belong to the patient
        }

    }
}
