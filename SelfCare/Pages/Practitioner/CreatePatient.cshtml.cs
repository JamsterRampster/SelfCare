using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfCare.Models;
using System.ComponentModel.DataAnnotations;

namespace SelfCare.Pages.Practitioner
{
    [BindProperties]
    public class CreatePatientModel : PageModel
    {


        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        public string? Address3{ get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Country { get; set; }

        public DateOnly? ReferalDate { get; set; }
        public string? NhsId { get; set; }
        public int? GpId { get; set; }

        private readonly SelfCareContext _selfcareContext;

        public CreatePatientModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public IActionResult OnPostCreatePatient()
        {
            if (ModelState.IsValid)
            {
                Models.Patient newPatient = new Models.Patient();
                newPatient.FirstName = FirstName;
                newPatient.LastName = LastName;
                newPatient.DateOfBirth = DateOfBirth; 
                newPatient.Address1 = Address1; 
                newPatient.Address2 = Address2; 
                newPatient.Address3 = Address3; 
                newPatient.Town = Town;
                newPatient.County = County;
                newPatient.Country = Country;
                newPatient.PostCode = Postcode;
                newPatient.ReferalDate = ReferalDate;
                newPatient.Nhsid = NhsId;
                newPatient.Gpid = GpId;

                newPatient.PractitionerKey = Guid.NewGuid();
                if (( GpId != null ) && (!(_selfcareContext.Gps.Any(u => u.Gpid == GpId)))) {
                    ViewData["Response"] = "Incorrect GP Id or GP Doesnt Exist";
                    return Page();
                }
                _selfcareContext.Patients.Add(newPatient);
                _selfcareContext.SaveChanges();

                //Show success and Practitioner Key
                ViewData["Response"] = $"You Practitioner KEy is :- {newPatient.PractitionerKey}";


                //Clear Form
                WipeModel();


                ModelState.Clear();
                

            }
            return Page();
        }
        public IActionResult OnGet()
        {
            int userType = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserType) ?? 0;
            string loggedInStatus = HttpContext.Session.GetString(SessionVariables.SessionKeyLoggedIn) ?? "";

            if ((loggedInStatus != "true") || (userType != (int)Infrastructure.Enums.UserType.Practitioner))
            {

                return RedirectToPage("/login");
            }

            return Page();
        }

    private void WipeModel()
        {
            FirstName = string.Empty; 
            LastName = string.Empty; 
            DateOfBirth = DateOnly.MinValue; 
            Address1 = string.Empty; 
            Address2 = string.Empty; 
            Address3 = string.Empty; 
            Town = string.Empty; 
            County = string.Empty; 
            Country = string.Empty; 
            Postcode = string.Empty;
            ReferalDate = null; 
            NhsId = null; 
            GpId = null;
        }
    }
}
