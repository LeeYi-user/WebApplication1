using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class DownloadModel : PageModel
    {
        public ActionResult OnGet(int id)
        {
            string filename = "";
            string filedata = "";

            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Picture2] WHERE id=@id";
            command.Parameters.AddWithValue("id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    filename = reader[4].ToString();
                    filedata = reader[5].ToString();
                }
            }
            connection.Close();

            byte[] decodedByteArray = Convert.FromBase64String(filedata);
            return File(decodedByteArray + filedata, "image/jpeg", filename);
        }
    }
}
