using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// Получает NavigationHelper, используемый для облегчения навигации и управления жизненным циклом процессов.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Получает DefaultViewModel. Эту модель можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
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

            await LoadTimetable();
        }

        /// <summary>
        /// Вызывается при щелчке заголовка HubSection.
        /// </summary>
        /// <param name="sender">Главная страница, которая содержит элемент HubSection, заголовок которого щелкнул пользователь.</param>
        /// <param name="e">Данные события, описывающие, каким образом был инициирован щелчок.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри раздела.
        /// </summary>
        /// <param name="sender">Представление сетки или списка
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemPage), itemId);
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
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async Task LoadTimetable()
        {
            var group = await GetSelectedGroup();
            if (group == null) return;

            ProgressBar.IsIndeterminate = true;

            try
            {
                var dr = DateUtils.GetDateRange(SettingsProvider.DateRangeType);

                var days = await DataProvider.GetTimetableByGroup(group.Id, dr);
//                var days = await DataProvider.GetTimetableByGroup("557", dr);
                ColorsHelper.SetRandomColors(days);
                DefaultViewModel["Days"] = days;

                await CacheProvider.SaveTimetable(days);
            }
            catch (WebException)
            {
                await new MessageDialog(
                    "Невозможно загрузить данные. Пожалуйста, проверьте Ваше подключение к Интернет и повторите попытку.",
                    "Проблемы с соединением").ShowAsync();
            }

            ProgressBar.IsIndeterminate = false;
        }

        private async Task<Group> GetSelectedGroup()
        {
            var group = SettingsProvider.Group;
            if (group == null)
            {
                try
                {
                    var groups = await DataProvider.GetGroups();
                    if (groups.Count > 0)
                    {
                        group = groups[0];
                    }
                    SettingsProvider.Group = group;
                }
                catch (WebException)
                {
                    await new MessageDialog(
                        "Невозможно загрузить данные. Пожалуйста, проверьте Ваше подключение к Интернет и повторите попытку.",
                        "Проблемы с соединением").ShowAsync();
                }
            }

            return group;
        }

        private async void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            await LoadTimetable();
        }
    }
}
