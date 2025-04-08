using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages;

public class IndexModel : PageModel
{
    public Chart DoughnutDataSet { get; set; }
    public List<Chart> BarLineDataSet { get; set; }

    public void OnGet()
    {
        BarLineDataSet = new List<Chart>
        {
            new()
            {
                Label = "Apple",
                Data = new List<int> { 100, 200, 250, 170, 50 },
                BackgroundColor = new[] { "#f72585" }, //https://coolors.co/palettes/trending
                BorderColor = "#b5838d"
            },
            new()
            {
                Label = "Samsung",
                Data = new List<int> { 200, 300, 350, 270, 100 },
                BackgroundColor = new[] { "#7209b7" },
                BorderColor = "#ffafcc"
            },
            new()
            {
                Label = "Total",
                Data = new List<int> { 300, 500, 600, 440, 150 },
                BackgroundColor = new[] { "#4cc9f0" },
                BorderColor = "#023e8a"
            }
        };
        DoughnutDataSet = new Chart
        {
            Label = "Apple",
            Data = new List<int> { 100, 200, 250, 170, 50 },
            BorderColor = "#ffcdb2",
            BackgroundColor = new[] { "#000814", "#001d3d", "#003566", "#ffc300", "#ffd60a" }
        };
    }
}

public class Chart
{
    [JsonProperty(PropertyName = "label")] public string Label { get; set; }

    [JsonProperty(PropertyName = "data")] public List<int> Data { get; set; }

    [JsonProperty(PropertyName = "backgroundColor")]
    public string[] BackgroundColor { get; set; }

    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }
}