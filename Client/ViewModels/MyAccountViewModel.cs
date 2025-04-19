using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Client.Helpers;
using Client.Views;
using System.Windows;

namespace Client.ViewModels;

public partial class MyAccountViewModel : ObservableObject
{
    [RelayCommand]
    public void Logout()
    {
        SessionManager.ClearSession();

        if (App.Current.MainWindow is MainWindow mw)
            mw.ShowLoginView();
    }
}
