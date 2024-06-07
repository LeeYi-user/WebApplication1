using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class Login1Model : PageModel
    {
        public string message { set; get; }

        [BindProperty]
        public string user_id { set; get; }

        [BindProperty]
        public string user_password { set; get; }

        public void OnGet()
        {
        }

        public void OnPostLogin()
        {
            if (user_id == "leeyi" && user_password == "123456")
            {
                //message = "success to login.";
                Response.Redirect("Index");
            }
            else
            {
                message = "faild to login.";
            }
        }
    }
}
