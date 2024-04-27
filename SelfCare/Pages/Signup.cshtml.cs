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
        [EmailAddress(ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and one integer.")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Guid? PractitionerCode { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Postcode { get; set; }
        public string? ProductCode { get; set; }


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
            if (ModelState.IsValid)
            {
                if (ConfirmPassword != Password)
                {
                    ViewData["Response"] = "Passwords must match.";
                    return Page();
                }

                User user = _selfcareContext.Users.Where(u => u.Username == Username || u.Email == Email).FirstOrDefault();
                if (user != null)
                {
                    ViewData["Response"] = "Account already exists with that email/username.";
                    return Page();
                }

                if (radialValue == "practitioner")
                {
                    if (ProductCode == null || ProductCode == String.Empty)
                    {
                        ViewData["Response"] = "Product Code is required.";
                        return Page();
                    }

                    Models.ProductKey productkey = _selfcareContext.ProductKeys.Where(u => u.KeyId == ProductCode).FirstOrDefault();

                    if ((productkey != null) && (_selfcareContext.Practitioners.Where(u => u.ProductId == productkey.ProductId).FirstOrDefault() == null)) //If product key exists and not already claimed
                    {
                        Models.Practitioner newPractitioner = new Models.Practitioner();

                        newPractitioner.ProductId = productkey.ProductId;
                        newPractitioner.FirstName = FirstName;
                        newPractitioner.LastName = LastName;
                        newPractitioner.UserId = 1;

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

                        Models.Practitioner finishedPractitioner = _selfcareContext.Practitioners.Where(u => u.ProductId == productkey.ProductId).FirstOrDefault();

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

                            try
                            {
                                _selfcareContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                ViewData["Message"] = "An error has occured";
                                return Page();
                            }
                            return RedirectToPage("/login");
                        }
                        ViewData["Response"] = "An error occured";
                        return Page();
                    }

                    ViewData["Response"] = $"Product code: {ProductCode} does not match with any account or has already been claimed.";
                    return Page();
                }

                if (radialValue == "patient")
                {
                    if (DateOfBirth == null || PractitionerCode == null || Postcode == null)
                    {
                        ViewData["Response"] = "All patient fields are required.";
                        return Page();
                    }


                    Models.Patient patient = _selfcareContext.Patients.Where(u => u.DateOfBirth == DateOfBirth && u.PostCode == Postcode && u.FirstName == FirstName && u.LastName == LastName).FirstOrDefault();

                    if (patient == null)
                    {
                        ViewData["Response"] = "Could not find matching credentials stored. Message your practitioner for more information.";
                        return Page();
                    }

                    if (PractitionerCode != patient.PractitionerKey)
                    {
                        ViewData["Response"] = "The Account found practitioner code does not match";
                        return Page();
                    }

                    if ((patient != null) && (patient.PractitionerKey == PractitionerCode)) //If patient key exists and key not already claimed
                    {
                        Models.User newUser = new Models.User();

                        newUser.Username = Username;
                        newUser.Password = Password;
                        newUser.Email = Email;
                        newUser.UserTypeId = (int)Infrastructure.Enums.UserType.Patient;
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
                            ViewData["Message"] = "An error has occured.";
                            return Page();
                        }

                        Models.Patient finishedPatient = _selfcareContext.Patients.Where(u => u.PractitionerKey == PractitionerCode).FirstOrDefault();
                        if (finishedPatient != null)
                        {
                            finishedPatient.UserId = newUser.UserId;
                            finishedPatient.DateUpdated = DateTime.Now;

                            try
                            {
                                _selfcareContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                ViewData["Message"] = "An error has occured.";
                                return Page();
                            }

                            return RedirectToPage("/login");
                        }

                        ViewData["Response"] = "An error occured. Could not find updated patient.";
                        return Page();
                    }

                    ViewData["Response"] = "An error occured. Code might be claimed";
                }
            }

            if (radialValue == null)
            {
                ViewData["Response"] = "You must select practitioner or patient.";
                return Page();
            }

            ViewData["Response"] = "An error occured, the model was not validated";
            return Page();
        }
    }
}
