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
            if (command == ".tables")
            {
                command = "SELECT name FROM sqlite_schema WHERE type ='table' AND name NOT LIKE 'sqlite_%'";
            }

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
                    return Content(ConvertDataTableToHTML(dataTable));
                }
                else
                {
                    sqliteCommand.ExecuteNonQuery();
                    connection.Close();
                    return Content("<pre class=\"text-success border-success\">SUCCESS</pre>");
                }
            }
            catch (Exception e)
            {
                connection.Close();
                return Content("<pre class=\"text-danger border-danger\">" + e.ToString() + "</pre>");
            }
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table class=\"table table-bordered\">";
            //add header row
            html += "<thead class=\"table-light\"><tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<th>" + dt.Columns[i].ColumnName + "</th>";
            html += "</tr></thead><tbody>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</tbody></table>";
            return html;
        }
    }
}
