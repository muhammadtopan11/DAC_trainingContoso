using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.ViewModels;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class KursusController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Kursus
        public ActionResult Index()
        {
            ViewBag.courses = db.Courses.ToList();
            CourseSearchVM model = new CourseSearchVM();
            return View(model);
        }

        public ActionResult IndexProses(CourseSearchVM model, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.CreditSortParm = sortOrder == "Credit" ? "credit_desc" : "Credit";

            IEnumerable<Course> courses= db.Courses;

            switch (sortOrder)
            {
                case "title_desc":
                    courses = courses.OrderByDescending(x => x.Title);
                    break;
                case "Credit":
                    courses = courses.OrderBy(x => x.Credits);
                    break;
                case "credit_desc":
                    courses = courses.OrderByDescending(x => x.Credits);
                    break;
                default:
                    courses = courses.OrderBy(s => s.Title);
                    break;
            }

            if (!String.IsNullOrEmpty(model.Title))
            {
                courses = courses.Where(x => x.Title.Contains(model.Title));
            }
            if (model.Credits != 0)
            {
                courses = courses.Where(x => x.Credits == model.Credits);
            }

            ViewBag.courses = courses.ToList();

            return View("Index", model);

        }

        // GET: Kursus/Details/5
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

        // GET: Kursus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kursus/Create
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

        // GET: Kursus/Edit/5
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

        // POST: Kursus/Edit/5
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

        // GET: Kursus/Delete/5
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

        // POST: Kursus/Delete/5
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
