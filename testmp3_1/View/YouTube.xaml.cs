using MediaToolkit.Model;
using MediaToolkit;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using testmp3_1.ViewModel;
using VideoLibrary;
using System.ComponentModel;
using System;
using System.IO;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace testmp3_1.View;

public partial class YouTube : UserControl
{
    public BackgroundWorker? worker;
    public static ObservableCollection<string>? YoutubeMusicList;

    public YouTube()
    {
        InitializeComponent();

        YoutubeMusicList = PlayerVM.Musics;

        worker = new BackgroundWorker();
        worker.DoWork += Do_Work!;
        worker.ProgressChanged += Progress_Back!;

        worker.WorkerSupportsCancellation = false;
        worker.WorkerReportsProgress = true;
    }

    private void progressBar_KeyDown(object sender, KeyEventArgs e)
    {

        if (e.Key == Key.Enter)
        {
            prog.Value = 0;
            string url = txt.Text!;
            txt.IsEnabled = false;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    var youtube = VideoLibrary.YouTube.Default;
                    var vid = youtube.GetVideo(url);
                    if (!YoutubeMusicList!.Contains($"{vid}.mp3"))
                    {
                        SaveMP3(vid);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Url not found");
                }

            }));

            thread.Start();

        }
    }


    public void SaveMP3(YouTubeVideo video)
    {

        if (!worker!.IsBusy)
            worker.RunWorkerAsync();

        File.WriteAllBytes(PlayerVM.Mpath + '\\' + video.FullName, video.GetBytes());

        worker.ReportProgress(10);


        string v = video.FullName.Substring(0, video.FullName.Length - 4);

        if (!PlayerVM.Mpath!.Contains($"{v}.mp3"))
        {
            var inputFile = new MediaFile { Filename = Path.Combine(PlayerVM.Mpath!, video.FullName) };
            var outputFile = new MediaFile { Filename = Path.Combine(PlayerVM.Mpath!, $"{v}.mp3") };


            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }

            worker.ReportProgress(20);

        }
        File.Delete(Path.Combine(PlayerVM.Mpath!, video.FullName));


        worker.WorkerSupportsCancellation = true;


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
            if (worker!.WorkerSupportsCancellation)
            {
                i = 100;
                worker.ReportProgress(i);
                MessageBox.Show("Download was completed");
                worker.WorkerSupportsCancellation = false;

                Dispatcher.Invoke(() =>
                {
                    prog.Value = 0;
                });

                worker.Dispose();
                break;
            }
            else
            {
                Thread.Sleep(3000);
                worker.ReportProgress(i / 2);
            }
        }
    }

}
