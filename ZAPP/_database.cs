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
        private readonly string DATA_URL = "https://fakemyapi.com/api/fake?id=cccc48d3-aec7-4447-b4ce-cc9a76e60647"; 
        private string connectionString;

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
                res.GetString(Resource.String.createTableTask),
                res.GetString(Resource.String.createTableSubTask)
            };

            string dbname = "_db_" + app_name + "_" + app_version + ".sqlite";

            string documentsPath = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal);
            string pathToDatabase = Path.Combine(documentsPath, dbname);
            var connectionString = String.Format("Data Source={0};Version=3;",
                                                    pathToDatabase);

            this.connectionString = connectionString;


            if (!File.Exists(pathToDatabase))
            {
                SqliteConnection.CreateFile(pathToDatabase);

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {

                        foreach (string command in createTablesCommands)
                        {
                            cmd.CommandText = command;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("table created");
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                Console.WriteLine("Database not created");
            }
                //this.downloadData();
        }

        public void nonQueryToDatabase(string command)
        {
            using (var conn = new SqliteConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                
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

        public void saveTaskRecord(TaskRecord record)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.addTaskToTable);
            command = String.Format(
                command, record.id, record.datetime, WebUtility.HtmlEncode(record.client_name),
                        WebUtility.HtmlEncode(record.client_address), record.client_zipcode, WebUtility.HtmlEncode(record.client_city),
                        record.client_phonenumber, record.comment);
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
                foreach (var allTasks in (JsonObject)value)
                {
                    var key = allTasks.Key;
                    JsonValue taskValue = allTasks.Value;
                    foreach (JsonObject task in taskValue)
                    {
                        TaskRecord taskRecord = new TaskRecord(task);
                        this.saveTaskRecord(taskRecord);

                    }
                }

            }
            catch (WebException)
            {
                // even niks doen
            }
        }

        public ArrayList getAllTasks()
        {
            ArrayList allData = new ArrayList();
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getAllTasks);
            using (var conn = new SqliteConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    SqliteDataReader record = cmd.ExecuteReader();
                    do
                    {
                        allData.Add(new TaskRecord(record));
                    } while (record.Read());
                    record.Close();

                }
                conn.Close();

            }
            return allData;

        }
    }

}

