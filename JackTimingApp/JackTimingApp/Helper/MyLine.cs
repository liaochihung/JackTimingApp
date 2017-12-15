using Xamarin.Forms;

namespace JackTimingApp.Helper
{
    public class MyLine
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public MyLine()
        {
            StartPoint = new Point(0, 0);
            EndPoint = new Point(0, 0);
        }

        public void ReCalcPoints(Point refPt)
        {
            StartPoint = new Point(StartPoint.X + refPt.X, StartPoint.Y + refPt.Y);
            EndPoint = new Point(EndPoint.X + refPt.X, EndPoint.Y + refPt.Y);
            //StartPoint.Offset(refPt.X, refPt.Y);
            //EndPoint.Offset(refPt.X, refPt.Y);
        }
    }
}