using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_System.Models;

namespace HR_System.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HrSysContext _context;

        public EmployeesController(HrSysContext context)
        {
            _context = context;
        }

        // GET: Employees
        public IActionResult Index()
        {
            var admin_id = HttpContext.Session.GetString("adminId");
            var user_id = HttpContext.Session.GetString("userId");

            if (admin_id != null)
            {
                ViewBag.PagesRules = null;
            }
            else if (user_id != null)
            {
                var b = HttpContext.Session.GetString("groupId");
                if (b != null)
                {
                    List<Crud> Rules = _context.CRUDs.Where(n => n.GroupId == int.Parse(b)).ToList();
                    ViewBag.PagesRules = Rules;

                }
            }
            return View();
        }
        // GET: AllEmployees 
        public IActionResult allEmployees(string search, int show)
        {
            var gId = HttpContext.Session.GetString("groupId");
           string pageName = "Employees"; 
if (gId != null)
            {
                ViewBag.groupId = _context.CRUDs.Where(n => n.GroupId == int.Parse(gId) && n.Page.PageName == pageName).FirstOrDefault();
            }
            var employees = _context.Employees.Include(e => e.Dept).ToList();
            if (search != null && show != 0)
            {
                var emps = employees.Where(e => e.EmpName.Contains(search)).Take(show);
                return PartialView(emps);
            }
            if (search != null)
            {
                var emps = _context.Employees.Include(e => e.Dept).Where(e => e.EmpName.Contains(search));
                return PartialView(emps);
            }
            if (show != 0)
            {
                return PartialView(employees.Take(show));
            }
            return PartialView(employees.Take(10));
        }
        // GET: Employees/Details/5
        public IActionResult Details(int? id)
        {
            var admin_id = HttpContext.Session.GetString("adminId");
            var user_id = HttpContext.Session.GetString("userId");

            if (admin_id != null)
            {
                ViewBag.PagesRules = null;
            }
            else if (user_id != null)
            {
                var b = HttpContext.Session.GetString("groupId");
                if (b != null)
                {
                    List<Crud> Rules = _context.CRUDs.Where(n => n.GroupId == int.Parse(b)).ToList();
                    ViewBag.PagesRules = Rules;

                }
            }
            var gId = HttpContext.Session.GetString("groupId");
            string pageName = "Employees";
            if (gId != null)
            {
                ViewBag.groupId = _context.CRUDs.Where(n => n.GroupId == int.Parse(gId) && n.Page.PageName == pageName).FirstOrDefault();
            }            
if (id == null)
            {
                return NotFound();
            }

            var employee = _context.Employees
                .Include(e => e.Dept)
                .FirstOrDefault(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        // Remote Validations for Employee BirthDate and HireDate...
        public IActionResult birthdatecheck(DateTime Birthdate)
        {

            DateTime datebefore20 = new DateTime(DateTime.Now.Year-20,DateTime.Now.Month,DateTime.Now.Day);
            
            if (Birthdate < datebefore20) return Json(true);
            else return Json(false);
        }
        public JsonResult hiredatecheck(DateTime Hiredate)
        {
            DateTime companystartdate = new DateTime(2008,1,1);

            if (Hiredate > companystartdate) return Json(true);
            else return Json(false);
        }
        //public TimeSpan? attTime;
        //public IActionResult DeptTimeCheck(TimeSpan? AttTime, TimeSpan? DepartureTime)
        //{
        //    if(AttTime != null)
        //    {
        //        attTime = AttTime;
        //        return Json(true);
        //    }
        //    if (DepartureTime != null)
        //    {
        //        if(DepartureTime > attTime)
        //        {
        //            return Json(true);
        //        }
        //        else
        //        {
        //            return Json(false);
        //        }
        //    }
        //    return Json(false);
        //}
        // GET: Employees/Create
        public IActionResult Create()
        {
            var admin_id = HttpContext.Session.GetString("adminId");
            var user_id = HttpContext.Session.GetString("userId");

            if (admin_id != null)
            {
                ViewBag.PagesRules = null;
            }
            else if (user_id != null)
            {
                var b = HttpContext.Session.GetString("groupId");
                if (b != null)
                {
                    List<Crud> Rules = _context.CRUDs.Where(n => n.GroupId == int.Parse(b)).ToList();
                    ViewBag.PagesRules = Rules;

                }
            }
            ViewBag.Gender = new SelectList(new List<string>() { "Male", "Female" });
            ViewBag.Depts = new SelectList(_context.Departments, "DeptId", "DeptName");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gender = new SelectList(new List<string>() { "Male", "Female" }, employee.Gender);
            ViewBag.Depts = new SelectList(_context.Departments, "DeptId", "DeptName", employee.DeptId);
            return View(employee);
        }


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Gender = new SelectList(new List<string>() { "Male", "Female" }, employee.Gender);
            ViewBag.Depts = new SelectList(_context.Departments, "DeptId", "DeptName", employee.DeptId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmpId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Gender = new SelectList(new List<string>() { "Male", "Female" }, employee.Gender);
            ViewBag.Depts = new SelectList(_context.Departments, "DeptId", "DeptId", employee.DeptId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        //// POST: Employees/Delete/5
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmpId == id);
        }
    }
}
