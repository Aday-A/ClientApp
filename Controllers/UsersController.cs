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

        UserModel user = new()
        {
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


    //GET: Display edit value
    public async Task<IActionResult> Edit(int? ID)
    {
        GetGender();
        List<Models.CountryModel>? countries = _applicationDbContext.Countries.ToList();
        ViewBag.Countries = countries;


        if (ID == null)
        {
            return Problem("Entity set is null.");
        }
        var details = await _applicationDbContext.Users.Include(x => x.Country).Include(x=>x.Gender).FirstOrDefaultAsync(x => x.ID == ID);
        if (details == null)
        {
            return Problem("Entity set is null.");
        }
        return View(details);
    }

    //POST: Edit details & update DB
    [HttpPost, ActionName("EditConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditConfirmed(
        int ID,
        [Bind(include: "ID,FirstName,LastName,UserName,GenderID,CountryID,Telephone,Email")] UserModel model)
    {

        var data = await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ID == ID);
        if (data is null)
        {
            return NotFound();
        }
        GetGender();
        List<Models.CountryModel>? countries = _applicationDbContext.Countries.ToList();
        ViewBag.Countries = countries;

        try
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Update(model);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            model.Country = data.Country;
            model.Gender = data.Gender;
            
            return View("Edit", model);
        }
        catch (System.Exception exception)
        {
            ModelState.AddModelError("UserName", exception.Message);
            return View("Edit", model);
        }



        //  try
        //  {
        //     if (ModelState.IsValid)
        //     {
        //         _applicationDbContext.Update(model);
        //         await _applicationDbContext.SaveChangesAsync();
        //         return RedirectToAction("Index");
        //      }
        //  }
        //  catch (System.Exception)
        //  {
        //     throw;
        //  }
        //  return RedirectToAction("Index");


        // if(ID != model.ID)
        // {
        //     return BadRequest();
        // }
        // _applicationDbContext.Entry(model).State = EntityState.Modified;

        // try
        // {
        //     await _applicationDbContext.SaveChangesAsync();
        // }
        // catch (DbUpdateConcurrencyException)
        // {
        //     throw;
        // }
        // return AcceptedAtAction("Index", new {ID = model.ID, 
        //                                         FirstName = model.FirstName, 
        //                                         LastName = model.LastName,
        //                                         GenderID = model.GenderID,
        //                                         CountryID = model.CountryID,
        //                                         UserName = model.UserName,
        //                                         Telephone = model.Telephone,
        //                                         Email = model.Email }, model);




        // if (ModelState.IsValid)
        // {
        //     return View(model);
        // }

        // _applicationDbContext.Update(model);
        // await _applicationDbContext.SaveChangesAsync();
        // return AcceptedAtAction("Index",  new {ID = model.ID, 
        //                                  FirstName = model.FirstName, 
        //                                  LastName = model.LastName,
        //                                  GenderID = model.GenderID,
        //                                  CountryID = model.CountryID,
        //                                  UserName = model.UserName,
        //                                 Telephone = model.Telephone,
        //                                  Email = model.Email}, model);
    }

    [HttpDelete]
    public ActionResult Delete(int ID)
    {
        var data = _applicationDbContext.Users.Include(x => x.Country).FirstOrDefault(x => x.ID == ID);
        _applicationDbContext.Users.Remove(data);
        _applicationDbContext.SaveChanges();
        return NoContent();
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
