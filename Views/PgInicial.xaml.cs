namespace parkSmart.Views;

public partial class PgInicial : ContentPage
{
	public PgInicial()
	{
		InitializeComponent();
	}

    private void btnMenu_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}