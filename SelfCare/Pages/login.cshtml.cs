using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfCare.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SelfCare.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly SelfCareContext _selfcareContext;
        
        [Required]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and one integer.")]
        public string Password { get; set; }


        public LoginModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }

        public IActionResult OnPostLoginInfo()
        {
            if (ModelState.IsValid)
            {
                User user = _selfcareContext.Users.Where(u => u.Username == Username).Include(u => u.UserType).FirstOrDefault();

                //Validation
                if (user == null) {
                    ViewData["ErrorMessage"] = "Username is incorrect or does not exist, Sign up to create an account.";
                    return Page(); 
                }

                //Process login credentials
                if (user.Password == Password)
                {
                    //Valid User

                    //Set session variables 
                    //For isloggedin, UserId, UserType

                    HttpContext.Session.SetInt32(SessionVariables.SessionKeyUserId, user.UserId);
                    HttpContext.Session.SetString(SessionVariables.SessionKeyLoggedIn, "true");
                    HttpContext.Session.SetInt32(SessionVariables.SessionKeyUserType, user.UserTypeId);

                    if (user.UserTypeId == (int)Infrastructure.Enums.UserType.Patient) {

                        return RedirectToPage("/patient/index");

                    }

                    if (user.UserTypeId == (int)Infrastructure.Enums.UserType.Practitioner)
                    {
                        return RedirectToPage("/practitioner/index");
                    }

                    ViewData["ErrorMessage"] = "Error Occured";
                    return Page();
                    
                }
                else 
                {
                    ViewData["ErrorMessage"] = "Username and password do not match.";
                    return Page();
                }

            } 
            else 
            {
                return Page();
            }
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
        }
    }
}
