using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Seism.Logique.Model;
using Seism.Logique.Services;

namespace Seism.Web.Components.Pages.HomePage;

public partial class Home(FirebaseDataService FirebaseDataService) : ComponentBase
{
    private List<AlertData> Alerts { get; set; } = new();
    private List<AlertData> Datas { get; set; } = new();


    private bool _initialized = false;
    protected override async Task OnInitializedAsync()
    {
        Alerts = new();
        Datas = await FirebaseDataService.GetAllAlert();

        Alerts.AddRange(Datas.Where(x => x.DateEncodage.Date >= DateTime.Now.Date && x.DateEncodage.Date <= DateTime.Now.AddDays(1).Date).ToList());

        FirebaseDataService.OnDataChanged += HandleDataChanged;

        FirebaseDataService.ListenForChanges();
    }

    private async void HandleDataChanged(object sender, AlertData data)
    {
        // Add new data to the list
        if (!Datas.Contains(data))
            Alerts.Add(data);

        // Notify Blazor to re-render the component
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        // Unsubscribe from the event
        FirebaseDataService.OnDataChanged -= HandleDataChanged;
    }

    private ElementReference chartContainer;
    private IJSObjectReference? chartInstance;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var config = new
            {
                Type = "bar",
                Data = new
                {
                    Labels = new[] { "Red", "Blue", "Yellow" },
                    Datasets = new[]
                    {
                        new { Label = "Votes", Data = new[] { 12, 19, 3 } }
                    }
                }
            };

            // Load JS module
            var jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./chartInterop.js");

            // Render chart
            // chartInstance = await jsModule.InvokeAsync<IJSObjectReference>(
            //     "renderChart", "myChart", config);

            // await jsModule.InvokeAsync<IJSObjectReference>(
            //     "sayHello"
            // );
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (chartInstance != null)
            await chartInstance.DisposeAsync();
    }
}
