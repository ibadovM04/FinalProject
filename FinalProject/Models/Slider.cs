﻿using FinalProject.Model;

namespace FinalProject.Models
{
    public class Slider : Entity<int>
    {
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Link { get; set; }
        public string BackgroundImageUrl { get; set; }

    }
}
