﻿using DatabaseSystemIntegration.Pages.Classes;
using Microsoft.Data.SqlClient;
using System.Data;

// by Noah Kurtz, Joel Abbott, Nic Jordan, Andrew, Declan 

namespace DatabaseSystemIntegration.Pages.Tools
{
    public class DatabaseControls
    {
        // Create connection string to connect to the database
       public static string ConnectionString = "Data Source=localhost;Initial Catalog=Sprint2;Integrated Security=True;Encrypt=False";
       public static string AuthConnString = "Data Source=Localhost;Initial Catalog=AUTH;Integrated Security=True;Encrypt=False";

        //array of tables, good for selections
        public static string[] Tables =
        {
        "AssignedProject",
        "AssignedTask",
        "Partner",
        "PartnerStatus",
        "ChildTask",
        "GrantCategory",
        "GrantStatus",
        "Grants",
        "MeetingMinutes",
        "Messages",
        "Role",
        "PersonalInfo",
        "Project",
        "ProjectNotes",
        "ProjectStatus",
        "Task",
        "UserRole",
        "UserStatus",
        "UserType",
        "Users"
        };

        //array of primary keys, good for selections
        public static string[] Keys =
            {
       "Assigned_Project_ID",
       "Assigned_Task_ID",
       "Partner_ID",
       "Partner_Status_ID",
       "Child_Task_ID",
       "Category_ID",
       "Status_ID",
       "Grant_ID",
       "Meeting_Minutes_ID",
       "Message_ID",
       "Role_ID",
       "Info_ID",
       "Project_ID",
       "Notes_ID",
       "Project_Status_ID",
       "Task_ID",
       "User_Role_ID",
       "User_Status_ID",
       "User_Type_ID",
       "User_ID"
            };


        public static string MakeID()
        {
            //Makes the primary key 
            string ID = "";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                ID += rand.Next(10);
            }
            return ID;
        }

