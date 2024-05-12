using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class PostGetModel : PageModel
    {
        public string message { set; get; }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            message = "Hello World";
        }
    }
}
