using System.Windows.Input;
using testmp3_1.Utilities;
using testmp3_1.View;


namespace testmp3_1.ViewModel;


class NavigationVM : ViewModelBase
{
    private object? _currentView;

    public Home HomeMainClass = new();
    public Player PLayerMainClass = new();
    public YouTube YouTubeMainClass = new();
    

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
            CurrentView = HomeMainClass;
    }
    private void Player(object obj)
    {
        if (CurrentView == null)
            CurrentView = new Player();
        else
            CurrentView = PLayerMainClass;
    }
    private void YouTube(object obj)
    {
        if (CurrentView == null)
            CurrentView = new YouTube();
        else
            CurrentView = YouTubeMainClass;
    }
}

