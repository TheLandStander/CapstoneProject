using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AddNotesModel : PageModel
    {
        [BindProperty]
        public string Meeting_Notes { get; set; }

        [BindProperty]
        public string Project_Notes { get; set; }

        [BindProperty]
        public string Bus_Rep_ID { get; set; }

        [BindProperty]
        public string Bus_Project_ID { get; set; }

        [BindProperty]
        public DateTime Meeting_Date { get; set; }


        private DateTime Note_Date { get; set; }


        [BindProperty]
        public BusRep[] Reps { get; set; } = ObjectConverter.ToBusRep(DatabaseControls.SelectNoFilter(3));

        [BindProperty]
        public BusProject[] Projects { get; set; } = DatabaseControls.GetActiveBusProjects();

        //auto populate doesnn't work.....
        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Meeting_Notes = "Sample meeting notes";
            Project_Notes = "Sample project notes";
            Reps = ObjectConverter.ToBusRep(DatabaseControls.SelectNoFilter(3));
            Projects = DatabaseControls.GetActiveBusProjects();
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            Reps = ObjectConverter.ToBusRep(DatabaseControls.SelectNoFilter(3));
            Projects = DatabaseControls.GetActiveBusProjects();
            return Page();
        }

        public void CheckAddMeetingMinute()
        {
            if (Meeting_Notes != null && Meeting_Date != DateTime.MinValue && Bus_Rep_ID != null)
            {
                MeetingMinutes MM = new MeetingMinutes(Meeting_Notes, Meeting_Date, Bus_Rep_ID);
                DatabaseControls.InsertMeetingMinutes(MM);
            }
        }

        public void CheckAddNotes()
        {
            if (Project_Notes != null && Bus_Project_ID != null)
            {
                Note_Date = DateTime.Now;
                ProjectNotes n = new ProjectNotes(Project_Notes, Note_Date, Bus_Project_ID);
                DatabaseControls.InsertProjectNote(n);
            }
        }

        public IActionResult OnPost() 
        {
            CheckAddMeetingMinute();
            CheckAddNotes();
            Reps = ObjectConverter.ToBusRep(DatabaseControls.SelectNoFilter(3));
            Projects = DatabaseControls.GetActiveBusProjects();
            return Page();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin" || HttpContext.Session.GetString("UserType") == "Employee" ||
                    HttpContext.Session.GetString("UserType") == "Faculty")
                {
                    Reps = ObjectConverter.ToBusRep(DatabaseControls.SelectNoFilter(3));
                    Projects = DatabaseControls.GetActiveBusProjects();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }
    }
}
