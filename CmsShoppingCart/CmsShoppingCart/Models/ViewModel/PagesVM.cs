using CmsShoppingCart.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CmsShoppingCart.Models.ViewModel
{
    public class PagesVM
    {
        public PagesVM()
        {

        }
        public PagesVM(PageDTO row)
        {
            ID = row.ID;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSideBar = row.HasSideBar;
        }
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 5)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSideBar { get; set; }
    }
}