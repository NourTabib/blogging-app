using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext AppDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext con, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            AppDbContext = con;
            _userManager = userManager;
        }

        public ApplicationDbContext GetApplicationDbContext()
        {
            return AppDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var Blogs = await AppDbContext.Blogs.ToListAsync();
            return View(Blogs);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                // Get the post from the database
                var poster = AppDbContext.Posts.Find(model.PostId);

                // Create a new comment
                var comment = new Commentaire
                {
                    post = poster,
                    containt = model.Content,
                    postedOn = DateTime.Now,
                    author = user,
                };
                if(poster.Commentaires == null)
                {
                    poster.Commentaires = new List<Commentaire>();
                }
                // Add the comment to the post's list of comments
                poster.Commentaires.Add(comment);

                // Save the changes to the database
                await AppDbContext.SaveChangesAsync();

                return View("PostDetails", poster);
            }
            var post = AppDbContext.Posts.Find(model.PostId);
            return View("PostDetails", post);
        }
        public ActionResult PostDetails(int postId)
        {
            var post = AppDbContext.Posts.Include(p => p.Commentaires).FirstOrDefault(p => p.PostId == postId);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        public async Task<IActionResult> BlogDetails(int? id)
        {
            if (id == null || AppDbContext.Blogs == null)
            {
                return NotFound();
            }

            var blog = await AppDbContext.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
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