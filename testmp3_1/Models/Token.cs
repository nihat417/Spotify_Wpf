using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testmp3_1.Models;

public class Token
{
    public string access_token { get; set; }

    public string token_type { get; set; }

    public int expires_in { get; set; }
}