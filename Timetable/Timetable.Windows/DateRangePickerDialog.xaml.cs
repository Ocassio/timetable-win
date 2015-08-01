using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Timetable.Models;

namespace Timetable
{
    public sealed partial class DateRangePickerDialog : UserControl
    {
        private TaskCompletionSource<DateRange> taskCompletionSource;

        private readonly DateRange dateRange;

        public DateRangePickerDialog(DateRange dateRange)
        {
            InitializeComponent();

            Window.Current.SizeChanged += OnWindowResize;

            this.dateRange = dateRange;
            From.Date = dateRange.From;
            To.Date = dateRange.To;
        }

        public Task<DateRange> ShowAsync()
        {
            RecalculateSize();
            m_Popup.IsOpen = true;
            taskCompletionSource = new TaskCompletionSource<DateRange>();
            return taskCompletionSource.Task;
        }

        private void RecalculateSize()
        {
            m_Rect1.Height = Window.Current.Bounds.Height;
            m_Rect1.Width = Window.Current.Bounds.Width;
            m_Rect2.Width = Window.Current.Bounds.Width;
        }

        private void OnWindowResize(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            RecalculateSize();
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

        private void From_OnDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            dateRange.From = From.Date.Date;
            if (dateRange.To != To.Date.Date)
            {
                To.Date = dateRange.To;
            }
        }

        private void To_OnDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            dateRange.To = To.Date.Date;
            if (dateRange.From != From.Date.Date)
            {
                From.Date = dateRange.From;
            }
        }
    }
}
