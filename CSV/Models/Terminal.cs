using System.ComponentModel.DataAnnotations;

namespace CSV.Models;

public class Terminal
{
    [Key] 
    public int id { get; set; }
    [Required]
    public string deviceid { get; set; }
    public string esn { get; set; }
    public string location { get; set; }
    public string hub { get; set; }
    public string customer { get; set; }
    public string[] properties { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string address { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string ipv4 { get; set; }
    public string media_type { get; set; }
    public int links_quantity { get; set; }
    public int capacity { get; set; }
    public int cid { get; set; }
    public string data_type { get; set; }
    public string value_added { get; set; }
}