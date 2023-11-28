using System.ComponentModel.DataAnnotations.Schema;

namespace ClientApp.Models;

[Table("Gender")]
public class GenderModel
{
    public int ID { get; set; }
    public string? Gender { get; set; }

}