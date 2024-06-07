using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class UserEdit1Model : PageModel
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

        public void OnGet(string id)
        {
            ShowUser(id);
        }

        private void ShowUser(string id)
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Member] WHERE id = @id";
            command.Parameters.AddWithValue("id", id);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                user_id = reader[0].ToString();
                //user_password = reader[1].ToString();
                user_name = reader[2].ToString();
                user_age = int.Parse(reader[3].ToString());
                user_sex = int.Parse(reader[4].ToString());
            }
            connection.Close();
        }

        public void OnPostUpdate()
        {
            bool ok = false;
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE [Member] SET password = @password, name = @name, age = @age, sex = @sex WHERE id = @id";
                command.Parameters.AddWithValue("password", user_password);
                command.Parameters.AddWithValue("name", user_name);
                command.Parameters.AddWithValue("age", user_age);
                command.Parameters.AddWithValue("sex", user_sex);
                command.Parameters.AddWithValue("id", user_id);
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
            else message = "Faild to update this user.";
        }
    }
}
