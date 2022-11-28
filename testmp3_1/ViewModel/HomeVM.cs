using Newtonsoft.Json;
using RestSharp.Authenticators.OAuth2;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testmp3_1.Models;

namespace testmp3_1.ViewModel;

class HomeVM : Utilities.ViewModelBase
{
    public ObservableCollection<Item> Songs { get; set; }
    public HomeVM()
    {
        Songs = new ObservableCollection<Item>();
        PopulateCollection();
    }

    void PopulateCollection()
    {
        var client = new RestClient();
        client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator("BQBk-UNajjqN03xCq3TPfo3HgSDP9-pTx4Hlk1z9z1iaHGYIOhLooQWNTyXLn9a3eHwaK8_XsBzwvsyZf2kFV5l5cRJ641i-rEcoFkXv_4BxAw_q-eEHiDdqlA6yssEX18deykAWzv99F5llTuqEt8ANXbzJ2_4wITLQihIy6SPlWZItMw0pneRVPlJc66sQM8NtSDY", "Bearer");
        var request = new RestRequest("https://api.spotify.com/v1/browse/new-releases", Method.Get);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var responce = client.GetAsync(request).GetAwaiter().GetResult();
        var data = JsonConvert.DeserializeObject<TrackModel>(responce.Content);

        for (int i = 0; i < data.Albums.Limit; i++)
        {
            var track = data.Albums.Items[i];
            track.Duration = "2:32";
            Songs.Add(track);
        }

    }
}

