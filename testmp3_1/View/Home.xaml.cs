using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using testmp3_1.Helpers;
using testmp3_1.Models;

namespace testmp3_1.View;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();
       

        Task.Run(async () => await SearchHelper.GetTokenAsync());
    }

    private void txtSearch_KeyUp(object sender, KeyEventArgs e)
    {

        ListArtist.Visibility = Visibility.Visible;
        homevv.Visibility = Visibility.Hidden;

        if (txtSearch.Text == string.Empty)
        {
            ListArtist.ItemsSource = null;
            return;
        }

        var result = SearchHelper.SearchArtistOrSong(txtSearch.Text);

        if (result == null)
        {
            return;
        }

        var listArtist = new List<SpotifyArtist>();

        foreach (var item in result.Artists!.Items!)
        {
            listArtist.Add(new SpotifyArtist()
            {
                ID = item.Id,
                Image = item.Images!.Any() ? item.Images![0].Url : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
                Name = item.Name,
                Popularity = $"{item.Popularity}% popularidad",
                Followers = $"{item.Followers!.Total.ToString("N")} seguidores"
            });
        }

        ListArtist.ItemsSource = listArtist;
    }
}
