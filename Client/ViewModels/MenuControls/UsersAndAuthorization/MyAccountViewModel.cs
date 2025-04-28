using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client
{

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
}
