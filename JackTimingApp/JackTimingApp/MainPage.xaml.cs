using JackTimingApp.ViewModel;
using Xamarin.Forms;

namespace JackTimingApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

            MessagingCenter.Subscribe<MessageToken>(this, "message", (item) =>
            {
                switch (item.TokenType)
                {
                    case MessageTokenType.UpdateTimingDiagram:
                        DrawTiming();
                        break;
                }
            });
        }

        private void DrawTiming()
        {
            canvasView.InvalidateSurface();
        }

    }
}



