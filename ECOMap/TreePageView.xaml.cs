using ECOMap.API;

namespace ECOMap;

public partial class TreePageView : ContentPage
{
	treeData Tree;
	public TreePageView(treeData tree)
	{
		InitializeComponent();
		Tree = tree; 
	}
}