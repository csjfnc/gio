using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Website.BLL.Entities;
using Website.BLL.Enums;
using Website.BLL.Utils.Geocoding;
using Website.BLL.Utils.Security;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.Config;
using Website.MVC.Util;
using Website.MVC.WebApi.Bussiness;
using Website.MVC.WebApi.Models;
using Website.MVC.WebApi.OfflineMode;
using Website.MVC.WebApi.Security;

namespace Website.MVC.WebApi
{
    public class OfflineModeController : ApiController
    {
        private string IdUsuario = string.Empty;
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        protected override void Initialize(HttpControllerContext context)
        {
            if (context.Request.Headers.Contains(StringContantsWebsite.TOKEN))
            {
                IdUsuario = AESCrypt.Decrypt(context.Request.Headers.GetValues(StringContantsWebsite.TOKEN).First()).Split(':')[0];
            }

            base.Initialize(context);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UnitOfWork != null)
                {
                    UnitOfWork.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [LogExceptionMobile]
        public HttpResponseMessage GetStructureOs(long IdOrdemDeServico)
        {
            /// Consulta no Banco de dados
            OrdemDeServico OS = UnitOfWork.OrdemDeServicoRepository.Get(os => os.IdOrdemDeServico == IdOrdemDeServico).FirstOrDefault();
            Usuario User = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == OS.IdUsuario).FirstOrDefault();
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == OS.IdCidade).FirstOrDefault();
            List<PoligonoOS> poligonos = UnitOfWork.PoligonoOSRepository.Get(p => p.IdOrdemDeServico == OS.IdOrdemDeServico).ToList();

            OS.Cidade = cidade;
            OS.Usuario = User;
            OS.PoligonosOS = poligonos;

            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == IdOrdemDeServico && p.DataExclusao == null, includeProperties: "Fotos");
            IEnumerable<PontoEntrega> PontoEntregas = UnitOfWork.PontoEntregaRepository.Get(ponto => ponto.IdOrdemDeServico == IdOrdemDeServico && ponto.DataExclusao == null, includeProperties: "Fotos");
            IEnumerable<VaoPrimario> lstVaoPrimario = UnitOfWork.VaoPrimarioRepository.Get(vp => vp.IdOrdemDeServico == IdOrdemDeServico);
            IEnumerable<VaosDemandaPoste> VaosDemandaPostes = UnitOfWork.VaosDemandaPosteRepository.Get(vao => vao.IdOrdemDeServico == IdOrdemDeServico);
            IEnumerable<Anotacao> Anotacaos = UnitOfWork.AnotacaoRepository.Get(vao => vao.IdOrdemDeServico == IdOrdemDeServico);
            IEnumerable<DemandaStrand> DemandaStrands = UnitOfWork.DemandaStrandRepository.Get(dam => dam.IdOrdemDeServico == IdOrdemDeServico);
            IEnumerable<Quadra> Quadras = UnitOfWork.QuadrasRepository.Get(qd => qd.IdOrdemDeServico == IdOrdemDeServico);

