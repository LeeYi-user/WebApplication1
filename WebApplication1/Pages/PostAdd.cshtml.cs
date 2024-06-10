using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebApplication1.Pages
{
    public class PostAddModel : PageModel
    {
        [BindProperty]
        public string title { set; get; }

        [BindProperty]
        public string content { set; get; }

        [BindProperty]
        public string message { set; get; }

        public void OnGet()
        {
        }

        public void OnPostConfirm()
        {
            bool ok = false;
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [Posts] (title, content, date, user, comments) VALUES (@title, @content, @date, @user, 0)";
                command.Parameters.AddWithValue("title", title);
                command.Parameters.AddWithValue("content", content);
                command.Parameters.AddWithValue("date", DateTime.Now.ToString("yyyy/M/d"));
                command.Parameters.AddWithValue("user", HttpContext.Session.GetString("userId") == null ? "Guest" : HttpContext.Session.GetString("userId"));
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

            if (ok) Response.Redirect("Posts");
            else message = "Faild to add new user.";
        }
    }
}
