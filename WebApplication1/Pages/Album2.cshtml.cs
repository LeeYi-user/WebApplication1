using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class Album2Model : PageModel
    {
        [BindProperty]
        public DataTable dataTable { get; set; }

        public void OnGet()
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Picture2]";
            var reader = command.ExecuteReader();
            dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            connection.Close();
        }

        public void OnPostAdd()
        {
            Response.Redirect("Album2Add");
        }
    }
}
