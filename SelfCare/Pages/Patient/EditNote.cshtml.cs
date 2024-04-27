using Humanizer;
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

            return RedirectToPage("/patient/Notes"); //As This page isnt meant to be onget'd it redirects requests to notes
        }

        public IActionResult OnPost(int? editNoteId) // editNoteId is passed from the form via the https link
        { 
            if (editNoteId == null)
            {
                return RedirectToPage("/patient/Notes"); // If no note provided redirect them back
            }

            int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;

            if (userId != 0)
            {
                Models.Patient patient = _selfcareContext.Patients.Where(p => p.UserId == userId).FirstOrDefault(); //Get the patient from userid

                //Get the note that matches with the noteId and also belongs to the patient logged in
                editNote = _selfcareContext.Notes.Where(x => x.NoteId == editNoteId && x.PatientId == patient.PatientId).FirstOrDefault(); 
            }

            if (editNote == null) //If no notes were found send them to back to the patients
            {
                return RedirectToPage("/patient/Notes");
            }
            
            //Set the notename as the note that was founds title
            noteName = editNote.Title;
            //Set the notebody as the notes body
            noteBody = editNote.Body;
            //return the page with the populated variables
            return Page();
        }
        public IActionResult OnPostEditNote() //If the use clicks save
        {
            if (ModelState.IsValid) //Make sure the restrictions on the variables are enforced and are valid
            {
                int userId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserId) ?? 0;

                if (userId != 0)
                {
                    Models.Patient patient = _selfcareContext.Patients.Where(p => p.UserId == userId).FirstOrDefault();

                    if (patient != null)
                    {
                        //Get the note from using the note id
                        var noteToUpdate = _selfcareContext.Notes.Where(x => x.NoteId == noteId).FirstOrDefault();
                        
                        //If the note is populated
                        if (noteToUpdate != null)
                        {
                            //Set the notes properties to the new ones provided by the form and set the updated time to current system time
                            noteToUpdate.Title = noteName;
                            noteToUpdate.Body = noteBody;
                            noteToUpdate.DateUpdated = DateTime.Now;
                            //Save the changed object in the database
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
