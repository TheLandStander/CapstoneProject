using DatabaseSystemIntegration.Pages.Tools;
using System.Reflection;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Message
    {
        public string Message_ID { get; set; }
        public string Sender_ID { get; set; }
        public string Receiver_ID { get; set; }

        public DateTime Send_Date { get; set; }
        public string Message_Subject { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }

        public Users SendingUser { get; set; }
        public Users ReceivingUser { get; set; }


        public void SetVars()
        {
            SendingUser = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19,19, Sender_ID))[0];
            ReceivingUser = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, Receiver_ID))[0];
        }

        public Message(string subject, string content, DateTime sendDate, string sender, string receiver)
        {
            Message_ID = DatabaseControls.MakeID();
            Sender_ID = sender;
            Receiver_ID = receiver;
            Message_Subject = subject;
            Content = content;
            Send_Date = sendDate;
        }


    }
}
