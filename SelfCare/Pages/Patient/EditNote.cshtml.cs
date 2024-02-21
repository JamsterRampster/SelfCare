using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfCare.Models;
using System.Collections.Concurrent;

namespace SelfCare.Pages.Patient
{
    public class EditNoteModel : PageModel
    {
        Models.Note note { get; set; }

        private readonly SelfCareContext _selfcareContext;
        public EditNoteModel(SelfCareContext selfcareContext)
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

            int latestItemId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyLatestItemId) ?? 0;
            int latestItemType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyLatestItemType) ?? 0;

            note = _selfcareContext.Notes.Where(u => u.NoteId == latestItemId).FirstOrDefault();

            if ((latestItemType != (int)Infrastructure.Enums.ItemType.Note) || (latestItemId == 0) || (note == null))
            {
                return RedirectToPage();
            }

            return Page();
        }
    }
}
