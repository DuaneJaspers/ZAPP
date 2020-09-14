using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.CustomTabs;
using Android.Views;
using Android.Widget;
using Java.Net;
using Newtonsoft.Json;
using Org.Apache.Http.Client.Params;
using SQLite;

namespace ZAPP.Services
{
    class ApiService
    {
        private Context context;
        private string api_token;
        private string api_address = "http://192.168.1.41";
        //private string api_address = "http://192.168.178.166";
        //private string api_address = "http://192.168.178.65";



        private string api_port = "8080";
        private HttpClient _client;
        public ApiService(Context context)
        {
            this.context = context;
            api_token = context.Resources.GetString(Resource.String.api_token);
            this._client = new HttpClient();
        }

        public async Task<string> RequestUserToken(string username, string password)
        {
            string login_url = api_address + ":" + api_port + "/api/cockpit/authUser";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", api_token);
            string json = JsonConvert.SerializeObject(new loginData(username, password));
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(login_url, content);
            var jsonstring = await response.Content.ReadAsStringAsync();

            JsonValue value = JsonValue.Parse(jsonstring);
            if (value.ContainsKey("api_key"))
                return value["api_key"];
            return "error";
            
        }

        public async Task<JsonValue> getAppointments()
        {
            string appointment_url = api_address + ":" + api_port + "/api/collections/get/appointment";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Singleton.userToken);
            object body = new {populate = 1 };
            string json = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(appointment_url, content);
            var jsonstring = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject(jsonstring);
            JsonValue value = JsonValue.Parse(jsonstring);
            return value["entries"];

        }

        public async void postTimeToApi(string appointment_id, string columnName)
        {
            string appointment_url = api_address + ":" + api_port + "/api/collections/save/appointment";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Singleton.userToken);
            object data;
            if (columnName == "time_start")
            { 
                data = new
                {
                    _id = appointment_id,
                    start_time = DateTime.Now.ToString("HH:mm")
                };
            }
            else
            {
                data = new
                {
                    _id = appointment_id,
                    end_time = DateTime.Now.ToString("HH:mm")
                };
            }
            object body = new
            {
                data = data
            };
            string json = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(appointment_url, content);

        }



        private class loginData
        {
            public string user;
            public string password;

            public loginData(string username, string password)
            {
                this.user = username;
                this.password = password;
            }
        }
    }
}