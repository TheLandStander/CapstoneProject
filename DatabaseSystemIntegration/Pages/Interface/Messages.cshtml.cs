using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{

    public class MessagesModel : PageModel
    {
        [BindProperty]
        public string MessageSubject { get; set; }

        [BindProperty]
        public string MessageContent { get; set; }

        [BindProperty]
        public string Recipient_ID { get; set; }

        [BindProperty]
        public Users[] Users { get; set; }

        [BindProperty]
        public Message[] Messages { get; set; }

        public void SendMessage()
        {
            string ID = HttpContext.Session.GetString("UserID");
            if (MessageSubject != null && MessageContent != null && Recipient_ID !=  null)
            {
                Message m = new Message(MessageSubject,MessageContent,DateTime.Now,ID,Recipient_ID);
                DatabaseControls.SendMessage(m);

            }
        }

        public void OnPost() 
        {
            SendMessage();
            Messages = ObjectConverter.ToMessages(DatabaseControls.GetMessages(HttpContext.Session.GetString("UserID")));
            Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
        }
        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Messages = ObjectConverter.ToMessages(DatabaseControls.GetMessages(HttpContext.Session.GetString("UserID")));
            Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            MessageContent = "Sample message body";
            MessageSubject = "Sample message subject";

            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            Messages = ObjectConverter.ToMessages(DatabaseControls.GetMessages(HttpContext.Session.GetString("UserID")));
            Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));

            return Page();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                Messages = ObjectConverter.ToMessages(DatabaseControls.GetMessages(HttpContext.Session.GetString("UserID")));
                Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
                    return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
