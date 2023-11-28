using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClientApp.Models;
using ClientApp.Requests;
using System.Net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UsersController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public ActionResult Index()
    {
        var data = _applicationDbContext
            .Users
            .Include(x => x.Country)
            .ToList();

        List<UserViewModel> users = data
            .Select(user => new UserViewModel(
                ID: user.ID,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Gender: user.GenderID == 1 ? "Male" : "Female",
                Country: user.Country.Name,
                UserName: user.UserName,
                Telephone: user.Telephone,
                Email: user.Email))
            .ToList();

        ViewBag.Query = "";
        
        return View(users);
    }

    //GET: User/Create
    public ActionResult Create()
    {
        GetGender();
        List<Models.CountryModel>? countries = _applicationDbContext.Countries.ToList();
        ViewBag.Countries = countries;
        return View();
    }

    //POST: ENTRY INTO USER/Create
    [HttpPost]
    public ActionResult Create(CreateUserRequest model)
    {
        GetGender();
        List<Models.CountryModel>? countries = _applicationDbContext.Countries.ToList();
        ViewBag.Countries = countries;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        UserModel user = new(){
            FirstName = model.FirstName,
            LastName = model.LastName,
            GenderID = model.GenderID,
            CountryID = model.CountryID,
            UserName = model.UserName,
            Telephone = model.Telephone,
            Email = model.Email
        };

        var data = _applicationDbContext
            .Users
            .Add(user);

        _applicationDbContext.SaveChanges();
        string message = "User Created Successfully";
        ViewBag.Message = message;
        return RedirectToAction("index");
    }


    [HttpGet]
    public ActionResult Read()
    {
        var data = _applicationDbContext.Users.ToList();
        return View(data);
    }


    //Update: enable easy editing
    public ActionResult Update(int ID)
    {
        var data = _applicationDbContext.Users.Where(x => x.ID == ID).SingleOrDefault();
        return View(data);
    }

    //Update: User
    [HttpPost]
    [ValidateAntiForgeryToken]

    public ActionResult Update(int ID, UserModel model)
    {
        var data = _applicationDbContext.Users.FirstOrDefault(x => x.ID == ID);

        if (data != null)
        {
            data.FirstName = model.FirstName;
            _applicationDbContext.SaveChanges();

            return RedirectToAction("Read");
        }
        else
        {
            return View();
        }
    }

    public ActionResult Delete()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public ActionResult Delete(int ID)
    {

        var data = _applicationDbContext.Users.FirstOrDefault(x => x.ID == ID);
        if (data != null)
        {
            _applicationDbContext.Users.Remove(data);
            _applicationDbContext.SaveChanges();

            return RedirectToAction("Read");
        }
        else
        {
            return View();
        }
    }

    public void GetGender()
    {
        List<Models.GenderModel>? genders = new();
        genders = _applicationDbContext.Genders.ToList();
        ViewBag.Genders = genders;
    }


    public async Task<IActionResult> Search(string UserName)
    {
        if (_applicationDbContext.Users == null)
        {
            return Problem("Entity set is null.");
        }

        var data = UserName switch
        {
            null or "" => _applicationDbContext
                .Users
                .Include(x => x.Country)
                .ToList(),

            _ =>
                _applicationDbContext
                .Users
                .Where(x => x.UserName.Contains(UserName) || x.FirstName.Contains(UserName) || x.LastName.Contains(UserName))
                .Include(x => x.Country)
                .ToList()
        };

        List<UserViewModel> users = data
            .Select(user => new UserViewModel(
                ID: user.ID,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Gender: user.GenderID == 1 ? "Male" : "Female",
                Country: user.Country.Name,
                UserName: user.UserName,
                Telephone: user.Telephone,
                Email: user.Email))
            .ToList();

        ViewBag.Query = UserName;

        return View("Index", users);
    }

    [HttpPost]
    public string Search(string UserName, bool notUsed)
    {
        return "From [HttpPost]Index: filter on " + UserName;
    }




}
