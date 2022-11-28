using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection.Emit;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;
using testmp3_1.ViewModel;
using Path = System.IO.Path;

namespace testmp3_1.View
{
    public partial class Player : UserControl
    {
        string folderName = "Spotify_Folder";
        int selectedMusicIndex = -1;

        public MediaPlayer mediaplayer = new MediaPlayer();
        public DispatcherTimer? Timer { get; set; }
        public string? MusicPath { get; set; }
        public ObservableCollection<string>? Musics { get; set; }
        public double SliderMaximum { get; set; }
        public int Volume { get; set; } = 12;

        BackgroundWorker? worker;

        public Player()
        {
            InitializeComponent();
            DataContext = this;

            Timer = new();
            Musics = new();
            MusicPath = Directory.GetCurrentDirectory();


            label.Content = string.Empty;
            label1.Content = string.Empty;

            Folder_Path_Finder_OR_Creater(folderName);

            GetMusics_Form_Folder();

            //worker = new();
            //worker.DoWork += Do_Work!;
            //worker.ProgressChanged += Progress_Back!;

            //worker.WorkerSupportsCancellation = false;
            //worker.WorkerReportsProgress = true;

            Timer!.Tick += Timer_Tick;
            Timer!.Interval = new TimeSpan(50);

            mediaplayer.Volume = volm.Value/50;
        }

        void GetMusics_Form_Folder()
        {
            string[]? mus = Directory.GetFiles(MusicPath!);

            if (mus.Length > 0)
                foreach (string item in mus)
                {
                    FileInfo f = new(item);
                    if (f.Extension == ".mp3")
                        Musics!.Add(Path.GetFileName(item));
                }
        }

        /// <summary>
        /// Folder finder or creater. Depend on situation
        /// </summary>
        void Folder_Path_Finder_OR_Creater(string folderName)
        {
            string tempPath = string.Empty, temp2 = string.Empty;
            byte b = 0;

            while (true)
            {
                tempPath = Path.Combine(MusicPath!, folderName);

                if (MusicPath == Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()))
                {
                    Directory.CreateDirectory(Path.Combine(temp2, folderName));
                    break;
                }
                else if (!Directory.Exists(tempPath))
                {
                    MusicPath = Directory.GetParent(MusicPath!)!.ToString();
                    b++;
                    if (b == 4)
                        temp2 = MusicPath;
                }
                else
                {
                    MusicPath = tempPath;
                    break;
                }

            }
        }

        /// <summary>
        /// Timer Dispatcher method. Change slider value when music was played
        /// </summary>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            slider.ValueChanged -= Slider_ValueChanged;

