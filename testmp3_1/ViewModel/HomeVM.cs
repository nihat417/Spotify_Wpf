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
        client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator("BQBd15MzuaVV4fp0MS4JEl2BSOa4YYcoS5yAfxzDJ49R2p_D9tHE8TbwgEOPtxG_nbM1tv3HyHmgMa42Re7-7eSaNAOVYRydPmXw2Z6T4lpmLbR8aMAa4koHGm64fyPsgcDGioki6WBuy2lqHToaGdHJdeFLZPVLtD_7SF-eBeHhoqSBDbLnGv5lfbPte7rR4Mmg1KQ", "Bearer");
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

