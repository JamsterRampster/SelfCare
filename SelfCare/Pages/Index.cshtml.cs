using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SelfCare.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SelfCare.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SelfCareContext _selfcareContext;
        public List<Note> Notes { get; set; }
        public List<User> Users { get; set; }

        public IndexModel(SelfCareContext selfcareContext)
        {
            _selfcareContext = selfcareContext;

        }


        public IActionResult OnGetPartial()
        {

            Notes = _selfcareContext.Notes.ToList();
            return new PartialViewResult
            {
                ViewName = "_NoteTablePartial",
                ViewData = new ViewDataDictionary<List<Note>>(ViewData, Notes)
            };

        }

        public void OnGet()
        {
        }
    }
}
