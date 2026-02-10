using System;

namespace ProjectParadise2
{
    internal class NewsEntry
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Content { get; set; }
    }
}