using AutoUp.Core;
using System;


namespace AutoUp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand AppViewCommand { get; set; }
        public RelayCommand MyAppViewCommand { get; set; }
        public RelayCommand IgnorViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }


        public AppViewModel  AppVm { get; set; }
        public MyAppViewModel MyAppVm { get; set; }
        public IgnorViewModel IgnorVm { get; set; }
        public SettingsViewModel SetVm { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value; 
                OnPropertyChanged();
            }
        }


        public MainViewModel() {

            AppVm = new AppViewModel();
            MyAppVm = new MyAppViewModel();
            IgnorVm = new IgnorViewModel();
            SetVm = new SettingsViewModel();

            CurrentView = AppVm;

            AppViewCommand = new RelayCommand(o => 
            {
                CurrentView = AppVm;
            });
            MyAppViewCommand = new RelayCommand(o =>
            {
                CurrentView = MyAppVm;
            });
            IgnorViewCommand = new RelayCommand(o =>
            {
                CurrentView = IgnorVm;
            });
            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SetVm;
            });
        }
    }
}
