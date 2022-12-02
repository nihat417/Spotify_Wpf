using System.Windows.Input;
using testmp3_1.Utilities;
using testmp3_1.View;


namespace testmp3_1.ViewModel;


class NavigationVM : ViewModelBase
{
    private object? _currentView;

    Home _homeView = new();
    Player _playerView = new();
    YouTube _youtubeView = new();
    

    public NavigationVM()
    {
        HomeCommand = new RelayCommand(Home);
        PlayerCommand = new RelayCommand(Player);
        YouTubeCommand = new RelayCommand(YouTube);
        


        //Primary page
        CurrentView = new Home();
    }



    public object CurrentView
    {
        get { return _currentView!; }
        set { _currentView = value; OnPropertyChanged(); }
    }

    public ICommand HomeCommand { get; set; }
    public ICommand PlayerCommand { get; set; }
    public ICommand YouTubeCommand { get; set; }



    private void Home(object obj)
    {
        if (CurrentView == null)
            CurrentView = new Home();
        else
            CurrentView = _homeView;
    }
    private void Player(object obj)
    {
        if (CurrentView == null)
            CurrentView = new Player();
        else
            CurrentView = _playerView;
    }
    private void YouTube(object obj)
    {
        if (CurrentView == null)
            CurrentView = new YouTube();
        else
            CurrentView = _youtubeView;
    }

}

