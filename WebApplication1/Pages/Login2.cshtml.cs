using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Pages
{
    public class Login2Model : PageModel
    {
        public string message { set; get; }

        [BindProperty]
        [Required(ErrorMessage = "帳號未輸入")]
        public string user_id { set; get; }

        [BindProperty]
        [Required(ErrorMessage = "密碼未輸入")]
        public string user_password { set; get; }

        public void OnGet()
        {
        }

        public void OnPostLogin()
        {
            /*if (user_id == "leeyi" && user_password == "123456")
            {
                //message = "success to login.";
                HttpContext.Session.SetString("userId", user_id);
                Response.Redirect("User");
            }
            else
            {
                message = "faild to login.";
            }*/

            bool ok = false;
            string connection = @"Data Source=DESKTOP-1IK2TJR\SQLEXPRESS;Initial Catalog=MyDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //string query = "select [id],[password] from [Member] where [id] = '" + user_id + "' and [password] = '" + user_password + "'";
            string query = "select [id],[password],[name] from [Member] where [id] = @id and [password] = @password";

            SqlConnection sqlConnection = new SqlConnection(connection);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", user_id);
            sqlCommand.Parameters.AddWithValue("@password", user_password);
            sqlConnection.Open();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            if (sdr.HasRows)
            {
                ok = true;
                HttpContext.Session.SetString("userId", user_id);
            }
            else
            {
                ok = false;
                message = "faild to login.";
            }
            sqlConnection.Close();

            if (ok) Response.Redirect("User");
        }
    }
}
