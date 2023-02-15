using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receive.Domain.DTOs
{
    internal class VeiculosDTO
    {
        public string Carro { get; set; }
        public string Cor { get; set; }
        public string AnoVeiculo { get; set; }
        public string NomeProprietario { get; set; }
        public string CpfProprietario { get; set; }
    }
}
