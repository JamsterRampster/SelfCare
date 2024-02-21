using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfCare.Models;
using System.ComponentModel.DataAnnotations;

namespace SelfCare.Pages.Patient
{
    [BindProperties]
    public class CreateNoteModel : PageModel
    {
        [Required]
        public string noteName { get; set; }

        public string description { get; set; }

        private readonly SelfCareContext _selfcareContext;
        public CreateNoteModel(SelfCareContext selfcareContext)
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

            return Page();
        }

        public IActionResult OnPostCreateNote() 
        { 
            if (ModelState.IsValid)
            {
                int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;
                if (userId != 0) 
                {
                    Models.Patient patient = _selfcareContext.Patients.Where(p => p.UserId == userId).FirstOrDefault();
                    if (patient != null) 
                    { 
                        Note newNote = new Note();
                        newNote.Title = noteName;
                        newNote.Body = description;
                        newNote.PatientId = patient.PatientId;
                        _selfcareContext.Notes.Add(newNote);
                        _selfcareContext.SaveChanges();
                        return RedirectToPage("/patient/Notes");
                    }
                }
            }

            ViewData["Error"] = "An error has occured if this persists please contact support.";
            return Page();
        }
    }
}
