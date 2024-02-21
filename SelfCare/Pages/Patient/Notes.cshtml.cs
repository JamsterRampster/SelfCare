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
        public List<Models.Note> Notes { get; set; }

        public int editNoteId;

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

            PopulatePatients();

            return Page();
        }

        private void PopulatePatients()
        {
            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;
            Models.Patient patient = _selfcareContext.Patients.Where(x => x.UserId == userId).FirstOrDefault();
            Notes = _selfcareContext.Notes.Where(u => u.PatientId == patient.PatientId).ToList();
        }

        private IActionResult OnPostViewNote()
        {
            if ((ModelState.IsValid) && (editNoteId != null))
            {
                HttpContext.Session.SetInt32(SessionVariables.SessionKeyLatestItemId, editNoteId);
                HttpContext.Session.SetInt32(SessionVariables.SessionKeyLatestItemType, (int)Infrastructure.Enums.ItemType.Note);
            }
            return Page();
        }
    }
}
