using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class FileUploadModel : PageModel
    {
        [BindProperty]
        public List<IFormFile> files { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Directory.GetCurrentDirectory() + @"\wwwroot\fileupload\" + formFile.FileName;
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }


            return RedirectToPage("AccessItem");
        }
    }
}
