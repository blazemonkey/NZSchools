using NZSchools.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NZSchools.UserControls
{
    public sealed partial class MapIconUserControl : UserControl
    {
        public static readonly DependencyProperty DirectoryMapIconProperty =
            DependencyProperty.Register("Directory", typeof(Directory), typeof(MapIconUserControl), null);

        public Directory Directory
        {
            get { return this.GetValue(DirectoryMapIconProperty) as Directory; }
            set { this.SetValueDp(DirectoryMapIconProperty, value); }
        }

        public MapIconUserControl(Directory directory)
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            Directory = directory;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValueDp(DependencyProperty property, object value, [CallerMemberName]string propertyName = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
