using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientApp.Models;

[Table("Country")]
public class CountryModel
{
    public int ID { get; set; }
    
    [Display(Name ="Country")]
    public string? Name { get; set; }
    public string? Code { get; set; }

}