using JackTimingApp.Helper;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JackTimingApp
{
    public interface IDrawEngine
    {
        SKBitmap Draw(ObservableCollection<TimingMap> data);
    }
}