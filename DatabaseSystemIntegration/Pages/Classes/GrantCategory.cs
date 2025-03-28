using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class GrantCategory
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }

        public GrantCategory(string categoryName)
        {
            CategoryID = DatabaseControls.MakeID();  // Set primary key
            CategoryName = categoryName;
        }
    }

}
