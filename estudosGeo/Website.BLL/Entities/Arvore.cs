using System;
using System.Collections.Generic;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class Arvore
    {
        public Arvore() 
        {
            Fotos = new List<FotoArvore>();
        }

        public long IdArvore { get; set; }
        public string Logradouro { get; set; }
        public PorteArvore Porte { get; set; }
        public string Localizacao { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool Ativo { get; set; }
        public string NomeBloco { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Angulo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExclusao { get; set; }

        public long? IdPoste { get; set; }
        public virtual Poste Poste { get; set; }

        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }

        public long? IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }

        public virtual List<FotoArvore> Fotos { get; set; }
    }
}
