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


    //metodo para limpar os campos do formulário caso o usuario sair da tela de cadastro
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Limpa todos os campos do formulário
        txtPlaca.Text = "";
        txtMarca.Text = "";
        txtModelo.Text = "";
        txtCor.Text = "";
        txtNomeProprietario.Text = "";
        checkPago.IsChecked = false;
        TipoPicker.SelectedIndex = -1;
        LimparImagem();

        // Atualiza a data/hora de entrada
        dataEntrada = DateTime.Now;
        lblDataHoraEntrada.Text = dataEntrada.ToString("dd/MM/yyyy HH:mm");
        sImagemSelecionada = null;
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

        // Antes de criar o objeto Veiculos
        // Validação para verificar se já existe um veículo com a mesma placa
        var veiculosExistentes = veiculosController.GetByPlaca(placa);
        if (veiculosExistentes != null && veiculosExistentes.Count > 0)
        {
            DisplayAlert("Atenção", "Já existe um veículo cadastrado com esta placa.", "OK");
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

    private async void txtPlaca_Unfocused(object sender, FocusEventArgs e)
    {
        string placa = txtPlaca.Text?.Trim();

        if (!string.IsNullOrEmpty(placa))
        {
            // Consulta por placa
            var veiculos = veiculosController.GetByPlaca(placa);

            if (veiculos != null && veiculos.Count > 0)
            {
                var veiculo = veiculos.First();

                txtMarca.Text = veiculo.Marca;
                txtModelo.Text = veiculo.Modelo;
                txtCor.Text = veiculo.Cor;
                txtNomeProprietario.Text = veiculo.NomeProprietario;
                TipoPicker.SelectedItem = veiculo.TipoPlano;
                checkPago.IsChecked = veiculo.Pago;
                FotoImage.Source = veiculo.FotoVeic;
                sImagemSelecionada = veiculo.FotoVeic;
                btnRemover.IsVisible = !string.IsNullOrEmpty(veiculo.FotoVeic);

                await DisplayAlert("Atenção", "Placa já cadastrada! Dados carregados.", "OK");
            }
        }
    }
}