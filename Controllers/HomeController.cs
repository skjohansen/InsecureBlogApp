using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InsecureWebApp.Models;
using InsecureWebApp.DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace InsecureWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;


        }

        private List<BlogPost> GetPosts()
        {
            List<BlogPost> blogposts = new List<BlogPost>();
            using (var db = new BloggingContext())
            {
                blogposts = db.Posts.AsNoTracking().OrderBy(b => b.PostId).Select( t=> new BlogPost {Post =t.Content, Title = t.Title }).ToList();
            }

            return blogposts;
        }

        public IActionResult Index()
        {
            return View(GetPosts());
        }

        [HttpPost]
        public IActionResult Index(string blogTitle, string blogpost)
        {

            using (var db = new BloggingContext())
            {
                db.Add(
                    new Post
                    {
                        Title = blogTitle,
                        Content = blogpost
                    });
                db.SaveChanges();
            }

            return View(GetPosts());
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
