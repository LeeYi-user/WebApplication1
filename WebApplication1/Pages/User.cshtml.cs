using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class UserModel : PageModel
    {
        [BindProperty]
        public string user_id { get; set; }

        [BindProperty]
        public DataTable dataTable { get; set; }

        public void OnGet()
        {
            user_id = HttpContext.Session.GetString("userId");
            if (HttpContext.Session.GetString("userId") == null) Response.Redirect("Login3");
            else ShowUsers();
        }

        private void ShowUsers()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Member]";
            var reader = command.ExecuteReader();
            dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            connection.Close();
        }

        public void OnPostAdd()
        {
            Response.Redirect("UserAdd");
        }

        public void OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            Response.Redirect("Login3");
        }
    }
}
