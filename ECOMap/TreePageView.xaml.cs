using ECOMap.API;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ECOMap;

public partial class TreePageView : ContentPage
{
    treeData Tree;
    ObservableCollection<ImageSource> imageSources = new ObservableCollection<ImageSource>();

    public TreePageView(treeData tree)
    {
        InitializeComponent();
        Tree = tree;
        imagesCollection.ItemsSource = imageSources;
        LoadTreeImages(tree.id).ConfigureAwait(false);
        BindingContext = this;
        InitializeTreeDetails();
    }
    private void InitializeTreeDetails()
    {
        // Check if Tree is null before accessing its properties
        if (Tree != null)
        {
            Type.Text = string.IsNullOrEmpty(Tree.tree_type) ? "Not Provided" : Tree.tree_type;
            Age.Text = string.IsNullOrEmpty(Tree.plant_Age) ? "Not Provided" : Tree.plant_Age;
            Height.Text = Tree.height.HasValue ? Tree.height.Value.ToString() + " m" : "Not Provided";
            Circumference.Text = Tree.circumference.HasValue ? Tree.circumference.Value.ToString() + " cm" : "Not Provided";
            Info.Text = string.IsNullOrEmpty(Tree.comment) ? "Not Provided" : Tree.comment;
        }
        else
        {
            // If the Tree object itself is null, set all fields to "Not Provided"
            Type.Text = "Not Provided";
            Age.Text = "Not Provided";
            Height.Text = "Not Provided";
            Circumference.Text = "Not Provided";
            Info.Text = "Not Provided";
        }
    }
    private void OnPreviousClicked(object sender, EventArgs e)
    {
        // Logic to navigate to the previous tree
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        // Logic to navigate to the next tree
    }
    public async Task LoadTreeImages(int treeId)
    {
        var treeImages = await MauiProgram._ApiService.GetTreeImages(treeId);
        imageSources.Clear(); 
        foreach (var imageData in treeImages)
        {
            var imageSource = ConvertBase64ToImageSource(imageData.base64);
            if (imageSource != null)
            {
                imageSources.Add(imageSource);
            }
        }
    }
    public ImageSource ConvertBase64ToImageSource(string base64)
    {
        try
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to convert base64 to ImageSource: " + ex.Message);
            return null;
        }
    }
}