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
    public class AlbumAdd1Model : PageModel
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

        public AlbumAdd1Model(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
            picture_time = DateTime.Now;
        }

        /*public void OnPostAdd()
        {
            string file = Path.Combine(_environment.ContentRootPath, @"wwwroot\Pictures", picture_filename.FileName);
            //message = file;

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                picture_filename.CopyToAsync(fileStream);
            }
        }*/

        public async Task OnPostAsync()
        {
            string file = Path.Combine(_environment.ContentRootPath, @"wwwroot\Pictures", picture_filename.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await picture_filename.CopyToAsync(fileStream);

                bool ok = false;
                var connection = new SqliteConnection(@"data source=Databases\MyDB.db");
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Picture (title, description, time, filename) VALUES (@title, @description, @time, @filename)";
                    command.Parameters.AddWithValue("title", picture_title);
                    command.Parameters.AddWithValue("description", picture_description);
                    command.Parameters.AddWithValue("time", picture_time.ToString());
                    command.Parameters.AddWithValue("filename", picture_filename.FileName);
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

                if (ok) Response.Redirect("Album1");
                else message = "Faild to add new picture.";
            }
        }
    }
}
