using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public partial class Organization : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private string edrpouCode;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string cityForDocs;

        [ObservableProperty]
        private string fioForDocs;
    }
}
