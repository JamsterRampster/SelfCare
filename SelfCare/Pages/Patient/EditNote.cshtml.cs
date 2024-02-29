using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfCare.Models;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace SelfCare.Pages.Patient
{
    [BindProperties]
    public class EditNoteModel : PageModel
    {
        public Models.Note editNote { get; set; }

        [Required]
        public string noteName { get; set; }

        public string noteBody { get; set; }
        public int noteId { get; set; }

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

            return RedirectToPage("/patient/Notes");
        }

        public IActionResult OnPost(int? editNoteId) 
        { 
            if (editNoteId == null)
            {
                return RedirectToPage("/patient/Notes");
            }
            
            editNote = _selfcareContext.Notes.Where(x => x.NoteId == editNoteId).FirstOrDefault();

            if (editNote == null)
            {
                return RedirectToPage("/patient/Notes");
            }

            noteName = editNote.Title;
            noteBody = editNote.Body;
            return Page();
        }
        public IActionResult OnPostEditNote()
        {
            if (ModelState.IsValid)
            {
                int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;

                if (userId != 0)
                {
                    Models.Patient patient = _selfcareContext.Patients.Where(p => p.UserId == userId).FirstOrDefault();

                    if (patient != null)
                    {
                        var noteToUpdate = _selfcareContext.Notes.Where(x => x.NoteId == noteId).FirstOrDefault();

                        if (noteToUpdate != null)
                        {
                            noteToUpdate.Title = noteName;
                            noteToUpdate.Body = noteBody;
                            noteToUpdate.DateUpdated = DateTime.Now;
                            _selfcareContext.SaveChanges();
                            ViewData["SaveResponse"] = "Saved";
                            return Page();
                        }
                    }
                }
            }

            ViewData["Error"] = "An error has occured if this persists please contact support.";
            return Page();
        }
    }
}
