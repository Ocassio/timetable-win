using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Timetable.Providers;
using Timetable.Utils;

namespace Timtable.Lib.Tiles
{
    public class TileUpdater
    {
        private const int LESSON_LINES_COUNT = 4;

        private const string TILE_IMAGE = "ms-appx:///Assets/WideTileLogo.scale-100.png";

        public static async Task UpdateTile()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            var imageTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Image);
            var image = (XmlElement) imageTileXml.GetElementsByTagName("image")[0];
            image.SetAttribute("src", TILE_IMAGE);
            updater.Update(new TileNotification(imageTileXml));

            var group = SettingsProvider.Group;
            if (group == null) return;

            var dateRange = DateUtils.GetDefaultDateRange();

            try
            {
                var days = await DataProvider.GetTimetableByGroup(group.Id, dateRange);
                if (days.Count > 0)
                {
                    var lessons = days[0].Lessons;
                    var tilesCount = 0;
                    var lineNumber = 1;
                    var lessonNumber = 0;
                    while (lessonNumber < lessons.Count)
                    {
                        var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);
                        var textLines = tileXml.GetElementsByTagName("text");
                        textLines[0].InnerText = "Сегодня";

                        var lessonsRemains = lessons.Count - lessonNumber;
                        var linesRemains = LESSON_LINES_COUNT - lineNumber;

                        if (lineNumber == 0)
                        {
                            lineNumber++;
                            textLines[lineNumber].InnerText = "...";
                            linesRemains = LESSON_LINES_COUNT - lineNumber;
                            lineNumber++;
                        }

                        
                        while (lineNumber < Math.Min(linesRemains, lessonsRemains) + 1)
                        {
                            textLines[lineNumber].InnerText = lessons[lessonNumber].Number + ") " + lessons[lessonNumber].Name;
                            lineNumber++;
                            lessonNumber++;
                        }

                        if (lineNumber == LESSON_LINES_COUNT && lessonNumber < lessons.Count)
                        {
                            if (lessonNumber == lessons.Count - 1)
                            {
                                textLines[lineNumber].InnerText = lessons[lessonNumber].Number + ") " + lessons[lessonNumber].Name;
                                lessonNumber++;
                            }
                            else
                            {
                                textLines[lineNumber].InnerText = "...";
                                lineNumber = 0;
                            }
                        }

                        tilesCount++;
                        if (tilesCount <= 4)
                        {
                            updater.Update(new TileNotification(tileXml));
                        }
                    }
                }
                else
                {
                    var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);
                    var textLines = tileXml.GetElementsByTagName("text");
                    textLines[0].InnerText = "Сегодня";
                    textLines[1].InnerText = "Пары отсутствуют";
                    updater.Update(new TileNotification(tileXml));
                }
                
            }
            catch (WebException) { }
        }
    }
}
