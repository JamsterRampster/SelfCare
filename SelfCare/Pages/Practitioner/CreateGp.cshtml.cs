using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfCare.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SelfCare.Pages.Practitioner
{
    [BindProperties] //The variables on the webpage bind them to the variables on the controller and their requirements
    public class AddGpModel : PageModel
    {
        private readonly SelfCareContext _selfcareContext;

        [Required]
        public string Name { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        //Not required
        public string? Address3 { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Country { get; set; }

        public AddGpModel(SelfCareContext selfcareContext) //Constructor
        {
            _selfcareContext = selfcareContext;

        }

        public IActionResult OnPostCreateGP()
        {
            if (ModelState.IsValid) //If the form sent in the post has the correct variable standards
            {
                Models.Gp newGp = new Models.Gp(); //Generate a new GP and populate its data
                newGp.Name = Name;
                newGp.Address1 = Address1;
                newGp.Address2 = Address2;
                newGp.Address3 = Address3;
                newGp.Town = Town;
                newGp.County = County;
                newGp.Country = Country;
                newGp.PostCode = Postcode;

                try
                {
                    _selfcareContext.Gps.Add(newGp); //Add the new GP object into the table stored in server memory
                    _selfcareContext.SaveChanges(); //Save the data in the server memory to the SQL database
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); //If an error occured write the error in the page's console
                    ViewData["Message"] = "An error has occured";
                    return Page();
                }
                //Show success with GP name for clarification
                ViewData["Response"] = $"Successfully added {newGp.Name} as a GP, Manage your GP's on the GP Page.";
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
    }
}
