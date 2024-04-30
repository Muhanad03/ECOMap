using ECOMap.API;
using ECOMap.config;
using System.Reflection;
using System.Text;

namespace ECOMap
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();

           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //var treeDataList = await MauiProgram._ApiService.GetTreeDataAsync();

            //if(treeDataList.Count > 0 )
            //{
            //    var treeDataStringBuilder = new StringBuilder();
            //    foreach (var treeData in treeDataList)
            //    {
            //        treeDataStringBuilder.AppendLine(GetTreeDataProperties(treeData));
            //    }

            //    label.Text = treeDataStringBuilder.ToString();

            //}

            //var tree = new treeData()
            //{
            //    height = 200,
            //    circumference = 301,
            //    addedByUser_ID = 23,
            //    longitude = 8.2,
            //    latitude = -4.3,
            //    plant_Age = "Jarmey",
            //    planter_Name = "The French",
            //};
            //var t = await MauiProgram._ApiService.AddTreeDataAsync(tree);
            //System.Diagnostics.Debug.WriteLine(t);
            //await DisplayAlert("Error",t,"Yes");
        }

        private string GetTreeDataProperties(treeData data)
        {
            Type type = typeof(treeData);
            PropertyInfo[] properties = type.GetProperties();
            var stringBuilder = new StringBuilder();
            foreach (var prop in properties)
            {
                object value = prop.GetValue(data);
                stringBuilder.AppendLine($"{prop.Name}: {value}");
            }
            return stringBuilder.ToString();
        }
    }
}
