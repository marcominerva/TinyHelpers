using System;
using System.Collections.Generic;

namespace TinyNetHelpers.EntityFrameworkCore.Sample.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public DateTime Date { get; set; }

        public IList<Review> Reviews { get; set; }
    }

    public class Review
    {
        public string User { get; set; }

        public DateTime Date { get; set; }

        public int Score { get; set; }
    }
}
