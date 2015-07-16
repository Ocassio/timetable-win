using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Timetable.Models;

namespace Timetable.Utils
{
    public class ColorsHelper
    {
        private static readonly List<Brush> ACCENT_COLORS = new List<Brush>()
        {
            new SolidColorBrush(Color.FromArgb(255, 164, 196, 0)),
            new SolidColorBrush(Color.FromArgb(255, 96, 169, 23)),
            new SolidColorBrush(Color.FromArgb(255, 0, 138, 0)),
            new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
            new SolidColorBrush(Color.FromArgb(255, 27, 161, 226)),
            new SolidColorBrush(Color.FromArgb(255, 0, 80, 239)),
            new SolidColorBrush(Color.FromArgb(255, 106, 0, 255)),
            new SolidColorBrush(Color.FromArgb(255, 170, 0, 255)),
            new SolidColorBrush(Color.FromArgb(255, 244, 114, 208)),
            new SolidColorBrush(Color.FromArgb(255, 216, 0, 115)),
            new SolidColorBrush(Color.FromArgb(255, 162, 0, 37)),
            new SolidColorBrush(Color.FromArgb(255, 229, 20, 0)),
            new SolidColorBrush(Color.FromArgb(255, 250, 104, 0)),
            new SolidColorBrush(Color.FromArgb(255, 240, 163, 10)),
            new SolidColorBrush(Color.FromArgb(255, 227, 200, 0)),
            new SolidColorBrush(Color.FromArgb(255, 130, 90, 44)),
            new SolidColorBrush(Color.FromArgb(255, 109, 135, 100)),
            new SolidColorBrush(Color.FromArgb(255, 100, 118, 135)),
            new SolidColorBrush(Color.FromArgb(255, 118, 96, 138)),
            new SolidColorBrush(Color.FromArgb(255, 135, 121, 78))
        };

        private ColorsHelper() { }

        public static void SetRandomColors(ObservableCollection<Day> days)
        {
            var colorsMap = new Dictionary<string, Brush>();
            foreach (var lesson in days.SelectMany(day => day.Lessons))
            {
                if (colorsMap.ContainsKey(lesson.Name))
                {
                    lesson.Color = colorsMap[lesson.Name];
                }
                else
                {
                    var random = new Random();
                    Brush color;

                    if (colorsMap.Count < ACCENT_COLORS.Count)
                    {
                        do
                        {
                            color = ACCENT_COLORS[random.Next(ACCENT_COLORS.Count - 1)];
                        }
                        while (colorsMap.ContainsValue(color));
                    }
                    else
                    {
                        color = new SolidColorBrush(Color.FromArgb(100, (byte) random.Next(255), (byte) random.Next(255), (byte) random.Next(255)));
                    }

                    lesson.Color = color;
                    colorsMap[lesson.Name] = color;
                }
            }
        }
    }
}
