using SQLite;
using parkSmart.Services;
using parkSmart.Models;
using AppListView.Services;

namespace parkSmart.Controllers
{
    public class VeiculosController
    {
        private DatabaseService databaseService;
        private SQLiteConnection connection;

        public VeiculosController()
        {
            databaseService = new DatabaseService();
            connection = databaseService.GetConexao();

            connection.CreateTable<Veiculos>();

        }


        public bool Insert(Veiculos value)
        {

            return connection.Insert(value) > 0;
        }

        public bool Update(Veiculos value)
        {
            //seguir a mesma ideia do Insert
            return connection.Update(value) > 0;
        }

        public bool Delete(Veiculos value)
        {
            return connection.Delete(value) > 0;
        }

        //Rotina de consulta

        //Consultar para retornoar todos os dados
        public List<Veiculos> GetAll()
        {

            return
                connection.Table<Veiculos>().ToList();
        }

        //Consultar por ID
        public Veiculos GetById(int value)
        {

            return
                connection.Find<Veiculos>(value);
        }

        //Consulta filtrando pelo Nome
        public List<Veiculos> GetByPlaca(string value)
        {

            return
                connection.Table<Veiculos>().
                Where(x => x.Placa==(value)).
                ToList();
        }



        public bool AtualizarStatusPagamentoAsync(string placa, bool pago, DateTime datasaida)
        {
            
            var veiculo = connection.Table<Veiculos>().FirstOrDefault(v => v.Placa == placa);
            if (veiculo != null)
            {
                veiculo.Pago = pago;
                veiculo.DataSaida = datasaida;
                return Update(veiculo);
            }
            return false;
        }
    }
}

