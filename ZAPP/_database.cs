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
using System.Linq.Expressions;
using Android.Support.V4.View.Animation;
using Java.Nio.FileNio;
using System.Text.RegularExpressions;
using ZAPP.Services;
using System.Threading.Tasks;
using System.Threading;

namespace ZAPP
{
    public class _database
    {
        //Context Definieren
        private Context context;
        private string connectionstring;  
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
            this.connectionstring = connectionString;

            if (!File.Exists(pathToDatabase))
            {
                using (var conn = new SqliteConnection(connectionstring))
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
                    //this.downloadData();
                }
            }
            else
            {
                Console.WriteLine("no new db created.");        
            }
        }

        async public Task syncDatabase()
        {
            ArrayList allAppointmentRecords = getAllAppointments();
            ArrayList newAppointments = new ArrayList();
            ArrayList existingAppointments = new ArrayList();

            ApiService api = new ApiService(context);

            try { 
                Task<JsonValue> appointmentTasks = api.getAppointments();
                JsonValue value = await appointmentTasks;

                // loop through different appointments
                foreach (JsonObject appointment in value)
                {
                        ArrayList taskRecords = new ArrayList();
                        foreach (JsonObject taskObject in appointment["tasks"])
                        {
                            TaskRecord taskRecord = new TaskRecord(taskObject, appointment["_id"]);
                            taskRecords.Add(taskRecord);
                        }
                        AppointmentRecord appointmentRecord = new AppointmentRecord(appointment, taskRecords);
                        bool exists = false;
                        foreach (AppointmentRecord existingAppointmentRecord in allAppointmentRecords)
                        {
                            if (existingAppointmentRecord.Equals(appointmentRecord))
                            {
                                exists = true;
                                allAppointmentRecords.Remove(existingAppointmentRecord);
                                existingAppointments.Add(appointmentRecord);
                                break;
                            }
                        }
                        if (!exists)
                            newAppointments.Add(appointmentRecord);
                    //}

                }
                updateTables(newAppointments, existingAppointments, allAppointmentRecords);
            }
            catch (Java.Net.SocketTimeoutException)
            {
                Console.WriteLine("connection error");
            }
        }

        private void updateTables(ArrayList newAppointments, ArrayList existingAppointments, ArrayList allAppointmentRecords)
        {
            // handle new appointments 
            foreach (AppointmentRecord newRecord in newAppointments)
            {
                saveAppointmentRecord(newRecord);
                foreach (TaskRecord task in newRecord.taskRecords)
                    this.saveTaskRecord(task);
            }

            //handle existing appointments
            foreach (AppointmentRecord existingRecord in existingAppointments)
            {
                // update appointmentrecord
                updateExistingAppointment(existingRecord);
                updateExistingTasks(existingRecord);
            }

            // delete rest (function)
            foreach (AppointmentRecord oldRecord in allAppointmentRecords)
            {
                // delete leftovers from db
                deleteOldAppointment(oldRecord);
                deleteOldTasksByAppointment(oldRecord); 
            }
        }
        private void deleteOldAppointment(AppointmentRecord appointmentRecord)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.deleteAppointmentById);
            if (Singleton.currentlyWorking == appointmentRecord.id.ToString())
                Singleton.currentlyWorking = null;
            command = String.Format(command, appointmentRecord.id);
            this.nonQueryToDatabase(command);
        }
        private void updateExistingAppointment(AppointmentRecord appointmentRecord)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.updateAppointment);
            command = String.Format(command,
                appointmentRecord.id, appointmentRecord.datetime, appointmentRecord.client_name.Replace("'", "''"),
                appointmentRecord.client_address.Replace("'", "''"), appointmentRecord.client_zipcode, appointmentRecord.client_city.Replace("'", "''"),
                appointmentRecord.client_phonenumber.Replace("'", "''"), appointmentRecord.comment.Replace("'", "''"));
            this.nonQueryToDatabase(command);
        }
        public void updateExistingTasks(AppointmentRecord appointmentRecord)
        {
            string appointmentId = appointmentRecord.id.ToString();
            ArrayList allTasks = getAllTasksByAppointmentId(appointmentId);
            ArrayList newTasks = new ArrayList();
            ArrayList exisitingTasks = new ArrayList();
            foreach (TaskRecord possibleTask in appointmentRecord.taskRecords)
            {
                bool exists = false;
                foreach (TaskRecord task in allTasks)
                {
                    if (task.Equals(possibleTask))
                    {
                        allTasks.Remove(possibleTask);
                        exists = true;
                        break;
                        // dont do anything with it
                    }
                }
                if (!exists)
                {
                    //add task
                    saveTaskRecord(possibleTask);
                }
            }
            foreach (TaskRecord oldTask in allTasks)
            {
                // remove task from db
                this.deleteOldTasksByTask(oldTask);
            }


        }

        public void nonQueryToDatabase(string command)
        {
            var conn = new SqliteConnection(connectionstring);
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
        public void deleteOldTasksByAppointment(AppointmentRecord appointmentRecord)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.deleteTaskByAppointmentId);
            command = String.Format(command, appointmentRecord.id);
            this.nonQueryToDatabase(command);
        }
        public void deleteOldTasksByTask(TaskRecord task)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.deleteTaskById);
            command = String.Format(command, task.id);
            this.nonQueryToDatabase(command);

        }

        public void deleteTaskById(int id)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.deleteTaskById);
            command = String.Format(command, id);
            this.nonQueryToDatabase(command);

        }

        public AppointmentRecord getAppointmentById(string id)
        {
            AppointmentRecord appointmentRecord;

            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getAppointmentById);
            command = String.Format(command, id);
            using (var conn = new SqliteConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    SqliteDataReader record = cmd.ExecuteReader();
                    record.Read();
                    appointmentRecord = new AppointmentRecord(record);
                    record.Close();
                }
                conn.Close();
            }
            return appointmentRecord;

        }
        public void getUserToken()
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getUserToken);
            using (var conn = new SqliteConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    SqliteDataReader record = cmd.ExecuteReader();
                    if (record.HasRows)
                    {
                        record.Read();
                        Singleton.userToken = (string)record["user_token"];
                    } 
                    record.Close();
                }
                conn.Close();
            }
            return;
        }

        public void saveUserToken(string userToken)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.saveUserToken);
            command = String.Format(command, userToken);
            this.nonQueryToDatabase(command);
        }

        public void saveAppointmentRecord(AppointmentRecord record)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.addAppointmentToTable);
            command = String.Format(command,
                 record.id, record.datetime, record.client_name.Replace("'", "''"),
                 record.client_address.Replace("'", "''"), record.client_zipcode, record.client_city.Replace("'", "''"),
                 record.client_phonenumber.Replace("'", "''"), record.comment.Replace("'", "''"));
            this.nonQueryToDatabase(command);
        }

        public void saveTaskRecord(TaskRecord record)
        {
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.addTaskToTable);
            string id = (record.id).HasValue ? record.id.ToString() : "NULL";
            string complete = (record.id).HasValue ? record.complete.ToString() : "False";
            command = string.Format(command,
                id, record.appointment_id, record.description, complete);
            this.nonQueryToDatabase(command);
        }

        public ArrayList getAllAppointments()
        {
            ArrayList allData = new ArrayList();
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getAllAppointments);
            using (var conn = new SqliteConnection(connectionstring))
            {
                conn.Open();
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
                conn.Close();
            }
            return allData;

        }
        

        public void toggleTaskCompleteness(int id, bool complete)
        {
            TaskRecord record = this.getTaskById(id);
            deleteTaskById(id);
            record.complete = complete;
            this.saveTaskRecord(record);
        }

        public TaskRecord getTaskById(int id)
        {
            TaskRecord taskRecord;
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getTaskById);
            command = String.Format(command, id);
            using (var conn = new SqliteConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    SqliteDataReader record = cmd.ExecuteReader();
                    if (record.Read())
                    { 
                    taskRecord = new TaskRecord(record); 
                    }
                    else
                    {
                        Console.WriteLine("task not found");
                        taskRecord = null;
                    }
                 
                }
                conn.Close();
                return taskRecord;
            }

        }
        
        public void updateTimeForAppointment(string id, string columnName)
        {
            ApiService api = new ApiService(context);
            api.postTimeToApi(id, columnName);
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.updateTimeForAppointment);
            string sqlFormattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            command = String.Format(command, id, columnName, sqlFormattedDate);
            nonQueryToDatabase(command);
        }

        public ArrayList getAllTasksByAppointmentId(string id)
        {
            ArrayList allData = new ArrayList();
            Resources res = this.context.Resources;
            string command = res.GetString(Resource.String.getAllTasksByAppointmentId);
            command = String.Format(command, id);
            using (var conn = new SqliteConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    SqliteDataReader record = cmd.ExecuteReader();
                    while (record.Read())
                    {
                        allData.Add(new TaskRecord(record));
                    }
                    record.Close();

                }
                conn.Close();
            }
            return allData;

        }
    }

}

