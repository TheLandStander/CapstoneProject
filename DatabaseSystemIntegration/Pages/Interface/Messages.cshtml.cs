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
        public PersonalInfo[] Users { get; set; }

        [BindProperty]
        public Message[] Messages { get; set; }

        public void SendMessage()
        {
            string ID = HttpContext.Session.GetString("AccountID");
            Console.Write(MessageSubject != null && MessageContent != null && Recipient_ID != null);
            if (MessageSubject != null && MessageContent != null && Recipient_ID !=  null)
            {
                Message m = new Message(MessageSubject,MessageContent,DateTime.Now,ID,Recipient_ID);
                DatabaseControls.SendMessage(m);

            }
        }

        public void OnPost() 
        {
            SendMessage();
            Messages = ObjectConverter.ToMessage(DatabaseControls.GetMessages(HttpContext.Session.GetString("AccountID")));
            Users = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(14));
        }
        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Messages = ObjectConverter.ToMessage(DatabaseControls.GetMessages(HttpContext.Session.GetString("AccountID")));
            Users = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(14));
            MessageContent = "Sample message body";
            MessageSubject = "Sample message subject";

            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            Messages = ObjectConverter.ToMessage(DatabaseControls.GetMessages(HttpContext.Session.GetString("AccountID")));
            Users = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(14));

            return Page();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                Messages = ObjectConverter.ToMessage(DatabaseControls.GetMessages(HttpContext.Session.GetString("AccountID")));
                Users = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(14));
                    return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
