using CommunityToolkit.Maui.Views;
using ECOMap.API; 

namespace ECOMap;
public partial class CustomPopup : Popup
{
    public treeData Tree { get; }

    public CustomPopup(treeData treeData)
    {
        InitializeComponent();
        Tree = treeData;
        BindingContext = this;
    }
}
