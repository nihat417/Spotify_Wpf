using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testmp3_1.Models;

public class User
{
    [Key]
    public string? Name { get; set; }
    public string? Password { get; set; }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public override string ToString()
    {
        return @$"{Name}{Password}";
    }
}
