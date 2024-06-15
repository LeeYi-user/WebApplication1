using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text.Json;

namespace WebApplication1.Pages
{
    public class PostModel : PageModel
    {
        [BindProperty]
        public string post_id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        [BindProperty]
        public DataTable dataTable { get; set; }

        [BindProperty]
        public string comment { get; set; }

        public void OnGet(string id)
        {
            post_id = id;

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

        private void ShowComments(string id)
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM [Comments] WHERE post_id = @id";
            command.Parameters.AddWithValue("id", id);

            using (var reader = command.ExecuteReader())
            {
                dataTable = new DataTable();
                dataTable.Load(reader);
            }

            connection.Close();
        }

        public IActionResult OnPostComment()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [Comments] (post_id, content, user) VALUES (@post_id, @content, @user)";
                command.Parameters.AddWithValue("post_id", post_id);
                command.Parameters.AddWithValue("content", comment);
                command.Parameters.AddWithValue("user", HttpContext.Session.GetString("userName") == null ? "³X«È" : HttpContext.Session.GetString("userName"));
                command.ExecuteNonQuery();

                ShowComments(post_id);

                command = connection.CreateCommand();
                command.CommandText = @"UPDATE [Posts] SET comments = @comments WHERE id = @post_id";
                command.Parameters.AddWithValue("comments", dataTable.Rows.Count + 1);
                command.Parameters.AddWithValue("post_id", post_id);
                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }

            connection.Close();
            ShowComments(post_id);

            return Content(DataTableToJson(dataTable));
        }

        private string DataTableToJson(DataTable table)
        {
            // Convert DataTable to a list of dictionaries
            var rows = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                rows.Add(dict);
            }

            // Serialize the list of dictionaries to JSON
            return JsonSerializer.Serialize(rows);
        }
    }
}
