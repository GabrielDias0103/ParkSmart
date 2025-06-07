using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace parkSmart.Models
{
   public class Veiculos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string NomeProprietario { get; set; }
        public string TipoPlano { get; set; }
        public string FotoVeic { get; set; }
        public DatePicker DataEntrada { get; set; }
        public DatePicker DataSaida { get; set; }
        public bool Pago { get; set; }
    }
}
