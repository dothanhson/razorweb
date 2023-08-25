using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.Models;

namespace razorweb.Pages_Blog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly razorweb.Models.MyBlogContext _context;

        public IndexModel(razorweb.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }

        public const int ITEMS_PER_PAGE = 5;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage {get; set;}
        public int countPages {get; set;}



        public async Task OnGetAsync(string SearchString)
        {
            //Article = await _context.articles.ToListAsync();

            int totalArticle = await _context.articles.CountAsync();
            countPages = (int)Math.Ceiling((double)totalArticle/ITEMS_PER_PAGE);

            if(currentPage <1)
                currentPage = 1;
            if(currentPage >countPages)
                currentPage = countPages;

            var qr = (from a in _context.articles
                    orderby a.Created descending
                    select a).Skip((currentPage-1)*ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE);
            
            if(!string.IsNullOrEmpty(SearchString))
            {
                Article = await qr.Where(a => a.Title.Contains(SearchString)).ToListAsync();
            }
            else{
                Article = await qr.ToListAsync();
            }
            
        }
    }
}
