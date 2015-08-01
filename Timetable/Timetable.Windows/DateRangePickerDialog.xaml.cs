using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Timetable.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Timetable
{
    public sealed partial class DateRangePickerDialog : UserControl
    {
        private TaskCompletionSource<DateRange> taskCompletionSource;

        public DateRangePickerDialog()
        {
            InitializeComponent();
        }

        public Task<DateRange> ShowAsync()
        {
            InitFields();
            m_Popup.IsOpen = true;
            taskCompletionSource = new TaskCompletionSource<DateRange>();
            return taskCompletionSource.Task;
        }

        public void InitFields()
        {
            m_Rect1.Height = Window.Current.Bounds.Height;
            m_Rect1.Width = Window.Current.Bounds.Width;
            m_Rect2.Width = Window.Current.Bounds.Width;
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            var result = new DateRange(From.Date.Date, To.Date.Date);
            taskCompletionSource.SetResult(result);
            m_Popup.IsOpen = false;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            taskCompletionSource.SetResult(null);
            m_Popup.IsOpen = false;
        }
    }
}
