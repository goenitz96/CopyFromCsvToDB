using System.Net.Http.Headers;
using CSV.Interfaces;
using CSV.Models;
using Newtonsoft.Json;

namespace CSV.Repository;

public class JupiterRepository : IJupiterService
{
    private HttpClient http;
    private readonly IConfiguration config;

    public JupiterRepository(HttpClient http, IConfiguration config)
    {
        this.http = http;
        this.config = config;
    }

    public async Task<IList<Terminal>> getDataFromJupiter()
    {
        http = new HttpClient();
        http.DefaultRequestHeaders.Accept.Clear();
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        http.DefaultRequestHeaders.Add("Authorization", "Bearer " + config.GetConnectionString("JupiterToken"));
        try
        {
            using (var response = await http.GetAsync(config.GetConnectionString("JupiterUrl") + "?showAll=true"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IList<Terminal>>(json)
                        .Where(x => x.deviceid != null);
                    return result.ToList();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}