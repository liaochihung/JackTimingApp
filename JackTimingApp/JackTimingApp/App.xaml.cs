using JackTimingApp.ViewModel;
using Xamarin.Forms;

namespace JackTimingApp
{
    public partial class App : Application
    {
        private MainViewModel _mainViewModel;

        public App()
        {
            InitializeComponent();

            _mainViewModel = new MainViewModel();

            // if we create the restore function
            //_mainViewModel.RestoreState();

            MainPage = new JackTimingApp.MainPage(_mainViewModel);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            //_mainViewModel.SaveState();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
