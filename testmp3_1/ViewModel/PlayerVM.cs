using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using testmp3_1.Utilities;

namespace testmp3_1.ViewModel;

class PlayerVM : ViewModelBase
{
    public static ObservableCollection<string>? Musics;
    public static string? Mpath;

    public readonly MediaPlayer Player;
    public readonly DispatcherTimer Timer;
    public string? MusicPath, FolderName = "Spotify_Folder";

    private double _slidervalue;
    private double _maximumLimit;

    private string? _selectedMP3;
    private string? _firstClock;
    private string? _secondClock;

    private int _selectedMusicIndex = -1;
    private int _volume;

    private bool _paused = false;
    private bool _mute = true;
    public bool _flag = true;
    public bool _buttonsEnable = false;
    public bool _animationEnable = false;

    private ICommand? _onClickMute;
    private ICommand? onClickPrev;
    private ICommand? onClickPlay;
    private ICommand? onClickStop;
    private ICommand? onClickNext;
    private ICommand? onClickBrowsButton;



    public PlayerVM()
    {
        Player = new MediaPlayer();
        Timer = new DispatcherTimer();
        Musics = new ObservableCollection<string>();

        MusicPath = Directory.GetCurrentDirectory();

        Folder_Path_Finder_OR_Creater(FolderName);

        GetMusics_Form_Folder();


        Timer!.Tick += Timer_Tick;
        Timer!.Interval = new TimeSpan(0);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _flag = false;
        if (Player.NaturalDuration.HasTimeSpan)
        {
            SliderValue = Player.Position.TotalSeconds;
            FirstClock = Player.Position.ToString(@"hh\:mm\:ss");
            SecondClock = (Player.NaturalDuration.TimeSpan - Player.Position).ToString(@"hh\:mm\:ss");
        }
        _flag = true;
    }

