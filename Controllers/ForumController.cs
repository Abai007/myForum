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
        public IActionResult Deitels(Answer answer)
        {
            Topic topic = new Topic();
            if (answer.UserId != null)
            {
                topic = _db.Topics.FirstOrDefault(t => t.Id == answer.TopicId);
                topic.User = _db.Users.FirstOrDefault(u => u.Id == topic.UserId);
                topic.Answers = _db.Answers.Where(a => a.TopicId == topic.Id).ToList();
            }
            if(answer.UserId == null)
            {
                topic = _db.Topics.FirstOrDefault(t => t.Id == answer.Id);
                topic.User = _db.Users.FirstOrDefault(u => u.Id == topic.UserId);
                topic.Answers = _db.Answers.Where(a => a.TopicId == topic.Id).ToList();
            }
            return View(topic);
        }
        [HttpPost]
        public IActionResult Deitel(string Id, string body)
        {
            int id = int.Parse(Id);
            Answer answer = new Answer();
            answer.AnswerText = body;
            answer.TopicId = id;
            answer.CreateDate = DateTime.Now;
            answer.UserId = CurrentUser().Result.Id;
            _db.Answers.Add(answer);
            _db.SaveChanges();
            return RedirectToAction("Deitels", answer);
        }
    }
}
