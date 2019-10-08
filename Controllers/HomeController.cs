using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TODOApp.Models;

namespace TODOApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext context;

        public int PageSize = 5;

        public HomeController(ApplicationContext appContext)
        {
            context = appContext;
        }

        [HttpPost]
        public IActionResult Index(string UserEmail, int page = 1)
        {
            User user = context.Users.FirstOrDefault(u => u.UserEmail == UserEmail);

            try
            {
                ViewBag.PageCount = ViewBag.PageCount = Math.Ceiling(Convert.ToDouble((context.Tasks.Where(t => t.UserId == user.Id).Count() / PageSize)));
            }catch(Exception ex)
            {
                ViewBag.PageCount = 1;
            }

            if (user != null)
            {
                    return View(context.Tasks.Where(t => t.UserId == user.Id)
                        .OrderBy(t => t.Id)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize));
            }
            else
            {
                context.Users.Add(new User { UserEmail = UserEmail });
                context.SaveChanges();

                User newUser = context.Users.FirstOrDefault(u => u.UserEmail == UserEmail);
                Models.Task newTask = new Models.Task { UserId = newUser.Id };

                return View("AddTask", newTask);
            }
        }

        [HttpGet]
        public IActionResult Index(int UserId, int page = 1)
        {
            User user = context.Users.FirstOrDefault(u => u.Id == UserId);
            IEnumerable<TODOApp.Models.Task> tasks = ViewBag.PageCount = context.Tasks.Where(t => t.UserId == user.Id)
                                                                            .OrderBy(t => t.Id)
                                                                            .Skip((page - 1) * PageSize)
                                                                            .Take(PageSize);

            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble((context.Tasks.Where(t => t.UserId == user.Id).Count() / PageSize)));

            return View(tasks);
 
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public IActionResult AddTask(int? UserId)
        {
            Models.Task task = new Models.Task();
            task.UserId = (int)UserId;

            return View(task);
        }

        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            context.Add(task);
            context.SaveChanges();

            //return View("Index", context.Tasks.Where(t => t.UserId == task.UserId));
            return RedirectToAction("Index", "Home", new { UserId = task.UserId });
        }

        [HttpGet]
        public IActionResult UpdateTask(int? Id)
        {
            Models.Task task = context.Tasks.First(t => t.Id == (int)Id);

            return View(task);
        }

        [HttpPost]
        public IActionResult UpdateTask(Models.Task task)
        {
            User user = context.Users.FirstOrDefault(u => u.Id == task.UserId);

            context.Tasks.Update(task);
            context.SaveChanges();

            return RedirectToAction("Index", "Home", new { UserId = task.UserId });
        }

        [HttpPost]
        public IActionResult DeleteTask(int Id)
        {
            Models.Task task = context.Tasks.FirstOrDefault(t => t.Id == Id);
            User user = context.Users.FirstOrDefault(u => u.Id == task.UserId);


            if (task != null)
            {
                context.Tasks.Remove(task);
                context.SaveChanges();

                if (context.Tasks.Where(t => t.UserId == user.Id).Count() != 0)
                {
                    return RedirectToAction("Index", "Home", new { UserId = task.UserId });
                }
                else
                {
                    Models.Task newTask = new Models.Task { UserId = user.Id };
                    return View("AddTask", newTask);
                }
            }

            return NotFound();
        }
    }
}
