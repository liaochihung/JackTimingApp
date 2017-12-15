using JackTimingApp.Helper;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace JackTimingApp.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private const string InitFileName = "untitled.atd";
        private const string InitMarkerData = "  |    |";
        //private const string InitTimingData = "Marker=  | |  \r\nTest=__~~__~~__";
        private const string InitTimingData = "Marker=  | |  \r\nTest=_~~__~~_\r\nTest2=__~~~__/~~~~\\__\r\nTest3=__/~~~~~\\____";

        private EditStatus _editStatus = EditStatus.Unchanged;

        public ICommand AddCommand { get; private set; }

        public ObservableCollection<TimingMap> TimingDatas { get; set; }
        public TimingMap SelectedTimingMap { get; set; }

        public ICommand SelectionChanged { get; set; }
        public ICommand InputCharCommand { get; set; }
        public ICommand ChangeSymbolCommand { get; set; }
        public ICommand PaintSurfaceCommand { get; set; }

        public string ApplicationTitle { get; set; }
        public EditStatus EditStatus
        {
            get { return _editStatus; }
            private set
            {
                _editStatus = value;
                EditStatusString = _editStatus.ToString();
            }
        }

        //  too lazy for create converter :)
        public string EditStatusString { get; set; }
        public string MarkerData { get; set; }

        public ICommand ChangeTimingCommand { get; private set; }

        public ICommand TestDialogServiceCommand { get; private set; }

        public string TimingCharacters { get; set; }

        public string TimingData { get; set; }

        private SKBitmap _bitmap;
        private IDrawEngine _drawEngine;
        public MainViewModel()
        {
            DrawParam.StartX = 50;
            DrawParam.StartY = 50;

            DrawParam.UnitX = 10;
            DrawParam.UnitY = 30;
            DrawParam.Margin = 10;
            DrawParam.VerticalSpace = 50;
            DrawParam.MarkHeight = 10;

            TimingData = InitTimingData;
            MarkerData = InitMarkerData;

            TimingDatas = TimingMapParser.Parse(TimingData);

            EditStatusString = EditStatus.Unchanged.ToString();

            MessagingCenter.Subscribe<MessageToken>(this, "message", (item) =>
            {
                if (item.TokenType == MessageTokenType.KeyChanged)
                {
                    EditStatus = EditStatus.Modified;
                    TimingDatas = TimingMapParser.Parse(TimingData);
                    UpdateTimingDiagram();
                }
            });

            InputCharCommand = new Command((ch) =>
              {
                  Debug.WriteLine("symbol label clicked, parameter: " + ch);
              });

            ChangeSymbolCommand = new Command((arg) =>
              {
                  Debug.WriteLine("symbol label clicked, parameter: " + arg);
              });

            ChangeTimingCommand = new Command((arg) =>
              {
                  Debug.WriteLine("Timing data changed : " + arg);
              });

            PaintSurfaceCommand = new Command((arg) =>
             {
                 var args = arg as SKPaintSurfaceEventArgs;

                 var info = args.Info;
                 var surface = args.Surface;
                 var canvas = surface.Canvas;

                 if (_bitmap == null)
                     _bitmap = new SKBitmap(info);

                 if (_drawEngine == null)
                 {
                     _drawEngine = new SkiaDrawEngine(_bitmap);
                 }

                 TimingDatas = TimingMapParser.Parse(TimingData);

                 _bitmap = _drawEngine.Draw(TimingDatas);

                 if (_bitmap != null)
                     canvas.DrawBitmap(_bitmap, 0, 0);

                 UpdateTimingDiagram();
             });

            UpdateTimingDiagram();
        }

        private void UpdateTimingDiagram()
        {
            MessagingCenter.Send(new MessageToken()
            {
                TokenType = MessageTokenType.UpdateTimingDiagram,
                Message = null
            }, "message");
        }
    }

    public class MessageToken
    {
        public MessageTokenType TokenType { get; set; }
        public object Message { get; set; }
    }

    public enum MessageTokenType
    {
        KeyChanged,
        UpdateTimingDiagram,
        CopyToClipboard,
        SaveBitmap
    }
}
