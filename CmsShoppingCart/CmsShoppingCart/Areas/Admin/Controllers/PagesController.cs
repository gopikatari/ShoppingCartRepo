using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PagesVM> pageList;
            using (Db db = new Db())
            {
                //pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PagesVM(x)).ToList();
                pageList = db.Pages.ToList().Select(x => new PagesVM(x)).ToList();
            }
            return View(pageList);
        }
        // GET: Admin/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPage(PagesVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string slug;
            PageDTO dto = new PageDTO();
            using (Db db = new Db())
            {
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(' ', '-').ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(' ', '-').ToLower();
                }

                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "the Entered Title or Slug already Exists:");
                    return View(model);
                }
                dto.Slug = slug;
                dto.Title = model.Title;
                dto.Body = model.Body;
                dto.Sorting = 100;
                dto.HasSideBar = model.HasSideBar;
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            TempData["SM"] = "You have Added a Page";
            return RedirectToAction("AddPage");
        }
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PagesVM model;
            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    return Content("This Page does not exist");
                }
                model = new PagesVM(dto);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult EditPage(PagesVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int id = model.ID;
            string slug = string.Empty;

            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                dto.Title = model.Title;
                if (model.Title != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //make suere  Title and slug should be unique
                if (db.Pages.Where(x => x.ID != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.ID != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "The Title or Slug already Exists");
                    return View(model);
                }
                dto.Slug = model.Slug;
                dto.Body = model.Body;
                dto.HasSideBar = model.HasSideBar;
                db.SaveChanges();
                TempData["SM"] = " You Have Edited the Page";
            }
            return RedirectToAction("EditPage");
        }

        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            PagesVM model;
            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    return Content("This Page does not exist");
                }
                model = new PagesVM(dto);
            }
            return View(model);
        }
        public ActionResult DeletePage(int id)
        {

            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    return Content("This Page does not exist");
                }
                db.Pages.Remove(dto);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void Reorderpages(int[] Id)
        {
            int count = 1;
            PageDTO dto;
            using (Db db = new Db())
            {
                foreach (var pageId in Id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model;
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                model = new SidebarVM(dto);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                dto.Body = model.Body;

                db.SaveChanges();

            }
            TempData["SM"] = "you have Edited the sidebar";
            return RedirectToAction("EditSidebar");
        }
    }
}