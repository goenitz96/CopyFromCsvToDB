using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;

namespace CSV.Models;

public class Hourly
{
    [Key] 
    [Name("SiteID")]
    public string SiteId { get; set; }
    [Name("Start hour")]
    public DateTime startHour { get; set; }
    [Name("Final hour")]
    public DateTime finalHour { get; set; } 
    [Name("Minutes connected")]
    public int minutesUsed { get; set; }
    [Name("Uplink VolumeUsage(bits)")]
    public long bytesUpload { get; set; }
    [Name("Downlink VolumeUsage(bits)")]
    public long bytesDownload { get; set; }
    [NotMapped] 
    public List<Terminals> Terminals { get; set; }
}