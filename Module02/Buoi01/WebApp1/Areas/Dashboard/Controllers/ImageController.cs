using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp1.Controllers;
using WebApp1.Models;

namespace WebApp1.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class ImageController : BaseController
    {
        public ImageController(SiteProvider provider) : base(provider) { }
        
        public IActionResult WebUrl()
        {
            return View();
        }
        [HttpPost]
        public IActionResult WebUrl(string url)
        {
            string extension = Path.GetExtension(url);
            string imageUrl = Helper.Helper.RandomString(32 - extension.Length) + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageUrl);
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
            Image image = new Image
            {
                Id = Guid.NewGuid(),
                Original = Path.GetFileName(url),
                Size = 0,
                Type = extension,
                Url = imageUrl
            };
            provider.Image.Add(image);
            return Redirect("/dashboard/image");
        }
        public IActionResult Index()
        {
            return View(provider.Image.GetImages());
        }
        public IActionResult Delete(Guid id)
        {
            Image image = provider.Image.GetImageById(id);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", image.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                provider.Image.Delete(image);
            }
            return Redirect("/dashboard/image");
        }

        public IActionResult Ajax(IFormFile f)
        {
            if (f != null)
            {
                string extension = Path.GetExtension(f.FileName);
                string imageUrl = Helper.Helper.RandomString(32 - extension.Length) + extension;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageUrl);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    f.CopyTo(stream);
                }
                Image image = new Image
                {
                    Id = Guid.NewGuid(),
                    Original = f.FileName,
                    Url = imageUrl,
                    Size = f.Length,
                    Type = f.ContentType
                };
                provider.Image.Add(image);
                return Json(image);

            }
            return NotFound();
        }
        public IActionResult Icon()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Icon(IFormFile f)
        {
            return Create(f);
        }
        public IActionResult Multiple()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Multiple(IFormFile[] mf)
        {
            if (mf !=null)
            {
                string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                List<Image> list = new List<Image>();
                foreach (IFormFile item in mf)
                {
                    string extension = Path.GetExtension(item.FileName);
                    string imageUrl = Helper.Helper.RandomString(32 - extension.Length) + extension;
                    using (Stream stream = new FileStream(Path.Combine(root,imageUrl),FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    list.Add(new Image
                    {
                        Id = Guid.NewGuid(),
                        Original = item.FileName,
                        Size = item.Length,
                        Url = imageUrl,
                        Type = item.ContentType
                    });
                    
                }
                provider.Image.Add(list);
                return Redirect("/dashboard/image");
            }
            ModelState.AddModelError("", "Please choos images");
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IFormFile f)
        {
            if (f!= null)
            {
                string imageUrl = Helper.Helper.RandomString(28) + Path.GetExtension(f.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageUrl);
                using(FileStream stream =  new FileStream(path, FileMode.Create))
                {
                    f.CopyTo(stream);
                }
                Image image = new Image
                {
                    Id = Guid.NewGuid(),
                    Original = f.FileName,
                    Url = imageUrl,
                    Size = f.Length,
                    Type = f.ContentType
                };
                provider.Image.Add(image);
                return Redirect("/dashboard/image");

            }
            ModelState.AddModelError("", "Please choose image");
            return View();
        }
    }
}
