using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class Login3Model : PageModel
    {
        public string message { set; get; }

        [BindProperty]
        [Required(ErrorMessage = "帳號未輸入")]
        public string user_id { set; get; }

        [BindProperty]
        [Required(ErrorMessage = "密碼未輸入")]
        public string user_password { set; get; }

        public void OnGet()
        {
        }

        public void OnPostLogin()
        {
            bool ok = false;

            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"select [id],[password],[name] from [Member] where [id] = @id and [password] = @password";
            command.Parameters.AddWithValue("id", user_id);
            command.Parameters.AddWithValue("password", user_password);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    ok = true;

                    reader.Read();
                    string user_name = reader[2].ToString();

                    HttpContext.Session.SetString("userId", user_id);
                    HttpContext.Session.SetString("userName", user_name);
                }
                else
                {
                    ok = false;
                    message = "faild to login.";
                }
            }

            connection.Close();
            if (ok) Response.Redirect("User");
        }
    }
}
