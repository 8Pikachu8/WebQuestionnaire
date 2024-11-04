using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using WebQuestionnaire.Models;

namespace WebQuestionnaire.Controllers
{

    public class CreatePost
    {
        public string ImageCaption { set; get; }
        public string ImageDescription { set; get; }
        public IFormFile MyImage { set; get; }
    }

    public class HomeController : Controller
    {

        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult TestEnumerable()
        {
            List<string> list = new List<string>() { "sldkfjlsd", "Hi", "Hello"};
            List<int> list2 = new List<int>() { 2, 4, 5 };
            return View(list2);
        }

        public IActionResult Index()
        {
            int b = 5555;
            string g = "lsdfjlkds";
            return View(b);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Opros()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View(new CreatePost());
        }

        [HttpPost]
        public IActionResult Create(CreatePost model)
        {
            // do other validations on your model as needed
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   
            }
            // to do  : Return something
            return RedirectToAction("Index", "Home");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        [HttpPost]
        public async Task<IActionResult> Submit( string feedback, IFormFile file, IFormFile file2)
        {
            if (file != null && file.Length > 0 && file2 != null && file2.Length > 0)
            {
                // Путь, куда вы хотите сохранить файл
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                var filePath = Path.Combine(uploadDir, file.FileName);

                var filePath2 = Path.Combine(uploadDir, file2.FileName);
                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await file2.CopyToAsync(stream);
                }
            }

            // Здесь вы можете добавить логику для обработки других полей (name, email, feedback)
            // Например, сохранить их в базе данных

            // Перенаправление на другую страницу или возвращение результата
            return RedirectToAction("Index");
        }
    }
}
