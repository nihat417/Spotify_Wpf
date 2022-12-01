using MediaToolkit.Model;
using MediaToolkit;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using testmp3_1.ViewModel;
using VideoLibrary;
using System.ComponentModel;
using System;
using System.IO;
using System.Collections.ObjectModel;

namespace testmp3_1.View;

public partial class YouTube : UserControl
{
    public BackgroundWorker? Worker;
    public ObservableCollection<string>? YoutubeMusicList { get; set; }



    public YouTube()
    {
        InitializeComponent();
        DataContext = this;

        YoutubeMusicList = PlayerVM.Musics;

        Worker = new BackgroundWorker();
        Worker.DoWork += Do_Work!;
        Worker.ProgressChanged += Progress_Back!;

        Worker.WorkerSupportsCancellation = false;
        Worker.WorkerReportsProgress = true;

    }

    private void txt_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            prog.Value = 0;
            string url = txt.Text!;
            txt.IsEnabled = false;

            try
            {
                var youtube = VideoLibrary.YouTube.Default;
                var vid = youtube.GetVideo(url);
                string musicName = vid.FullName.Substring(0, vid.FullName.Length - 4);
                if (!YoutubeMusicList!.Contains($"{musicName}.mp3"))
                {
                    Thread thread = new Thread(new ThreadStart(() =>
                {
                    SaveMP3(vid);
                }));
                    thread.Start();
                }
                else
                {
                    messageLabel.Content = "This music has already been added";
                    txt.Text = string.Empty;
                    txt.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                messageLabel.Content = "Url not found";
                txt.IsEnabled = true;
            }
        }
    }

    public void SaveMP3(YouTubeVideo video)
    {
        if (!Worker!.IsBusy)
            Worker.RunWorkerAsync();

        Worker.ReportProgress(3);

        Dispatcher.Invoke(() =>
        {
            messageLabel.Content = "Downloading...";
        });

        File.WriteAllBytes(PlayerVM.Mpath + '\\' + video.FullName, video.GetBytes());

        string v = video.FullName.Substring(0, video.FullName.Length - 4);

        Worker.ReportProgress(10);

        Dispatcher.Invoke(() =>
        {
            messageLabel.Content = "Converting to mp3...";
        });

        if (!PlayerVM.Mpath!.Contains($"{v}.mp3"))
        {
            var inputFile = new MediaFile { Filename = Path.Combine(PlayerVM.Mpath!, video.FullName) };
            var outputFile = new MediaFile { Filename = Path.Combine(PlayerVM.Mpath!, $"{v}.mp3") };


            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }

            Worker.ReportProgress(20);

        }
        File.Delete(Path.Combine(PlayerVM.Mpath!, video.FullName));


        Worker.WorkerSupportsCancellation = true;


        Dispatcher.Invoke(() =>
        {

            txt.IsEnabled = true;
            if (!PlayerVM.Mpath!.Contains($"{v}.mp3"))
                YoutubeMusicList!.Add($"{v}.mp3");
        });
    }

    public void Progress_Back(object sender, ProgressChangedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            prog.Value += e.ProgressPercentage;
        });
    }

    void Do_Work(object sender, EventArgs e)
    {
        int i = 0;
        while (true)
        {
            i++;
            if (Worker!.WorkerSupportsCancellation)
            {
                i = 100;
                Worker.ReportProgress(i);
                Worker.WorkerSupportsCancellation = false;

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Download was completed");
                    messageLabel.Content = "Download was completed";
                    prog.Value = 0;
                    
                });

                Worker.Dispose();
                break;
            }
            else
            {
                Thread.Sleep(2000);
                Worker.ReportProgress(i / 2);
            }
        }
    }

    private void listBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        MenuItem_Click(sender, e);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var menuItem = sender as MenuItem;

        string header = menuItem?.Header.ToString()!;
        string? name;
        if (listBox.SelectedItem is not null)
            name = listBox.SelectedItem.ToString();
        else
            name = string.Empty;

        if (header == "_delete")
        {
            if (YoutubeMusicList!.Contains(name!))
            {
                File.Delete(Path.Combine(PlayerVM.Mpath!, name));
                YoutubeMusicList.Remove(name);
            }
        }
    }

}
