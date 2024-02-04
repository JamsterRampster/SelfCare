using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public string Address3{ get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Country { get; set; }

        public DateOnly ReferalDate { get; set; }
        public string NhsId { get; set; }
        public int GpId { get; set; }

        private readonly SelfCareContext _selfcareContext;

        public CreatePatientModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public void OnPostCreatePatient()
        {
            if (ModelState.IsValid)
            {
                Models.Patient newPatient = new Models.Patient();
                newPatient.FirstName = FirstName;
                newPatient.LastName = LastName;
                newPatient.DateOfBirth = DateOfBirth; newPatient.Address1 = Address1; newPatient.Address2 = Address2; newPatient.Address3 = Address3; newPatient.Town = Town;
                newPatient.County = County;
                newPatient.Country = Country;
                newPatient.PostCode = Postcode;
                newPatient.ReferalDate = ReferalDate;
                newPatient.Nhsid = NhsId;
                newPatient.Gpid = GpId;

                newPatient.PractitionerKey = Guid.NewGuid();

                _selfcareContext.Patients.Add(newPatient);
                _selfcareContext.SaveChanges();
            }
        }
            public void OnGet(){ }
    }
}
