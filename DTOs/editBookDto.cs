﻿namespace DTOs
{
    public class editBookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Pages { get; set; }
        public string Price { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string Categories { get; set; }
        public string Publisher { get; set; }
    }
}
