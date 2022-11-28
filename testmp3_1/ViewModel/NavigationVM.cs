using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using testmp3_1.Utilities;

namespace testmp3_1.ViewModel;

class NavigationVM : ViewModelBase
{
    private object _currentView;

    public object CurrentView
    {
        get { return _currentView; }
        set { _currentView = value; OnPropertyChanged(); }
    }

    public ICommand HomeCommand { get; set; }
    public ICommand PlayerCommand { get; set; }
    public ICommand YouTubeCommand { get; set; }


    private void Home(object obj) => CurrentView = new HomeVM();
    private void Player(object obj) => CurrentView = new PlayerVM();
    private void YouTube(object obj) => CurrentView = new YouTubeVM();



    public NavigationVM()
    {
        HomeCommand = new RelayCommand(Home);
        PlayerCommand = new RelayCommand(Player);
        YouTubeCommand = new RelayCommand(YouTube);
        //baslangic seyfeni qyrq

        CurrentView = new HomeVM();
    }
}

