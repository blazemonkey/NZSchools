using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace NZSchools.Helpers
{
    public static class VisualHelper
    {
        public static T FindChildControl<T>(DependencyObject control, string ctrlName) where T : DependencyObject
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                var child = VisualTreeHelper.GetChild(control, i);
                var fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return (T)child;
                }
                else
                {
                    // Not found it - search children
                    var nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }
    }
}
