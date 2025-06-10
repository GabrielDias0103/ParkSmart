using parkSmart.Controllers;
using parkSmart.Models;

namespace parkSmart.Views;

public partial class PgListagem : ContentPage
{
    private VeiculosController veiculosController;
    private List<Veiculos> listaOriginal = new();
    public PgListagem()
	{
        InitializeComponent();
        FiltroStatusPicker.SelectedIndex = 0; // "Todos" padrão
        veiculosController = new VeiculosController();
        listaOriginal = veiculosController.GetAll();
        AtualizarListView();

    }

    private void AtualizarListView()
    {
        listaOriginal = veiculosController.GetAll();
        VeiculosListView.ItemsSource = listaOriginal;
    }

    private void btnAplicarFiltros_Clicked(object sender, EventArgs e)
    {
        listaOriginal = veiculosController.GetAll(); // Sempre pega a lista mais atual

        string placaFiltro = FiltroPlacaEntry.Text?.ToLower() ?? "";
        DateTime? dataFiltro = FiltroDataPicker.Date;
        string statusFiltro = FiltroStatusPicker.SelectedItem?.ToString();

        var filtrados = listaOriginal.Where(v =>
            (string.IsNullOrEmpty(placaFiltro) || v.Placa.ToLower().Contains(placaFiltro)) &&
            (v.DataEntrada.Date == dataFiltro.Value.Date) &&
            (statusFiltro == "Todos" ||
                (statusFiltro == "Pago" && v.Pago) ||
                (statusFiltro == "Não Pago" && !v.Pago))
        ).ToList();

        VeiculosListView.ItemsSource = filtrados;
    }

    private void btnPagamento_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnExcluir_Clicked(object sender, EventArgs e)
    {
        TappedEventArgs tapped = (TappedEventArgs)e;
        if (tapped.Parameter is Veiculos registro)
        {
            //iremos usar um display alert para confirmar a exclusao do registro.
            //pelo fato do display ficar aberto até a tomada de decisão, é preciso chama-lo de maneira assinctrona
            bool decisao = await DisplayAlert("Confirmação", "Deseja realmente excluir o registro?", "Sim", "Não");

            if (decisao)
            {
                //iremos chamar a rotina delete da camada controller e atualizar a listview
                veiculosController.Delete(registro);
                AtualizarListView();
            }
        }
    }
}