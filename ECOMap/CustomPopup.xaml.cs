using CommunityToolkit.Maui.Views;
using ECOMap.API; 

namespace ECOMap;
public partial class CustomPopup : Popup
{
    public treeData Tree { get; }
    ContentPage page;
    public CustomPopup(treeData treeData, ContentPage page)
    {
        InitializeComponent();
        Tree = treeData;
        this.page = page;
        BindingContext = this;
        Label1.Text = $"Tree type: {Tree.tree_type}";
        Label2.Text = $"Tree age: {Tree.plant_Age}";

        if(Tree.guardian_ID != null )
        {
            Label3.Text = "This tree has a guardian";
        }
        else
        {
            Label3.Text = "This tree does not have a guardian";

        }


    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        page.Navigation.PushAsync(new TreePageView(Tree));
        this.Close();
    }
}
