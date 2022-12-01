using System.Collections.Generic;

namespace testmp3_1.Models;

public class SpotifySearch
{
    public class ExternalUrls
    {
        public string? Spotify { get; set; }
    }

    public class Followers
    {
        public object? Href { get; set; }
        public int Total { get; set; }
    }

    public class ImageSP
    {
        public int Width { get; set; }
        public string? Url { get; set; }
        public int Height { get; set; }
    }

    public class Item
    {
        public ExternalUrls? External_urls { get; set; }
        public Followers? Followers { get; set; }
        public List<string>? Genres { get; set; }
        public string? Href { get; set; }
        public string? Id { get; set; }
        public List<ImageSP>? Images { get; set; }
        public string? Name { get; set; }
        public int Popularity { get; set; }
        public string? Type { get; set; }
        public string? Uri { get; set; }
    }

    public class Artists
    {
        public string? Href { get; set; }
        public List<Item>? Items { get; set; }
        public int Limit { get; set; }
        public string? Next { get; set; }
        public int Offset { get; set; }
        public object? Previous { get; set; }
        public int Total { get; set; }
    }

    public class SpotifyResult
    {
        public Artists? Artists { get; set; }
    }
}