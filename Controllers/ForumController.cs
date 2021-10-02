using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private IWebHostEnvironment _appEnvironment;

        private ForumContext _db;
        public ForumController(UserManager<User> userManager, SignInManager<User> signInManager, ForumContext db, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            List<Topic> topics = (from model in _db.Topics.ToList()
                                            orderby model.CreateDate descending
                                            select model).ToList();
            return View(topics);
        }
        public IActionResult Create()
        {
            return View();
        }
        private async Task<User> CurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost]
        public IActionResult Create(Topic topic)
        {
            User user = CurrentUser().Result;
            Topic topic1 = new Topic();
            topic1.AnswerCount = _db.Answers.Where(a => a.TopicId == topic.Id).ToList().Count;
            topic1.CreateDate = DateTime.Now;
            topic1.Description = topic.Description;
            topic1.Title = topic.Title;
            topic1.AuthorName = topic.AuthorName;
            topic1.UserId = user.Id;
            topic1.User = user;
            topic1.AuthorName = user.Name;
            _db.Topics.Add(topic1);
            _db.SaveChanges();
            return RedirectToAction("Index", "Forum");
        }
        public IActionResult Deitels(int id)
        {
            Topic topic = _db.Topics.FirstOrDefault(t => t.Id == id);
            return View(topic);
        }
    }
}