        //I need to use this for Employee since it has a boolean value which is stored as a byte in the database
        public static byte tobyte(bool b)
        {
            if (b == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static Role GetRole(string name)
        {
            Role[] All = ObjectConverter.ToRole(SelectNoFilter(10));

            foreach (Role r in All)
            {
                if (r.RoleName == name)
                {
                    return r;
                }
            }
            return new Role("INVALD");
        }

        public static GrantStatus GetGrantStatus(string name)
        {
            GrantStatus[] All = ObjectConverter.ToGrantStatus(SelectNoFilter(6));

            foreach (GrantStatus i in All)
            {
                if (i.GrantStatusName == name)
                {
                    return i;
                }
            }
            return new GrantStatus("INVALD");
        }

        public static ProjectStatus GetProjectStatus(string name)
        {
            ProjectStatus[] All = ObjectConverter.ToProjectStatus(SelectNoFilter(14));

            foreach (ProjectStatus i in All)
            {
                if (i.ProjectStatusName == name)
                {
                    return i;
                }
            }
            return new ProjectStatus("INVALD");
        }

        public static UserStatus GetUserStatus(string name)
        {
            UserStatus[] All = ObjectConverter.ToUserStatus(SelectNoFilter(17));

            foreach (UserStatus i in All)
            {
                if (i.StatusFlag == name)
                {
                    return i;
                }
            }
            return new UserStatus("INVALD");
        }

        public static PartnerStatus GePartnerStatus(string name)
        {
            PartnerStatus[] All = ObjectConverter.ToPartnerStatus(SelectNoFilter(3));

            foreach (PartnerStatus i in All)
            {
                if (i.Status_Flag == name)
                {
                    return i;
                }
            }
            return new PartnerStatus("INVALD");
        }

        public static GrantCategory GeGrantCategory(string name)
        {
            GrantCategory[] All = ObjectConverter.ToGrantCategory(SelectNoFilter(5));

            foreach (GrantCategory i in All)
            {
                if (i.CategoryName == name)
                {
                    return i;
                }
            }
            return new GrantCategory("INVALD");
        }

        public static UserType GeUserType(string name)
        {
            UserType[] All = ObjectConverter.ToUserType(SelectNoFilter(18));

            foreach (UserType i in All)
            {
                if (i.UserTypeName == name)
                {
                    return i;
                }
            }
            return new UserType("INVALD");
        }

        public static Users[] GetUsersWithRole(string Role)
        {
            List<Users> Matches = new List<Users>();
            UserRole[] AllUserRoles = ObjectConverter.ToUserRole(SelectNoFilter(16));
            foreach (UserRole UR in AllUserRoles)
            {
                if (UR.role.RoleName == Role)
                {
                    Matches.Add(UR.getUser());
                }
            
            }
            return Matches.ToArray();
        }

        public static Project[] GetActiveProjects()
        {
            List<Project> Holder = new List<Project>();
            Project[] AllPrjects = ObjectConverter.ToProject(SelectNoFilter(12));
            foreach (Project p in AllPrjects)
            {
                if(p.Status.ProjectStatusName == "Ongoing")
                {
                    Holder.Add(p);
                }
            }
            return Holder.ToArray();
        }

        public static void Execute(string Query)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = Query;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd.Connection.Close();
        }

        public static void CompleteTask(Tasks t)
        {
            String sqlQuery = "UPDATE Task SET Completed = " + t.Completed + " WHERE Task_ID = " + t.TaskID + ";";
            Execute(sqlQuery);

            sqlQuery = "UPDATE Task SET End_Date '" + DateOnly.FromDateTime(DateTime.Now) + "' WHERE Task_ID = " + t.TaskID + ";";
            Execute(sqlQuery);
        }

        public static void CompleteChildTask(ChildTask t)
        {
            String sqlQuery = "UPDATE ChildTask SET Completed = " + t.Completed + " WHERE Child_Task_ID = " + t.ChildTaskID + ";";
            Execute(sqlQuery);

            sqlQuery = "UPDATE ChildTask SET End_Date '" + DateOnly.FromDateTime(DateTime.Now) + "' WHERE Child_Task_ID = " + t.ChildTaskID + ";";
            Execute(sqlQuery);
        }

        public static void SetProjectLead(Project P, Users U)
        {
            String sqlQuery = "UPDATE Project SET Project_Lead_ID = " + U.UserID + " WHERE Project_ID = " + P.ProjectID + ";";
            Execute(sqlQuery);
        }

        public static void UpdateProjectStatus(string ProjectID, string StatusID)
        {
            String sqlQuery = "UPDATE Project SET Project_Status_ID = " + StatusID + " WHERE Project_ID = " + ProjectID + ";";
            Execute(sqlQuery);

            ProjectStatus ps = ObjectConverter.ToProjectStatus(SelectFilter(14, 14, StatusID))[0];

            if (ps.ProjectStatusName != "Ongoing")
            {
                sqlQuery = "UPDATE Project SET End_Date = '" + DateOnly.FromDateTime(DateTime.Now) + "' WHERE Project_ID = " + ProjectID + ";";
                Execute(sqlQuery);
            }

        }

        public static void UpdateProjectDueDate(string ProjectID, DateOnly date)
        {
            String sqlQuery = "UPDATE Project SET Due_Date = '" + date.ToString("yyyy-MM-dd") + "' WHERE Project_ID = " + ProjectID + ";";
            Execute(sqlQuery);
        }


        public static void Insert(PartnerStatus ps)
        {
            String sqlQuery = "INSERT INTO PartnerStatus(Partner_Status_ID, Status_Flag) VALUES ('";
            sqlQuery += ps.Partner_Status_ID + "','";
            sqlQuery += ps.Status_Flag;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(Partner p)
        {
            String sqlQuery = "INSERT INTO Partner(Partner_ID, Bus_Name, Address, Phone, Email, Description, Partner_Status_ID) VALUES ('";
            sqlQuery += p.PartnerID + "','";
            sqlQuery += p.BusName + "','";
            sqlQuery += p.Address + "','";
            sqlQuery += p.Phone + "','";
            sqlQuery += p.Email + "','";
            sqlQuery += p.Description + "','";
            sqlQuery += p.PartnerStatusID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(MeetingMinutes mm)
        {
            String sqlQuery = "INSERT INTO MeetingMinutes(Meeting_Minutes_ID, Meeting_Notes, Date, Partner_ID) VALUES ('";
            sqlQuery += mm.MeetingMinutesID + "','";
            sqlQuery += mm.MeetingNotes + "','";
            sqlQuery += mm.Date + "','";
            sqlQuery += mm.PartnerID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(PersonalInfo pi)
        {
            String sqlQuery = "INSERT INTO PersonalInfo(Info_ID, Primary_Contact, Secondary_Contact, User_Name, Password) VALUES ('";
            sqlQuery += pi.getInfoID() + "','";
            sqlQuery += pi.GetPrimaryContact() + "','";
            sqlQuery += pi.GetSecondaryContact() + "','";
            sqlQuery += pi.getUsername() + "','";
            sqlQuery += pi.getPassword();
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(UserType ut)
        {
            String sqlQuery = "INSERT INTO UserType(User_Type_ID, User_Type) VALUES ('";
            sqlQuery += ut.UserTypeID + "','";
            sqlQuery += ut.UserTypeName;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(UserStatus us)
        {
            String sqlQuery = "INSERT INTO UserStatus(User_Status_ID, Status_Flag) VALUES ('";
            sqlQuery += us.UserStatusID + "','";
            sqlQuery += us.StatusFlag;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(Users u)
        {
            String sqlQuery = "INSERT INTO Users(User_ID, Name, User_Type_ID, User_Status_ID, Info_ID, Partner_ID) VALUES ('";
            sqlQuery += u.UserID + "','";
            sqlQuery += u.Name + "','";
            sqlQuery += u.UserTypeID + "','";
            sqlQuery += u.UserStatusID + "','";
            sqlQuery += u.InfoID + "','";
            sqlQuery += u.PartnerID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(Role r)
        {
            String sqlQuery = "INSERT INTO Role(Role_ID, Role) VALUES ('";
            sqlQuery += r.RoleID + "','";
            sqlQuery += r.RoleName;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(UserRole ur)
        {
            String sqlQuery = "INSERT INTO UserRole(User_Role_ID, User_ID, Role_ID) VALUES ('";
            sqlQuery += ur.UserRoleID + "','";
            sqlQuery += ur.UserID + "','";
            sqlQuery += ur.RoleID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(ProjectStatus ps)
        {
            String sqlQuery = "INSERT INTO ProjectStatus(Project_Status_ID, Project_Status) VALUES ('";
            sqlQuery += ps.ProjectStatusID + "','";
            sqlQuery += ps.ProjectStatusName;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(Project p)
        {
            String sqlQuery = "INSERT INTO Project(Project_ID, Project_Name, Description, Start_Date, End_Date, Due_Date, Project_Lead_ID, Project_Status_ID) VALUES ('";
            sqlQuery += p.ProjectID + "','";
            sqlQuery += p.ProjectName + "','";
            sqlQuery += p.Description + "','";
            sqlQuery += p.StartDate + "','";
            sqlQuery += p.EndDate + "','";
            sqlQuery += p.DueDate + "','";
            sqlQuery += p.ProjectLeadID + "','";
            sqlQuery += p.ProjectStatusID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(AssignedProject ap)
        {
            String sqlQuery = "INSERT INTO AssignedProject(Assigned_Project_ID, User_ID, Project_ID) VALUES ('";
            sqlQuery += ap.AssignedProjectID + "','";
            sqlQuery += ap.UserID + "','";
            sqlQuery += ap.ProjectID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(Tasks t)
        {
            String sqlQuery = "INSERT INTO Task(Task_ID, Task_Name, Description, StartDate,DueDate ,EndDate, Completed, Project_ID) VALUES ('";
            sqlQuery += t.TaskID + "','";
            sqlQuery += t.TaskName + "','";
            sqlQuery += t.Description + "','";
            sqlQuery += t.StartDate + "','";
            sqlQuery += t.DueDate + "','";
            sqlQuery += t.EndDate + "','";
            sqlQuery += t.Completed + "','";
            sqlQuery += t.ProjectID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(ChildTask ct)
        {
            String sqlQuery = "INSERT INTO ChildTask(Child_Task_ID, Task_Name, Description, StartDate, DueDate, EndDate, Completed, Parent_Task_ID) VALUES ('";
            sqlQuery += ct.ChildTaskID + "','";
            sqlQuery += ct.TaskName + "','";
            sqlQuery += ct.Description + "','";
            sqlQuery += ct.StartDate + "','";
            sqlQuery += ct.DueDate + "','";
            sqlQuery += ct.EndDate + "','";
            sqlQuery += ct.Completed + "','";
            sqlQuery += ct.ParentTaskID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(AssignedTask at)
        {
            String sqlQuery = "INSERT INTO AssignedTask(Assigned_Task_ID, User_ID, Task_ID) VALUES ('";
            sqlQuery += at.AssignedTaskID + "','";
            sqlQuery += at.UserID + "','";
            sqlQuery += at.TaskID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }


        public static void Insert(GrantStatus gs)
        {
            String sqlQuery = "INSERT INTO GrantStatus(Status_ID, Grant_Status) VALUES ('";
            sqlQuery += gs.StatusID + "','";
            sqlQuery += gs.GrantStatusName;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(GrantCategory gc)
        {
            String sqlQuery = "INSERT INTO GrantCategory(Category_ID, Category_Name) VALUES ('";
            sqlQuery += gc.CategoryID + "','";
            sqlQuery += gc.CategoryName;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(Grant g)
        {
            String sqlQuery = "INSERT INTO Grants(Grant_ID, Grant_Name, Amount, Submission_Date, Award_Date, Due_Date, Status_ID, Category_ID, Project_ID) VALUES ('";
            sqlQuery += g.GrantID + "','";
            sqlQuery += g.GrantName + "','";
            sqlQuery += g.Amount + "','";
            sqlQuery += g.SubmissionDate + "','";
            sqlQuery += g.AwardDate + "','";
            sqlQuery += g.DueDate + "','";
            sqlQuery += g.StatusID + "','";
            sqlQuery += g.CategoryID + "','";
            sqlQuery += g.ProjectID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static void Insert(ProjectNotes pn)
        {
            String sqlQuery = "INSERT INTO ProjectNotes(Notes_ID, Project_Notes, Date, Project_ID) VALUES ('";
            sqlQuery += pn.NotesID + "','";
            sqlQuery += pn.Notes + "','";
            sqlQuery += pn.Date + "','";
            sqlQuery += pn.ProjectID;
            sqlQuery += "');";

            Execute(sqlQuery);
        }

        public static string[][] ParseResults(SqlDataReader r)
        {
            //Parses and closes the connection
            List<string[]> ParsedResults = new List<string[]>();

               while (r.Read())
                {
                string[] row = new string[r.FieldCount];
                for (int i = 0; i < r.FieldCount; i++)
                {
                    row[i] = r.GetValue(i).ToString();
                }

                ParsedResults.Add(row);
                }
            r.Dispose();
            return ParsedResults.ToArray();
        }

        public static SqlDataReader LogIn(string UserName, string Password)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = "SELECT Info_ID FROM " + Tables[11] + " WHERE User_Name = '" + UserName + "' AND Password = '"  + Password + "'" + ";";
            SqlDataReader tempReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return tempReader;
        }

        //Select everything from a table, no filter
        public static SqlDataReader SelectNoFilter(int Table)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = "SELECT * FROM " + Tables[Table] + ";" ;

            SqlDataReader tempReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return tempReader;
        }

        //Select everything from a table, given a key filter
        public static SqlDataReader SelectFilter(int SearchTable, int SearchField, string Identifier)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = "SELECT * FROM " + Tables[SearchTable] + " Where " + Keys[SearchField] + " = '" + Identifier + "' ;";
            SqlDataReader tempReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return tempReader;
        }
      
        public static void CreateAccount(PersonalInfo P)
        {
            String sqlQuery = "INSERT INTO PersonalInfo(Info_ID,Primary_Contact,Secondary_Contact,User_Name,Password) VALUES ('";
            sqlQuery += P.getInfoID() + "','";
            sqlQuery += P.GetPrimaryContact() + "','";
            sqlQuery += P.GetSecondaryContact() + "','";
            sqlQuery += P.getUsername() + "','";
            sqlQuery += P.getPassword();
            sqlQuery += "');";

            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = sqlQuery;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd.Connection.Close();

        }

       

        public static void UpdateGrantAwardDate(string ID, DateOnly AwardDate)
        {
            String sqlQuery = "UPDATE Grants SET AWARD_DATE = '" + AwardDate + "' WHERE GRANT_ID = " + ID + ";";

            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = sqlQuery;
            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd.Connection.Close();

        }

        public static void UpdateGrantStatus(string ID, string Status)
        {
            String sqlQuery = "UPDATE Grants SET STATUS_ID = '" + Status + "' WHERE GRANT_ID = " + ID + ";";

            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = sqlQuery;
            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd.Connection.Close();

        }


        public static void SendMessage(Classes.Message M)
        {
            String sqlQuery = "INSERT INTO Message (Message_ID,Message_Subject,Content,Send_Date,Sender_ID,Receiver_ID) VALUES ('";
            sqlQuery += M.Message_ID + "','";
            sqlQuery += M.Message_Subject+ "','";
            sqlQuery += M.Content + "','";
            sqlQuery += M.Send_Date + "','";
            sqlQuery += M.Sender_ID + "','";
            sqlQuery += M.Receiver_ID;
            sqlQuery += "');";

            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = sqlQuery;
            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public static SqlDataReader GetMessages(string ID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.Connection.Open();

            cmd.CommandText = "SELECT * FROM " + Tables[9] + " WHERE " + "Receiver_ID" + " = '" + ID + "' ;";
            SqlDataReader tempReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return tempReader;
        }


        public static int SecureLogIn(string Username, string Password)
        {
            string AccountQuery =
            "SELECT COUNT(*) FROM PersonalInfo where User_Name = @User_Name and Password = @Password";
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = new SqlConnection(ConnectionString);
            cmdLogin.CommandText = AccountQuery;
            cmdLogin.CommandType = CommandType.StoredProcedure;
            cmdLogin.Parameters.AddWithValue("@User_Name", Username);
            cmdLogin.Parameters.AddWithValue("@Password", Password);
            cmdLogin.CommandText = "sp_Lab3Login";
            cmdLogin.Connection.Open();
            int rowCount = (int)cmdLogin.ExecuteScalar();
            return rowCount;
        }

        public static void CreateHashedUser(string Username, string Password)
        {
            string loginquery =
                "INSERT INTO HashedCredentials (Username,Password) VALUES (@Username, @Password)";

            SqlCommand cmdlogin = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmdlogin.Connection = Database;
            cmdlogin.Connection.ConnectionString = AuthConnString;

            cmdlogin.CommandText = loginquery;
            cmdlogin.Parameters.AddWithValue("@Username", Username);
            cmdlogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));

            cmdlogin.Connection.Open();

            cmdlogin.ExecuteNonQuery();

        }

        public static bool HashedParameterLogin(string Username, string Password)
        {
            string loginquery =
                "SELECT Password FROM HashedCredentials WHERE Username = @Username";

            SqlCommand cmdlogin = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmdlogin.Connection = Database;
            cmdlogin.Connection.ConnectionString = AuthConnString;

            cmdlogin.CommandText = loginquery;
            cmdlogin.Parameters.AddWithValue("@Username", Username);

            cmdlogin.Connection.Open();

            SqlDataReader hashreader = cmdlogin.ExecuteReader();
            if (hashreader.Read())
            {
                string correcthash = hashreader["Password"].ToString();

                if (PasswordHash.ValidatePassword(Password, correcthash))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
