using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientApp.Models;

[Table("Gender")]
public class GenderModel
{
    public int ID { get; set; }
    public string? Gender { get; set; }

}