using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Enums;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class AjaxOrdemDeServicoController : BaseController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetCidades()
        {
            var ciades = UnitOfWork.CidadeRepository.Get(c => c.IdCidade > 0 && c.DataExclusao == null).ToList().OrderBy(c => c.Nome);

            return Json(ciades, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetOSByCidade(long idCidade)
        {

            List<OrdemDeServico> OrdemDeServicos = UnitOfWork.OrdemDeServicoRepository.Get(os => os.Cidade.IdCidade == idCidade, includeProperties: "Cidade,PoligonosOS,Usuario").ToList();
            List<LimiteCidade> LimiteCidades = UnitOfWork.LimiteCidadeRepository.Get(l => l.IdCidade == idCidade, includeProperties: "Cidade").ToList();
            return Json(
                new {
                    ListaOs = new OrdemServicoView().GenerateOSObjects(OrdemDeServicos),
                    Limites = new LimitesCidadeView().LimitesByCidade(LimiteCidades) 
                },
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetOSByCidadeHome(long idCidade)
        {
            return Json(
                new
                {
                    ListaOs = new OrdemServicoView().GenerateOSNumero(UnitOfWork.OrdemDeServicoRepository.Get(os => os.Cidade.IdCidade == idCidade).ToList()),
                   // Limites = new LimitesCidadeView().LimitesByCidade(UnitOfWork.LimiteCidadeRepository.Get(l => l.IdCidade == idCidade, includeProperties: "Cidade").ToList())
                },
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetOSByNumero(string NumOs)
        {
            var obj = new OrdemServicoView().GetOSByNum(UnitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NumOs, includeProperties: "Postes").FirstOrDefault());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult SalvarEdicaoOS(string NumOs, string DhFinal, string IdUsuario, SituacaoOrdemServico situacao, string Observacao)
        {
            OrdemDeServico OS_BD = UnitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NumOs, includeProperties: "Usuario").FirstOrDefault();

            string Resul = "OK";
            string MsgErro = "";

            #region Alterando Dados Ordem Serviço

            if (!DhFinal.Equals(string.Empty))
            {
                DateTime dhEncerramento = Convert.ToDateTime(DhFinal);
                
                if (OS_BD.DataInicio <= dhEncerramento)
                    OS_BD.DataFinal = dhEncerramento;
                else
                    Resul = "Erro"; MsgErro = "Data de Encerramento é menor que a data de Criação.";
            }

            if (!string.IsNullOrEmpty(IdUsuario))
                OS_BD.IdUsuario = IdUsuario;

            OS_BD.Situacao = situacao;
            OS_BD.Observacao = Observacao;

            #endregion

            UnitOfWork.OrdemDeServicoRepository.Update(OS_BD);
            UnitOfWork.Save();

            OrdemServicoView Os_View = new OrdemServicoView() 
            { 
                NumeroOrdemServico = OS_BD.NumeroOS, 
                Situacao = OS_BD.Situacao,
                Colaborador = OS_BD.Usuario != null ? OS_BD.Usuario.UserName.ToUpper() : "SEM COLABORADOR"
            };

            return Json(new { OS_Return = Os_View, Msg = Resul, ErroMsg = MsgErro }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.ADICIONAR })]
        public ActionResult CriarOsPeloSite(string[] latlons, string[] PostesSelecionados, string[] StrandsSelecionados, int IdCidade, string NomeOrdem, string IdUsuario, SituacaoOrdemServico Situacao, string Observacao)
        {
            ConverterLatLonToUtm converter = null;
            var cidade = UnitOfWork.CidadeRepository.Get(id => id.IdCidade == IdCidade).FirstOrDefault();
            if(cidade != null){
                converter = new ConverterLatLonToUtm(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
            }
            

            OrdemDeServico OrdemDeServico = new OrdemDeServico()
            {
                IdCidade = IdCidade,
                IdUsuario = IdUsuario,
                Observacao = Observacao,
                NumeroOS = NomeOrdem,                
                Situacao = Situacao,
                DataInicio = DateTime.Now
                
            };

            UnitOfWork.OrdemDeServicoRepository.Insert(OrdemDeServico);
            UnitOfWork.Save();

            if (PostesSelecionados != null) {
                
                for(int i = 0; i < PostesSelecionados.Length; i++){
                    long idP = Convert.ToInt64(PostesSelecionados[i]);
                    
                    //Realoca Poste para nova OS
                    var posteselect = UnitOfWork.PosteRepository.Get(id => id.IdPoste == idP).FirstOrDefault();
                    if (posteselect != null)
                    {
                        posteselect.IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico;
                    }

                    //Realoca Poste para nova OS
                    var demanda = UnitOfWork.PontoEntregaRepository.Get(id => id.IdPoste == idP).ToList();
                    foreach (var item in demanda)
                    {
                        item.IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico;
                    }
                            
                    //Realoca VaosDemandas para nova OS
                    var vao = UnitOfWork.VaosDemandaPosteRepository.Get(id => id.IdPoste == idP).ToList();
                    foreach (var item in vao)
                    {
                        item.IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico;
                    }
                        
                    //    (vao != null)
                    //{
                    //    vao.IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico;
                    //}          

                }
                UnitOfWork.Save();       
        

            }

            if (StrandsSelecionados != null)
            {
                for (int i = 0; i < StrandsSelecionados.Length; i++)
                {
                    long idS = Convert.ToInt64(StrandsSelecionados[i]);
                    var strandSelect = UnitOfWork.DemandaStrandRepository.Get(id => id.ID == idS).FirstOrDefault();
                    if(strandSelect != null){
                        strandSelect.IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico;
                    }
                }
                UnitOfWork.Save();
            }
        

            int ordem = -1;
            List<string> latlongs = new List<string>();
            bool pode = false;

            double latInicio = 0, lonInicio = 0;
            double latPrimo = 0, lonPrimo = 0;

            List<double> lates = new List<double>();

            for (int i = 0; i < latlons.Length; i++)
            { 
                var x1y1 = latlons[i].Replace("(", "").Replace(")", "").Replace(" ", "").Split(',');
                for (int x = 0; x < x1y1.Length; x++)
                {
                    double lastlons = Double.Parse(x1y1[x], System.Globalization.CultureInfo.InvariantCulture);
                    // Console.WriteLine(lastlons.ToString(new System.Globalization.CultureInfo("en-US", true)));                   
                    lates.Add(lastlons);
                }
            }
            int con = lates.Count();
            int contadorFiel = con;
            int contador = 0;
            double latitude1 = 0, longitude1 = 0, latitude2 = 0, longitude2 = 0;
            for (int d = 0; d <= con; d++)
            {
                if(lonInicio == 0){
                    if (contador == 3)
                    {
                        longitude2 = lates[d];
                        contador++;
                        pode = true;
                        contador = 5; 
                    }

                    if (contador == 2)
                    {
                        latitude2 = lates[d];
                        contador++;
                    }

                    if (contador == 1)
                    {
                        longitude1 = lates[d];
                        contador++;
                    }

                    if (contador == 0)
                    {
                        latitude1 = lates[d];
                        contador++;
                    }
                }
                else
                {

                    if (contadorFiel > d)
                    {                        
                        bool sovai = false;
                        if (contador == 1)
                        {
                            longitude1 = lates[d];
                            contador++;
                            pode = false;
                            sovai = true;
                            
                        }

                        if (contador == 0)
                        {
                            latitude1 = lates[d];
                            contador++;
                        }
                        if (sovai)
                        {
                            UTM lonIni = converter.Convert(latInicio, lonInicio);
                            UTM latvem = converter.Convert(latitude1, longitude1);
                            ordem++;
                            PoligonoOS PoligonoOS = new PoligonoOS()
                            {
                                IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico,
                                Ordem = ordem,
                                X1 = latInicio,
                                Y1 = lonInicio,
                                X2 = latvem.X,
                                Y2 = latvem.Y,
                            };
                            UnitOfWork.PoligonoOSRepository.Insert(PoligonoOS);
                            UnitOfWork.Save();

                            latInicio = latvem.X;
                            lonInicio = latvem.Y;
                            contador = 0;
                        }
                    }
                    else
                    {
                        ordem++;
                        PoligonoOS PoligonoOS = new PoligonoOS()
                        {
                            IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico,
                            Ordem = ordem,
                            X1 = latInicio,
                            Y1 = lonInicio,
                            X2 = latPrimo,
                            Y2 = lonPrimo
                        };
                        UnitOfWork.PoligonoOSRepository.Insert(PoligonoOS);
                        UnitOfWork.Save();
                    }

                }             

                if (pode)
                {
                    UTM xy1 = converter.Convert(latitude1, longitude1);
                    UTM xy2 = converter.Convert(latitude2, longitude2);

                    if (latInicio == 0)
                    {
                        ordem++;
                        PoligonoOS PoligonoOS = new PoligonoOS()
                         {
                             IdOrdemDeServico = OrdemDeServico.IdOrdemDeServico,
                             Ordem = ordem,
                             X1 = xy1.X,
                             Y1 = xy1.Y,
                             X2 = xy2.X,
                             Y2 = xy2.Y,
                         };
                        UnitOfWork.PoligonoOSRepository.Insert(PoligonoOS);
                        UnitOfWork.Save();

                    }         
                    latInicio = xy2.X;
                    lonInicio = xy2.Y;
                    contador = 0;
                    pode = false;
                    if(lonPrimo == 0){
                        latPrimo = xy1.X;
                        lonPrimo = xy1.Y;
                    }
                }
            }

            OrdemDeServico OrdemBd = UnitOfWork.OrdemDeServicoRepository.Get(or => or.IdOrdemDeServico == OrdemDeServico.IdOrdemDeServico).FirstOrDefault();

            //OrdemServicoView OrdemServicoView = new OrdemServicoView().GetOSByNum(OrdemBd);
            
         //   return Json(new {Ordem = new OrdemServicoView().GetOSByNum(OrdemBd)}, JsonRequestBehavior.AllowGet);
            return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApagarOrdemAndPoligonos(string numeroOs)
        {

            OrdemDeServico OrdemDeDB = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == numeroOs).FirstOrDefault();
            if (OrdemDeDB != null)
            {
                var postesDb = UnitOfWork.PosteRepository.Get(idor => idor.IdOrdemDeServico == OrdemDeDB.IdOrdemDeServico).ToList();
                if (postesDb != null)
                {
                    foreach (var item in postesDb)
                    {
                        item.IdOrdemDeServico = null;
                    }
                    UnitOfWork.Save();
                }

                var demandasDb = UnitOfWork.PontoEntregaRepository.Get(idor => idor.IdOrdemDeServico == OrdemDeDB.IdOrdemDeServico).ToList();
                if (demandasDb != null)
                {
                    foreach (var item in demandasDb)
                    {
                        item.IdOrdemDeServico = null;
                    }
                    UnitOfWork.Save();
                }

                var vaos = UnitOfWork.VaosDemandaPosteRepository.Get(idor => idor.IdOrdemDeServico == OrdemDeDB.IdOrdemDeServico).ToList();
                if (vaos != null)
                {
                    foreach (var item in vaos)
                    {
                        item.IdOrdemDeServico = null;
                    }
                    UnitOfWork.Save();
                }

                var notas = UnitOfWork.AnotacaoRepository.Get(idor => idor.IdOrdemDeServico == OrdemDeDB.IdOrdemDeServico).ToList();
                if (notas != null)
                {
                    foreach (var item in notas)
                    {
                        item.IdOrdemDeServico = null;
                    }
                    UnitOfWork.Save();
                }

                var strands = UnitOfWork.DemandaStrandRepository.Get(idor => idor.IdOrdemDeServico == OrdemDeDB.IdOrdemDeServico).ToList();
                if (strands != null)
                {
                    foreach (var item in strands)
                    {
                        item.IdOrdemDeServico = null;
                    }
                    UnitOfWork.Save();
                }

                List<PoligonoOS> PoligonoOS = new List<PoligonoOS>();

                PoligonoOS = UnitOfWork.PoligonoOSRepository.Get(ordem => ordem.IdOrdemDeServico == OrdemDeDB.IdOrdemDeServico).ToList();
                foreach (var item in PoligonoOS)
                {
                    UnitOfWork.PoligonoOSRepository.Delete(item);
                }

                UnitOfWork.OrdemDeServicoRepository.Delete(OrdemDeDB);
                UnitOfWork.Save();

                UnitOfWork.Save();
                return Json(new { Msg = "OK", NumeroOs = numeroOs }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Msg = "Ordem de Serviço não encontrada!", NumeroOs = numeroOs }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public ActionResult CadastrarCidade(Cidade cidade)
        {             
            Cidade Cidade = new Cidade
            {
                Datum = 0,
                Nome = cidade.Nome.ToUpper(),
                CidadeDiretorio = cidade.Nome.ToUpper(),
                NorteOuSul = cidade.NorteOuSul,
                SetimoDigito = cidade.SetimoDigito,
                Zona = cidade.Zona                
            };

            UnitOfWork.CidadeRepository.Insert(Cidade);
            UnitOfWork.Save();
            if (Cidade.IdCidade != 0)
            {
                return Json(new ResponseView { Status = Status.OK, Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseView { Status = Status.NotFound, Result = "Erro" }, JsonRequestBehavior.AllowGet);
            } 
        }
    }
}