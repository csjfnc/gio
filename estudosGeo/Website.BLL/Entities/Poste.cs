using System;
using System.Collections.Generic;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class Poste
    {
        public Poste()
        {   
            //IP = new List<IP>();
            Transformadores = new List<Transformador>();
            Fotos = new List<FotoPoste>();
            PontoDeEntrega = new List<PontoEntrega>();
        }

        public int Ocupante_s { get; set; }
        public int Ocupante_d { get; set; }

        public long IdPoste { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExclusao { get; set; }
        public bool Finalizado { get; set; }
        public DateTime? DataFinalizado { get; set; }
        public long CodigoGeo { get; set; }
        public String Status { get; set; }
        public int NumeroPosteNaOS { get; set; }

        public AlturaPoste? Altura { get; set; }
        public TipoPoste TipoPoste { get; set; }
        public Esforco? Esforco { get; set; }
        public string Descricao { get; set; }

        public long? IdPoste_1 { get; set; }

        public double? XAtualizacao { get; set; }
        public double? YAtualizacao { get; set; }

        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }

        public long? IdLogradouro { get; set; }
        public virtual Logradouro Logradouro { get; set; }
        
        public long? IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }

       // public virtual ICollection<IP> IP { get; set; }
        public virtual ICollection<Transformador> Transformadores { get; set; }
        public virtual ICollection<FotoPoste> Fotos { get; set; }
        public virtual ICollection<PontoEntrega> PontoDeEntrega { get; set; }

        ///Campos do BD Geosurvey
        public string logradouro { get; set; }
        public string equipamento1 { get; set; }
        public string equipamento2 { get; set; }
        public string equipamento3 { get; set; }
        public string nomedobloco { get; set; }
        public int id_temp { get; set; }
        public int ativo { get; set; }
        public string primario { get; set; }
        public string proprietario { get; set; }
        public DateTime data;
        public int quantidade_poste { get; set; }
        public int idpostecia { get; set; }
        public string caracteristica_cia { get; set; }
        public string aterropararaio_cia { get; set; }
        public string encontrado { get; set; }
        public string tipo_base { get; set; }
        public string para_raio { get; set; }
        public int qtde_ramalservico { get; set; }
        public int qtde_ramalligacao { get; set; }
        public string estai { get; set; }
        public int qtd_estai { get; set; }
        public int qtde_ocp { get; set; }
        public int qtde_drop { get; set; }
        public string estai2 { get; set; }
        public int qtde_estai2 { get; set; }
        public int lampsemaforo { get; set; }
        public string tipo_zona { get; set; }
        public string material { get; set; }
        public string avaria { get; set; }
       
        public string barramento { get; set; }
        public string aterramento { get; set; }
        public string estrutura_primaria { get; set; }
        public string estrutura_secundaria { get; set; }
        public string situacao { get; set; }
        public string mufla { get; set; }
        public string rede_primaria { get; set; }
        public string defeito { get; set; }
        public string ano { get; set; }

    }
}