            if (mediaplayer.NaturalDuration.HasTimeSpan)
            {
                label1.Content = (mediaplayer.NaturalDuration.TimeSpan - mediaplayer.Position).ToString(@"hh\:mm\:ss");

                slider.Value = mediaplayer.Position.TotalSeconds;

                label.Content = mediaplayer.Position.ToString(@"hh\:mm\:ss");
            }
            slider.ValueChanged += Slider_ValueChanged;
        }


        private void Button_Click(object sender, RoutedEventArgs e) // Open File Dialog
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
            if (openFileDialog.ShowDialog() == true)
            {
                string name = Path.GetFileName(openFileDialog.FileName);
                if (!Musics!.Contains(name))
                {
                    string path = Path.Combine(MusicPath!, name);
                    Musics!.Add(name);
                    File.Copy(openFileDialog.FileName, path);
                    mediaplayer.Open(new Uri(path));
                }
            }
        }

        /// <summary>
        /// Play Button
        /// </summary>
        private void Play_Click(object sender, RoutedEventArgs e) // Play Button 
        {
            if (playCheck.IsChecked == true)
            {
                if (mediaplayer.HasAudio)
                {
                    if (!Timer!.IsEnabled)
                        Timer.Start();

                    mediaplayer.Play();
                    Tg_btn.IsChecked = true;
                }
            }
            else if (playCheck.IsChecked == false)
            {
                mediaplayer.Pause();
                if (Timer != null)
                {
                    Timer!.Stop();
                    Tg_btn.IsChecked = false;
                }
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaplayer.Position = TimeSpan.FromSeconds(slider.Value);
            if (mediaplayer.NaturalDuration.HasTimeSpan)
            {
                label.Content = mediaplayer.Position.ToString(@"hh\:mm\:ss");
                label1.Content = (mediaplayer.NaturalDuration.TimeSpan - mediaplayer.Position).ToString(@"hh\:mm\:ss");
            }
        }

        /// <summary>
        /// Change music slider value with mouse left button
        /// </summary>
        private void Slider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaplayer.NaturalDuration.HasTimeSpan)
            {
                mediaplayer.Pause();
                if (Timer != null)
                    Timer!.Stop();
            }
        }

        /// <summary>
        /// Release music slider value with mouse left button
        /// </summary>
        private void Slider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mediaplayer.NaturalDuration.HasTimeSpan)
            {
                mediaplayer.Play();
                if (Timer != null)
                    Timer.Start();
            }
        }

        /// <summary>
        /// Stop method
        /// </summary>
        private void Click_Stop(object sender, RoutedEventArgs e)
        {
            if (mediaplayer.NaturalDuration.HasTimeSpan)
            {
                mediaplayer.Stop();
                if (Timer != null)
                    Timer!.Stop();
                label.Content = mediaplayer.Position.ToString(@"hh\:mm\:ss");
                label1.Content = mediaplayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
                slider.Value = 0;
                playCheck.IsChecked = false;
                Tg_btn.IsChecked = false;
            }
        }

        /// <summary>
        /// Musics select method
        /// </summary>
        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list?.SelectedItem != null)
            {
                string? music = list.SelectedItem as string;
                mediaplayer.Open(new Uri(MusicPath! + "\\" + music));
                selectedMusicIndex = Musics!.IndexOf(music!);
                tbfilname.Content = list?.SelectedItem.ToString();
                Thread.Sleep(500);

                if (mediaplayer.NaturalDuration.HasTimeSpan)
                {
                    slider.Maximum = mediaplayer.NaturalDuration.TimeSpan.TotalSeconds;
                    slider.Value = 0;

                    label.Content = mediaplayer.Position.ToString(@"hh\:mm\:ss");
                    label1.Content = mediaplayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");

                    if (playCheck.IsChecked == true && Timer!.IsEnabled)
                        mediaplayer.Play();
                }
            }
        }

        /// <summary>
        /// Next and Previous buttons....
        /// </summary>
        private void Button_Next_Prev(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn == next)
            {
                if (Musics!.Count > selectedMusicIndex + 1 && Musics.Count > 0 && mediaplayer.HasAudio)
                    Next_Previous(++selectedMusicIndex);
            }
            else if (btn == prev)
            {
                if (selectedMusicIndex - 1 >= 0 && Musics?.Count > 0)
                    Next_Previous(--selectedMusicIndex);
            }
        }
        /// <summary>
        /// Next & Prev buttons method. Used in Button_Next_Prev method...
        /// </summary>
        void Next_Previous(int index)
        {
            string song = Musics![index];
            mediaplayer.Close();
            mediaplayer.Open(new Uri(Path.Combine(MusicPath!, song)));
            tbfilname.Content = song;
            Thread.Sleep(500);

            if (mediaplayer.NaturalDuration.HasTimeSpan)
            {
                slider.Maximum = mediaplayer.NaturalDuration.TimeSpan.TotalSeconds;
                slider.Value = 0;

                label.Content = mediaplayer.Position.ToString(@"hh\:mm\:ss");
                label1.Content = mediaplayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");

                if (playCheck.IsChecked == true && Timer!.IsEnabled)
                    mediaplayer.Play();
            }
        }

        /// <summary>
        /// Volume Changer 
        /// </summary>
        private void Volm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaplayer.Volume = (double)volm.Value/50;
        }

        /// <summary>
        /// Music add with drop...
        /// </summary>
        private void list_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                foreach (var item in files)
                {
                    FileInfo f = new(item);
                    if (f.Extension == ".mp3" && !Musics!.Contains(f.Name))
                    {
                        Musics?.Add(Path.GetFileName(item));
                        File.Copy(item, $"{MusicPath}\\{Path.GetFileName(item)}", true);
                    }
                }
            }
        }






    }
}
