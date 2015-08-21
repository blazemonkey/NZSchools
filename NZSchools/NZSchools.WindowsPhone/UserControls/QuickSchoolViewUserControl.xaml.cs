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
using Windows.UI.Core;
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
    public sealed partial class QuickSchoolViewUserControl : UserControl
    {
        public static readonly DependencyProperty DirectoryQuickViewProperty =
            DependencyProperty.Register("Directory", typeof(Directory), typeof(QuickSchoolViewUserControl), null);

        public Directory Directory
        {
            get { return this.GetValue(DirectoryQuickViewProperty) as Directory; }
            set { this.SetValueDp(DirectoryQuickViewProperty, value); }
        }

        public EventHandler BackToMapButtonTapped;

        public QuickSchoolViewUserControl(Directory directory)
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            PopupGrid.Width = Window.Current.Bounds.Width;
            PopupGrid.Height = Window.Current.Bounds.Height;          
  
            Directory = directory;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            PopupGrid.Width = Window.Current.Bounds.Width;
            PopupGrid.Height = Window.Current.Bounds.Height;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValueDp(DependencyProperty property, object value, [CallerMemberName]string propertyName = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackToMapButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BackToMapButtonTapped.Invoke(sender, EventArgs.Empty);
        }
    }
}
