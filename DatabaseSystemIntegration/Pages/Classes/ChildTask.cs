﻿using DatabaseSystemIntegration.Pages.Tools;
using System.Threading.Tasks;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ChildTask
    {
        public string ChildTaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool Completed { get; set; }
        public string ParentTaskID { get; set; }


        public void CompleteTask()
        {
            DatabaseControls.CompleteChildTask(this);
        }


        public Tasks GetParentTask()
        {
           return ObjectConverter.ToTask(DatabaseControls.SelectFilter(15, 15, ParentTaskID))[0];
        }

        public ChildTask(string taskName, string description, DateOnly Start, DateOnly Due, bool completed, string parentTaskID)
        {
            ChildTaskID = DatabaseControls.MakeID();  // Set primary key
            TaskName = taskName;
            Description = description;
            StartDate = Start;
            DueDate = Due;
            Completed = completed;
            ParentTaskID = parentTaskID;
        }
    }

}