            if (OS == null)
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.OS_Not_Found });

            /// Set o Status Da Os
            OS.Situacao = SituacaoOrdemServico.EM_CAMPO;
            /// Atualizando a OS
            UnitOfWork.OrdemDeServicoRepository.Update(OS);
            /// Commit da transação
            UnitOfWork.Save();

            /// Carregando a lista de Postes e IP's e Ponto de Entrega
            //List<IpOffline> IpAux = new List<IpOffline>();
            List<PosteOffline> PostesAux = new List<PosteOffline>();
            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(OS.Cidade.Datum, OS.Cidade.NorteOuSul, OS.Cidade.Zona);
            List<VaoPrimarioOffline> lstVPoffline = new List<VaoPrimarioOffline>();
            List<PontoEntregaOffline> PontoEntregaAux = new List<PontoEntregaOffline>();
            List<VaosDemandaPosteOffiline> VaosDemandaPosteOffilineAux = new List<VaosDemandaPosteOffiline>();
            List<AnotacaoOffline> AnotacaoOfflineAux = new List<AnotacaoOffline>();
            List<DemandaStrandOffline> DemandaStrandOfflineAux = new List<DemandaStrandOffline>();
            List<QuadraOffline> QuadraOfflineAux = new List<QuadraOffline>();
            //List<MedidorOffline> MedidorAux = null;

            foreach (Poste p in Postes)
            {
                /// Fotos dentro do Poste
                List<FotoAPI> FotosApi = new List<FotoAPI>();

                foreach (FotoPoste foto in p.Fotos.Where(f => f.DataExclusao == null).OrderBy(f => f.NumeroFoto))
                {
                    FotoAPI ft = new FotoAPI();
                    ft.NumeroFoto = foto.NumeroFoto;
                    ft.DataFoto = foto.DataFoto.ToString("yyyy-MM-dd HH:mm:sss"); //ConvertDate.DateTimeToUnixTimestamp(foto.DataFoto);
                    FotosApi.Add(ft);
                }
                /*              
                  ////Ponto de Entrega dentro do Poste
                  List<PontoEntregaAPI> PontoEntregaAPI = new List<Models.PontoEntregaAPI>();

                  foreach (PontoEntrega itemPE in p.PontoDeEntrega.Where(ponto_entrega => ponto_entrega.DataExclusao == null))
                  {
                      LatLon objLL = converter.Convert(itemPE.X, itemPE.Y);
                      double lat = objLL.Lat;
                      double lon = objLL.Lon;

                      PontoEntregaAPI peAPI = new Models.PontoEntregaAPI();
                      peAPI.x_pe = lat;
                      peAPI.y_pe = lon;
                      PontoEntregaAPI.Add(peAPI);
                  }*/

                PostesAux.Add(new PosteOffline()
                {
                    IdPoste = p.IdPoste,
                    Posicao = converter.Convert(p.X, p.Y),
                    DataCadastro = p.DataCadastro,
                    DataExclusao = p.DataExclusao,
                    Finalizado = p.Finalizado,
                    CodigoGeo = p.CodigoGeo,
                    IdLogradouro = p.IdLogradouro,
                    IdCidade = OS.IdCidade,
                    IdOrdemDeServico = p.IdOrdemDeServico,
                    Fotos = FotosApi,
                    Altura = p.Altura,
                    TipoPoste = p.TipoPoste,
                    Esforco = p.Esforco,
                    Descricao = p.Descricao,
                    NumeroPosteNaOS = p.NumeroPosteNaOS,
                    Excluido = false,
                    Update = false,
                    Ocupante_d = p.Ocupante_d,
                    Ocupante_s = p.Ocupante_s,
                    ParaRario = p.para_raio,
                    Equipamento = p.equipamento1,
                    Encontrado = p.encontrado,
                    Barramento = p.barramento,
                    Aterramento = p.aterramento,
                    EstruturaPrimaria = p.estrutura_primaria,
                    EstruturaSecundaria = p.estrutura_secundaria,
                    Situacao = p.situacao,
                    Mufla = p.mufla,
                    RedePrimaria = p.rede_primaria,
                    Defeito = p.defeito,
                    Ano = p.ano,
                    QtdEstai = p.qtd_estai.ToString()

                    //  PontoEntregaAPI = PontoEntregaAPI
                });

                /*  foreach (IP ip in p.IP.Where(i => i.DataExclusao == null))
                  {
                      IpAux.Add(new IpOffline()
                      {
                          IdIp = ip.IdIp,
                          IdPoste = ip.IdPoste,
                          TipoBraco = ip.TipoBraco,
                          TipoLuminaria = ip.TipoLuminaria,
                          QtdLuminaria = ip.QtdLuminaria,
                          TipoLampada = ip.TipoLampada,
                          Potencia = ip.Potencia,
                          Fase = ip.Fase,
                          Acionamento = ip.Acionamento,
                          LampadaAcesa = ip.LampadaAcesa,
                          QtdLampada = ip.QtdLampada,
                          Excluido = false,
                          Update = false
                      });
                  }*/

                //Ponto de Entrega
                /*
                foreach (PontoEntrega pe in p.PontoDeEntrega.Where(ponto => ponto.DataExclusao == null))
                {
                    //Medidores
                    List<Medidor> lstMedidor = UnitOfWork.MedidoresRepository.Get(m => m.IdPontoEntrega == pe.IdPontoEntrega && m.DataExclusao == null).ToList();

                    /*MedidorAux = new List<MedidorOffline>();

                    foreach (Medidor m in lstMedidor)
                    {
                        if (m != null)
                        {
                            MedidorAux.Add(new MedidorOffline()
                                {
                                    IdMedidor = m.IdMedidor,
                                    IdPontoEntrega = m.IdPontoEntrega,
                                    NumeroMedidor = m.NumeroMedidor,
                                    ComplementoResidencial = m.ComplementoResidencial,
                                    DataExclusao = m.DataExclusao,
                                    Update = false,
                                    Excluido = false
                                });
                        }
                    }*/

                /*   //Fotos Ponto de entrega
                   List<FotoPontoEntrega> lstFotosPE = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdPontoEntrega == pe.IdPontoEntrega && f.DataExclusao == null).ToList();

                   List<FotoAPI> fotosPE = new List<FotoAPI>();

                   foreach (FotoPontoEntrega fotoPE in lstFotosPE)
                   {
                       fotosPE.Add(new FotoAPI() { 
                           DataFoto = fotoPE.DataFoto.ToString("yyyy-MM-dd HH:mm:sss"),
                           NumeroFoto = fotoPE.NumeroFoto 
                       });
                   }

                   LatLon objLL = converter.Convert(pe.X, pe.Y);
                   double lat = objLL.Lat;
                   double lon = objLL.Lon;

                   PontoEntregaAux.Add(new PontoEntregaOffline()
                       {
                           IdPontoEntrega = pe.IdPontoEntrega,
                           IdPoste = pe.IdPoste,
                           CodigoGeoBD = pe.CodigoGeoBD,
                          // Status = pe.Status,
                           //ClasseAtendimento = pe.ClasseAtendimento,
                           //TipoConstrucao = pe.TipoConstrucao,
                           NumeroLocal = pe.Numero,
                           ClasseSocial = pe.ClasseSocial,
                           //Logradouro = pe.Logradouro,
                           //Fase = pe.Fase,
                           //EtLigacao = pe.EtLigacao,
                           //Observacao = pe.Observacao,
                           X = lat,
                           Y = lon,
                           DataExclusao = pe.DataExclusao,
                           //Medidor = MedidorAux,
                           Update = false,
                           Excluido = false,
                           Fotos = fotosPE,
                           //todo
                           IdOrdemServico = pe.IdOrdemServico,
                           Classificacao = pe.Classificacao,
                           Complemento1 = pe.Complemento1,
                           Complemento2 = pe.Complemento2,
                           TipoImovel = pe.TipoImovel,
                           NumeroAndaresEdificio = pe.NumeroAndaresEdificio,
                           NumeroTotalApartamentos = pe.TotalApartamentosEdificio,
                           NomeEdificio = pe.NomeEdificio

                       });
               }*/
            }

            foreach (PontoEntrega pe in PontoEntregas.Where(ponto => ponto.DataExclusao == null))
            {
                //Medidores
                //List<Medidor> lstMedidor = UnitOfWork.MedidoresRepository.Get(m => m.IdPontoEntrega == pe.IdPontoEntrega && m.DataExclusao == null).ToList();               

                //Fotos Ponto de entrega
                /* List<FotoPontoEntrega> lstFotosPE = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdPontoEntrega == pe.IdPontoEntrega && f.DataExclusao == null).ToList();

                 List<FotoAPI> fotosPE = new List<FotoAPI>();

                 foreach (FotoPontoEntrega fotoPE in lstFotosPE)
                 {
                     fotosPE.Add(new FotoAPI() { 
                         DataFoto = fotoPE.DataFoto.ToString("yyyy-MM-dd HH:mm:sss"),
                         NumeroFoto = fotoPE.NumeroFoto 
                     });
                 }*/

                /// Fotos dentro do Poste
                List<FotoAPI> FotosApi = new List<FotoAPI>();

                foreach (FotoPontoEntrega foto in pe.Fotos.Where(f => f.DataExclusao == null).OrderBy(f => f.NumeroFoto))
                {
                    FotoAPI ft = new FotoAPI();
                    ft.NumeroFoto = foto.NumeroFoto;
                    ft.DataFoto = foto.DataFoto.ToString("yyyy-MM-dd HH:mm:sss"); //ConvertDate.DateTimeToUnixTimestamp(foto.DataFoto);
                    FotosApi.Add(ft);
                }

                LatLon objLL = converter.Convert(pe.X, pe.Y);
                LatLon objPontoAtualiazacao = converter.Convert(pe.X_atualizacao, pe.Y_atualizacao);
                double lat = objLL.Lat;
                double lon = objLL.Lon;
                double lat_atualizacao = objPontoAtualiazacao.Lat;
                double lon_atualizacao = objPontoAtualiazacao.Lon;

                PontoEntregaAux.Add(new PontoEntregaOffline()
                {
                    IdPontoEntrega = pe.IdPontoEntrega,
                    IdPoste = pe.IdPoste,
                    CodigoGeoBD = pe.CodigoGeoBD,
                    // Status = pe.Status,
                    //ClasseAtendimento = pe.ClasseAtendimento,
                    //TipoConstrucao = pe.TipoConstrucao,
                    NumeroLocal = pe.Numero,
                    ClasseSocial = pe.ClasseSocial,
                    //Logradouro = pe.Logradouro,
                    //Fase = pe.Fase,
                    //EtLigacao = pe.EtLigacao,
                    //Observacao = pe.Observacao,
                    X = lat,
                    Y = lon,
                    X_atualizacao = lat_atualizacao,
                    Y_atualizacao = lon_atualizacao,
                    DataExclusao = pe.DataExclusao,
                    //Medidor = MedidorAux,
                    Update = false,
                    Excluido = false,
                    Fotos = FotosApi,
                    //todo
                    IdOrdemDeServico = pe.IdOrdemDeServico,
                    Classificacao = pe.Classificacao,
                    Complemento1 = pe.Complemento1,
                    Complemento2 = pe.Complemento2,
                    TipoImovel = pe.TipoImovel,
                    NumeroAndaresEdificio = pe.NumeroAndaresEdificio,
                    NumeroTotalApartamentos = pe.TotalApartamentosEdificio,
                    NomeEdificio = pe.NomeEdificio,
                    TipoComercio = pe.TipoComercio,
                    QtdSalas = pe.QtdSalas,
                    QtdDomicilio = pe.QtdDomicilio,
                    QtdBlocos = pe.QtdBlocos,
                    Ocorrencia = pe.Ocorrencia
                });
            }

            /// Pontos do poligono
            List<LatLon> PoligonosOs = OS.PoligonosOS.OrderBy(o => o.Ordem).Select(os => converter.Convert(os.X1, os.Y1)).ToList();

            //Vão Primário
            foreach (VaoPrimario itemVP in lstVaoPrimario)
            {
                LatLon objLL1 = converter.Convert(itemVP.X1, itemVP.Y1);
                LatLon objLL2 = converter.Convert(itemVP.X2, itemVP.Y2);
                double lat1 = objLL1.Lat;
                double lon1 = objLL1.Lon;
                double lat2 = objLL2.Lat;
                double lon2 = objLL2.Lon;

                lstVPoffline.Add(new VaoPrimarioOffline()
                {
                    x1 = lat1,
                    x2 = lat2,
                    y1 = lon1,
                    y2 = lon2
                });
            }

            //Fotos Ponto Entrega


            foreach (VaosDemandaPoste vaosDemandaPoste in VaosDemandaPostes.Where(v => v.DataExclusao == null))
            {
                LatLon objLL1 = converter.Convert(vaosDemandaPoste.X1, vaosDemandaPoste.Y1);
                LatLon objLL2 = converter.Convert(vaosDemandaPoste.X2, vaosDemandaPoste.Y2);
                double lat1 = objLL1.Lat;
                double lon1 = objLL1.Lon;
                double lat2 = objLL2.Lat;
                double lon2 = objLL2.Lon;

                VaosDemandaPosteOffilineAux.Add(new VaosDemandaPosteOffiline()
                {
                    X1 = lat1,
                    X2 = lat2,
                    Y1 = lon1,
                    Y2 = lon2,
                    Id = vaosDemandaPoste.IdVaosDemandaPoste,
                    IdPontoEntrega = vaosDemandaPoste.IdPontoEntrega,
                    IdOrdemDeServico = vaosDemandaPoste.IdOrdemDeServico

                });
            }

            foreach (Anotacao anota in Anotacaos.Where(ano => ano.DataExclusao == null))
            {
                LatLon latlonAnotacao = converter.Convert(anota.X, anota.Y);
                double lat = latlonAnotacao.Lat;
                double lon = latlonAnotacao.Lon;

                AnotacaoOfflineAux.Add(new AnotacaoOffline()
                {
                    Descricao = anota.Descricao,
                    IdAnotacao = anota.IdAnotacao,
                    IdOrdemServico = anota.IdOrdemDeServico,
                    X = lat,
                    Y = lon
                });
            }

            foreach (DemandaStrand demandaStrand in DemandaStrands.Where(s => s.DataExclusao == null))
            {
                LatLon latlon1 = converter.Convert(demandaStrand.X1, demandaStrand.Y1);
                LatLon latlon2 = converter.Convert(demandaStrand.X2, demandaStrand.Y2);
                double x1 = latlon1.Lat;
                double x2 = latlon2.Lat;
                double y1 = latlon1.Lon;
                double y2 = latlon2.Lon;

                DemandaStrandOfflineAux.Add(new DemandaStrandOffline()
                {
                    CodigoBdGeo = demandaStrand.CodigoBdGeo,
                    ID = demandaStrand.ID,
                    OrderID = demandaStrand.IdOrdemDeServico,
                    X1 = x1,
                    X2 = x2,
                    Y1 = y1,
                    Y2 = y2
                });

            }

            foreach (Quadra quadraDB in Quadras)
            {

                LatLon latlon1 = converter.Convert(quadraDB.X1, quadraDB.Y1);
                LatLon latlon2 = converter.Convert(quadraDB.X2, quadraDB.Y2);

                QuadraOfflineAux.Add(new QuadraOffline
                {
                    ID = quadraDB.ID,
                    X1 = latlon1.Lat,
                    Y1 = latlon1.Lon,
                    X2 = latlon2.Lat,
                    Y2 = latlon2.Lon

                });
            }

            /// Criando o Objeto para ser serializado
            StructureOffline retorno = new StructureOffline()
            {
                OrdemDeServico = new OSOffline()
                {
                    IdOrdemDeServico = OS.IdOrdemDeServico,
                    NumeroOS = OS.NumeroOS,
                    DataInicio = OS.DataInicio,
                    DataFinal = OS.DataFinal,
                    Usuario = OS.Usuario.UserName,
                    NumeroDePostes = OS.Postes.Where(p => p.DataExclusao == null).Count(),
                    PoligonosOS = PoligonosOs,
                    Status = CalculaStatusOs.Calcular(OS),
                    Update = false
                },
                Postes = PostesAux,
                //IPS = IpAux,
                VaosPrimarios = lstVPoffline,
                PontoEntrega = PontoEntregaAux,
                VaosPontoPoste = VaosDemandaPosteOffilineAux,
                Anotacao = AnotacaoOfflineAux,
                Strand = DemandaStrandOfflineAux,
                Quadra = QuadraOfflineAux
                //Medidor = MedidorAux
            };

            /// Criando o Stream para o arquivo de Download
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(retorno));

            /// Criando o cabeçalho Http para o Cliente Mobile
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(bytes));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = " OfflineModeStructureOs" + DateTime.Now.Ticks + ".json";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return response;
        }

        [HttpPost]
        [MimeMultipart]
        [LogExceptionMobile]
        public async Task<HttpResponseMessage> SaveStructureOs()
        {
            StructureOffline jsonOffline = null;
            MultipartMemoryStreamProvider streamProvider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith((tsk) =>
            {
                HttpContent ctnt = streamProvider.Contents.FirstOrDefault();
                using (StreamReader reader = new StreamReader(ctnt.ReadAsStreamAsync().Result, Encoding.UTF8))
                {
                    try
                    {
                        jsonOffline = JsonConvert.DeserializeObject<StructureOffline>(reader.ReadToEnd());
                    }
                    catch
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Resources.Messages.Invalid_File_Upload));
                    }

                    // Buscando o Usuario no Banco de dados
                    //UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == IdUsuario).FirstOrDefault();
                    OrdemDeServico OS = UnitOfWork.OrdemDeServicoRepository.Get(os => /*os.IdUsuario == IdUsuario &&*/ os.IdOrdemDeServico == jsonOffline.OrdemDeServico.IdOrdemDeServico, includeProperties: "Cidade").FirstOrDefault();
                    ConverterLatLonToUtm converter = new ConverterLatLonToUtm(OS.Cidade.Datum, OS.Cidade.NorteOuSul, OS.Cidade.Zona);
                    Usuario User = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == OS.IdUsuario).FirstOrDefault();
                    bool finalizadoColaborador = jsonOffline.FinalizadoColaborador;

                    #region etapa 0 - Inserindo Publicação
                    OSOffline itemOs = jsonOffline.OrdemDeServico;

                    //DateTime dataPublic = DateTime.Now;
                    string dataformat = string.Format("{0:dd/MM/yyyy}", DateTime.Now);

                    PublicacaoOrdemColaborador publicadoAux = new PublicacaoOrdemColaborador()
                    {
                        IdOrdemDeServico = itemOs.IdOrdemDeServico,
                        Usuario = itemOs.Usuario,
                        Data_publicado = dataformat,
                        NumeroOs = itemOs.NumeroOS
                    };

                    UnitOfWork.PublicacaoOrdemColaboradorRepository.Insert(publicadoAux);
                    UnitOfWork.Save();

                    #endregion etapa 0


                    #region Etapa 1 - Inserindo novos Postes
                    foreach (PosteOffline postedoJson in jsonOffline.Postes.Where(poste => poste.IdPoste < 0 && poste.Update)) // Verificando se existe postes novos
                    {
                        UTM utmPoste = converter.Convert(postedoJson.Posicao.Lat, postedoJson.Posicao.Lon);
                        UTM utmAtualizacao = new UTM() { X = 0, Y = 0 };
                        if (postedoJson.PontoAtualizacao != null)
                            utmAtualizacao = converter.Convert(postedoJson.PontoAtualizacao.Latitude, postedoJson.PontoAtualizacao.Longitude);

                        var pontos = jsonOffline.PontoEntrega.Where(p => p.IdPoste == postedoJson.IdPoste).ToList();
                        var listaDeVaosPoste = jsonOffline.VaosPontoPoste.Where(v => v.IdPoste == postedoJson.IdPoste).ToList();


                        Poste posteAux = new Poste()
                        {
                            X = utmPoste.X,
                            Y = utmPoste.Y,
                            OrdemDeServico = OS,
                            DataCadastro = postedoJson.DataCadastro != null ? Convert.ToDateTime(postedoJson.DataCadastro) : DateTime.Now,
                            Cidade = OS.Cidade,
                            Altura = postedoJson.Altura,
                            TipoPoste = postedoJson.TipoPoste,
                            Esforco = postedoJson.Esforco,
                            Descricao = postedoJson.Descricao,
                            Finalizado = postedoJson.Finalizado,
                            NumeroPosteNaOS = postedoJson.NumeroPosteNaOS,
                            XAtualizacao = utmAtualizacao.X,
                            YAtualizacao = utmAtualizacao.Y,
                            Ocupante_s = postedoJson.Ocupante_s,
                            Ocupante_d = postedoJson.Ocupante_d,
                            para_raio = postedoJson.ParaRario != null ? postedoJson.ParaRario : "",
                            equipamento1 = postedoJson.Equipamento != null ? postedoJson.Equipamento : "",
                            encontrado = postedoJson.Encontrado != null ? postedoJson.Encontrado : "",
                            barramento = postedoJson.Barramento != null ? postedoJson.Barramento : "",
                            aterramento = postedoJson.Aterramento != null ? postedoJson.Aterramento : "",
                            estrutura_primaria = postedoJson.EstruturaPrimaria != null ? postedoJson.EstruturaPrimaria : "",
                            estrutura_secundaria = postedoJson.EstruturaSecundaria != null ? postedoJson.EstruturaSecundaria : "",
                            situacao = postedoJson.Situacao != null ? postedoJson.Situacao : "",
                            mufla = postedoJson.Mufla != null ? postedoJson.Mufla : "",
                            rede_primaria = postedoJson.RedePrimaria != null ? postedoJson.RedePrimaria : "",
                            defeito = postedoJson.Defeito != null ? postedoJson.Defeito : "",
                            ano = postedoJson.Ano != null ? postedoJson.Ano : "",
                            qtd_estai = int.Parse(postedoJson.QtdEstai != null ? postedoJson.QtdEstai : "0")                            

                        };

                        UnitOfWork.PosteRepository.Insert(posteAux);
                        UnitOfWork.Save();

                        #region Arvore
                        /*
                        if (posteAux.TipoPoste == TipoPoste.ARVORE_P || posteAux.TipoPoste == TipoPoste.ARVORE_M || posteAux.TipoPoste == TipoPoste.ARVORE_G)
                        {
                            PorteArvore porte_corrente = PorteArvore.SEM_INFORMACÃO;

                            switch (posteAux.TipoPoste)
                            {
                                case TipoPoste.ARVORE_P:
                                    porte_corrente = PorteArvore.PEQUENA;
                                    break;
                                case TipoPoste.ARVORE_M:
                                    porte_corrente = PorteArvore.MEDIA;
                                    break;
                                case TipoPoste.ARVORE_G:
                                    porte_corrente = PorteArvore.GRANDE;
                                    break;
                            }

                            Arvore arvore = new Arvore();

                            arvore.Logradouro = "null";
                            arvore.Porte = porte_corrente;
                            arvore.Localizacao = "null";
                            arvore.X = posteAux.X;
                            arvore.Y = posteAux.Y;
                            arvore.Ativo = false;
                            arvore.NomeBloco = "null";
                            arvore.Latitude = postedoJson.Posicao.Lat;
                            arvore.Longitude = postedoJson.Posicao.Lon;
                            arvore.Angulo = 0;
                            arvore.DataCadastro = DateTime.Now;
                            arvore.DataExclusao = null;
                            arvore.IdPoste = posteAux.IdPoste;
                            arvore.IdOrdemDeServico = posteAux.IdOrdemDeServico;
                            arvore.IdCidade = posteAux.IdCidade;


                            arvore.OrdemDeServico = OS;
                            arvore.Cidade = OS.Cidade;
                            arvore.Poste = posteAux;

                            UnitOfWork.ArvoreRepository.Insert(arvore);

                            /// Inserindo as novas fotos
                            foreach (FotoAPI foto in postedoJson.Fotos)
                            {
                                DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto);
                                string Data = DataDiretorio.ToString("ddMMyyyy");

                                FotoArvore new_foto = new FotoArvore
                                {
                                    CodigoGeoBD = -1,
                                    IdArvore = arvore.IdArvore,
                                    NumeroFoto = foto.NumeroFoto.Trim(),
                                    DataFoto = DataDiretorio,
                                    Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteAux.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())
                                };

                                UnitOfWork.FotoArvoreRepository.Insert(new_foto);
                            }
                        }
                        */
                        #endregion

                        #region Inserindo as novas fotos

                        foreach (FotoAPI foto in postedoJson.Fotos)
                        {
                            DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto); //ConvertDate.UnixTimestampToDateTime(foto.DataFoto);                            
                            string Data = DataDiretorio.ToString("ddMMyyyy");

                            FotoPoste new_foto = new FotoPoste
                            {
                                CodigoGeoBD = -1,
                                IdPoste = posteAux.IdPoste,
                                NumeroFoto = foto.NumeroFoto.Trim(),
                                DataFoto = DataDiretorio,
                                Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteAux.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())
                            };

                            UnitOfWork.FotoPosteRepository.Insert(new_foto);
                            UnitOfWork.Save();
                        }

                        #endregion

                        #region Inserindo ips deste novo Poste
                        /*
                        List<IpOffline> novosIps = jsonOffline.IPS.Where(ip => ip.IdPoste == postedoJson.IdPoste && ip.IdIp < 0 && ip.Update).ToList();
                        
                        foreach (IpOffline ipOffiline in novosIps)
                        {
                            IP IPAux = new IP()
                            {
                                TipoBraco = ipOffiline.TipoBraco,
                                TipoLuminaria = ipOffiline.TipoLuminaria,
                                QtdLuminaria = ipOffiline.QtdLuminaria,
                                TipoLampada = ipOffiline.TipoLampada,
                                Potencia = ipOffiline.Potencia,
                                CodigoGeoBD = -1,
                                Acionamento = ipOffiline.Acionamento,
                                LampadaAcesa = ipOffiline.LampadaAcesa,
                                Fase = ipOffiline.Fase,
                                QtdLampada = ipOffiline.QtdLampada,
                                IdPoste = posteAux.IdPoste
                            };

                            UnitOfWork.IPRepository.Insert(IPAux);
                        }

                        jsonOffline.IPS.RemoveAll(ip => ip.IdPoste == postedoJson.IdPoste);
                        */
                        #endregion

                        #region Inserindo Ponto de Entrega do novo Poste
                        /*
                        if (jsonOffline.PontoEntrega != null)
                        {
                            List<PontoEntregaOffline> novosPE = jsonOffline.PontoEntrega.Where(pe => pe.IdPoste == postedoJson.IdPoste && pe.IdPontoEntrega < 0 && pe.Update).ToList();
                            
                            foreach (PontoEntregaOffline peOffiline in novosPE)
                            {
                                UTM utmPE = converter.Convert(peOffiline.Posicao.Lat, peOffiline.Posicao.Lon);

                                PontoEntrega peAux = new PontoEntrega()
                                {
                                    //ClasseAtendimento = peOffiline.ClasseAtendimento,
                                    ClasseSocial = peOffiline.ClasseSocial,
                                    CodigoGeoBD = -1,
                                    //EtLigacao = peOffiline.EtLigacao,
                                    //Fase = peOffiline.Fase,
                                    IdPoste = posteAux.IdPoste,
                                    //Logradouro = peOffiline.Logradouro,
                                    Numero = peOffiline.NumeroLocal,
                                    //Observacao = peOffiline.Observacao,
                                    //Status = peOffiline.Status,
                                    //TipoConstrucao = peOffiline.TipoConstrucao,
                                    X = utmPE.X,
                                    Y = utmPE.Y,
                                    IdLagradouro = peOffiline.IdLagradouro,
                                    Classificacao = peOffiline.Classificacao,
                                    Complemento1 = peOffiline.Complemento1,
                                    Complemento2 = peOffiline.Complemento2,
                                    TipoImovel = peOffiline.TipoImovel,
                                    NumeroAndaresEdificio = peOffiline.NumeroAndaresEdificio,
                                    TotalApartamentosEdificio = peOffiline.NumeroTotalApartamentos,
                                    NomeEdificio = peOffiline.NomeEdificio                                   

                                };

                                UnitOfWork.PontoEntregaRepository.Insert(peAux);
                                UnitOfWork.Save();

                                //////Medidores
                             /*   if (peOffiline.Medidor != null)
                                {
                                    foreach (MedidorOffline itemM in peOffiline.Medidor)
                                    {
                                        Medidor objMedidor = new Medidor();
                                        objMedidor.ComplementoResidencial = itemM.ComplementoResidencial;
                                        objMedidor.IdPontoEntrega = peAux.IdPontoEntrega;
                                        objMedidor.NumeroMedidor = itemM.NumeroMedidor;

                                        UnitOfWork.MedidoresRepository.Insert(objMedidor);
                                    }
                                }*/

                        //////Fotos do ponto de Entrega
                        /*if (peOffiline.Fotos != null)
                        {
                            foreach (FotoAPI fotoPE in peOffiline.Fotos)
                            {
                                /// Nao deve Vim foto_view.DataFoto vazio mas se vim nao quebra o codigo
                                DateTime DataDiretorio = fotoPE.DataFoto != String.Empty ? Convert.ToDateTime(fotoPE.DataFoto) : DateTime.Now;
                                String Data = DataDiretorio.ToString("ddMMyyyy");

                                UnitOfWork.FotoPontoEntregaRepository.Insert(new FotoPontoEntrega()
                                {
                                    CodigoGeoBD = -1,
                                    DataFoto = Convert.ToDateTime(fotoPE.DataFoto),
                                    IdPontoEntrega = peAux.IdPontoEntrega,
                                    NumeroFoto = fotoPE.NumeroFoto.Trim(),
                                    DataExclusao = null,
                                    Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], OS.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), fotoPE.NumeroFoto.Trim().ToUpper())
                                });
                            }
                        }
                    }
                    ///Removendo do Json os Pontos de Entregas novos Ja inseridos. 
                    jsonOffline.PontoEntrega.RemoveAll(pe => pe.IdPoste == postedoJson.IdPoste && pe.IdPontoEntrega < 0 && pe.Update);
                }*/

                        #endregion

                        //    UnitOfWork.Save();

                        #region

                        foreach (var ponto_corrente in pontos)
                        {
                            UTM utmPontoEntrega = converter.Convert(ponto_corrente.X, ponto_corrente.Y);
                            UTM utmPontoEntregaAtualizacao = converter.Convert(ponto_corrente.X_atualizacao, ponto_corrente.Y_atualizacao);
                            PontoEntrega pontoNovo = new PontoEntrega
                                {
                                    //OrdemDeServico = OS,
                                    IdOrdemDeServico = OS.IdOrdemDeServico,
                                    // Cidade = OS.Cidade,
                                    IdCidade = OS.Cidade.IdCidade,
                                    ClasseSocial = ponto_corrente.ClasseSocial,
                                    Classificacao = ponto_corrente.Classificacao,
                                    CodigoGeoBD = ponto_corrente.CodigoGeoBD,
                                    Complemento1 = ponto_corrente.Complemento1,
                                    Complemento2 = ponto_corrente.Complemento2,
                                    DataExclusao = ponto_corrente.DataExclusao,
                                    IdLagradouro = 1,
                                    IdPoste = posteAux.IdPoste,
                                    NomeEdificio = ponto_corrente.NomeEdificio,
                                    Numero = ponto_corrente.NumeroLocal,
                                    NumeroAndaresEdificio = ponto_corrente.NumeroAndaresEdificio,
                                    TipoImovel = ponto_corrente.TipoImovel,
                                    TotalApartamentosEdificio = ponto_corrente.NumeroTotalApartamentos,
                                    X = utmPontoEntrega.X,
                                    Y = utmPontoEntrega.Y,
                                    X_atualizacao = utmPontoEntregaAtualizacao.X,
                                    Y_atualizacao = utmPontoEntregaAtualizacao.Y,
                                    DataInclusao = DateTime.Now,
                                    TipoComercio = ponto_corrente.TipoComercio,
                                    QtdSalas = ponto_corrente.QtdSalas,
                                    QtdDomicilio = ponto_corrente.QtdDomicilio,
                                    QtdBlocos = ponto_corrente.QtdBlocos,
                                    Ocorrencia = ponto_corrente.Ocorrencia

                                };

                            UnitOfWork.PontoEntregaRepository.Insert(pontoNovo);
                            UnitOfWork.Save();

                            var vao_corrente = listaDeVaosPoste.Where(v => v.IdPoste == postedoJson.IdPoste && v.IdPontoEntrega == ponto_corrente.IdPontoEntrega).FirstOrDefault();

                            VaosDemandaPoste VaosDemandaPoste = new VaosDemandaPoste();
                            VaosDemandaPoste.IdPontoEntrega = vao_corrente.IdPontoEntrega;

                            UTM X1 = converter.Convert(vao_corrente.X1, vao_corrente.Y1);
                            UTM X2 = converter.Convert(vao_corrente.X2, vao_corrente.Y2);

                            VaosDemandaPoste vaosdemandaposteAux = new VaosDemandaPoste
                            {
                                IdOrdemDeServico = vao_corrente.IdOrdemDeServico,
                                IdPontoEntrega = pontoNovo.IdPontoEntrega,
                                X1 = X1.X,
                                Y1 = X1.Y,
                                X2 = X2.X,
                                Y2 = X2.Y,
                                IdCidade = OS.Cidade.IdCidade,
                                IdPoste = posteAux.IdPoste,
                            };
                            UnitOfWork.VaosDemandaPosteRepository.Insert(vaosdemandaposteAux);
                            UnitOfWork.Save();
                        }
                        #endregion


                    }
                    #endregion

                    #region Etapa 2 - Atualizando os Postes e Fotos (Exclusão e Atualização)

                    foreach (PosteOffline posteOffline in jsonOffline.Postes.Where(poste => poste.Update && poste.IdPoste > 0).ToList()) // Obtendo todos os postes atualizados
                    {
                        Poste posteBD = UnitOfWork.PosteRepository.Get(p => p.IdPoste == posteOffline.IdPoste && p.DataExclusao == null, includeProperties: "Fotos").FirstOrDefault();
                        if (posteBD != null)
                        {
                            UTM utmAtualizacao = new UTM() { X = 0, Y = 0 };
                            if (posteOffline.PontoAtualizacao != null)
                                utmAtualizacao = converter.Convert(posteOffline.PontoAtualizacao.Latitude, posteOffline.PontoAtualizacao.Longitude);

                            posteBD.XAtualizacao = utmAtualizacao.X;
                            posteBD.YAtualizacao = utmAtualizacao.Y;

                            #region Poste Excluido

                            if (posteOffline.Excluido)
                            {
                                posteBD.DataExclusao = DateTime.Now;

                                // Deletando as fotos do poste
                                List<FotoPoste> fotosAuxBD = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste == posteBD.IdPoste && f.DataExclusao == null).ToList();
                                foreach (FotoPoste foto in fotosAuxBD)
                                {
                                    foto.DataExclusao = DateTime.Now;
                                    UnitOfWork.FotoPosteRepository.Update(foto);
                                }

                                UnitOfWork.PosteRepository.Update(posteBD);

                            #endregion
                                UnitOfWork.Save();
                                continue; // Continua para o próximo item da lista 
                            }
                    #endregion

                            #region Caso o poste foi Atualizado faça abaixo

                            UTM utmPoste = converter.Convert(posteOffline.Posicao.Lat, posteOffline.Posicao.Lon);
                            posteBD.X = utmPoste.X;
                            posteBD.Y = utmPoste.Y;
                            posteBD.Altura = posteOffline.Altura;
                            posteBD.TipoPoste = posteOffline.TipoPoste;
                            posteBD.Esforco = posteOffline.Esforco != null ? posteOffline.Esforco : 0;
                            posteBD.Descricao = !String.IsNullOrEmpty(posteOffline.Descricao) ? posteOffline.Descricao.ToUpper() : "";
                            posteBD.Finalizado = posteOffline.Finalizado;
                            posteBD.DataCadastro = DateTime.Now;
                            posteBD.NumeroPosteNaOS = posteOffline.NumeroPosteNaOS;
                            posteBD.Ocupante_s = posteOffline.Ocupante_s;
                            posteBD.Ocupante_d = posteOffline.Ocupante_d;
                            posteBD.para_raio = posteOffline.ParaRario;
                            posteBD.equipamento1 = posteOffline.Equipamento;
                            posteBD.encontrado = posteOffline.Encontrado;
                            posteBD.barramento = posteOffline.Barramento;
                            posteBD.aterramento = posteOffline.Aterramento;
                            posteBD.estrutura_primaria = posteOffline.EstruturaPrimaria;
                            posteBD.estrutura_secundaria = posteOffline.EstruturaSecundaria;
                            posteBD.situacao = posteOffline.Situacao;
                            posteBD.mufla = posteOffline.Mufla;
                            posteBD.rede_primaria = posteOffline.RedePrimaria;
                            posteBD.defeito = posteOffline.Defeito;
                            posteBD.ano = posteOffline.Ano;
                            posteBD.qtd_estai = int.Parse(posteOffline.QtdEstai);

                            if (posteBD.IdCidade != posteOffline.IdCidade)
                            {
                                posteBD.IdCidade = posteOffline.IdCidade;
                                posteBD.Cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == posteOffline.IdCidade).FirstOrDefault();
                            }

                            if (posteBD.IdOrdemDeServico != posteOffline.IdOrdemDeServico)
                            {
                                posteBD.IdOrdemDeServico = posteOffline.IdOrdemDeServico;
                                posteBD.OrdemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdOrdemDeServico == posteOffline.IdOrdemDeServico).FirstOrDefault();
                            }


                            foreach (var vaosim in UnitOfWork.VaosDemandaPosteRepository.Get(v => v.IdPoste == posteOffline.IdPoste))
                            {
                                //TODO
                                vaosim.X1 = utmPoste.X;
                                vaosim.Y1 = utmPoste.Y;
                                UnitOfWork.VaosDemandaPosteRepository.Update(vaosim);
                                UnitOfWork.Save();
                            }

                            // Atualizando as fotos                            
                            foreach (FotoPoste f in posteBD.Fotos.Where(f => f.DataExclusao == null).ToList())
                            {
                                f.DataExclusao = DateTime.Now;
                                UnitOfWork.FotoPosteRepository.Update(f);
                            }

                            // Verificando as alterações das fotos
                            if (posteOffline.Fotos != null && posteOffline.Fotos.Count > 0)
                            {
                                foreach (FotoAPI foto in posteOffline.Fotos)
                                {
                                    if (foto != null)
                                    {
                                        DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto); //ConvertDate.UnixTimestampToDateTime(foto.DataFoto);
                                        string Data = DataDiretorio.ToString("ddMMyyyy");

                                        FotoPoste f = UnitOfWork.FotoPosteRepository.Get(fto => fto.IdPoste == posteBD.IdPoste && fto.NumeroFoto.Trim() == foto.NumeroFoto.Trim()).FirstOrDefault();
                                        if (f != null)
                                        {
                                            f.DataExclusao = null;
                                            f.DataFoto = DataDiretorio;
                                            f.Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteBD.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim());
                                            UnitOfWork.FotoPosteRepository.Update(f);
                                        }
                                        else
                                        {
                                            FotoPoste fAux = new FotoPoste
                                            {
                                                CodigoGeoBD = -1,
                                                IdPoste = posteBD.IdPoste,
                                                NumeroFoto = foto.NumeroFoto.Trim(),
                                                DataFoto = DataDiretorio,
                                                Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteBD.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())
                                            };

                                            UnitOfWork.FotoPosteRepository.Insert(fAux);
                                        }
                                    }
                                }
                            }

                            UnitOfWork.PosteRepository.Update(posteBD);
                            UnitOfWork.Save();
                        }
                    }
                            #endregion

                    #region Cadastrando Ponto de Entrega

                    foreach (PontoEntregaOffline pontoentrega in jsonOffline.PontoEntrega.Where(ponto => ponto.IdPontoEntrega < 0 && ponto.IdPoste >= 0))
                    {

                        UTM utmPontoEntrega = converter.Convert(pontoentrega.X, pontoentrega.Y);
                        UTM utmPontoEntregaAtualizacao = converter.Convert(pontoentrega.X_atualizacao, pontoentrega.Y_atualizacao);

                        /* if(pontoentrega.Logradouro.){
                             Logradouro l = new Logradouro();

                             UnitOfWork.LogradouroRepository.Insert(l);
                             UnitOfWork.Save();   
                         }*/

                        var VaoCerto = jsonOffline.VaosPontoPoste.Where(v => v.IdPontoEntrega == pontoentrega.IdPontoEntrega).FirstOrDefault();
                        PosteOffline poste = jsonOffline.Postes.FirstOrDefault();
                        Poste poste_bd = UnitOfWork.PosteRepository.Get(p => p.IdPoste == VaoCerto.IdPoste).FirstOrDefault();

                        /*foreach (var vao in listaDeVaos)
                        {
                            jsonOffline.VaosPontoPoste.Remove(vao.Id);
                        }*/

                        PontoEntrega pontoNovo = new PontoEntrega
                        {
                            //OrdemDeServico = OS,
                            IdOrdemDeServico = OS.IdOrdemDeServico,
                            // Cidade = OS.Cidade,
                            IdCidade = OS.Cidade.IdCidade,
                            ClasseSocial = pontoentrega.ClasseSocial,
                            Classificacao = pontoentrega.Classificacao,
                            CodigoGeoBD = pontoentrega.CodigoGeoBD,
                            Complemento1 = pontoentrega.Complemento1,
                            Complemento2 = pontoentrega.Complemento2,
                            DataExclusao = pontoentrega.DataExclusao,
                            IdLagradouro = 1,
                            IdPoste = pontoentrega.IdPoste,
                            NomeEdificio = pontoentrega.NomeEdificio,
                            Numero = pontoentrega.NumeroLocal,
                            NumeroAndaresEdificio = pontoentrega.NumeroAndaresEdificio,
                            TipoImovel = pontoentrega.TipoImovel,
                            TotalApartamentosEdificio = pontoentrega.NumeroTotalApartamentos,
                            X = utmPontoEntrega.X,
                            Y = utmPontoEntrega.Y,
                            X_atualizacao = utmPontoEntregaAtualizacao.X,
                            Y_atualizacao = utmPontoEntregaAtualizacao.Y,
                            DataInclusao = DateTime.Now,
                            TipoComercio = pontoentrega.TipoComercio,
                            QtdSalas = pontoentrega.QtdSalas,
                            QtdDomicilio = pontoentrega.QtdDomicilio,
                            QtdBlocos = pontoentrega.QtdBlocos,
                            Ocorrencia = pontoentrega.Ocorrencia
                        };

                        UnitOfWork.PontoEntregaRepository.Insert(pontoNovo);
                        UnitOfWork.Save();
                        //  foreach (var vao in listaDeVaos)
                        //   {
                        if (poste_bd != null)
                        {
                            VaoCerto.IdPontoEntrega = pontoNovo.IdPontoEntrega;

                            UTM X1 = converter.Convert(VaoCerto.X1, VaoCerto.Y1);
                            UTM X2 = converter.Convert(VaoCerto.X2, VaoCerto.Y2);

                            //  if (poste_bd.IdPoste == VaoCerto.IdPoste)
                            //   {
                            VaosDemandaPoste vaosdemandaposteAux0 = new VaosDemandaPoste
                            {
                                IdOrdemDeServico = VaoCerto.IdOrdemDeServico,
                                IdPontoEntrega = VaoCerto.IdPontoEntrega,
                                X1 = poste_bd.X,
                                Y1 = poste_bd.Y,
                                X2 = X2.X,
                                Y2 = X2.Y,
                                IdCidade = OS.Cidade.IdCidade,
                                IdPoste = pontoNovo.IdPoste
                            };
                            UnitOfWork.VaosDemandaPosteRepository.Insert(vaosdemandaposteAux0);
                            UnitOfWork.Save();
                        }
                        //    }
                        /*  else
                          {
                              VaosDemandaPoste vaosdemandaposteAux = new VaosDemandaPoste
                              {
                                  IdOrdemDeServico = vao.IdOrdemDeServico,
                                  IdPontoEntrega = vao.IdPontoEntrega,
                                  X1 = X1.X,
                                  Y1 = X1.Y,
                                  X2 = X2.X,
                                  Y2 = X2.Y,
                                  IdCidade = OS.Cidade.IdCidade,
                                  IdPoste = pontoNovo.IdPoste
                              };
                              UnitOfWork.VaosDemandaPosteRepository.Insert(vaosdemandaposteAux);
                              UnitOfWork.Save();
                          }*/
                        //    }

                        /// Inserindo as novas fotos
                        foreach (FotoAPI foto in pontoentrega.Fotos)
                        {
                            DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto);
                            string Data = DataDiretorio.ToString("ddMMyyyy");

                            FotoPontoEntrega new_foto = new FotoPontoEntrega
                            {
                                IdPontoEntrega = pontoNovo.IdPontoEntrega,
                                CodigoGeoBD = -1,
                                NumeroFoto = foto.NumeroFoto.Trim(),
                                DataFoto = DataDiretorio,
                                Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], OS.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())
                            };

                            UnitOfWork.FotoPontoEntregaRepository.Insert(new_foto);
                            UnitOfWork.Save();
                        }
                    }
                    #endregion Fim cadastro de Ponto de Entrega

                    #region Atualiza e Exclui Demanda (Ponto de Entrega)

                    var pontosoff = jsonOffline.PontoEntrega.Where(demanda => demanda.Update && demanda.IdPontoEntrega > 0).ToList();

                    foreach (PontoEntregaOffline pontoEntregaOffline in pontosoff)
                    {
                        PontoEntrega pontoEntregaBD = UnitOfWork.PontoEntregaRepository.Get(pon => pon.IdPontoEntrega == pontoEntregaOffline.IdPontoEntrega && pon.DataExclusao == null, includeProperties: "Fotos").FirstOrDefault();
                        if (pontoEntregaBD != null)
                        {
                            //Exclui o ponto
                            #region exlcui Ponto
                            if (pontoEntregaOffline.Excluido)
                            {
                                // Deletando as fotos do Ponto entrega
                                List<FotoPontoEntrega> fotosAuxBD = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdPontoEntrega == pontoEntregaBD.IdPontoEntrega && f.DataExclusao == null).ToList();
                                foreach (FotoPontoEntrega foto in fotosAuxBD)
                                {
                                    foto.DataExclusao = DateTime.Now;
                                    UnitOfWork.FotoPontoEntregaRepository.Update(foto);
                                }

                                VaosDemandaPoste vaosDemandaPosteBD = UnitOfWork.VaosDemandaPosteRepository.Get(vao => vao.IdPontoEntrega == pontoEntregaOffline.IdPontoEntrega && vao.DataExclusao == null).FirstOrDefault();
                                if (vaosDemandaPosteBD != null)
                                {
                                    vaosDemandaPosteBD.DataExclusao = DateTime.Now;
                                    UnitOfWork.VaosDemandaPosteRepository.Update(vaosDemandaPosteBD);
                                }
                                var vaoDeletadoJuntoDemanda = UnitOfWork.VaosDemandaPosteRepository.Get(v => v.IdPontoEntrega == pontoEntregaOffline.IdPontoEntrega && v.DataExclusao == null);
                                if (pontoEntregaBD.X_atualizacao != 0)
                                {
                                    UTM utmAtualizacao = converter.Convert(pontoEntregaBD.X_atualizacao, pontoEntregaBD.Y_atualizacao);
                                    pontoEntregaBD.X_atualizacao = utmAtualizacao.X;
                                    pontoEntregaBD.Y_atualizacao = utmAtualizacao.Y;
                                }

                                pontoEntregaBD.DataExclusao = DateTime.Now;
                                //     UnitOfWork.PontoEntregaRepository.Update(pontoEntregaBD);

                            }
                            #endregion fim exclui Ponto

                            UTM utmPontoEntrega = converter.Convert(pontoEntregaOffline.X, pontoEntregaOffline.Y);
                            UTM utmPontoEtregaUpdate = converter.Convert(pontoEntregaOffline.X_atualizacao, pontoEntregaOffline.Y_atualizacao);

                            pontoEntregaBD.ClasseSocial = pontoEntregaOffline.ClasseSocial;
                            pontoEntregaBD.Classificacao = pontoEntregaOffline.Classificacao;
                            pontoEntregaBD.CodigoGeoBD = pontoEntregaOffline.CodigoGeoBD;
                            pontoEntregaBD.Complemento1 = pontoEntregaOffline.Complemento1;
                            pontoEntregaBD.Complemento2 = pontoEntregaOffline.Complemento2;
                            pontoEntregaBD.IdLagradouro = 1;
                            pontoEntregaBD.IdPoste = pontoEntregaOffline.IdPoste;
                            pontoEntregaBD.NomeEdificio = pontoEntregaOffline.NomeEdificio;
                            pontoEntregaBD.Numero = pontoEntregaOffline.NumeroLocal;
                            pontoEntregaBD.NumeroAndaresEdificio = pontoEntregaOffline.NumeroAndaresEdificio;
                            pontoEntregaBD.TipoImovel = pontoEntregaOffline.TipoImovel;
                            pontoEntregaBD.TotalApartamentosEdificio = pontoEntregaOffline.NumeroTotalApartamentos;
                            pontoEntregaBD.X_atualizacao = utmPontoEtregaUpdate.X;
                            pontoEntregaBD.Y_atualizacao = utmPontoEtregaUpdate.Y;
                            pontoEntregaBD.X = utmPontoEntrega.X;
                            pontoEntregaBD.Y = utmPontoEntrega.Y;
                            pontoEntregaBD.TipoComercio = pontoEntregaOffline.TipoComercio;
                            pontoEntregaBD.QtdSalas = pontoEntregaOffline.QtdSalas;
                            pontoEntregaBD.QtdDomicilio = pontoEntregaOffline.QtdDomicilio;
                            pontoEntregaBD.QtdBlocos = pontoEntregaOffline.QtdBlocos;
                            pontoEntregaBD.Ocorrencia = pontoEntregaOffline.Ocorrencia;

                            /*if (pontoEntregaBD.IdCidade != pontoEntregaOffline.IdCidade)
                            {                                
                                pontoEntregaBD.Cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == pontoEntregaOffline.IdCidade).FirstOrDefault();
                            }*/

                            if (pontoEntregaBD.IdOrdemDeServico != pontoEntregaOffline.IdOrdemDeServico)
                            {
                                pontoEntregaBD.IdOrdemDeServico = pontoEntregaOffline.IdOrdemDeServico;
                                pontoEntregaBD.OrdemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdOrdemDeServico == pontoEntregaOffline.IdOrdemDeServico).FirstOrDefault();
                            }

                            foreach (var vaosim in UnitOfWork.VaosDemandaPosteRepository.Get(v => v.IdPontoEntrega == pontoEntregaOffline.IdPontoEntrega))
                            {
                                //TODO
                                vaosim.X2 = utmPontoEntrega.X;
                                vaosim.Y2 = utmPontoEntrega.Y;
                                UnitOfWork.VaosDemandaPosteRepository.Update(vaosim);
                                UnitOfWork.Save();
                            }


                            // Atualizando as fotos                            
                            foreach (FotoPontoEntrega f in pontoEntregaBD.Fotos.Where(f => f.DataExclusao == null).ToList())
                            {
                                f.DataExclusao = DateTime.Now;
                                UnitOfWork.FotoPontoEntregaRepository.Update(f);
                            }

                            // Verificando as alterações das fotos
                            if (pontoEntregaOffline.Fotos != null && pontoEntregaOffline.Fotos.Count > 0)
                            {
                                foreach (FotoAPI foto in pontoEntregaOffline.Fotos)
                                {
                                    if (foto != null)
                                    {
                                        DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto); //ConvertDate.UnixTimestampToDateTime(foto.DataFoto);
                                        string Data = DataDiretorio.ToString("ddMMyyyy");

                                        FotoPontoEntrega f = UnitOfWork.FotoPontoEntregaRepository.Get(fto => fto.IdPontoEntrega == pontoEntregaBD.IdPontoEntrega && fto.NumeroFoto.Trim() == foto.NumeroFoto.Trim()).FirstOrDefault();
                                        if (f != null)
                                        {
                                            f.DataExclusao = null;
                                            f.DataFoto = DataDiretorio;
                                            f.Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], OS.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim());
                                            UnitOfWork.FotoPontoEntregaRepository.Update(f);
                                        }
                                        else
                                        {
                                            FotoPontoEntrega fAux = new FotoPontoEntrega
                                            {
                                                CodigoGeoBD = -1,
                                                IdPontoEntrega = pontoEntregaBD.IdPontoEntrega,
                                                NumeroFoto = foto.NumeroFoto.Trim(),
                                                DataFoto = DataDiretorio,
                                                Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], OS.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())
                                            };

                                            UnitOfWork.FotoPontoEntregaRepository.Insert(fAux);
                                        }
                                    }
                                }
                            }
                        }
                        UnitOfWork.PontoEntregaRepository.Update(pontoEntregaBD);
                        UnitOfWork.Save();
                    }
                    #endregion Fim

                    #region Cadastro Anotacao

                    foreach (AnotacaoOffline anotacaoOff in jsonOffline.Anotacao.Where(ano => ano.IdAnotacao < 0))
                    {
                        UTM utmPosition = converter.Convert(anotacaoOff.X, anotacaoOff.Y);

                        Anotacao anotacao = new Anotacao
                        {
                            DataExclusao = anotacaoOff.DataExclusao,
                            Descricao = anotacaoOff.Descricao,
                            IdOrdemDeServico = anotacaoOff.IdOrdemServico,
                            X = utmPosition.X,
                            Y = utmPosition.Y,
                            IdCidade = OS.Cidade.IdCidade,
                            DataInclusao = DateTime.Now
                        };
                        UnitOfWork.AnotacaoRepository.Insert(anotacao);
                        UnitOfWork.Save();
                    }
                    #endregion Cadastro Anotacao

                    #region Edita e exclui Anotacao

                    foreach (AnotacaoOffline anoOff in jsonOffline.Anotacao.Where(ano => ano.Update && ano.IdAnotacao > 0).ToList())
                    {
                        Anotacao anotacaoBD = UnitOfWork.AnotacaoRepository.Get(ano => ano.IdAnotacao == anoOff.IdAnotacao && ano.DataExclusao == null).FirstOrDefault();

                        if (anoOff.Excluido)
                        {
                            if (anotacaoBD != null)
                            {
                                anotacaoBD.DataExclusao = DateTime.Now;
                                UnitOfWork.AnotacaoRepository.Update(anotacaoBD);
                            }
                        }

                        UTM utmEdit = converter.Convert(anoOff.X, anoOff.Y);
                        anotacaoBD.X = utmEdit.X;
                        anotacaoBD.Y = utmEdit.Y;
                        anotacaoBD.IdOrdemDeServico = anoOff.IdOrdemServico;
                        anotacaoBD.Descricao = anoOff.Descricao;

                        UnitOfWork.AnotacaoRepository.Update(anotacaoBD);
                        UnitOfWork.Save();
                    }
                    #endregion Fim Edita e exclui Anotacao

                    #region Cadastra DemandaStrad

                    foreach (DemandaStrandOffline demandaStrandOffline in jsonOffline.Strand.Where(s => s.ID < 0))
                    {

                        Poste poste_bd1 = UnitOfWork.PosteRepository.Get(p => p.IdPoste == demandaStrandOffline.PosteId1).FirstOrDefault();
                        Poste poste_bd2 = UnitOfWork.PosteRepository.Get(p => p.IdPoste == demandaStrandOffline.PosteId2).FirstOrDefault();

                        if (poste_bd1 != null && poste_bd2 != null)
                        {
                            DemandaStrand demandaStrand = new DemandaStrand
                            {
                                CodigoBdGeo = demandaStrandOffline.CodigoBdGeo,
                                IdOrdemDeServico = demandaStrandOffline.OrderID,
                                X1 = poste_bd1.X,
                                Y1 = poste_bd1.Y,
                                X2 = poste_bd2.X,
                                Y2 = poste_bd2.Y,
                                IdCidade = OS.Cidade.IdCidade,
                                DataInclusao = DateTime.Now

                            };
                            UnitOfWork.DemandaStrandRepository.Insert(demandaStrand);
                            UnitOfWork.Save();
                        }
                        else if (poste_bd1 != null && poste_bd2 == null)
                        {
                            UTM utmStrand1 = converter.Convert(demandaStrandOffline.X1, demandaStrandOffline.Y1);
                            UTM utmStrand2 = converter.Convert(demandaStrandOffline.X2, demandaStrandOffline.Y2);
                            DemandaStrand demandaStrand = new DemandaStrand
                            {
                                CodigoBdGeo = demandaStrandOffline.CodigoBdGeo,
                                IdOrdemDeServico = demandaStrandOffline.OrderID,
                                X1 = poste_bd1.X,
                                Y1 = poste_bd1.Y,
                                X2 = utmStrand2.X,
                                Y2 = utmStrand2.Y,
                                IdCidade = OS.Cidade.IdCidade,
                                DataInclusao = DateTime.Now

                            };
                            UnitOfWork.DemandaStrandRepository.Insert(demandaStrand);
                            UnitOfWork.Save();
                        }
                        else
                        {
                            UTM utmStrand1 = converter.Convert(demandaStrandOffline.X1, demandaStrandOffline.Y1);
                            UTM utmStrand2 = converter.Convert(demandaStrandOffline.X2, demandaStrandOffline.Y2);

                            DemandaStrand demandaStrand = new DemandaStrand
                            {
                                CodigoBdGeo = demandaStrandOffline.CodigoBdGeo,
                                IdOrdemDeServico = demandaStrandOffline.OrderID,
                                X1 = utmStrand1.X,
                                Y1 = utmStrand1.Y,
                                X2 = utmStrand2.X,
                                Y2 = utmStrand2.Y,
                                IdCidade = OS.Cidade.IdCidade,
                                DataInclusao = DateTime.Now

                            };
                            UnitOfWork.DemandaStrandRepository.Insert(demandaStrand);
                            UnitOfWork.Save();
                        }
                    }
                    #endregion Fim Cadastro Demanda Strand

                    #region Edita e exclui DemandaStrands

                    foreach (DemandaStrandOffline demandaStrandOffline in jsonOffline.Strand.Where(s => s.ID > 0 && s.Update))
                    {

                        UTM utmStrand1 = converter.Convert(demandaStrandOffline.X1, demandaStrandOffline.Y1);
                        UTM utmStrand2 = converter.Convert(demandaStrandOffline.X2, demandaStrandOffline.Y2);

                        DemandaStrand demandaStrandBD = UnitOfWork.DemandaStrandRepository.Get(s => s.ID == demandaStrandOffline.ID && s.DataExclusao == null).FirstOrDefault();

                        if (demandaStrandOffline.Excluido)
                        {
                            demandaStrandBD.DataExclusao = DateTime.Now;
                            UnitOfWork.DemandaStrandRepository.Update(demandaStrandBD);
                        }

                        demandaStrandBD.CodigoBdGeo = demandaStrandOffline.CodigoBdGeo;
                        demandaStrandBD.IdOrdemDeServico = demandaStrandOffline.OrderID;
                        demandaStrandBD.X1 = utmStrand1.X;
                        demandaStrandBD.X2 = utmStrand2.X;
                        demandaStrandBD.Y1 = utmStrand1.Y;
                        demandaStrandBD.Y2 = utmStrand2.Y;

                        UnitOfWork.DemandaStrandRepository.Update(demandaStrandBD);
                        UnitOfWork.Save();
                    }

                    #endregion Fim Edita e exclui DemandaStrands

                    OS.DataPublicacao = DateTime.Now;
                    UnitOfWork.OrdemDeServicoRepository.Update(OS);
                    UnitOfWork.Save();

                    #region Etapa 4 - Verifica se deve ser atualizando a OS

                    OS.DataFinal = jsonOffline.OrdemDeServico.DataFinal;

                    bool todos_os_postes_finalizados = finalizadoColaborador;
                    /*
                                        foreach (Poste _postebd in UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == OS.IdOrdemDeServico && p.DataExclusao != null).ToList())
                                        {
                                            if (_postebd.Finalizado == false)
                                            {
                                                todos_os_postes_finalizados = false;
                                            }
                                        }*/

                    OS.Situacao = todos_os_postes_finalizados == true ? SituacaoOrdemServico.FINALIZADO_CAMPO : OS.Situacao;

                    UnitOfWork.OrdemDeServicoRepository.Update(OS);

                    UnitOfWork.Save(); // Commit da Transação

                    #endregion
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Message = Resources.Messages.Save_OK });
        }
        

    }
}