using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.UI.Notifications;
using Timetable.Providers;
using Timetable.Utils;

namespace Timetable.TileUpdater
{
    public sealed class TileUpdateTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            await Timtable.Lib.Tiles.TileUpdater.UpdateTile();

            deferral.Complete();
        }
    }
}
