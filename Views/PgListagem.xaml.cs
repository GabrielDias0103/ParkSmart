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

        // Filtro por placa e data
        var veiculos = listaOriginal.Where(v =>
            (string.IsNullOrEmpty(placaFiltro) || v.Placa.ToLower().Contains(placaFiltro)) &&
            (v.DataEntrada.Date == dataFiltro.Value.Date)
        ).ToList();

        // Filtro por status de pagamento
        if (statusFiltro == "Pago")
            veiculos = veiculos.Where(v => v.Pago).ToList();
        else if (statusFiltro == "Não Pago")
            veiculos = veiculos.Where(v => !v.Pago).ToList();

        VeiculosListView.ItemsSource = veiculos;
    }

    private async void btnPagamento_Clicked(object sender, EventArgs e)
    {
        var image = sender as Image;
        var veiculo = image?.BindingContext as Veiculos;
        if (veiculo == null) return;

        if (veiculo.Pago == true)
        {
            await DisplayAlert("Aviso", "Veículo já está pago", "Ok");
            return;
        }
        else
        {

        
            // Mensagem de confirmação
            bool confirmar = await DisplayAlert("Confirmação", "Deseja realmente marcar este veículo como pago?", "Sim", "Não");
        if (!confirmar)
            return;

            // Atualiza o status para pago
            var horaSaida = DateTime.Now;
            veiculo.Pago = true;
            veiculo.DataSaida = horaSaida;

            // Atualize no banco de dados9
            var controller = new VeiculosController();
            bool atualizado = controller.AtualizarStatusPagamentoAsync(veiculo.Placa, true, horaSaida);
          


           

        // Atualize a lista (se necessário, recarregue os dados)
        await DisplayAlert("Pagamento", "Status de pagamento atualizado!", "OK");
        // Opcional: Reaplique filtros se estiverem ativos
    }
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