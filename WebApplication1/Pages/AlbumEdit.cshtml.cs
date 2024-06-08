using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Pages
{
    public class AlbumEditModel : PageModel
    {
        [BindProperty]
        public string picture_id { get; set; }

        [BindProperty]
        public string picture_title { get; set; }

        [BindProperty]
        public string picture_description { get; set; }

        [BindProperty]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime picture_time { get; set; }

        [BindProperty]
        public string message { get; set; }

        private IWebHostEnvironment _environment;

        public AlbumEditModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet(string id)
        {
            ShowUser(id);
        }

        private void ShowUser(string id)
        {
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM [Picture2] WHERE id = @id";
            command.Parameters.AddWithValue("id", id);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                picture_id = id;
                picture_title = reader[1].ToString();
                picture_description = reader[2].ToString();
                picture_time = DateTime.ParseExact(reader[3].ToString(), "yyyy/M/d tt h:mm:ss", null, System.Globalization.DateTimeStyles.None);
            }
            connection.Close();
        }

        public void OnPostUpdate()
        {
            bool ok = false;
            var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE Picture2 SET title = @title, description = @description, time = @time WHERE id = @id";
                command.Parameters.AddWithValue("title", picture_title);
                command.Parameters.AddWithValue("description", picture_description);
                command.Parameters.AddWithValue("time", picture_time.ToString());
                command.Parameters.AddWithValue("id", picture_id);
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

            if (ok) Response.Redirect("Album");
            else message = "Faild to add new picture.";
        }
    }
}
