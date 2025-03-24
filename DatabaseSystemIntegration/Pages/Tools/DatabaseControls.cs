using DatabaseSystemIntegration.Pages.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.ObjectPool;
using System.Data;
using System.Diagnostics.CodeAnalysis;

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
        "BusPartner",
        "BusPartnerStatus ",
        "BusProject",
        "BusRep",
        "Employee",
        "EmployeeProject",
        "Faculty",
        "FacultyProject",
        "GrantCategory",
        "GrantProject",
        "Grants",
        "GrantStatus",
        "MeetingMinutes",
        "OrgType",
        "PersonalInfo",
        "ProjectNotes",
        "Task",
        "Message",
        "ChildTask",
        "Assigned_Task"
        };

        //array of primary keys, good for selections
        public static string[] Keys =
            {
        "Bus_Partner_ID",
        "Partner_Status_ID",
        "Bus_Project_ID",
        "Bus_Rep_ID",
        "Employee_ID",
        "Employee_Project_ID",
        "Faculty_ID",
        "Faculty_Project_ID",
        "Category_ID",
        "Grant_Project_ID",
        "Grant_ID",
        "Status_ID",
        "Meeting_Minutes_ID",
        "Org_Type_ID",
        "Info_ID",
        "Project_Notes_ID",
        "Task_ID",
        "Message_ID",
        "Child_Task_ID",
        "Assigned_Task_ID"
        };

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

        /*takes the sqldata reader, datareader goes by rows
        make a while loop /while read() is true /reading rows
        make a new row every time it checks for a new row
        inside the for loop, fill the row array with the column values (getvalue)
        after the loop goes through all of the columns in the current row, add the 
        row to the ParsedResults list, after all rows are collected, return it as 2D array.
        */
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
            cmd.CommandText = "SELECT Info_ID FROM " + Tables[14] + " WHERE User_Name = '" + UserName + "' AND Password = '"  + Password + "'" + ";";
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

        //retreives matching records in foreign key table
        public static SqlDataReader SelectLeftJoin(int Primary, int Foreign, int JoinColumn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = "SELECT * FROM " + Tables[Foreign] + " JOIN " + Tables[Primary] + " ON " +
            Tables[Foreign] + "." + Keys[JoinColumn] + " = " + Tables[Primary] + "." + Keys[JoinColumn] + ";";


            SqlDataReader tempReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return tempReader;
        }

        //choose by identigier...
        public static SqlDataReader SelectLeftJoinFilter(int Primary, int Foreign, int JoinColumn, int IDField,string Identifier)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = "SELECT * FROM " + Tables[Foreign] + " JOIN " + Tables[Primary] + " ON " +
            Tables[Foreign] + "." + Keys[JoinColumn] + " = " + Tables[Primary] + "." + Keys[JoinColumn] + " WHERE " +
            Keys[IDField] + " = " + Identifier + ";";


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

        //extracts columns from a table 
        public static string[][] GetColumn(SqlDataReader data, int[] column)
        {
            string[][] Table = ParseResults(data);
            string[][] FilterTable = new string[Table.Length][];

            for (int i = 0; i < Table.Length; i++)
            {
                FilterTable[i] = new string[column.Length];
                for (int j = 0; j < Table[i].Length; j++)
                {
                    foreach (int col in column)
                    {
                        if (j == col)
                        {
                            FilterTable[i][j] = Table[i][j];
                        }
                    }
                
                }
            }
            return FilterTable;
        }

        public static Faculty[] GetLeadFaculty()
        {
            List<Faculty> Leads = new List<Faculty>();
            Grant[] Grants =    ObjectConverter.ToGrant(SelectNoFilter(10));

            foreach (Grant g in Grants) 
            {
                Faculty Lead = ObjectConverter.ToFaculty(SelectFilter(6, 6, g.Lead_Faculty_ID))[0];
                if (Leads.Contains(Lead) == false)
                {
                    Leads.Add(Lead);
                }
            }

            return Leads.ToArray();
        }



       //returns a string array that represents a join with a foreign key-primary key table
       //default will just select the primary keys and second column...
       public static string[][] ReturnColumnJoin(int PrimaryTable, int ForeignTable, int ForeignKeyColumn, int PrimaryKeyColumn = 0, int ReturnColumn = 1)
        {
            string[][] PrimaryKeyTable = DatabaseControls.ParseResults(DatabaseControls.SelectNoFilter(PrimaryTable));
            string[][] ForeignKeyTable = DatabaseControls.ParseResults(DatabaseControls.SelectNoFilter(ForeignTable));

            string[][] KeyPair = new string[ForeignKeyTable.Length][];
            for (int i = 0; i < KeyPair.Length; i++)
            {
                //you must search through the entire primary key table.
                for (int j =0; j < PrimaryKeyTable.Length; j++) 
                {
                    //compares the primary and foreign key, if they match, add the key tot eh first element, and the selected column in the primary key table
                    if (ForeignKeyTable[i][ForeignKeyColumn] == PrimaryKeyTable[j][PrimaryKeyColumn])
                    {
                        //initialze the column with each loop before assigning...
                        KeyPair[i] = new string[2];
                        KeyPair[i][0] = PrimaryKeyTable[j][0];
                        KeyPair[i][1] = PrimaryKeyTable[j][ReturnColumn];
                        //break to avoid unnececary calculations once the key is found
                        break;
                    }
                }

            }
            return KeyPair;
        }

        //returns list of admins
        public static Employee[] GetAdmins(Employee[] Employees)
        {
            List<Employee> Admins = new List<Employee>();

            foreach (Employee e in Employees)
            {
                if (e.is_Admin)
                {
                    Admins.Add(e);
                }
            }

            return Admins.ToArray();
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
            cmd.Dispose();

        }
       

        //Inserts Grant
        public static void InsertGrant(Grant G)
        {
            String sqlQuery = "INSERT INTO Grants (Grant_ID,Grant_Name,Amount,Submission_Date,Award_Date,Lead_Faculty_ID,Status_ID,Category_ID,Bus_Partner_ID,Grant_Project_ID) VALUES ('";
            sqlQuery += G.Grant_ID + "','";
            sqlQuery += G.Grant_Name + "','";
            sqlQuery += G.Amount + "','";
            sqlQuery += G.Submission_Date + "','";
            sqlQuery += G.Award_Date + "','";
            sqlQuery += G.Lead_Faculty_ID + "','";
            sqlQuery += G.Grant_Status_ID + "','";
            sqlQuery += G.Grant_Category_ID + "','";
            sqlQuery += G.Bus_Partner_ID + "','";
            sqlQuery += G.Grant_Project_ID;
            sqlQuery += "');";

            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = sqlQuery;
            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd.Connection.Close();

        }

        //Insert Employee
        public static void InsertEmployee(Employee E)
        {
            String sqlQuery = "INSERT INTO Employee (Employee_ID,Employee_Name,is_Admin,Info_ID) VALUES ('";
            sqlQuery += E.Employee_ID + "','";
            sqlQuery += E.Employee_Name + "','";
            sqlQuery += tobyte(E.is_Admin) + "','";
            sqlQuery += E.getInfoID();
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

        //Insert Faculty
        public static void InsertFaculty(Faculty F)
        {
            String sqlQuery = "INSERT INTO Faculty (Faculty_ID,Faculty_Name,Info_ID) VALUES ('";
            sqlQuery += F.Faculty_ID + "','";
            sqlQuery += F.Faculty_Name + "','";
            sqlQuery += F.getInfoID();
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

        //Insert Business Partner
        public static void InsertBusPartner(BusPartner BP)
        {
            String sqlQuery = "INSERT INTO BusPartner (Bus_Partner_ID,Bus_Name,Partner_Status_ID,Org_Type_ID,Info_ID) VALUES ('";
            sqlQuery += BP.Bus_Partner_ID + "','";
            sqlQuery += BP.Bus_Name + "','";
            sqlQuery += BP.Partner_Status_ID + "','";
            sqlQuery += BP.Org_Type_ID + "','";
            sqlQuery += BP.getInfoID();
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

        public static void InsertBusRep(BusRep BR)
        {
            String sqlQuery = "INSERT INTO BusRep (Bus_Rep_ID,Rep_Name,Bus_Partner_ID,Info_ID) VALUES ('";
            sqlQuery += BR.Bus_Rep_ID + "','";
            sqlQuery += BR.Rep_Name + "','";
            sqlQuery += BR.Bus_Partner_ID + "','";
            sqlQuery += BR.GetInfoID();
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

        public static void InsertBusProject(BusProject BP)
        {
            String sqlQuery = "INSERT INTO BusProject (Bus_Project_ID,Project_Name,Description,Start_Date,End_Date,Due_Date) VALUES ('";
            sqlQuery += BP.Bus_Project_ID + "','";
            sqlQuery += BP.Project_Name + "','";
            sqlQuery += BP.Description + "','";
            sqlQuery += BP.Start_Date + "','";
            sqlQuery += BP.End_Date + "','";
            sqlQuery += BP.Due_Date + "','";
            sqlQuery += BP.Grant_Project_ID;
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

        public static void InsertTask(Classes.Task T)
        {
            String sqlQuery = "INSERT INTO Task (Task_ID,Task_Name,Task_Description,Completed,Bus_Project_ID) VALUES ('";
            sqlQuery += T.Task_ID + "','";
            sqlQuery += T.Task_Name + "','";
            sqlQuery += T.Task_Description + "','";
            sqlQuery += tobyte(T.Completed) + "','";
            sqlQuery += T.Bus_Project_ID;
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

        public static void InsertChildTask(ChildTask CT)
        {
            String sqlQuery = "INSERT INTO ChildTask(Child_Task_ID,Task_Name,Task_Description,Completed,Parent_Task_ID) VALUES ('";
            sqlQuery += CT.Child_Task_ID + "','";
            sqlQuery += CT.Task_Name + "','";
            sqlQuery += CT.Task_Description + "','";
            sqlQuery += tobyte(CT.Completed) + "','";
            sqlQuery += CT.Parent_Task_ID;
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
            cmd.Dispose();

        }

        public static void InsertGrantProject(GrantProject GP)
        {
            String sqlQuery = "INSERT INTO GrantProject (Grant_Project_ID,Project_Name,Description) VALUES ('";
            sqlQuery += GP.Grant_Project_ID + "','";
            sqlQuery += GP.Project_Name + "','";
            sqlQuery += GP.Description;
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

        public static void InsertProjectNote(ProjectNotes PN)
        {
            String sqlQuery = "INSERT INTO ProjectNotes (Notes_ID,Project_Notes,Date_Recorded,Bus_Project_ID) VALUES ('";
            sqlQuery += PN.Notes_ID + "','";
            sqlQuery += PN.Project_Note + "','";
            sqlQuery += PN.Date_Recorded + "','";
            sqlQuery += PN.Bus_Project_ID;
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

        public static void InsertMeetingMinutes(MeetingMinutes MM)
        {
            String sqlQuery = "INSERT INTO MeetingMinutes (Meeting_Minutes_ID,Meeting_Notes,Meeting_Date,Bus_Rep_ID) VALUES ('";
            sqlQuery += MM.Meeting_Minutes_ID + "','";
            sqlQuery += MM.Meeting_Notes + "','";
            sqlQuery +=  MM.Meeting_Date + "','";
            sqlQuery += MM.Bus_Rep_ID;
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


        //creates entry into the EmployeeProject table
        public static void AssignProject(EmployeeProject EP)
        {
            String sqlQuery = "INSERT INTO EmployeeProject (Employee_Project_ID,Assigning_Admin_ID,Employee_ID,Bus_Project_ID) VALUES ('";
            sqlQuery += EP.Employee_Project_ID + "','";
            sqlQuery += EP.Assigning_Admin_ID + "','";
            sqlQuery += EP.Employee_ID + "','";
            sqlQuery += EP.Bus_Project_ID;
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

        public static void AssignFacultyProject(FacultyProject FP)
        {
            String sqlQuery = "INSERT INTO FacultyProject (Faculty_Project_ID,Faculty_ID,Grant_Project_ID) VALUES ('";
            sqlQuery += FP.Faculty_Project_ID + "','";
            sqlQuery += FP.Faculty_ID + "','";
            sqlQuery += FP.Grant_Project_ID;
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

        public static void UpdateBusProject(string ID, DateOnly EndDate)
        {
            String sqlQuery = "UPDATE BusProject SET END_DATE = '" + EndDate + "' WHERE Bus_Project_ID = " + ID + ";";
            SqlCommand cmd = new SqlCommand();
            SqlConnection Database = new SqlConnection(ConnectionString);
            cmd.Connection = Database;
            cmd.CommandText = sqlQuery;
            cmd.Connection.Open();
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

            cmd.CommandText = "SELECT * FROM " + Tables[17] + " WHERE " + "Receiver_ID" + " = '" + ID + "' ;";
            SqlDataReader tempReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return tempReader;
        }


        public static BusProject[] GetActiveBusProjects()
        {
            BusProject[] Projects = ObjectConverter.ToBusProject(SelectNoFilter(2));
            List<BusProject> Active = new List<BusProject>();
            foreach (BusProject p in Projects)
            {
                //check if the end date was not set & add to list, by default it is min date
                if (p.End_Date == DateOnly.MinValue)
                {
                    Active.Add(p);
                }
            }
            return Active.ToArray();
        }
        public static int SecureLogIn(string Username, string Password)
        {
            string AccountQuery =
            "SELECT COUNT(*) FROM PersonalInfo where User_Name = @User_Name and Password = @Password";
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = new SqlConnection(ConnectionString);
            cmdLogin.CommandText = AccountQuery;
            cmdLogin.CommandType = System.Data.CommandType.StoredProcedure;
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
