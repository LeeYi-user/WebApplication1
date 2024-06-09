using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Data;

namespace WebApplication1.Pages
{
    public class PostsModel : PageModel
    {
        [BindProperty]
        public DataTable dataTable { get; set; }

        public void OnGet()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Posts]";
            var reader = command.ExecuteReader();
            dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            connection.Close();
        }

        public void OnPostUpdate(string id)
        {
            Response.Redirect("PostEdit?id=" + id);
        }

        public void OnPostDelete(string id)
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM [Posts] WHERE id = $id";
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }

            connection.Close();

            Response.Redirect("Posts");
        }
    }
}
