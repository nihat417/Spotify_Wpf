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
    #region Fields_and_Properties

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
    public bool _buttonsEnable = false;
    public bool _animationEnable = false;
    public bool _playButtonIsChecked = false;

    private ICommand? _onClickMute;
    private ICommand? onClickPrev;
    private ICommand? onClickPlay;
    private ICommand? onClickStop;
    private ICommand? onClickNext;
    private ICommand? onClickBrowsButton;

    #endregion

    /// <summary>
    /// Constructor
    /// </summary>
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

    #region Construtor Methods

    /// <summary>
    /// Timer Ticker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (Player.NaturalDuration.HasTimeSpan)
        {
            SliderValue = Player.Position.TotalSeconds;
            FirstClock = Player.Position.ToString(@"hh\:mm\:ss");
            SecondClock = (Player.NaturalDuration.TimeSpan - Player.Position).ToString(@"hh\:mm\:ss");
            if (SliderValue == SliderMaximum)
                AnimationEnable = false;
            else
                AnimationEnable = true;
        }
    }

    /// <summary>
    /// Folder creater and finder
    /// </summary>
    /// <param name="folderName"></param>
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

    /// <summary>
    /// Music files getter
    /// </summary>
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

    #endregion

    #region Buttons_Values

    /// <summary>
    /// Buttons enabler
    /// </summary>
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

    /// <summary>
    /// Animations enabler
    /// </summary>
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

    /// <summary>
    /// Pause button
    /// </summary>
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

    /// <summary>
    /// Started timer
    /// </summary>
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

    /// <summary>
    /// Last timer
    /// </summary>
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

    /// <summary>
    /// Slider value
    /// </summary>
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

    /// <summary>
    /// Volume value
    /// </summary>
    public double Volume
    {
        get { return _volume = (int)(Player.Volume * 10); }
        set
        {
            Player.Volume = value / 10;
            OnPropertyChanged("Volume");
        }
    }

    /// <summary>
    /// Slider maximum value
    /// </summary>
    public double SliderMaximum
    {
        get { return _maximumLimit; }
        set
        {
            _maximumLimit = value;

            OnPropertyChanged("SliderMaximum");
        }
    }

    /// <summary>
    /// Music Slider value
    /// </summary>
    public double SliderValue
    {
        get { return _slidervalue; }
        set
        {
            _slidervalue = value;

            OnPropertyChanged("SliderValue");
        }
    }

    /// <summary>
    /// Selected music index
    /// </summary>
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

    /// <summary>
    /// Selected music name
    /// </summary>
    public string? SelectedMP3
    {
        get { return _selectedMP3; }

        set
        {

            _selectedMP3 = value;

            SelectedMp3();

            OnPropertyChanged("SelectedMP3");
        }
    }

    /// <summary>
    /// Music list
    /// </summary>
    public ObservableCollection<string>? ListMP3
    {
        get { return Musics; }

        set
        {
            Musics = value;
            OnPropertyChanged("m_listBox");
        }
    }

    #endregion

    #region Commands

    /// <summary>
    /// Music files Brows Button
    /// </summary>
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

    /// <summary>
    /// Mute Click Command
    /// </summary>
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

    /// <summary>
    /// Play Click Command
    /// </summary>
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

    /// <summary>
    /// Stop Click Command
    /// </summary>
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

    /// <summary>
    /// Next Click Command
    /// </summary>
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

    /// <summary>
    /// Previous Click Command
    /// </summary>
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

    #endregion

    #region Methods

    /// <summary>
    /// Methods of Commands
    /// </summary>
    /// 
    void SelectedMp3()
    {
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
            SliderValue = 0;
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

    #endregion
}