using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acotral.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Acotral.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<News> listNews = new List<News>();
            using (var context = new testContext())
            {
                foreach(var post in context.News.ToList())
                {
                    listNews.Add(post);
                }
            }
            return View(listNews);
        }

        public ActionResult New()
        {
            return View("New");
        }
       
        [HttpPost]
        public async Task<ActionResult> New(News model, List<IFormFile> image1 )
        {
            try
            {               

                if (ModelState.IsValid)
                {
                    using(var context = new testContext())
                    {
                        foreach (var item in image1)
                        {
                            if (item.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await item.CopyToAsync(stream);
                                    model.Images = stream.ToArray();
                                }
                            }
                        }
                        model.Dates = DateTime.Now;
                        context.News.Add(model);
                        context.SaveChanges();
                    }
                    return Redirect("/Home");
                }

                return View(model);
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public ActionResult Update(int Id)
        {
            News model = new News();
            using(var context = new testContext())
            {
                var oNews = context.News.Find(Id);
                model.Title = oNews.Title;
                model.Body = oNews.Body;
                model.Dates = oNews.Dates;
                model.Images = oNews.Images;
                model.Visible = oNews.Visible;
                model.Id = oNews.Id;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(News model, List<IFormFile> image1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new testContext())
                    {
                        foreach (var item in image1)
                        {
                            if (item.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await item.CopyToAsync(stream);
                                    model.Images = stream.ToArray();
                                }
                            }
                        }
                        model.Dates = DateTime.Now;                        
                        context.Entry(model).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    return Redirect("/Home");
                }

                return View(model);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Delete(int Id)
        {
            DeleteViewModel oDelete = new DeleteViewModel();
            oDelete.Id = Id;
            return View(oDelete);
        }

        [HttpPost]
        public ActionResult Delete(DeleteViewModel model)
        {
            using (var context = new testContext())
            {
                var oNews = context.News.Find(model.Id);
                context.Remove(oNews);
                context.SaveChanges();
            }
            return Redirect("/Home");

        }

        public IActionResult News(int Id)
        {
            List<News> listNews = new List<News>();
            using (var context = new testContext())
            {
                foreach (var post in context.News.ToList())
                {
                    if(post.Visible == true)
                    {
                        listNews.Add(post);
                    }                    
                }
            }
            ViewBag.Total = listNews.Count;
            ViewBag.Index = Id;
            return View(listNews);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
