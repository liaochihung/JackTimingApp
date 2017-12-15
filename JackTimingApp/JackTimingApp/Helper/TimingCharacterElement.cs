using System.Collections.Generic;
using Xamarin.Forms;

namespace JackTimingApp.Helper
{
    public class TimingCharacterElement
    {
        public List<MyLine> Lines { get; set; }
        public string Char { get; set; }

        private Point _topLeftPoint;
        public Point TopLeftPoint
        {
            get { return _topLeftPoint; }
            set
            {
                _topLeftPoint = value;

                if (Lines == null)
                    return;

                foreach (var myLine in Lines)
                {
                    myLine.ReCalcPoints(_topLeftPoint);
                }
            }
        }

        public TimingCharacterElement()
        {
            TopLeftPoint = new Point(0, 0);
            Lines = new List<MyLine>();
            Char = string.Empty;
        }
    }
}