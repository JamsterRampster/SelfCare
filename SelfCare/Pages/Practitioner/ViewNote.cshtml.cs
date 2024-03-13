using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfCare.Models;

namespace SelfCare.Pages.Practitioner
{
    [BindProperties]
    public class ViewNoteModel : PageModel
    {
        public Models.Note Viewnote { get; set; }

        public string? noteName { get; set; }

        public string? noteBody { get; set; }
        public int noteId { get; set; }

        private readonly SelfCareContext _selfcareContext;
        public ViewNoteModel(SelfCareContext selfcareContext)
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

            return RedirectToPage("/practitioner/patients");
        }

        public IActionResult OnPost(int noteId) 
        {
            if (noteId == null)
            {
                return RedirectToPage("/practitioner/patients");
            }

            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;

            if (userId != 0)
            {
                Models.Practitioner practitioner = _selfcareContext.Practitioners.Where(p => p.UserId == userId).FirstOrDefault();

                Viewnote = _selfcareContext.Notes.Where(x => x.NoteId == noteId && x.Patient.PractitionerId == practitioner.PractitionerId).FirstOrDefault();
            }

            if (Viewnote == null)
            {
                return RedirectToPage("/practitioner/patients");
            }

            noteName = Viewnote.Title;
            noteBody = Viewnote.Body;
            return Page();
        }
    }
}
