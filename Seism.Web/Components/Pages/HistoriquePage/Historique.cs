using Microsoft.AspNetCore.Components;
using Seism.Logique.Model;
using Seism.Logique.Services;

namespace Seism.Web.Components.Pages.HistoriquePage;

public partial class Historique(FirebaseDataService FirebaseDataService) : ComponentBase
{
    private List<AlertData> Alerts { get; set; } = new() { new AlertData() { Value = 34.3, DateEncodage = DateTime.Now } };
    private DateTime dateStart = DateTime.Now;
    private DateTime dateEnd = DateTime.Now;

    protected override async Task OnInitializedAsync()
    {
        Alerts = await FirebaseDataService.GetAllAlert();
    }

    private async void Filter()
    {
        Alerts = await FirebaseDataService.GetAllAlert();
        Alerts = Alerts.Where(a => a.DateEncodage >= dateStart && a.DateEncodage <= dateEnd).ToList();
        await InvokeAsync(StateHasChanged);
    }
}
