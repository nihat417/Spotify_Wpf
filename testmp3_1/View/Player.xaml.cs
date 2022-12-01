using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using testmp3_1.ViewModel;
using Path = System.IO.Path;

namespace testmp3_1.View;

public partial class Player : UserControl
{
    public Player()
    {
        InitializeComponent();
    }

    private void slider_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var PlayerVM = (PlayerVM)DataContext;
        if (PlayerVM!.Player.HasAudio)
        {
            PlayerVM!.Player.Pause();
            if (PlayerVM!.Timer != null)
                PlayerVM!.Timer!.Stop();
        }
    }

    private void slider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        var PlayerVM = (PlayerVM)DataContext;
        if (PlayerVM!.Player.HasAudio)
        {
            PlayerVM!.Player.Play();
            if (PlayerVM!.Timer != null)
                PlayerVM!.Timer!.Start();
        }
    }

    private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var PlayerVM = (PlayerVM)DataContext;
        if (PlayerVM.flag)
        {
            if (PlayerVM!.Player.HasAudio)
            {
                PlayerVM!.Player.Position = TimeSpan.FromSeconds(slider.Value);

                PlayerVM.FirstClock = PlayerVM.Player.Position.ToString(@"hh\:mm\:ss");
                PlayerVM.SecondClock = (PlayerVM.Player.NaturalDuration.TimeSpan - PlayerVM.Player.Position).ToString(@"hh\:mm\:ss");
            }
        }
    }

    private void listBox_Drop(object sender, DragEventArgs e)
    {
        var PlayerVM = (PlayerVM)DataContext;
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            foreach (var item in files)
            {
                FileInfo f = new(item);
                if (f.Extension == ".mp3" && !PlayerVM.Musics!.Contains(f.Name))
                {
                    PlayerVM.Musics?.Add(Path.GetFileName(item));
                    File.Copy(item, $"{PlayerVM.MusicPath}\\{Path.GetFileName(item)}", true);
                }
            }
        }
    }
}
