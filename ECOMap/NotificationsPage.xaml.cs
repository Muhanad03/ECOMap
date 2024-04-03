using ECOMap.API;
using System.Text;

namespace ECOMap;

public partial class NotificationsPage : ContentPage
{

	public NotificationsPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var treeDataList = await MauiProgram._ApiService.GetTreeDataAsync();

        var treeDataStringBuilder = new StringBuilder();
        foreach (var treeData in treeDataList)
        {
            treeDataStringBuilder.AppendLine($"ID: {treeData.id}, Latitude: {treeData.latitude}, Longitude: {treeData.longitude}");
        }

        label.Text = treeDataStringBuilder.ToString();
    }


}