using parkSmart.Controllers;

namespace parkSmart.Views;

public partial class PgListagem : ContentPage
{
    private VeiculosController veiculosController;
	public PgListagem()
	{
		InitializeComponent();

        veiculosController = new VeiculosController();
        AtualizarListView();

    }

    private void AtualizarListView()
    {
        VeiculosListView.ItemsSource =
                veiculosController.GetAll();
    }

    private void btnAplicarFiltros_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPagamento_Clicked(object sender, EventArgs e)
    {

    }

    private void btnExcluir_Clicked(object sender, EventArgs e)
    {

    }

}