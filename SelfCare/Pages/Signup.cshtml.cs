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
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PractitionerCode { get; set; }
        public string DateOfBirth { get; set; }
        public string Postcode { get; set; }
        public string ProductCode { get; set; }


        private readonly SelfCareContext _selfcareContext;
        public SignupModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }
        public void OnGet()
        {

        }

        public IActionResult OnPostSignup([FromForm] string radialValue) 
        {
            if (radialValue == "practitioner") 
            {
                if (ProductCode == null)
                {
                    ViewData["Response"] = "Product Code is required.";
                    return Page();
                }
            }

            if (radialValue == "patient") 
            {
                if (DateOfBirth == null || PractitionerCode == null || Postcode == null) 
                {
                    ViewData["Response"] = "All patient fields are required.";
                    return Page();
                }
            }

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

                            newUser.Username = Username;
                            newUser.Password = Password;
                            newUser.Email = Email;
                            newUser.UserTypeId = (int)Infrastructure.Enums.UserType.Practitioner;
                            newUser.DateUpdated = DateTime.Now;
                            newUser.DateCreated = DateTime.Now;

                            try
                            {
                                _selfcareContext.Users.Add(newUser);
                                _selfcareContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                ViewData["Message"] = "An error has occured";
                                return Page();
                            }

                            finishedPractitioner.UserId = newUser.UserId;
                            finishedPractitioner.DateUpdated = DateTime.Now;
                        }
                    }

                    ViewData["Response"] = $"";
                }
                return Page();
            }
            return Page();
        }
    }
}
