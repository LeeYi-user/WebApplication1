using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Data;

namespace WebApplication1.Pages
{
    public class PostModel : PageModel
    {
        public string title { get; set; }

        public string content { get; set; }

        [BindProperty]
        public DataTable dataTable { get; set; }

        public void OnGet(string id)
        {
            ShowPost(id);
            ShowComments(id);
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

        private void ShowComments(string post_id)
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM [Comments] WHERE post_id = @post_id";
            command.Parameters.AddWithValue("post_id", post_id);

            using (var reader = command.ExecuteReader())
            {
                dataTable = new DataTable();
                dataTable.Load(reader);
            }

            connection.Close();
        }
    }
}
