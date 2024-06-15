using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace WebApplication1.Pages
{
    public class SQLiteModel : PageModel
    {
        [BindProperty]
        public DataTable dataTable { get; set; }

        [BindProperty]
        public string command { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostCommand()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var sqliteCommand = connection.CreateCommand();
            sqliteCommand.CommandText = command;

            try
            {
                if (command.Split(" ")[0].ToUpper() == "SELECT")
                {
                    var reader = sqliteCommand.ExecuteReader();
                    dataTable = new DataTable();
                    dataTable.Load(reader);
                    connection.Close();
                    return Content(DataTableToJson(dataTable));
                }
                else
                {
                    sqliteCommand.ExecuteNonQuery();
                    connection.Close();
                    return Content("[SUCCESS]");
                }
            }
            catch
            {
                connection.Close();
                return Content("[FAIL]");
            }
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
