using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Timetable.Models;

namespace Timetable.Providers
{
    public class CacheProvider
    {
        private const string TIMETABLE_FILE = "timetable.json";

        private CacheProvider() { }

        public static async Task SaveTimetable(ObservableCollection<Day> timetable)
        {
            if (timetable == null) return;

            var json = JsonConvert.SerializeObject(timetable.ToList());
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(TIMETABLE_FILE,
                CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var writer = new DataWriter(stream))
                {
                    writer.WriteString(json);
                    await writer.StoreAsync();
                }
            }
        }

        public static async Task<ObservableCollection<Day>> LoadTimetable()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(TIMETABLE_FILE);
                using (var stream = await file.OpenReadAsync())
                {
                    using (var reader = new DataReader(stream))
                    {
                        var length = (uint) stream.Size;
                        await reader.LoadAsync(length);
                        var timetable = JsonConvert.DeserializeObject<List<Day>>(reader.ReadString(length));

                        return new ObservableCollection<Day>(timetable);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return new ObservableCollection<Day>();
            }
        } 
    }
}
