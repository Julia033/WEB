using Lab_2.Models.Entities;
using Lab_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Lab_2.Controllers
{
    public class Lab2Controller : Controller
    {
        // GET: Lab2
        [AllowAnonymous]
        public ActionResult ListOfPeople()
        {
            List<Person> people = new List<Person>();
            using (var db = new NazarenkoWEBEntities())
            {
                people = db.People.OrderByDescending(x => x.age)
                                  .ThenBy(x => x.last_name)
                                  .ThenBy(x => x.first_name).ToList();
            }
            return View(people);
        }
        [HttpGet]
        [Authorize]
        public ActionResult PersonDetails(Guid person_ID)
        {
            Person model = new Person();
            using (var db = new NazarenkoWEBEntities())
            {
                model = db.People.Find(person_ID);
            }
            return View(model);
        }

        List<Tuple<string,string>> GetGendersList()
        {
            List<Tuple<string,string>> Genders = new List<Tuple<string,string>>()
            {
                new Tuple<string,string>("М","Мужской"),
                 new Tuple<string,string>("Ж","Женский")
            };

            return Genders;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreatePerson()
        {
            ViewBag.Genders = new SelectList(GetGendersList(), "Value", "Text");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult CreatePerson(PersonVM newPerson)
        {
            if (ModelState.IsValid)
            {
                 using (var context = new NazarenkoWEBEntities())
                {
                    Person person = new Person
                    {
                        ID = Guid.NewGuid(),
                        last_name = newPerson.last_name,
                        first_name = newPerson.first_name,
                        patronymic = newPerson.patronymic,
                        gender = newPerson.gender,
                        age = newPerson.age,
                        has_job = newPerson.has_job
                    };
                    context.People.Add(person);
                    context.SaveChanges();
                }
                return RedirectToAction("ListOfPeople");
            }
            return View(newPerson);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPerson(Guid person_ID)
        {
            PersonVM model;
            using (var context = new NazarenkoWEBEntities())
            {
                Person person = context.People.Find(person_ID);
                model = new PersonVM
                {
                    ID = person.ID,
                    last_name = person.last_name,
                    first_name = person.first_name,
                    patronymic = person.patronymic,
                    gender = person.gender,
                    age = person.age,
                    has_job = person.has_job
                };
            }
            ViewBag.Genders = new SelectList(GetGendersList(), "Item1", "Item2", model.gender);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()] 
        public ActionResult EditPerson(PersonVM model) 
        {
            if (ModelState.IsValid) 
            {
                using (var context = new NazarenkoWEBEntities())
                {
                    Person editedPerson = new Person
                    {
                        ID = model.ID,
                        last_name = model.last_name,
                        first_name = model.first_name,
                        patronymic = model.patronymic,
                        gender = model.gender,
                        age = model.age,
                        has_job = model.has_job
                    };
                    context.People.Attach(editedPerson);
                    context.Entry(editedPerson).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                return RedirectToAction("ListOfPeople");
            }
            ViewBag.Genders = new SelectList(GetGendersList(), "Item1", "Item2");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePerson(Guid person_ID)
        {
            Person personToDelete;
            using (var context = new NazarenkoWEBEntities())
            {
                personToDelete = context.People.Find(person_ID);
            }
            return View(personToDelete);
        }

        [HttpPost, ActionName("DeletePerson")]
        public ActionResult DeleteConfirmed(Guid person_ID)
        {
            using (var context = new NazarenkoWEBEntities())
            {
                Person personToDelete = new Person
                { ID = person_ID };
                context.Entry(personToDelete).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            return RedirectToAction("ListOfPeople");
        }

        [ChildActionOnly]
        public ActionResult QuestionAnswered(Guid person_ID)
        {
            string message = "";
            using (var context = new NazarenkoWEBEntities())
            {
                int questionAnsweredNumber = context.People.Find(person_ID).Answers.Count;
                message = $"Вопросов отвечено: {questionAnsweredNumber}.";
            }
            return PartialView("QuestionAnswered", message);
        }




    }
}


