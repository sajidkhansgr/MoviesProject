using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Release_Year { get; set; }
        public string Locations { get; set; }
        public string Production_Company { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actor_1 { get; set; }
        public string Actor_2 { get; set; }
        public string Actor_3 { get; set; }
    }
}
