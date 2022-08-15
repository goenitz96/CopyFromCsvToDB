using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace CSV.Models;

public class Hourly
{
    [Key] 
    [Name("")]
    public string SiteId { get; set; }
    [Name("")]
    public DateTime startHour { get; set; }
    [Name("")]
    public DateTime finalHour { get; set; }
    [Name("")]
    public int minutesUsed { get; set; }
    [Name("")]
    public int bytesUpload { get; set; }
    [Name("")]
    public int bytesDownload { get; set; }
}