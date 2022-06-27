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
    public class SiswaController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Siswa
        public ActionResult Index()
        {
            ViewBag.students = db.Students.ToList();
            StudentSearchVM model = new StudentSearchVM();
            return View(model);
        }
        public ActionResult IndexProses(StudentSearchVM model, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.MidSortParm = sortOrder == "Mid" ? "mid_desc" : "Mid";

            IEnumerable<Student> students = db.Students;
            
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(x => x.LastName);
                    break;
                case "mid_desc":
                    students = students.OrderBy(x => x.FirstMidName);
                    break;
                case "Mid":
                    students = students.OrderByDescending(x => x.FirstMidName);
                    break;
                case "Date":
                    students = students.OrderBy(x => x.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(x => x.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            if (model.EnrollmentDateFrom != null && model.EnrollmentDateUntil != null)
            {
                students = students.Where(x => x.EnrollmentDate >= model.EnrollmentDateFrom && x.EnrollmentDate <= model.EnrollmentDateUntil);
            }
            if (!String.IsNullOrEmpty(model.LastName))
            {
                students = students.Where(x => x.LastName.Contains(model.LastName));
            }
            if (!String.IsNullOrEmpty(model.FirstMidName))
            {
                students = students.Where(x => x.FirstMidName.Contains(model.FirstMidName));
            }

            ViewBag.students = students.ToList();
            return View("Index", model);
        }

        // GET: Siswa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Siswa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Siswa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate,Secret")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Siswa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Siswa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate,Secret")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Siswa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Siswa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
