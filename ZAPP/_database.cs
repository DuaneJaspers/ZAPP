using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Json;
using System.Collections;
using System.Data;
using System.Web.Services;
using Mono.Data.Sqlite;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZAPP.Records;
using System.Web;

namespace ZAPP
{
    public class _database
    {
        //Context Definieren
        private Context context;
        private readonly string LOGIN_URL = "";
        //private readonly string DATA_URL = "https://gist.githubusercontent.com/DuaneJaspers/2e47a7c38e8f736a2036c52221362cef/raw/e9c7ccabd91217202f653aa3af3e2007fa155a1c/tasks.json";
        private readonly string DATA_URL = "https://fakemyapi.com/api/fake?id=9c8c603a-bdbc-4d41-b955-0420ca1730a8"; 

        // constructor
        public _database(Context context)
        {
            this.context = context;
            this.createDatabase();
        }

        //Database maken
        public void createDatabase()
        {
            Resources res = this.context.Resources;
            string app_name =
                res.GetString(Resource.String.app_name);
            string app_version =
                res.GetString(Resource.String.app_version);
            ArrayList createTablesCommands = new ArrayList
            {
                res.GetString(Resource.String.createTableUser),
                res.GetString(Resource.String.createTableAppointment),
                res.GetString(Resource.String.createTableTask),
            };

            string dbname = "_db_" + app_name + "_" + app_version + ".sqlite";

            string documentsPath = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal);
            string pathToDatabase = Path.Combine(documentsPath, dbname);
            var connectionString = String.Format("Data Source={0};Version=3;",
                                                    pathToDatabase);


            if (!File.Exists(pathToDatabase))
            {
                SqliteConnection.CreateFile(pathToDatabase);
                Global.saveConnection(new SqliteConnection(connectionString)); 
            
                using (var cmd = Global.sqliteConnection.CreateCommand())
                {
                    foreach (string command in createTablesCommands)
                    {
                        cmd.CommandText = command;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("table created");
                    }
                }
                this.downloadData();
            }
            else
            {
                Global.saveConnection(new SqliteConnection(connectionString));
            }
        }

        public void nonQueryToDatabase(string command)
        {
            using (var cmd = Global.sqliteConnection.CreateCommand())
            {
                // Table data
                cmd.CommandText =  command;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        //public void getUserRecordFromDatabase(string command)
        //{
        //    using (var conn = new SqliteConnection(this.connectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            // Table data
        //            cmd.CommandText = command;
        //            cmd.CommandType = CommandType.Text;
        //            SqliteDataReader record = cmd.ExecuteReader();
        //            if (record.Read())
        //            {
        //                UserRecord user = new UserRecord(record);
        //            } else
        //            {
        //                Console.WriteLine("No user found...");
        //                // TODO: send loginrequest to server, if login, save user 
        //            }
        //        }
        //        conn.Close();

        //    }
        //}

        public void saveAppointmentRecord(AppointmentRecord record)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.addAppointmentToTable);
            command = String.Format(command,
                record.id, record.datetime, WebUtility.HtmlEncode(record.client_name),
                WebUtility.HtmlEncode(record.client_address), record.client_zipcode, WebUtility.HtmlEncode(record.client_city),
                record.client_phonenumber, record.comment);
            this.nonQueryToDatabase(command);
        }

        public void saveTaskRecord(TaskRecord record)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.addTaskToTable);
            string id = (record.id).HasValue ? record.id.ToString() : "NULL";
            string complete = (record.id).HasValue ? record.complete.ToString() : "NULL";
            command = string.Format(command,
                id, record.appointment_id, record.description, complete);
            this.nonQueryToDatabase(command);
        }

        public void downloadData()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            try
            {
                byte[] myDataBuffer = webClient.DownloadData(this.DATA_URL);
                string download = Encoding.ASCII.GetString(myDataBuffer);

                JsonValue value = JsonValue.Parse(download);
                
                // loop through differenct tasks
                foreach (var allAppointments in (JsonObject)value)
                {
                    var key = allAppointments.Key;
                    JsonValue taskValue = allAppointments.Value;
                    foreach (JsonObject appointment in taskValue)
                    {
                        AppointmentRecord appointmentRecord = new AppointmentRecord(appointment);
                        this.saveAppointmentRecord(appointmentRecord);
                        foreach (JsonObject taskObject in appointment["tasks"])
                        {
                            TaskRecord taskRecord = new TaskRecord(taskObject, appointment["id"]);
                            this.saveTaskRecord(taskRecord);
                        }
                    }
                }

            }
            catch (WebException)
            {
                // even niks doen
            }
        }

        public ArrayList getAllAppointments()
        {
            ArrayList allData = new ArrayList();
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getAllAppointments);
            using (var conn = Global.sqliteConnection)
            {
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    SqliteDataReader record = cmd.ExecuteReader();
                    while (record.Read())
                    {
                        allData.Add(new AppointmentRecord(record));
                    } 
                    record.Close();

                }
            }
            return allData;

        }
    }

}

