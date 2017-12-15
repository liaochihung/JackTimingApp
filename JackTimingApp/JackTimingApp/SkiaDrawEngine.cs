using JackTimingApp.Helper;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace JackTimingApp
{
    public class SkiaDrawEngine : IDrawEngine
    {
        private SKCanvas _canvas;
        private SKBitmap _bitmap;
        public string TimingCharacters { get; set; }
        private int _lineIndex;
        private TimingMap _timingMap;

        public SkiaDrawEngine(SKBitmap bitmap)
        {
            _bitmap = bitmap;
            _canvas = new SKCanvas(_bitmap);
        }

        private ObservableCollection<TimingMap> _timingDatas;

        public SKBitmap Draw(ObservableCollection<TimingMap> data)
        {
            InitDrawParam();

            _timingDatas = (data);
            _canvas.DrawColor(SKColors.White);

            var paint = new SKPaint
            {
                TextSize = 28.0f,
                IsAntialias = true,
                Color = (SKColor)0xFF4281A4,
                TextEncoding = SKTextEncoding.Utf8
            };

            // calc the width of symbol
            var max = 0.0;
            foreach (var timingData in _timingDatas)
            {
                var x = paint.MeasureText(timingData.Symbol);
                if (max < x)
                    max = x;
            }

            DrawParam.StartX = (int)max + 30;

            // determine the width of entire diagram 
            max = 0;
            foreach (var timingData in _timingDatas)
            {
                if (max < (int)timingData.Timing.Length)
                    max = (int)timingData.Timing.Length;
            }

            var lineIndex = 0;

            // draw content
            foreach (var timingMap in _timingDatas)
            {
                _timingMap = timingMap;
                if (IsMarker())
                {
                    lineIndex++;
                    continue;
                }

                DrawTimingCharacter();

                lineIndex++;
                DrawParam.StartY += DrawParam.VerticalSpace;
            }

            DrawMarker(lineIndex);

            return _bitmap;
        }

        private void DrawTimingCharacter()
        {
            var style = new SKPaint()
            {
                TextSize = 28.0f,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = (SKColor)0xFF4281A4,
                StrokeWidth = 2
            };

            _canvas.DrawText(_timingMap.Symbol,
                5, DrawParam.StartY + (_lineIndex * DrawParam.UnitY),
                style);

            // draw elements
            var xIndex = 0;
            foreach (var element in _timingMap.Elements)
            {
                if (element == null)
                    continue;

                // re calc reference point
                element.TopLeftPoint = new Point(
                    5 + DrawParam.StartX + (xIndex * DrawParam.UnitX),
                    DrawParam.StartY + (_lineIndex * DrawParam.UnitY) - 30);

                // get line data and draw out
                foreach (var myLine in element.Lines)
                {
                    _canvas.DrawLine(
                        (float)myLine.StartPoint.X,
                        (float)myLine.StartPoint.Y,
                        (float)myLine.EndPoint.X,
                        (float)myLine.EndPoint.Y,
                        style
                    );

                    //if (element.Char != string.Empty)
                    //    GraphicHelper.DrawText(
                    //        Canvas,
                    //        element.Char,
                    //        new Point(element.TopLeftPoint.X,
                    //            element.TopLeftPoint.Y),
                    //        14,
                    //        HorizontalAlignment.Center,
                    //        Brushes.Black);
                }
                xIndex++;

            }

            // 畫出連接線
            for (int i = 1; i < _timingMap.Timing.Length; i++)
            {
                var ch1 = _timingMap.Timing[i - 1];
                var ch2 = _timingMap.Timing[i];

                if (ch1 == ch2)
                    continue;

                if (ch1 == '*' || ch2 == '*')
                    continue;
                if (ch1 == ':' || ch2 == ':')
                    continue;
                if (ch1 == '<' || ch2 == '<')
                    continue;


                if (_timingMap.Elements[i - 1] == null ||
                    _timingMap.Elements[i] == null)
                    continue;

                if (_timingMap.Elements[i - 1].Lines == null)
                    continue;

                if (_timingMap.Elements[i - 1].Lines.Count < 1)
                    continue;

                if (_timingMap.Elements[i].Lines == null)
                    continue;

                if (_timingMap.Elements[i].Lines.Count < 1)
                    continue;

                _canvas.DrawLine(
                    (float)_timingMap.Elements[i - 1].Lines[0].EndPoint.X,
                    (float)_timingMap.Elements[i - 1].Lines[0].EndPoint.Y,
                    (float)_timingMap.Elements[i].Lines[0].StartPoint.X,
                    (float)_timingMap.Elements[i].Lines[0].StartPoint.Y, style
                );

            }
        }

        public bool IsMarker()
        {
            if (_timingMap.Timing.Contains("|"))
                return true;

            return false;
        }

        private void InitDrawParam()
        {
            DrawParam.StartX = 50;
            DrawParam.StartY = 50;
        }

        public void DrawMarker(int max)
        {
            var style = new SKPaint()
            {
                TextSize = 28.0f,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = (SKColor)0xFF4281A4,
                StrokeWidth = 2
            };

            _canvas.DrawLine(
                DrawParam.StartX,
                5,
                DrawParam.StartX + (5 * DrawParam.UnitX),
                5,
                style);

            for (int i = 0; i <= max; i++)
            {
                int x = i * DrawParam.UnitX + DrawParam.StartX;
                _canvas.DrawLine(
                    x,
                    5,
                    x,
                    DrawParam.MarkHeight,
                    style);
            }

            _timingMap = _timingDatas[0];

            var labelIndex = new List<int>();
            for (var index = 0; index < _timingMap.Timing.Length; index++)
            {
                if (_timingMap.Timing[index] == '|')
                {
                    labelIndex.Add(index);
                }
            }

            var pathStyle = new SKPaint()
            {
                TextSize = 24.0f,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.GreenYellow,
                StrokeWidth = 4,
                StrokeCap = SKStrokeCap.Square,
                PathEffect = SKPathEffect.CreateDash(new float[] { 1, 1 }, 2),
            };

            var path = new SKPath();
            foreach (var i in labelIndex)
            {
                Debug.WriteLine("marker position : " + i);
                path.MoveTo(
                    5 + DrawParam.StartX + i * DrawParam.UnitX,
                    8);

                path.LineTo(
                    5 + DrawParam.StartX + i * DrawParam.UnitX,
                    8 + (max / 2 * (DrawParam.UnitY + DrawParam.VerticalSpace))
                    );

                _canvas.DrawPath(path, pathStyle);
            }
        }
    }
}