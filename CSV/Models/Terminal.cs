using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace CSV.Models;

[System.ComponentModel.DataAnnotations.Schema.Table("Terminals")]
public class Terminal
{
    [System.ComponentModel.DataAnnotations.Key] 
    public int id { get; set; }
    [Required]
    public string deviceid { get; set; }
    public string esn { get; set; }
    public string gatewayID { get; set; }
    public string hub { get; set; }
    public string customer { get; set; }
    public string terminalStatus { get; set; }
    public string associatedIPGWName { get; set; }
    public string satelliteName { get; set; }
    public int fapStatus { get; set; }
    public long bytesTxSinceAssoc { get; set; }
    public long bytesRxSinceAssoc { get; set; }
    public int sai { get; set; }
    public int beamID { get; set; }
    [Write(false)]
    [Computed]
    public object properties { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string address { get; set; }
    //public string latitude { get; set; }
    //public string longitude { get; set; }
    public string ipv4 { get; set; }
    public string media_type { get; set; }
    public int links_quantity { get; set; }
    public int capacity { get; set; }
    public int cid { get; set; }
    public string data_type { get; set; }
    public string value_added { get; set; }
}