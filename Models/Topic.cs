using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorName { get; set; }
        public int AnswerCount { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
