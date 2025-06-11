namespace parkSmart.Views;
using parkSmart.Controllers;
using parkSmart.Models;
using parkSmart.Services;

public partial class Pgcadastro : ContentPage
{
    private VeiculosController veiculosController;
    private DateTime dataEntrada;

    public Pgcadastro()
	{
		InitializeComponent();

        veiculosController = new VeiculosController();
        dataEntrada = DateTime.Now;

        // Define a data/hora atual na label ao abrir a tela
        lblDataHoraEntrada.Text = dataEntrada.ToString("dd/MM/yyyy HH:mm");
    }


    private string sImagemSelecionada;
    private async void btnSelecionarFoto_Clicked(object sender, EventArgs e)
    {
        sImagemSelecionada =
           await ImageService.SelecionarImagem();
        //Apresentar a imagem ao usuario
        FotoImage.Source = sImagemSelecionada;
        //Exibir o botão remover
        btnRemover.IsVisible = true;
    }

    private void btnSalvar_Clicked(object sender, EventArgs e)
    {
        string placa = txtPlaca.Text;
        string marca = txtMarca.Text;
        string modelo = txtModelo.Text;
        string cor = txtCor.Text;
        string nomeProprietario = txtNomeProprietario.Text;
        string tipoPlano = TipoPicker.SelectedItem?.ToString();
        string fotoVeic = sImagemSelecionada?.ToString();
        

        //Validar os registro
        if (string.IsNullOrEmpty(placa) ||
            string.IsNullOrEmpty(marca) ||
                string.IsNullOrEmpty(cor) ||
                string.IsNullOrEmpty(tipoPlano) ||
                string.IsNullOrEmpty(modelo) ||
                string.IsNullOrEmpty(fotoVeic))
                
        {
            //Se um dos dois estiver vazio
            //ja abortamos a rotina
            DisplayAlert(
                "Atençao",
                "Preencha os campos corretamente.",
                "OK");
            //Abortar a rotina
            return;
        }

        //Iremos criar e popular o objeto pessoa
        Veiculos veiculo = new Veiculos();
        veiculo.Placa = placa;
        veiculo.Marca = marca;
        veiculo.Modelo = modelo;
        veiculo.Cor = cor;
        veiculo.NomeProprietario = nomeProprietario;
        veiculo.TipoPlano = tipoPlano;
        veiculo.FotoVeic = ImageService.CopiarImagem(sImagemSelecionada);
        veiculo.Pago = checkPago.IsChecked;
        veiculo.DataEntrada = dataEntrada;

        //Chamara a rotina para copiar a imagem
        //e iremos gravar no banco
        //o diretorio da nova imagem(copia)
        veiculo.FotoVeic =
            ImageService.CopiarImagem(sImagemSelecionada);

        //Realizar a inserção no banco de dados
        if (veiculosController.Insert(veiculo))
        {
            //Notificamos que deu tudo certo
            DisplayAlert(
                "Informação",
                "Registro salvo com sucesso.",
                "OK");
            txtPlaca.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtCor.Text = "";
            txtNomeProprietario.Text = "";
            checkPago.IsChecked = false;
            TipoPicker.SelectedIndex = -1;
            LimparImagem();
        }
        else
        {
            DisplayAlert(
                "Erro",
                "Falha ao salvar o registro no " +
                "banco de dados",
                "OK");
        }
    }

    private void LimparImagem()
    {
        //Remover a imagem da tela
        FotoImage.Source = "";
        //Ocultar o botão Remover
        btnRemover.IsVisible = false;
    }

    private void btnRemover_Clicked(object sender, EventArgs e)
    {
        LimparImagem();
    }
}