    void Folder_Path_Finder_OR_Creater(string folderName)
    {
        string tempPath = string.Empty;
        byte b = 0;
        bool flag = false;

        while (b++ != 4)
        {
            tempPath = Path.Combine(MusicPath!, folderName);

            if (!Directory.Exists(tempPath))
                MusicPath = Directory.GetParent(MusicPath!)!.ToString();
        }

        if (!flag)
        {
            MusicPath = Path.Combine(MusicPath!, folderName);
            Directory.CreateDirectory(MusicPath);
        }
        else
            MusicPath = Path.Combine(MusicPath!, folderName);

        Mpath = MusicPath;
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

    public bool ButtonEnable 
    { 
        get
        {
            return _buttonsEnable;
        }
        set
        {
            _buttonsEnable = value;
            OnPropertyChanged("ButtonEnable");
        }
    }

    public bool AnimationEnable
    {
        get
        {
            return _animationEnable;
        }
        set
        {
            _animationEnable = value;
            OnPropertyChanged("AnimationEnable");
        }
    }

    public bool Pause
    {
        get
        {
            return _paused;
        }
        set
        {
            _paused = value;
            OnPropertyChanged("Pause");
        }
    }

    public string? FirstClock
    {
        get
        {
            return _firstClock;
        }
        set
        {
            _firstClock = value;
            OnPropertyChanged("FirstClock");
        }
    }

    public string? SecondClock
    {
        get
        {
            return _secondClock;
        }
        set
        {
            _secondClock = value;
            OnPropertyChanged("SecondClock");
        }
    }

    public double ChangePosition
    {
        get
        {
            return _slidervalue;
        }
        set
        {
            Player.Position = TimeSpan.FromSeconds(SliderValue);
            OnPropertyChanged("Changeposition");
        }
    }

    public double Volume
    {
        get { return _volume = (int)(Player.Volume * 10); }
        set
        {
            Player.Volume = value / 10;
            OnPropertyChanged("Volume");
        }
    }

    public double SliderMaximum
    {
        get { return _maximumLimit; }
        set
        {
            _maximumLimit = value;

            OnPropertyChanged("SliderMaximum");
        }
    }

    public ICommand OnClickBrowsButton
    {
        get
        {
            return onClickBrowsButton ?? (onClickBrowsButton = new RelayCommand((o) =>
            {
                ClickBrows();
            }));
        }
    }

    public double SliderValue
    {
        get { return _slidervalue; }
        set
        {
            _slidervalue = value;

            OnPropertyChanged("SliderValue");
        }
    }

    public int SelectedIndex
    {
        get
        {
            return _selectedMusicIndex;
        }
        set
        {
            _selectedMusicIndex = value;
            OnPropertyChanged("SelectedIndex");
        }
    }

    public string? SelectedMP3
    {
        get { return _selectedMP3; }

        set
        {
            _selectedMP3 = value;


            SelectedIndex = Musics!.IndexOf(_selectedMP3!);

            Player.Open(new Uri(Path.Combine(MusicPath!, _selectedMP3!)));


            Thread.Sleep(500);

            if (Player.NaturalDuration.HasTimeSpan)
            {
                FirstClock = Player.Position.ToString(@"hh\:mm\:ss");
                SecondClock = Player.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");

                SliderMaximum = Player.NaturalDuration.TimeSpan.TotalSeconds;

                if (_paused)
                {
                    Player.Play();
                    AnimationEnable = true;
                }
                else
                {
                    Player.Pause();
                    AnimationEnable = false;
                }
            }

            ButtonEnable = true;

            OnPropertyChanged("SelectedMP3");
        }
    }

    public ObservableCollection<string>? ListMP3
    {
        get { return Musics; }

        set
        {
            Musics = value;
            OnPropertyChanged("m_listBox");
        }
    }

    public ICommand OnClickMute
    {
        get
        {
            return _onClickMute ?? (_onClickMute = new RelayCommand((o) =>
            {
                ClickMute();
            }));
        }
    }

    public ICommand OnClickPlay
    {
        get
        {
            return onClickPlay ?? (onClickPlay = new RelayCommand((o) =>
            {
                OnClickPlayButton();
            }));
        }
    }

    public ICommand OnClickStop
    {
        get
        {
            return onClickStop ?? (onClickStop = new RelayCommand((o) =>
            {
                OnClickStopButton();
            }));
        }
    }

    public ICommand OnClickNext
    {
        get
        {
            return onClickNext ?? (onClickNext = new RelayCommand((o) =>
            {
                OnClickNextButton();
            }));
        }
    }

    public ICommand OnClickPrev
    {
        get
        {
            return onClickPrev ?? (onClickPrev = new RelayCommand((o) =>
            {
                OnClickPrevButton();
            }));
        }
    }

    void ClickMute()
    {
        if (_mute)
        {
            _mute = false;
            Player.Volume = 0;
        }
        else
        {
            _mute = true;
            Player.Volume = _volume;
        }
    }

    private void OnClickStopButton()
    {
        if (Player.HasAudio)
        {
            _paused = true;
            Player.Stop();
            Timer.Stop();
            AnimationEnable = false;
        }
    }

    private void OnClickNextButton()
    {
        if (Musics!.Count > SelectedIndex + 1 && SelectedIndex >= 0)
        {
            ++SelectedIndex;
            string song = Musics[SelectedIndex];
            Player.Open(new Uri(Path.Combine(MusicPath!, song)));


            if (Player.NaturalDuration.HasTimeSpan)
            {
                _maximumLimit = Player.NaturalDuration.TimeSpan.TotalSeconds;

                SelectedMP3 = song;
                SelectedIndex = Musics.IndexOf(song);


                SliderMaximum = Player.NaturalDuration.TimeSpan.TotalSeconds;


                FirstClock = Player.Position.ToString(@"hh\:mm\:ss");
                SecondClock = Player.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            }

            if (_paused)
            {
                Player.Play();
                AnimationEnable = true;
            }
            else
            {
                Player.Pause();
                AnimationEnable = false;
            }
        }
    }

    private void OnClickPrevButton()
    {
        if (0 <= SelectedIndex - 1 && SelectedIndex >= 0)
        {
            --SelectedIndex;
            string song = Musics![SelectedIndex];
            Player.Open(new Uri(Path.Combine(MusicPath!, song)));


            if (Player.NaturalDuration.HasTimeSpan)
            {
                _maximumLimit = Player.NaturalDuration.TimeSpan.TotalSeconds;

                SelectedMP3 = song;
                SelectedIndex = Musics.IndexOf(song);


                SliderMaximum = Player.NaturalDuration.TimeSpan.TotalSeconds;

                FirstClock = Player.Position.ToString(@"hh\:mm\:ss");
                SecondClock = Player.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            }

            if (_paused)
            {
                Player.Play();
                AnimationEnable = true;
            }
            else
            {
                Player.Pause();
                AnimationEnable = false;
            }
        }
    }

    private void OnClickPlayButton()
    {
        if (Player.HasAudio)
        {
            if (!_paused)
            {
                _paused = true;
                Timer.Start();
                Player.Play();
                AnimationEnable = true;
            }
            else
            {
                _paused = false;
                Player.Pause();
                Timer.Stop();
                AnimationEnable = false;
            }
        }
    }

    void ClickBrows()
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
                Player.Open(new Uri(path));
            }

            SelectedMP3 = name;
        }
    }
}