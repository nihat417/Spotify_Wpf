using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testmp3_1.Models;

public partial class TrackModel
{
    [JsonProperty("albums")]
    public Albums Albums { get; set; }
}

public partial class Albums
{
    [JsonProperty("href")]
    public Uri Href { get; set; }

    [JsonProperty("items")]
    public Item[] Items { get; set; }

    [JsonProperty("limit")]
    public long Limit { get; set; }

    [JsonProperty("next")]
    public Uri Next { get; set; }

    [JsonProperty("offset")]
    public long Offset { get; set; }

    [JsonProperty("previous")]
    public Uri Previous { get; set; }

    [JsonProperty("total")]
    public long Total { get; set; }
}

public partial class Item
{
    [JsonProperty("album_type")]
    public string AlbumType { get; set; }

    [JsonProperty("artists")]
    public Artist[] Artists { get; set; }

    [JsonProperty("available_markets")]
    public string[] AvailableMarkets { get; set; }

    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonProperty("href")]
    public Uri Href { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("images")]
    public Image[] Images { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("release_date")]
    public DateTimeOffset ReleaseDate { get; set; }

    [JsonProperty("release_date_precision")]
    public string ReleaseDatePrecision { get; set; }

    [JsonProperty("total_tracks")]
    public long TotalTracks { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }
    public string Duration { get; set; }
}

public partial class Artist
{
    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonProperty("href")]
    public Uri Href { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }
}

public partial class ExternalUrls
{
    [JsonProperty("spotify")]
    public Uri Spotify { get; set; }
}

public partial class Image
{
    [JsonProperty("height")]
    public long Height { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("width")]
    public long Width { get; set; }
}
