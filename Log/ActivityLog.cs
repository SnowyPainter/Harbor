using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ADPC.Log
{
    public interface IActivityLog //Button clicks, mouse click somewhere etc...
    {
        DateTime Time { get; }
        FrameworkElement ActivatedElement { get; }
        Point CursorPosition { get; }
    }
    public class ActivityLog:IActivityLog
    {
        public DateTime Time { get; }
        public FrameworkElement ActivatedElement { get; }
        public Point CursorPosition { get; }
        public ActivityLog(DateTime time, FrameworkElement element, Point cursorPoint)
        {
            Time = time;
            ActivatedElement = element;
            CursorPosition = cursorPoint;
        }
    }
}
