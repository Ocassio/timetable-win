using Timetable.Common;
using Timetable.Data;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Timetable.Models;
using Timetable.Providers;

// Документацию по шаблону проекта "Универсальное приложение с Hub" см. по адресу http://go.microsoft.com/fwlink/?LinkID=391955

namespace Timetable
{
    /// <summary>
    /// Страница, на которой показываются общие сведения об отдельной группе, включая предварительный просмотр элементов
    /// внутри группы.
    /// </summary
    public sealed partial class GroupPage : Page
    {
        public GroupPage()
        {
            this.InitializeComponent();

            this.NavigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        /// <summary>
        /// Получает объект <see cref="NavigationHelper"/>, связанный с данным объектом <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper { get; }

        /// <summary>
        /// Получает модель представлений для данного объекта <see cref="Page"/>.
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

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
            await LoadGroups();
        }

        #region Регистрация NavigationHelper

        /// <summary>
        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// <para>
        /// Логика страницы должна быть размещена в обработчиках событий для
        /// <see cref="NavigationHelper.LoadState"/>
        /// и <see cref="NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.
        /// </para>
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async Task LoadGroups()
        {
            var selectedGroup = SettingsProvider.Group;
            try
            {
                DefaultViewModel["Groups"] = await DataProvider.GetGroups();

                if (selectedGroup != null)
                {
                    GroupList.SelectedItem = selectedGroup;
                }
                else
                {
                    GroupList.SelectedIndex = 0;
                }
            }
            catch (WebException)
            {
                if (selectedGroup != null)
                {
                    DefaultViewModel["Groups"] = new ObservableCollection<Group> { selectedGroup };
                    GroupList.SelectedIndex = 0;
                }

                await new MessageDialog(
                        "Невозможно загрузить данные. Пожалуйста, проверьте Ваше подключение к Интернет и повторите попытку.",
                        "Проблемы с соединением").ShowAsync();
            }
        }

        private void GroupList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 0) return;

            SettingsProvider.Group = (Group) e.AddedItems[0];
        }
    }
}