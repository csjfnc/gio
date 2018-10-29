using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class AnotacaoRelatorio
    {
        public long IdAnotacao { get; set; }
        public string Descricao { get; set; }
        public long IdOrdemServico { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime? DataExclusao { get; set; }
        public bool Excluido { get; set; }
        public bool Update { get; set; }
        public long CodigoBDGeo { get; set; }
        public string Cidade { get; set; }
        public string OrdemServico { get; set; }
        public string Colaborador { get; set; }
        public double Angulo { get; set; }
        public int Ativo { get; set; }
    }
}
