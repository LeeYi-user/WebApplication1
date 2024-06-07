using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class UserAdd1Model : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "帳號未輸入")]
        public string user_id { set; get; }

        [BindProperty]
        [Required(ErrorMessage = "密碼未輸入")]
        [DataType(DataType.Password)]
        public string user_password { set; get; }

        [BindProperty]
        public string user_name { set; get; }

        [BindProperty]
        public int? user_age { set; get; }

        [BindProperty]
        public int? user_sex { set; get; }

        [BindProperty]
        public string message { set; get; }

        public void OnGet()
        {
        }

        public void OnPostAdd()
        {
            bool ok = false;
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Member (id, password, name, age, sex) VALUES (@id, @password, @name, @age, @sex)";
                command.Parameters.AddWithValue("id", user_id);
                command.Parameters.AddWithValue("password", user_password);
                command.Parameters.AddWithValue("name", user_name);
                command.Parameters.AddWithValue("age", user_age);
                command.Parameters.AddWithValue("sex", user_sex);
                command.ExecuteNonQuery();

                transaction.Commit();
                ok = true;
            }
            catch
            {
                transaction.Rollback();
                ok = false;
            }

            connection.Close();

            if (ok) Response.Redirect("User");
            else message = "Faild to add new user.";
        }
    }
}
