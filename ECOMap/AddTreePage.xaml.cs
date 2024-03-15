namespace ECOMap;

public partial class AddTreePage : ContentPage
{
    
	public AddTreePage()
	{
        InitializeComponent();

    }
    private void Button_Pressed(object sender, EventArgs e)
    {
        ((Button)sender).BackgroundColor = Colors.White;
    }

    private void Button_Released(object sender, EventArgs e)
    {
        ((Button)sender).BackgroundColor = new Color(28, 115, 25);
    }
}