using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieUI.Models;

namespace MovieUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ReleaseSortParm"] = String.IsNullOrEmpty(sortOrder) ? "release_desc" : "";
            ViewData["LocationSortParm"] = String.IsNullOrEmpty(sortOrder) ? "location_desc" : "";
            ViewData["DistributorSortParm"] = String.IsNullOrEmpty(sortOrder) ? "distributor_desc" : "";
            ViewData["DirectorSortParm"] = String.IsNullOrEmpty(sortOrder) ? "director_desc" : "";
            ViewData["WriterSortParm"] = String.IsNullOrEmpty(sortOrder) ? "writer_desc" : "";

            HttpClient client = new HttpClient();
            var result = client.GetAsync("https://data.sfgov.org/resource/yitu-d5am.json").Result;

            IEnumerable<Movie> movies = result.Content.ReadAsAsync<List<Movie>>().Result;
            var movieList = from s in movies
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                movieList = movieList.OfType<Movie>().Where(s => s.Title.Contains(searchString)||
                s.Locations.Contains(searchString) ||
                s.Release_Year.Contains(searchString) ||
                s.Director.Contains(searchString) ||
                s.Writer.Contains(searchString) ||
                s.Production_Company.Contains(searchString)


                );
                
            }


            switch (sortOrder)
            {
                case "title_desc":
                    movieList = movieList.OrderByDescending(s => s.Title);
                    break;
                case "title_asc":
                    movieList = movieList.OrderBy(s => s.Title);
                    break;
                case "release_desc":
                    movieList = movieList.OrderByDescending(s => s.Release_Year);
                    break;
                case "release_asc":
                    movieList = movieList.OrderBy(s => s.Release_Year);
                    break;
                case "location_desc":
                    movieList = movieList.OrderByDescending(s => s.Locations);
                    break;
                case "location_asc":
                    movieList = movieList.OrderBy(s => s.Locations);
                    break;
                case "distributor_desc":
                    movieList = movieList.OrderByDescending(s => s.Production_Company);
                    break;
                case "distributor_asc":
                    movieList = movieList.OrderBy(s => s.Production_Company);
                    break;
                case "director_desc":
                    movieList = movieList.OrderByDescending(s => s.Director);
                    break;
                case "director_asc":
                    movieList = movieList.OrderBy(s => s.Director);
                    break;
                case "writer_desc":
                    movieList = movieList.OrderByDescending(s => s.Writer);
                    break;
                case "writer_asc":
                    movieList = movieList.OrderBy(s => s.Writer);
                    break;
                default:
                    movieList = movieList.OrderBy(s => s.Title);
                    break;
            }

            return View(movieList);
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
