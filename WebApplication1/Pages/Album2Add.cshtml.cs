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
    public class Album2AddModel : PageModel
    {
        [BindProperty]
        public string picture_title { get; set; }

        [BindProperty]
        public string picture_description { get; set; }

        [BindProperty]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime picture_time { get; set; }

        [BindProperty]
        public IFormFile picture_filename { get; set; }

        [BindProperty]
        public string message { get; set; }

        private IWebHostEnvironment _environment;

        public Album2AddModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
            picture_time = DateTime.Now;
        }
        public async Task OnPostAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await picture_filename.CopyToAsync(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                string base64 = Convert.ToBase64String(bytes);

                bool ok = false;
                var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Picture2 (title, description, time, filename, data) VALUES (@title, @description, @time, @filename, @data)";
                    command.Parameters.AddWithValue("title", picture_title);
                    command.Parameters.AddWithValue("description", picture_description);
                    command.Parameters.AddWithValue("time", picture_time.ToString());
                    command.Parameters.AddWithValue("filename", picture_filename.FileName);
                    command.Parameters.AddWithValue("data", base64);
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

                if (ok) Response.Redirect("Album2");
                else message = "Faild to add new picture.";
            }
        }
    }
}
