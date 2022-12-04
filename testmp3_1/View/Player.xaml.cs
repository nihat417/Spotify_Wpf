using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using testmp3_1.ViewModel;
using Path = System.IO.Path;

namespace testmp3_1.View;

public partial class Player : UserControl
{
    public readonly MediaPlayer? mediaPLayer;
    public readonly DispatcherTimer? Timer;
    public Player()
    {
        InitializeComponent();
        var vm = (PlayerVM)DataContext;
        mediaPLayer = vm.Player;
        Timer = vm.Timer;
    }

    #region Events

    /// <summary>
    /// Slider Press Button Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void slider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var PlayerVM = (PlayerVM)DataContext;
        if (PlayerVM!.Player.HasAudio)
        {
            PlayerVM!.Player.Pause();
            if (PlayerVM!.Timer != null)
                PlayerVM!.Timer!.Stop();
        }
    }

    /// <summary>
    /// Slider Release Button Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Slider value chang event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var PlayerVM = (PlayerVM)DataContext;

        if (PlayerVM._flag)
            if (PlayerVM!.Player.HasAudio)
            {
                PlayerVM!.Player.Position = TimeSpan.FromSeconds(slider.Value);

                PlayerVM.FirstClock = PlayerVM.Player.Position.ToString(@"hh\:mm\:ss");
                PlayerVM.SecondClock = (PlayerVM.Player.NaturalDuration.TimeSpan - PlayerVM.Player.Position).ToString(@"hh\:mm\:ss");

                if (PlayerVM!.SliderValue == PlayerVM!.SliderMaximum)
                {
                    PlayerVM!.AnimationEnable = false;
                    PlayerVM!.PlayerChecker = false;
                }

                if(PlayerVM!.PlayerChecker == true)
                {

                }

            }
    }

    /// <summary>
    /// List drop event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    #endregion

}
