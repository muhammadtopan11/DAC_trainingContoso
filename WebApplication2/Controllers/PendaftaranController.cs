using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class PendaftaranController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Pendaftaran
        public ActionResult Index()
        {
            //var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student).ToList();
            ViewBag.enrollments = db.Enrollments.ToList();
            EnrollmentSearchVM model = new EnrollmentSearchVM();
            return View(model);
        }

        public ActionResult IndexProses(EnrollmentSearchVM model, string sortOrder)
        {
            //var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Last" ? "last_desc" : "Last";
            ViewBag.GradeSortParm = sortOrder == "Grade" ? "grade_desc" : "Grade";

            IEnumerable<Enrollment> enrollments= db.Enrollments;

            switch (sortOrder)
            {
                case "Title_desc":
                    enrollments = enrollments.OrderByDescending(x => x.Course.Title);
                    break;
                case "last_desc":
                    enrollments = enrollments.OrderBy(x => x.Student.LastName);
                    break;
                case "Last":
                    enrollments = enrollments.OrderByDescending(x => x.Student.LastName);
                    break;
                case "Grade":
                    enrollments = enrollments.OrderBy(x => x.Grade);
                    break;
                case "grade_desc":
                    enrollments = enrollments.OrderByDescending(x => x.Grade);
                    break;
                default:
                    enrollments = enrollments.OrderBy(s => s.Course.Title);
                    break;
            }

            if (!String.IsNullOrEmpty(model.LastName))
            {
                enrollments = enrollments.Where(x => x.Student.LastName.Contains(model.LastName));
            }
            if (!String.IsNullOrEmpty(model.Title))
            {
                enrollments = enrollments.Where(x => x.Course.Title.Contains(model.Title));
            }
            if (model.GradeFrom != null && model.GradeUntil != null)
            {
                enrollments = enrollments.Where(x => x.Grade >= model.GradeFrom && x.Grade <= model.GradeUntil);
            }

            ViewBag.enrollments = enrollments.ToList();
            return View("Index", model);
        }

        // GET: Pendaftaran/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Pendaftaran/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName");
            return View();
        }

        // POST: Pendaftaran/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Pendaftaran/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Pendaftaran/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Pendaftaran/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Pendaftaran/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
