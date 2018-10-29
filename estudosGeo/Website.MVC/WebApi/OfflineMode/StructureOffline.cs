using System;
using System.Collections.Generic;
using Website.BLL.Entities;
using Website.BLL.Enums;
using Website.BLL.Utils.Geocoding;
using Website.MVC.WebApi.Models;

namespace Website.MVC.WebApi.OfflineMode
{
    public class StructureOffline
    {
        public OSOffline OrdemDeServico { get; set; }
        public List<PosteOffline> Postes { get; set; }
       // public List<IpOffline> IPS { get; set; }
        public List<VaoPrimarioOffline> VaosPrimarios { get; set; }
        public List<PontoEntregaOffline> PontoEntrega { get; set; }
        public List<VaosDemandaPosteOffiline> VaosPontoPoste { get; set; }
        public List<AnotacaoOffline> Anotacao { get; set; }
        public List<DemandaStrandOffline> Strand { get; set; }
        public List<QuadraOffline> Quadra { get; set; }
        //public List<MedidorOffline> Medidor { get; set; }
        public bool FinalizadoColaborador { get; set; }
    }

    public class OSOffline
    {
        public long IdOrdemDeServico { get; set; }
        public string NumeroOS { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string Usuario { get; set; }
        public int NumeroDePostes { get; set; }
        public List<LatLon> PoligonosOS { get; set; }
        public StatusOs Status { get; set; }
        public bool Update { get; set; }
       
    }

    public class PosteOffline
    {
        public long IdPoste { get; set; }
        public LatLon Posicao { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataExclusao { get; set; }
        public bool Finalizado { get; set; }
        public long CodigoGeo { get; set; }
        public long? IdLogradouro { get; set; }
        public long IdCidade { get; set; }
        public long? IdOrdemDeServico { get; set; }
        public List<FotoAPI> Fotos { get; set; }
        public List<PontoEntregaAPI> PontoEntregaAPI { get; set; }
        public int NumeroPosteNaOS { get; set; }
        public AlturaPoste? Altura { get; set; }
        public TipoPoste TipoPoste { get; set; }
        public Esforco? Esforco { get; set; }
        public string Descricao { get; set; }
        public  int Ocupante_s { get; set; }
        public  int Ocupante_d { get; set; }
        public bool Excluido { get; set; }
        public PontoAtualizacao PontoAtualizacao { get; set; }
        public bool Update { get; set; }

        public string ParaRario { get; set; }
        public string Equipamento { get; set; }
        public string Encontrado { get; set; }
        public string Barramento { get; set; }
        public string Aterramento { get; set; }
        public string EstruturaPrimaria { get; set; }
        public string EstruturaSecundaria { get; set; }
        public string Situacao { get; set; }
        public string Mufla { get; set; }
        public string RedePrimaria { get; set; }
        public string Defeito { get; set; }
        public string Ano { get; set; }
        public string QtdEstai { get; set; }
    }

  /*  public class IpOffline
    {
        public long IdIp { get; set; }
        public long IdPoste { get; set; }
        public string TipoBraco { get; set; }
        public string TipoLuminaria { get; set; }
        public int? QtdLuminaria { get; set; }
        public string TipoLampada { get; set; }
        public double? Potencia { get; set; }
        public string Fase { get; set; }
        public string Acionamento { get; set; }
        public string LampadaAcesa { get; set; }
        public int QtdLampada { get; set; }
        public bool Excluido { get; set; }
        public PontoAtualizacao PontoAtualizacao { get; set; }
        public bool Update { get; set; }
    }*/

    public class PontoEntregaOffline
    {
        public long IdPontoEntrega { get; set; }
        public long IdPoste { get; set; }
        public long CodigoGeoBD { get; set; }
        public long? IdOrdemDeServico { get; set; }
        public long IdLagradouro { get; set; }
        public LatLon Posicao { get; set; }
        public string NumeroLocal { get; set; }
        public ClasseSocial ClasseSocial { get; set; }
        public string Logradouro { get; set; }
        public string Classificacao { get; set; }
        public string Complemento1 { get; set; }
        public string Complemento2 { get; set; }
        public string TipoImovel { get; set; }
        public int NumeroAndaresEdificio { get; set; }
        public int NumeroTotalApartamentos { get; set; }
        public string NomeEdificio { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double X_atualizacao { get; set; }
        public double Y_atualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public bool Update { get; set; }
        public bool Excluido { get; set; }
        public List<FotoAPI> Fotos { get; set; }
        public long IdCidade { get; set; }
        public int Ativo { get; set; }
        public int AnoLevantamentoEdificio { get; set; }
        public double Angulo { get; set; }
        public string PosteBox { get; set; }

        public string QtdDomicilio { get; set; }
        public string TipoComercio { get; set; }
        public string QtdSalas { get; set; }
        public string Ocorrencia { get; set; }
        public string QtdBlocos { get; set; }
        
    }

   /* public class MedidorOffline
    {
        public long IdMedidor { get; set; }
        public long IdPontoEntrega { get; set; }
        public string NumeroMedidor { get; set; }
        public string ComplementoResidencial { get; set; }
        public DateTime? DataExclusao { get; set; }
        public bool Update { get; set; }
        public bool Excluido { get; set; }
    }*/

    public class PontoAtualizacao
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long DataAtualização { get; set; }
    }

    public class VaoPrimarioOffline
    {
        //public long IdVaoPrimario { get; set; }
        //public long IdOrdemServico { get; set; }
        public double x1 { get; set; }
        public double x2 { get; set; }
        public double y1 { get; set; }
        public double y2 { get; set; }
    }

    public class VaosDemandaPosteOffiline
    {
        public long Id { get; set; }
        public long IdPontoEntrega { get; set; }
        public long IdPoste { get; set; }
        public long? IdOrdemDeServico { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public DateTime? DataExclusao { get; set; }
        public bool Excluido { get; set; }        
    }

    public class AnotacaoOffline
    {
        public long IdAnotacao { get; set; }
        public string Descricao { get; set; }
        public long? IdOrdemServico { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public bool Excluido { get; set; }
        public bool Update { get; set; }
        public double Angulo { get; set; }
        public int Ativo { get; set; }
    }

    public class DemandaStrandOffline
    {
        public long ID { get; set; }
        public long? OrderID { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public bool Update { get; set; }
        public bool Excluido { get; set; }
        public long CodigoBdGeo { get; set; }
        public int Ativo { get; set; }
        public long PosteId1 { get; set; }
        public long PosteId2 { get; set; }
    }

    public class QuadraOffline
    {
        public long ID { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }

    public class LatLonQuadrasOffline
    {
        public long ID { get; set; }
        public long QuadraID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}