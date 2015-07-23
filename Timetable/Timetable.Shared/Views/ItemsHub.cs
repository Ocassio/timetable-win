using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Timetable.Views
{
    class ItemsHub : Hub
    {
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ItemsHub), new PropertyMetadata(null, ItemTemplateChanged));

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ItemsHub), new PropertyMetadata(null, ItemsSourceChanged));

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsHub hub = d as ItemsHub;
            if (hub != null)
            {
                DataTemplate template = e.NewValue as DataTemplate;
                if (template != null)
                {
                    // Apply template
                    foreach (var section in hub.Sections)
                    {
                        section.ContentTemplate = template;
                    }
                }
            }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsHub hub = d as ItemsHub;
            if (hub == null) return;
            IList items = e.NewValue as IList;
            if (items == null) return;
            hub.Sections.Clear();
            foreach (var item in items)
            {
                HubSection section = new HubSection
                {
                    DataContext = item,
                    Header = item
                };
                DataTemplate template = hub.ItemTemplate;
                section.ContentTemplate = template;
                hub.Sections.Add(section);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
