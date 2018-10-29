using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class ComercioRelatorio
    {

        public long ID { get; set; }
        public long? IdOrdemDeServico { get; set; }
        public long CodigoGeoBD { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Classificacao { get; set; }
        public string Complemento1 { get; set; }
        public string Complemento2 { get; set; }
        public string TipoImovel { get; set; }  
        public long IdCidade { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Colaborador { get; set; }
        public string OrdemServico { get; set; }
        public long? CodPoste { get; set; }
        public int? Ativo { get; set; }
        public double? Angulo { get; set; }
        public int? Divisao { get; set; }
        public int? Qtdedem { get; set; }
        public string PosteBox { get; set; }
    }
}
