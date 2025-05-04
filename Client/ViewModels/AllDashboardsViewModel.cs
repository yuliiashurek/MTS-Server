using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels
{

    public partial class AllDashboardsViewModel : ObservableObject
    {
        [ObservableProperty]
        private int selectedTabIndex = 0;
    }
}