namespace testmp3_1.Models;

public class Token
{
    public string? Access_token { get; set; }
    public string? Token_type { get; set; }
    public int Expires_in { get; set; }
}