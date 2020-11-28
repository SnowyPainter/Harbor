using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace Harbor.Log
{
    public interface IActivityLog //Button clicks, mouse click somewhere etc...
    {
        DateTime Time { get; }
        string ActivatedElementXaml { get; }
        Point CursorPosition { get; }
    }
    [Serializable]
    public class ActivityLog:IActivityLog
    {
        public DateTime Time { get; }
        [NonSerialized]
        public FrameworkElement ActivatedElement;
        public string ActivatedElementXaml { get; }
        public Point CursorPosition { get; }
        public ActivityLog(DateTime time, FrameworkElement element, Point cursorPoint)
        {
            Time = time;
            ActivatedElement = element;
            CursorPosition = cursorPoint;
            ActivatedElementXaml = XamlWriter.Save(element);
        }
    }
}
