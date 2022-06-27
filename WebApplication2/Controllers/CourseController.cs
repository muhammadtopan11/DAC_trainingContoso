using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        public ActionResult Index(string sortOrder, string currentFilterTitle, string currentFiltercredits
                                , string SearchTitle, string SearchCredits, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.CreditsSortParm = sortOrder == "Credits" ? "credits_desc" : "Credits";


            if (SearchTitle != null)
            {
                page = 1;
            }
            else
            {
                SearchTitle = currentFilterTitle;
            }

            ViewBag.CurrentFilterTitle = SearchTitle;

            if (SearchCredits != null)
            {
                page = 1;
            }
            else
            {
                SearchCredits = currentFiltercredits;
            }

            ViewBag.CurrentFiltercredits = SearchCredits;



            var cource = from s in db.Courses
                           select s;
            if (!String.IsNullOrEmpty(SearchTitle))
            {
                cource = cource.Where(s => s.Title.Contains(SearchTitle));
            }
            if (!String.IsNullOrEmpty(SearchCredits))
            {
                cource = cource.Where(s => s.Credits.ToString().Contains(SearchCredits));
            }


            switch (sortOrder)
            {
                case "title_desc":
                    cource = cource.OrderByDescending(s => s.Title);
                    break;
                case "Credits":
                    cource = cource.OrderBy(s => s.Credits);
                    break;
                case "credits_desc":
                    cource = cource.OrderByDescending(s => s.Credits);
                    break;
                default:
                    cource = cource.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(cource.ToPagedList(pageNumber, pageSize));
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
