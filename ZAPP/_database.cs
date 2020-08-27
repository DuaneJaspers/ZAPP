using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Json;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZAPP.Records;

namespace ZAPP
{
    public class _database
    {
        //Context Definieren
        private Context context;
        private string loginUrl = "https://webservices.educom.nu/services/first/";
        private string DATA_URL = "https://gist.githubusercontent.com/DuaneJaspers/2e47a7c38e8f736a2036c52221362cef/raw/e9c7ccabd91217202f653aa3af3e2007fa155a1c/tasks.json";
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
            ArrayList createTablesCommands = new ArrayList();

            createTablesCommands.Add(res.GetString(Resource.String.createTableUser));
            createTablesCommands.Add(res.GetString(Resource.String.createTableTask));
            createTablesCommands.Add(res.GetString(Resource.String.createTableSubTask));

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
        }

    }

}

