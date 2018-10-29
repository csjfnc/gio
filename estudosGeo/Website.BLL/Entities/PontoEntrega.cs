using System;
using System.Collections.Generic;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class PontoEntrega
    {

        public PontoEntrega()
        {   
            //IP = new List<IP>();            
            Fotos = new List<FotoPontoEntrega>();            
        }
        public long IdPontoEntrega { get; set; }
        public long IdPoste { get; set; }
        public long IdLagradouro { get; set; }

        public long? IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }
        
        public long CodigoGeoBD { get; set; }
        public ClasseSocial ClasseSocial { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double X_atualizacao { get; set; }
        public double Y_atualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string Classificacao { get; set; }
        public string Complemento1 { get; set; }
        public string Complemento2 { get; set; }
        // public virtual OrdemDeServico OrdemDeServico { get; set; }
        public string TipoImovel { get; set; }
        public int NumeroAndaresEdificio { get; set; }
        public int TotalApartamentosEdificio { get; set; }
        public string NomeEdificio { get; set; }
        //public bool Update { get; set; }
       
        public virtual ICollection<FotoPontoEntrega> Fotos { get; set; }
        
        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }

        //public StatusEquipamento Status { get; set; }
        //public ClasseAtendimento ClasseAtendimento { get; set; }
        // public TipoConstrucao TipoConstrucao { get; set; }
        public string Numero { get; set; }

        public virtual Poste Poste { get; set; }
        public int? Ativo { get; set; }
        public int? AnoLevantamentoEdificio { get; set; }
        public double? Angulo { get; set; }
        public int? Divisao { get; set; }
        public string PosteBox { get; set; }
        public int? Qtdedem { get; set; }

        public string QtdDomicilio { get; set; }       
        public string TipoComercio { get; set; }
        public string QtdSalas { get; set; }
        public string Ocorrencia { get; set; }
        public string QtdBlocos { get; set; } 
    }
}