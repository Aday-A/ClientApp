//Update: enable easy editing
  /*  [HttpGet]
    public ActionResult Edit(int ID)
    {
        var data = _applicationDbContext.Users.Where(x => x.ID == ID).FirstOrDefault();
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
            data.LastName = model.LastName;
            data.CountryID = model.CountryID;
            data.Telephone = model.Telephone;
            data.Email = model.Email;

            _applicationDbContext.SaveChanges();

            string message = "User Created Successfully";
            ViewBag.Message = message;

            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }



         public ActionResult Edit(int? ID)
    {
        
        if (_applicationDbContext.Users == null)
        {
            return Problem("Entity set is null.");
        }

        //var details = _applicationDbContext.Users.Find(ID);
        var details = _applicationDbContext.Users.FirstOrDefault(x => x.ID == ID);

        if (details != null)
        {
            _applicationDbContext.Users.Find(ID);
        }

        return View(details);
    }


    } */