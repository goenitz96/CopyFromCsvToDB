using CSV.Models;
using CsvHelper.Configuration;

namespace CSV.Helpers;

public class HourlyDataMap : ClassMap<Hourly>
{
    public HourlyDataMap()
    {
        Map(x => x.SiteId).Name("SiteID");
        Map(x => x.startHour).Name("Start hour").Convert(row => row.Row.GetField<DateTime>("Start hour"));
        Map(x => x.finalHour).Name("Final hour").Convert(row => row.Row.GetField<DateTime>("Final hour"));
        Map(x => x.minutesUsed).Name("Minutes connected").Convert(row => row.Row.GetField<int>("Minutes connected"));
        Map(x => x.bytesUpload).Name("Uplink VolumeUsage(bits)").Convert(row => row.Row.GetField<long>("Uplink VolumeUsage(bits)"));
        Map(x => x.bytesDownload).Name("Downlink VolumeUsage(bits)").Convert(row => row.Row.GetField<long>("Downlink VolumeUsage(bits)"));
    }
}