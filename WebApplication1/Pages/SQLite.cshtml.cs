using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class SQLiteModel : PageModel
    {
        [BindProperty]
        public string message { get; set; }

        public void OnGet()
        {

        }

        public void OnPostCreate()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE user (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void OnPostInsert()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            //command.CommandText = @"INSERT INTO user VALUES (1, 'Bruce') (2, 'Alex') (3, 'Nat');";
            command.CommandText = @"INSERT INTO user (name) VALUES ('John');";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void OnPostUpdate()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE user SET name = 'Bill' WHERE name = 'John'";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void OnPostSelect()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            //command.CommandText = @"SELECT id FROM user WHERE name = $name";
            //command.Parameters.AddWithValue("name", "Bill");
            command.CommandText = @"SELECT name FROM user";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    message += reader.GetString(0) + " ";
                }
            }

            connection.Close();
        }

        public void OnPostDelete()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM user WHERE name = $name";
            command.Parameters.AddWithValue("name", "Bill");
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void OnPostTransaction()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM user WHERE name = $name";
                command.Parameters.AddWithValue("name", "Bill");
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                message = "Failed to delete.";
            }

            connection.Close();
        }
    }
}
