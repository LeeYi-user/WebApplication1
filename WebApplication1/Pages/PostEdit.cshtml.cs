using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class PostEditModel : PageModel
    {
        [BindProperty]
        public string post_id { get; set; }

        [BindProperty]
        public string title { set; get; }

        [BindProperty]
        public string content { set; get; }

        [BindProperty]
        public string message { set; get; }

        public void OnGet(string id)
        {
            post_id = id;
            ShowPost(id);
        }

        private void ShowPost(string id)
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Posts] WHERE id = @id";
            command.Parameters.AddWithValue("id", id);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                title = reader[1].ToString();
                content = reader[2].ToString();
            }
            connection.Close();
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
                command.CommandText = @"UPDATE [Posts] SET title = @title, content = @content WHERE id = @post_id";
                command.Parameters.AddWithValue("title", title);
                command.Parameters.AddWithValue("content", content);
                command.Parameters.AddWithValue("post_id", post_id);
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
            else message = "Faild to add new picture.";
        }
    }
}
