using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models;

public class UserModel
{
    public int ID { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public int GenderID { get; set; }
    public int CountryID { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }

    public virtual CountryModel Country { get; set; }
}