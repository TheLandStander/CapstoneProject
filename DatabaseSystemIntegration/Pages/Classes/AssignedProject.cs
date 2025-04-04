﻿using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class AssignedProject
    {
        public string AssignedProjectID { get; set; }
        public string UserID { get; set; }
        public string ProjectID { get; set; }


        public void SetVars()
        {
            
        }

        public Project GetProject()
        {
            return ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 12, ProjectID))[0];
        }

        public Users GetAssignedUser()
        { 
        return ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, UserID))[0];
        }

        public AssignedProject(string userID, string projectID)
        {
            AssignedProjectID = DatabaseControls.MakeID();  // Set primary key
            UserID = userID;
            ProjectID = projectID;
        }
    }

}
