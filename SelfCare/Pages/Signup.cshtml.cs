using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfCare.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SelfCare.Pages
{
    [BindProperties]
    public class SignupModel : PageModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PractitionerCode { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string ProductCode { get; set; }


        private readonly SelfCareContext _selfcareContext;
        public SignupModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public void OnGet()
        {

        }

        public IActionResult OnPostSignup() 
        {
            if (ModelState.IsValid)
            {
                if (ProductCode != String.Empty && PractitionerCode == null && DateOfBirth == null && Postcode == null) //If the practitioner is selected
                {
                    Models.ProductKey productkey = _selfcareContext.ProductKeys.Where(u => u.KeyId == ProductCode).FirstOrDefault();

                    if ((productkey != null) && (_selfcareContext.Practitioners.Where(u => u.ProductId == productkey.ProductId).FirstOrDefault() == null)) //If product key exists and not already claimed
                    {
                        Models.Practitioner newPractitioner = new Models.Practitioner();

                        newPractitioner.ProductId = productkey.ProductId;
                        newPractitioner.FirstName = FirstName;
                        newPractitioner.LastName = LastName;

                        try
                        {
                            _selfcareContext.Practitioners.Add(newPractitioner);
                            _selfcareContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            ViewData["Message"] = "An error has occured";
                            return Page();
                        }

                        Models.Practitioner finishedPractitioner = _selfcareContext.Practitioners.Where(u => u.PractitionerId == productkey.ProductId).FirstOrDefault();

                        if (finishedPractitioner != null)
                        {
                            Models.User newUser = new Models.User();

                        }
                    }
                    //Show success and Practitioner Key
                    ViewData["Response"] = $"";
                }
                return Page();
            }
            return Page();
        }
    }
}
