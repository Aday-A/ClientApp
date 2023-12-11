namespace ClientApp.Requests;

public class UserDetailRequest 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public int GenderID { get; set; }
    public int CountryID { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
}