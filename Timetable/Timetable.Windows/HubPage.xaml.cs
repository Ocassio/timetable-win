using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
using Timetable.Data;
using Timetable.Common;
using Timetable.Providers;
using System.Threading.Tasks;
using Timetable.Models;
using System.Text;
using Windows.UI;
using Windows.UI.Popups;
using Timetable.Utils;

// Документацию по шаблону проекта "Универсальное приложение с Hub" см. по адресу http://go.microsoft.com/fwlink/?LinkID=391955

namespace Timetable
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        /// <summary>
        /// Получает NavigationHelper, используемый для облегчения навигации и управления жизненным циклом процессов.
        /// </summary>
        public NavigationHelper NavigationHelper { get; }

        /// <summary>
        /// Получает DefaultViewModel. Эту модель можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

        public HubPage()
        {
            this.InitializeComponent();
            this.NavigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации.  Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, object)"/> при первоначальном запросе этой страницы, и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Это состояние будет равно NULL при первом посещении страницы.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var days = await CacheProvider.LoadTimetable();
            ColorsHelper.SetRandomColors(days);
            DefaultViewModel["Days"] = days;

            LoadDateRange();
            await LoadGroups();
            await LoadTimetable();
        }

        #region Регистрация NavigationHelper

        /// <summary>
        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// и <see cref="Common.NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            await LoadTimetable();
        }

        private async void GroupList_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var selectedGroup = (Group) selectionChangedEventArgs.AddedItems[0];
            SettingsProvider.Group = selectedGroup;
            await LoadTimetable();
        }

        private async Task LoadTimetable()
        {
            ProgressBar.IsIndeterminate = true;

            var group = ((Group) GroupList.SelectedItem).Id;
            var dateRange = ((ComboBoxItem) DateRangeList.SelectedItem).Tag.ToString();

            var dr = DateUtils.GetDateRange(dateRange);

            var days = await DataProvider.GetTimetableByGroup(group);
            ColorsHelper.SetRandomColors(days);
            DefaultViewModel["Days"] = days;

            await CacheProvider.SaveTimetable(days);

            ProgressBar.IsIndeterminate = false;
        }

        private async Task LoadGroups()
        {
            DefaultViewModel["Groups"] = await DataProvider.GetGroups();
            var selectedGroup = SettingsProvider.Group;
            if (selectedGroup != null)
            {
                GroupList.SelectedItem = selectedGroup;
            }
            else
            {
                GroupList.SelectedIndex = 0;
            }
        }

        private void LoadDateRange()
        {
            var dateRange = SettingsProvider.DateRangeType;
            DateRangeList.SelectedItem = DateRangeList.Items.Single(o => ((ComboBoxItem) o).Tag.Equals(dateRange));
        }

        private async void DateRangeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 0) return;

            var dateRangeType = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();

            SettingsProvider.DateRangeType = dateRangeType;
            await LoadTimetable();

            if (dateRangeType == "custom")
            {
                //TODO show date range selection dialog
            }
        }
    }
}
