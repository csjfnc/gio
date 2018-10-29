using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Website.BLL.Enums;
using Website.BLL.Entities;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models;
using Website.MVC.Models.Relatorios;
using Website.MVC.Models.Maps;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Website.MVC.Controllers
{
    public class RelatoriosController : BaseController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult FotosPostes()
        {
            ViewBag.IdCidade = new SelectList(UnitOfWork.CidadeRepository.Get().ToList(), "IdCidade", "Nome", "0").OrderBy(c => c.Text);
            return View(new List<FotosPosteRelatorioView>());
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.RELATORIOS })]
        public ActionResult FotosPostes(string IdCidade)
        {
            long Id = long.Parse(IdCidade);
            ViewBag.IdCidade = new SelectList(UnitOfWork.CidadeRepository.Get().ToList(), "IdCidade", "Nome", IdCidade);
            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == Id && p.DataExclusao == null && p.OrdemDeServico.DataFinal == null, includeProperties: "OrdemDeServico,Fotos");
            List<FotosPosteRelatorioView> Lista = RelatorioFotosPoste.Processar(Postes).ToList();

            return View(Lista);
        }

        public ActionResult OsbyUser()
        {
            ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.RELATORIOS })]
        public ActionResult OsbyUser(string cidade)
        {
            ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);

            int id_Cidade = Convert.ToInt32(cidade);

            List<RelatorioOsByUserViewModel> Info_Os = new List<RelatorioOsByUserViewModel>();

            List<OrdemDeServico> lst_os = new List<OrdemDeServico>();

            lst_os = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_Cidade).ToList();

            foreach (OrdemDeServico os_corrente in lst_os)
            {
                os_corrente.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == os_corrente.IdUsuario).FirstOrDefault();
                os_corrente.Postes = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == os_corrente.IdOrdemDeServico && p.DataExclusao == null).ToList();
            }

            int t_postes = 0;
            int t_arvores = 0;
            int t_invasao = 0;

            foreach (OrdemDeServico os in lst_os)
            {
                t_postes = 0;
                t_arvores = 0;
                t_invasao = 0;

                t_arvores = os.Postes.Where(p => p.TipoPoste == TipoPoste.ARVORE_P || p.TipoPoste == TipoPoste.ARVORE_M || p.TipoPoste == TipoPoste.ARVORE_G).Count();
                t_invasao = os.Postes.Where(p => p.TipoPoste == TipoPoste.INVASAO_RISCO_01 || p.TipoPoste == TipoPoste.INVASAO_RISCO_02).Count();
                t_postes = os.Postes.Count() - (t_arvores + t_invasao);

                RelatorioOsByUserViewModel reletorio = new RelatorioOsByUserViewModel
                {
                    OrdemServico = os.NumeroOS,
                    DataEncerramento = os.DataFinal,
                    DataCriacao = os.DataInicio,
                    TotalPostes = t_postes,
                    TotalArvores = t_arvores,
                    TotalInvasao = t_invasao,
                    PorcentagemFinalizados = CalculaPorcentagemFinalizados(os.Postes.Count(), os.Postes.Where(p => p.Finalizado == true).Count()),
                    Colaborador = os.Usuario == null ? "Sem Colaborador" : os.Usuario.UserName,
                    IdOS = os.IdOrdemDeServico
                };

                Info_Os.Add(reletorio);
            }

            return View(Info_Os);
        }

        public ActionResult RelatorioPoste()
        {
            ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
            ViewBag.OrdensServicos = new List<OrdemDeServico>();

            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.RELATORIOS })]
        public ActionResult RelatorioPoste(string cidade, string ordemservico)
        {
            List<RelatorioPosteViewModel> Relatorio = new List<RelatorioPosteViewModel>();
            int id_ordem_servico = 0;
            int id_cidade = 0;

            if (cidade != null && cidade != string.Empty)
                id_cidade = Convert.ToInt32(cidade);

            //if (ordemservico != null)
            if (ordemservico != "")
                id_ordem_servico = Convert.ToInt32(ordemservico);

            if (id_cidade != 0)
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList().OrderBy(os => os.NumeroOS);
            }
            else
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = new List<OrdemDeServico>();
            }

            string NomeCidade = null, NomeOS = null;
            if (id_cidade != 0 /*&& id_ordem_servico != 0*/)
            {
                List<Poste> postes = new List<Poste>();

                if (id_cidade != 0 && id_ordem_servico != 0)
                    postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == id_cidade && p.IdOrdemDeServico == id_ordem_servico, includeProperties: "Cidade,OrdemDeServico").ToList();
                else
                    postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == id_cidade, includeProperties: "Cidade,OrdemDeServico").ToList();

                foreach (Poste poste in postes)
                {
                    NomeCidade = (NomeCidade == null) ? poste.Cidade.Nome : NomeCidade;
                    NomeOS = (NomeOS == null) ? poste.OrdemDeServico.NumeroOS : NomeOS;
                    Usuario Colaborador_BD = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == poste.OrdemDeServico.IdUsuario).FirstOrDefault();
                    
                    Relatorio.Add(new RelatorioPosteViewModel
                    {
                        CodGeo = poste.CodigoGeo,
                        IdPoste = poste.IdPoste,
                        //StatusComparativo = (poste.DataExclusao != null) ? Resources.Messages.StatusComparativo_Poste_Excluido : (poste.CodigoGeo <= 0) ? 
                        //Resources.Messages.StatusComparativo_Poste_Novo : Resources.Messages.StatusComparativo_Poste_Atualizado,
                        StatusComparativo = (poste.DataExclusao != null) ? "X" : (poste.CodigoGeo <= 0) ?
                        "N" : "A",
                        X = poste.X,
                        Y = poste.Y,
                        Cidade = poste.Cidade.CidadeDiretorio,
                        OrdemServico = poste.OrdemDeServico.NumeroOS,
                        Colaborador = Colaborador_BD != null ? Colaborador_BD.UserName : "Sem Colaborador Associado",
                        DataCadastro = poste.DataCadastro,
                        Altura = poste.Altura,
                        TipoPoste = RetornaTipoPoste(poste.TipoPoste.ToString()).ToString(), //poste.TipoPoste.ToString(),
                        Esforco = poste.Esforco,
                        Descricao = poste.Descricao,
                        NumeroPosteNaOS = poste.NumeroPosteNaOS,
                        logradouro = poste.logradouro,
                        equipamento1 = poste.equipamento1,
                        equipamento2 = poste.equipamento2,
                        equipamento3 = poste.equipamento3,
                        aterramento = "NAO", //poste.aterropararaio_cia,
                        nomedobloco = (poste.DataExclusao != null) ? "PGE_S_I" : (poste.CodigoGeo <= 0) ?
                        "PGN_S_I" : "PGA_S_I", //poste.nomedobloco,
                        id_temp = poste.id_temp,
                        ativo = poste.ativo,
                        primario = poste.primario,
                        proprietario = poste.proprietario,
                        node = poste.OrdemDeServico.Cidade.Nome,
                        qtdPoste = (poste.DataExclusao != null) ? 0 : (poste.CodigoGeo <= 0) ?
                        1 : 1,
                        idPotencia = poste.idpostecia,
                        caracteristica = poste.caracteristica_cia,
                        aterropararraio = poste.aterropararaio_cia,
                        encontrado = (poste.DataExclusao != null) ? "NAO" : (poste.CodigoGeo <= 0) ?
                        "SIM" : "SIM", //poste.encontrado,
                        material = RetornaMaterial(poste.TipoPoste.ToString()), //poste.material,
                        tipoBase = poste.tipo_base,
                        paraRaio = poste.para_raio,
                        estai = poste.estai,
                        qtdRL = poste.qtde_ramalligacao,
                        qtdRS = poste.qtde_ramalservico,
                        qtdEstai = poste.qtd_estai,
                        avaria = poste.avaria,
                        ocupantes = "NAO", //provisório
                        qtdOcupantes = poste.Ocupante_s,
                        qtdDrop = poste.Ocupante_d,
                        estai2 = poste.estai2,
                        qtdEstai2 = poste.qtde_estai2,
                        lampSemaforo = poste.lampsemaforo,
                        tipoZona = poste.tipo_zona
                    });
                }
            }

            ViewBag.NameFile = "P_" + NomeCidade + "_" + NomeOS + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            return View(Relatorio);
        }

        public ActionResult RelatorioFotos()
        {
            ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
            ViewBag.OrdensServicos = new List<OrdemDeServico>();

            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.RELATORIOS })]
        public ActionResult RelatorioFotos(string cidade, string ordemservico)
        {
            List<RelatorioFotosOrdemServicoViewModel> Relatorio = new List<RelatorioFotosOrdemServicoViewModel>();
            int id_ordem_servico = 0, id_cidade = 0;

            if (cidade != null && cidade != String.Empty)
                id_cidade = Convert.ToInt32(cidade);

            if (ordemservico != null)
                id_ordem_servico = Convert.ToInt32(ordemservico);

            if (id_cidade != 0)
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList().OrderBy(os => os.NumeroOS);
            }
            else
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = new List<OrdemDeServico>();
            }

            string NomeCidade = null, NomeOS = null;
            if (id_cidade != 0 && id_ordem_servico != 0)
            {
                List<Poste> postes = new List<Poste>();
                postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == id_cidade && p.IdOrdemDeServico == id_ordem_servico, includeProperties: "Cidade,OrdemDeServico,Fotos").ToList();

                foreach (Poste poste in postes)
                {
                    NomeCidade = (NomeCidade == null) ? poste.Cidade.Nome : NomeCidade;
                    NomeOS = (NomeOS == null) ? poste.OrdemDeServico.NumeroOS : NomeOS;
                    Usuario Colaborador_BD = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == poste.OrdemDeServico.IdUsuario).FirstOrDefault();

                    if (poste.Fotos.Where(f => f.DataExclusao == null).Count() > 0)
                    {
                        foreach (FotoPoste foto in poste.Fotos)
                        {
                            Relatorio.Add(new RelatorioFotosOrdemServicoViewModel
                            {
                                CodGeo = poste.CodigoGeo,
                                IdPoste = poste.IdPoste,
                                StatusComparativo = (poste.DataExclusao != null) ? Resources.Messages.StatusComparativo_Poste_Excluido : (poste.CodigoGeo <= 0) ? Resources.Messages.StatusComparativo_Poste_Novo : Resources.Messages.StatusComparativo_Poste_Atualizado,
                                X = poste.X,
                                Y = poste.Y,
                                Cidade = poste.Cidade.Nome,
                                OrdemServico = poste.OrdemDeServico.NumeroOS,
                                Colaborador = Colaborador_BD != null ? Colaborador_BD.UserName : "Sem Colaborador Associado",
                                Fotos = foto.NumeroFoto,
                                DataCadastro = foto.DataFoto,
                                NumeroPosteNaOS = poste.NumeroPosteNaOS
                            });
                        }
                    }
                    else
                    {
                        Relatorio.Add(new RelatorioFotosOrdemServicoViewModel
                        {
                            CodGeo = poste.CodigoGeo,
                            IdPoste = poste.IdPoste,
                            StatusComparativo = (poste.DataExclusao != null) ? Resources.Messages.StatusComparativo_Poste_Excluido : (poste.CodigoGeo <= 0) ? Resources.Messages.StatusComparativo_Poste_Novo : Resources.Messages.StatusComparativo_Poste_Atualizado,
                            X = poste.X,
                            Y = poste.Y,
                            Cidade = poste.Cidade.Nome,
                            OrdemServico = poste.OrdemDeServico.NumeroOS,
                            Colaborador = Colaborador_BD != null ? Colaborador_BD.UserName : "Sem Colaborador Associado",
                            Fotos = "Nao há Fotos",
                            DataCadastro = null,
                            NumeroPosteNaOS = poste.NumeroPosteNaOS
                        });
                    }
                }
            }

            ViewBag.NameFile = "F_" + NomeCidade + "_" + NomeOS + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            return View(Relatorio);
        }

        public ActionResult RelatorioOsEvolucao() 
        {
            ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
            ViewBag.OrdensServicos = new List<OrdemDeServico>();

            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.RELATORIOS })]
        public ActionResult RelatorioOsEvolucao(string cidade, string ordemservico) 
        {
            List<RelatorioControleDiario> Relatorio = new List<RelatorioControleDiario>();

            int id_ordem_servico = 0, id_cidade = 0;

            if (cidade != null && cidade != String.Empty)
                id_cidade = Convert.ToInt32(cidade);

            if (ordemservico != null)
                id_ordem_servico = Convert.ToInt32(ordemservico);

            if (id_cidade != 0)
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList().OrderBy(os => os.NumeroOS);
            }
            else
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = new List<OrdemDeServico>();
            }

            if (id_cidade != 0 && id_ordem_servico != 0)
            {
                long id_ordem_servido = id_ordem_servico;

                OrdemDeServico os = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdOrdemDeServico == id_ordem_servido).FirstOrDefault();

                Dictionary<DateTime, List<Poste>> dia_postes = new Dictionary<DateTime, List<Poste>>();

                if (os != null)
                {
                    os.Postes = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == os.IdOrdemDeServico && p.DataExclusao == null && p.Finalizado).ToList();
                    os.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == os.IdUsuario).FirstOrDefault();

                    if (os.Postes != null)
                    {
                        if (os.Postes.Count > 0)
                        {
                            foreach (Poste poste_corrente in os.Postes)
                            {
                                if (dia_postes.ContainsKey(poste_corrente.DataCadastro.Date))
                                {
                                    dia_postes[poste_corrente.DataCadastro.Date].Add(poste_corrente);
                                }
                                else
                                {
                                    dia_postes.Add(poste_corrente.DataCadastro.Date, new List<Poste>());
                                    dia_postes[poste_corrente.DataCadastro.Date].Add(poste_corrente);
                                }
                            }

                            int t_postes = 0;
                            int t_arvores = 0;
                            int t_invasao = 0;

                            foreach (var item_corrente in dia_postes)
                            {
                                t_postes = 0;
                                t_arvores = 0;
                                t_invasao = 0;

                                t_arvores = item_corrente.Value.Where(p => p.TipoPoste == TipoPoste.ARVORE_P || p.TipoPoste == TipoPoste.ARVORE_M || p.TipoPoste == TipoPoste.ARVORE_G).Count();
                                t_invasao = item_corrente.Value.Where(p => p.TipoPoste == TipoPoste.INVASAO_RISCO_01 || p.TipoPoste == TipoPoste.INVASAO_RISCO_02).Count();
                                t_postes = item_corrente.Value.Count() - (t_arvores + t_invasao);

                                RelatorioControleDiario row_relarotio = new RelatorioControleDiario()
                                {
                                    Cadastro = item_corrente.Key,
                                    Colaborador = os.Usuario != null ? os.Usuario.UserName : "Sem Colaborador",
                                    TotalArvore = t_arvores,
                                    TotalInvasao = t_invasao,
                                    TotalPoste = t_postes
                                };

                                Relatorio.Add(row_relarotio);
                            }
                        }
                    }
                }
            }

            return View(Relatorio);
        }

        public ActionResult RelatorioByOrdemServico() 
        {
            ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
            ViewBag.OrdensServicos = new List<OrdemDeServico>();

            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.RELATORIOS })]
        public ActionResult RelatorioByOrdemServico(string cidade, string ordemservico) 
        {
            List<RelatorioPosteViewModel> Relatorio = new List<RelatorioPosteViewModel>();
            int id_ordem_servico = 0;
            int id_cidade = 0;

            if (cidade != null && cidade != string.Empty)
                id_cidade = Convert.ToInt32(cidade);

            if (ordemservico != null)
                id_ordem_servico = Convert.ToInt32(ordemservico);

            if (id_cidade != 0)
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList().OrderBy(os => os.NumeroOS);
            }
            else
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = new List<OrdemDeServico>();
            }

            string NomeCidade = null, NomeOS = null;
            if (id_cidade != 0 && id_ordem_servico != 0)
            {
                List<Poste> postes = new List<Poste>();
                postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == id_cidade && p.IdOrdemDeServico == id_ordem_servico, includeProperties: "Cidade,OrdemDeServico").ToList();

                foreach (Poste poste in postes)
                {
                    NomeCidade = (NomeCidade == null) ? poste.Cidade.Nome : NomeCidade;
                    NomeOS = (NomeOS == null) ? poste.OrdemDeServico.NumeroOS : NomeOS;
                    Usuario Colaborador_BD = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == poste.OrdemDeServico.IdUsuario).FirstOrDefault();

                    Relatorio.Add(new RelatorioPosteViewModel
                    {
                        CodGeo = poste.CodigoGeo,
                        IdPoste = poste.IdPoste,
                        //StatusComparativo = (poste.DataExclusao != null) ? Resources.Messages.StatusComparativo_Poste_Excluido : (poste.CodigoGeo <= 0) ? 
                        //Resources.Messages.StatusComparativo_Poste_Novo : Resources.Messages.StatusComparativo_Poste_Atualizado,
                        StatusComparativo = (poste.DataExclusao != null) ? "X" : (poste.CodigoGeo <= 0) ?
                        "N" : "A",
                        X = poste.X,
                        Y = poste.Y,
                        Cidade = poste.Cidade.CidadeDiretorio,
                        OrdemServico = poste.OrdemDeServico.NumeroOS,
                        Colaborador = Colaborador_BD != null ? Colaborador_BD.UserName : "Sem Colaborador Associado",
                        DataCadastro = poste.DataCadastro,
                        Altura = poste.Altura,
                        TipoPoste = RetornaTipoPoste(poste.TipoPoste.ToString()).ToString(), //poste.TipoPoste.ToString(),
                        Esforco = poste.Esforco,
                        Descricao = poste.Descricao,
                        NumeroPosteNaOS = poste.NumeroPosteNaOS,
                        logradouro = poste.logradouro,
                        equipamento1 = poste.equipamento1,
                        equipamento2 = poste.equipamento2,
                        equipamento3 = poste.equipamento3,
                        aterramento = "NAO", //poste.aterropararaio_cia,
                        nomedobloco = (poste.DataExclusao != null) ? "PGE_S_I" : (poste.CodigoGeo <= 0) ?
                        "PGN_S_I" : "PGA_S_I", //poste.nomedobloco,
                        id_temp = poste.id_temp,
                        ativo = poste.ativo,
                        primario = poste.primario,
                        proprietario = poste.proprietario,
                        node = poste.OrdemDeServico.Cidade.Nome,
                        qtdPoste = (poste.DataExclusao != null) ? 0 : (poste.CodigoGeo <= 0) ?
                        1 : 1,
                        idPotencia = poste.idpostecia,
                        caracteristica = poste.caracteristica_cia,
                        aterropararraio = poste.aterropararaio_cia,
                        encontrado = (poste.DataExclusao != null) ? "NAO" : (poste.CodigoGeo <= 0) ?
                        "SIM" : "SIM", //poste.encontrado,
                        material = RetornaMaterial(poste.TipoPoste.ToString()), //poste.material,
                        tipoBase = poste.tipo_base,
                        paraRaio = poste.para_raio,
                        estai = poste.estai,
                        qtdRL = poste.qtde_ramalligacao,
                        qtdRS = poste.qtde_ramalservico,
                        qtdEstai = poste.qtd_estai,
                        avaria = poste.avaria,
                        ocupantes = "NAO", //provisório
                        qtdOcupantes = poste.Ocupante_s,
                        qtdDrop = poste.Ocupante_d,
                        estai2 = poste.estai2,
                        qtdEstai2 = poste.qtde_estai2,
                        lampSemaforo = poste.lampsemaforo,
                        tipoZona = poste.tipo_zona
                    });
                }
            }

            ViewBag.NameFile = "P_" + NomeCidade + "_" + NomeOS + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            
            return View(Relatorio);
        }

        protected JsonResult SendBigJson(object data, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue // Setando o tamanho max para o retorno do Json
            };
        }

        [HttpGet]
        public ActionResult GetRelatorioByCidade(string cidade, string ordemservico)
        {
            List<RelatorioPosteViewModel> Relatorio = new List<RelatorioPosteViewModel>();
            int id_ordem_servico = 0;
            int id_cidade = 0;

            if (cidade != null && cidade != string.Empty)
                id_cidade = Convert.ToInt32(cidade);

            //if (ordemservico != null)
            if (ordemservico != "")
                id_ordem_servico = Convert.ToInt32(ordemservico);

            if (id_cidade != 0)
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList().OrderBy(os => os.NumeroOS);
            }
            else
            {
                ViewBag.Cidades = UnitOfWork.CidadeRepository.Get().ToList().OrderBy(c => c.Nome);
                ViewBag.OrdensServicos = new List<OrdemDeServico>();
            }

            string NomeCidade = null, NomeOS = null;
            if (id_cidade != 0 /*&& id_ordem_servico != 0*/)
            {
                List<Poste> postes = new List<Poste>();

                if (id_cidade != 0 && id_ordem_servico != 0)
                    postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == id_cidade && p.IdOrdemDeServico == id_ordem_servico, includeProperties: "Cidade,OrdemDeServico").ToList();
                else
                    postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == id_cidade, includeProperties: "Cidade,OrdemDeServico").ToList();

                foreach (Poste poste in postes)
                {
                    NomeCidade = (NomeCidade == null) ? poste.Cidade.Nome : NomeCidade;
                    NomeOS = (NomeOS == null) ? poste.OrdemDeServico.NumeroOS : NomeOS;
                    Usuario Colaborador_BD = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == poste.OrdemDeServico.IdUsuario).FirstOrDefault();

                    Relatorio.Add(new RelatorioPosteViewModel
                    {
                        CodGeo = poste.CodigoGeo,
                        IdPoste = poste.IdPoste,
                        //StatusComparativo = (poste.DataExclusao != null) ? Resources.Messages.StatusComparativo_Poste_Excluido : (poste.CodigoGeo <= 0) ? 
                        //Resources.Messages.StatusComparativo_Poste_Novo : Resources.Messages.StatusComparativo_Poste_Atualizado,
                        StatusComparativo = (poste.DataExclusao != null) ? "X" : (poste.CodigoGeo <= 0) ?
                        "N" : "A",
                        X = poste.X,
                        Y = poste.Y,
                        Cidade = poste.Cidade.CidadeDiretorio,
                        OrdemServico = poste.OrdemDeServico.NumeroOS,
                        Colaborador = Colaborador_BD != null ? Colaborador_BD.UserName : "Sem Colaborador Associado",
                        DataCadastro = poste.DataCadastro,
                        Altura = poste.Altura,
                        TipoPoste = RetornaTipoPoste(poste.TipoPoste.ToString()).ToString(), //poste.TipoPoste.ToString(),
                        Esforco = poste.Esforco,
                        Descricao = poste.Descricao,
                        NumeroPosteNaOS = poste.NumeroPosteNaOS,
                        logradouro = poste.logradouro,
                        equipamento1 = poste.equipamento1,
                        equipamento2 = poste.equipamento2,
                        equipamento3 = poste.equipamento3,
                        aterramento = "NAO", //poste.aterropararaio_cia,
                        nomedobloco = (poste.DataExclusao != null) ? "PGE_S_I" : (poste.CodigoGeo <= 0) ?
                        "PGN_S_I" : "PGA_S_I", //poste.nomedobloco,
                        id_temp = poste.id_temp,
                        ativo = poste.ativo,
                        primario = poste.primario,
                        proprietario = poste.proprietario,
                        node = poste.OrdemDeServico.Cidade.Nome,
                        qtdPoste = (poste.DataExclusao != null) ? 0 : (poste.CodigoGeo <= 0) ?
                        1 : 1,
                        idPotencia = poste.idpostecia,
                        caracteristica = poste.caracteristica_cia,
                        aterropararraio = poste.aterropararaio_cia,
                        encontrado = (poste.DataExclusao != null) ? "NAO" : (poste.CodigoGeo <= 0) ?
                        "SIM" : "SIM", //poste.encontrado,
                        material = RetornaMaterial(poste.TipoPoste.ToString()), //poste.material,
                        tipoBase = poste.tipo_base,
                        paraRaio = poste.para_raio,
                        estai = poste.estai,
                        qtdRL = poste.qtde_ramalligacao,
                        qtdRS = poste.qtde_ramalservico,
                        qtdEstai = poste.qtd_estai,
                        avaria = poste.avaria,
                        ocupantes = "NAO", //provisório
                        qtdOcupantes = poste.Ocupante_s,
                        qtdDrop = poste.Ocupante_d,
                        estai2 = poste.estai2,
                        qtdEstai2 = poste.qtde_estai2,
                        lampSemaforo = poste.lampsemaforo,
                        tipoZona = poste.tipo_zona
                    });
                }
            }

            ViewBag.NameFile = "P_" + NomeCidade + "_" + NomeOS + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");

            return Json(new ResponseView() { Status = Status.OK, Result = Relatorio }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDadosRelatorioByOs(string id_ordem) 
        {
            List<RelatorioPosteViewModel> Dados = new List<RelatorioPosteViewModel>();

            if (!String.IsNullOrEmpty(id_ordem)) 
            {
                long id_ordem_servico = 0;
                id_ordem_servico = Convert.ToInt64(id_ordem);

                if (id_ordem_servico != 0) 
                {
                    OrdemDeServico os_corrente = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdOrdemDeServico == id_ordem_servico).FirstOrDefault();
                    os_corrente.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == os_corrente.IdUsuario).FirstOrDefault();
                    os_corrente.Cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == os_corrente.IdCidade).FirstOrDefault();
                    os_corrente.Postes = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == os_corrente.IdOrdemDeServico).ToList();

                    if (os_corrente != null) 
                    {
                        if (os_corrente.Postes != null) 
                        {
                            foreach (Poste poste in os_corrente.Postes)
                            {
                                #region Row Tatble

                                Dados.Add(new RelatorioPosteViewModel
                                {
                                    CodGeo = poste.CodigoGeo,
                                    IdPoste = poste.IdPoste,
                                    //StatusComparativo = (poste.DataExclusao != null) ? Resources.Messages.StatusComparativo_Poste_Excluido : (poste.CodigoGeo <= 0) ? 
                                    //Resources.Messages.StatusComparativo_Poste_Novo : Resources.Messages.StatusComparativo_Poste_Atualizado,
                                    StatusComparativo = (poste.DataExclusao != null) ? "X" : (poste.CodigoGeo <= 0) ?
                                    "N" : "A",
                                    X = poste.X,
                                    Y = poste.Y,
                                    Cidade = os_corrente.Cidade.CidadeDiretorio,
                                    OrdemServico = os_corrente.NumeroOS,
                                    Colaborador = os_corrente.Usuario != null ? os_corrente.Usuario.UserName : "Sem Colaborador Associado",
                                    DataCadastro = poste.DataCadastro,
                                    Altura = poste.Altura,
                                    TipoPoste = RetornaTipoPoste(poste.TipoPoste.ToString()).ToString(), //poste.TipoPoste.ToString(),
                                    Esforco = poste.Esforco,
                                    Descricao = poste.Descricao,
                                    NumeroPosteNaOS = poste.NumeroPosteNaOS,
                                    logradouro = poste.logradouro,
                                    equipamento1 = poste.equipamento1,
                                    equipamento2 = poste.equipamento2,
                                    equipamento3 = poste.equipamento3,
                                    aterramento = "NAO", //poste.aterropararaio_cia,
                                    nomedobloco = (poste.DataExclusao != null) ? "PGE_S_I" : (poste.CodigoGeo <= 0) ?
                                    "PGN_S_I" : "PGA_S_I", //poste.nomedobloco,
                                    id_temp = poste.id_temp,
                                    ativo = poste.ativo,
                                    primario = poste.primario,
                                    proprietario = poste.proprietario,
                                    node = os_corrente.Cidade.Nome,
                                    qtdPoste = (poste.DataExclusao != null) ? 0 : (poste.CodigoGeo <= 0) ?
                                    1 : 1,
                                    idPotencia = poste.idpostecia,
                                    caracteristica = poste.caracteristica_cia,
                                    aterropararraio = poste.aterropararaio_cia,
                                    encontrado = (poste.DataExclusao != null) ? "NAO" : (poste.CodigoGeo <= 0) ?
                                    "SIM" : "SIM", //poste.encontrado,
                                    material = RetornaMaterial(poste.TipoPoste.ToString()), //poste.material,
                                    tipoBase = poste.tipo_base,
                                    paraRaio = poste.para_raio,
                                    estai = poste.estai,
                                    qtdRL = poste.qtde_ramalligacao,
                                    qtdRS = poste.qtde_ramalservico,
                                    qtdEstai = poste.qtd_estai,
                                    avaria = poste.avaria,
                                    ocupantes = "NAO", //provisório
                                    qtdOcupantes = poste.Ocupante_s,
                                    qtdDrop = poste.Ocupante_d,
                                    estai2 = poste.estai2,
                                    qtdEstai2 = poste.qtde_estai2,
                                    lampSemaforo = poste.lampsemaforo,
                                    tipoZona = poste.tipo_zona
                                });

                                #endregion
                            }
                        }
                    }
                }
            }

            return Json(new ResponseView() { Status = Status.OK, Result = Dados }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult GetOsByCidade(string cidade)
        {
            long id_cidade = 0;

            List<object> ordens = new List<object>();

            if (!String.IsNullOrEmpty(cidade)) 
            { 
                id_cidade = Convert.ToInt32(cidade);

                foreach (OrdemDeServico ordem_corrente in UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList())
                {
                    ordens.Add(new { id = ordem_corrente.IdOrdemDeServico, numero = ordem_corrente.NumeroOS });
                }
            }

            return Json(new ResponseView() { Status = Status.OK, Result = ordens }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetCidades()
        {
            return Json(UnitOfWork.CidadeRepository.Get(c => c.IdCidade > 0).ToList().OrderBy(c => c.Nome), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOsByCidadeSimples(string cidade)
        {
            long id_cidade = 0;

            List<object> ordens = new List<object>();

            if (!String.IsNullOrEmpty(cidade))
            {
                id_cidade = Convert.ToInt32(cidade);

                foreach (OrdemDeServico ordem_corrente in UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == id_cidade).ToList())
                {
                    ordens.Add(new { id = ordem_corrente.IdOrdemDeServico, numero = ordem_corrente.NumeroOS });
                }
            }

            return Json(new ResponseView() { Status = Status.OK, Result = ordens }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RelatorioComponente()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CriaRelatorio(int idCidade, int idOs, bool strands, bool anotacoes, bool quadras, bool edificio, bool terreno, bool tiep, bool resid, bool cni)
        {

            RelatoriosGerais RelatoriosGerais = new RelatoriosGerais();

            if(strands){
                foreach (var item in UnitOfWork.DemandaStrandRepository.Get(dem => dem.IdOrdemDeServico == idOs, includeProperties: "Cidade, OrdemDeServico").ToList())
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                    RelatoriosGerais.StrandRelatorios.Add(new StrandRelatorio(){
                        
                        Cidade = item.Cidade.Nome,
                        Colaborador = item.OrdemDeServico.Usuario.UserName,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        CodigoBDGeo = item.CodigoBdGeo,
                        X1 = item.X1,
                        Y1 = item.Y1,
                        X2 = item.X2,
                        Y2 = item.Y2,
                        ID = item.ID
                    });
                }                
            }
            if (anotacoes)
            {
                foreach (var item in UnitOfWork.AnotacaoRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList())
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                    RelatoriosGerais.AnotacaoRelatorios.Add(new AnotacaoRelatorio()
                    {
                        IdAnotacao = item.IdAnotacao,
                        Descricao = item.Descricao,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        Cidade = item.Cidade.Nome,
                        Colaborador = item.OrdemDeServico.Usuario.UserName != null ? item.OrdemDeServico.Usuario.UserName : "Sem Colaborador Associado",
                        X = item.X,
                        Y = item.Y,                                                
                    });
                }
            }
            if (quadras)
            {
                Console.WriteLine("ola");
            }
            if (edificio)
            {
                
                foreach (var item in UnitOfWork.PontoEntregaRepository.Get(edi => edi.IdOrdemDeServico == idOs && edi.DataExclusao == null && edi.ClasseSocial == ClasseSocial.EDIFÍCIO_RES, includeProperties: "Cidade, OrdemDeServico"))
                {
                   item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                    
                    RelatoriosGerais.EdificioRelatorios.Add(new EdificioRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        CodigoGeoBD = item.CodigoGeoBD,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        Colaborador = item.OrdemDeServico.Usuario.UserName != null ? item.OrdemDeServico.Usuario.UserName : "Sem Colaborador Associado",
                        Cidade = item.Cidade.Nome,
                        Numero = item.Numero,
                        NumeroAndaresEdificio = item.NumeroAndaresEdificio,
                        TotalApartamentosEdificio = item.TotalApartamentosEdificio,
                        NomeEdificio = item.NomeEdificio,
                        X = item.X,
                        Y = item.Y,
                        Classificacao = item.Classificacao,
                        TipoImovel = item.TipoImovel,
                        Complemento1 = item.Complemento1,
                        Complemento2 = item.Complemento2
                    });
                }
            }
            if (terreno)
            {
                foreach (var item in UnitOfWork.PontoEntregaRepository.Get(edi => edi.IdOrdemDeServico == idOs && edi.DataExclusao == null && edi.ClasseSocial == ClasseSocial.TERRENO, includeProperties: "Cidade, OrdemDeServico"))
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.TerrenoRelatorios.Add(new TerrenoRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        CodigoGeoBD = item.CodigoGeoBD,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        Colaborador = item.OrdemDeServico.Usuario.UserName != null ? item.OrdemDeServico.Usuario.UserName : "Sem Colaborador Associado",
                        Cidade = item.Cidade.Nome,
                        Numero = item.Numero,
                        X = item.X,
                        Y = item.Y,
                        Classificacao = item.Classificacao,
                        TipoImovel = item.TipoImovel,
                        Complemento1 = item.Complemento1,
                        Complemento2 = item.Complemento2
                    });
                }
            }
            if (tiep)
            {
                Console.WriteLine("ola");
            }
            if (resid)
            {
                var lista = UnitOfWork.PontoEntregaRepository.Get(ponto => ponto.IdOrdemDeServico == idOs && ponto.DataExclusao == null && ponto.ClasseSocial == ClasseSocial.RESIDENCIAL, includeProperties: "Cidade, OrdemDeServico").ToList();
                foreach (var item in lista)
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.ReisidenciaRelatorios.Add(new ReisidenciaRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        CodigoGeoBD = item.CodigoGeoBD,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        Colaborador = item.OrdemDeServico.Usuario.UserName != null ? item.OrdemDeServico.Usuario.UserName : "Sem Colaborador Associado",
                        Cidade = item.Cidade.Nome,
                        Numero = item.Numero,
                        X = item.X,
                        Y = item.Y,
                        Classificacao = item.Classificacao,
                        TipoImovel = item.TipoImovel,
                        Complemento1 = item.Complemento1,
                        Complemento2 = item.Complemento2
                    });
                }
            }
            if (cni)
            {
                Console.WriteLine("ola");
            }

            return Json(new ResponseView() { Status = Status.OK, Result = RelatoriosGerais }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RelatorioComponenteGeral()
        {
            return View();
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetListaDataOrdemServico()
        {
            Dictionary<string, List<int>> data_ids = new Dictionary<string, List<int>>();

            List<OrdemDeServico> OrdemDeServicoFim = new List<OrdemDeServico>();
           // List<DataOrdemExibir> DataOrdemExibir = new List<DataOrdemExibir>();

            OrdemDeServicoFim = UnitOfWork.OrdemDeServicoRepository.Get(or => or.DataFinal == null && or.DataPublicacao != null).ToList();
            List<DataOrdemExibir> lista = new List<DataOrdemExibir>();
            foreach (var item in OrdemDeServicoFim)
            {
                string dhPublicacao = string.Format("{0:dd/MM/yyyy}", item.DataPublicacao);
                int idOrdem =  (int)item.IdOrdemDeServico;
                /*lista.Add(new DataOrdemExibir(){                             
                     ID = idOrdem,
                     DataOrdem = dhPublicacao
                 });*/

                if (data_ids.ContainsKey(dhPublicacao))
                {
                    data_ids[dhPublicacao].Add(idOrdem);
                }
                else {
                    data_ids.Add(dhPublicacao, new List<int>() { idOrdem });
                }
            }
            return Json(new ResponseView() { Status = Status.OK, Result = data_ids }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult ListaOrdemnsPorIDs(string Ids)
        {
            List<OrdemDeServico> OrdemDeServicoLista = new List<OrdemDeServico>();
            List<object> os = new List<object>();
            string[] idsSeparado = Ids.Split(';');

            foreach (string item in idsSeparado)
            {
                if(!item.Equals("")){
                    int id = Convert.ToInt32(item);
                    OrdemDeServico OrdemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(or => or.DataFinal == null && or.DataPublicacao != null && or.IdOrdemDeServico == id).FirstOrDefault();
                    OrdemDeServicoLista.Add(OrdemDeServico);
                }
            }
             
            foreach (var item in OrdemDeServicoLista)
            {
                os.Add(new 
                {
                    ID = item.IdOrdemDeServico,
                    Numero = item.NumeroOS
                });
            }

            return Json(new ResponseView() { Status = Status.OK, Result = os }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CriaRelatorioGeral (int idOs)
        {
            RelatoriosGerais RelatoriosGerais = new RelatoriosGerais();
       
                foreach (var item in UnitOfWork.DemandaStrandRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList())
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                    RelatoriosGerais.StrandRelatorios.Add(new StrandRelatorio()
                    {
                        ID = item.ID,
                        Ativo = item.Ativo,
                        //Cidade = item.Cidade.Nome,
                        //Colaborador = item.OrdemDeServico.Usuario.UserName,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        //CodigoBDGeo = item.CodigoBdGeo,
                        X1 = item.X1,
                        Y1 = item.Y1,
                        X2 = item.X2,
                        Y2 = item.Y2
                        
                    });
                }                    
                foreach (var item in UnitOfWork.AnotacaoRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList())
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                    RelatoriosGerais.AnotacaoRelatorios.Add(new AnotacaoRelatorio()
                    {
                        IdAnotacao = item.IdAnotacao,
                        Descricao = item.Descricao,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        //Cidade = item.Cidade.Nome,
                        //Colaborador = item.OrdemDeServico.Usuario.UserName != null ? item.OrdemDeServico.Usuario.UserName : "Sem Colaborador Associado",
                        X = item.X,
                        Y = item.Y,
                        Ativo = item.Ativo,
                        Angulo = item.Angulo
                    });
                }
             
                foreach (var item in UnitOfWork.PontoEntregaRepository.Get(edi => edi.IdOrdemDeServico == idOs && edi.DataExclusao == null && edi.ClasseSocial == ClasseSocial.EDIFÍCIO_RES, includeProperties: "Poste, OrdemDeServico"))
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.EdificioRelatorios.Add(new EdificioRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        Classificacao = item.Classificacao,
                        Numero = item.Numero,
                        Complemento1 = item.Complemento1,
                        Complemento2 = item.Complemento2,
                        X = item.X,
                        Y = item.Y,
                        CodPoste = item.Poste.IdPoste,
                        NumeroAndaresEdificio = item.NumeroAndaresEdificio,
                        TotalApartamentosEdificio = item.TotalApartamentosEdificio,
                        NomeEdificio = item.NomeEdificio,
                        Ativo = item.Ativo,
                        AnoLevantamentoEdificio = item.AnoLevantamentoEdificio,
                        Angulo = item.Angulo,
                        OrdemServico = item.OrdemDeServico.NumeroOS                                                                      
                    });
                }
            
        
                foreach (var item in UnitOfWork.PontoEntregaRepository.Get(edi => edi.IdOrdemDeServico == idOs && edi.DataExclusao == null && edi.ClasseSocial == ClasseSocial.TERRENO, includeProperties: "Poste, OrdemDeServico"))
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.TerrenoRelatorios.Add(new TerrenoRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        Classificacao = item.Classificacao,
                        Numero = item.Numero,
                        Ativo = item.Ativo,
                        TipoImovel = item.TipoImovel,
                        PosteBox = item.PosteBox,
                        CodPoste = item.Poste.IdPoste,                        
                        X = item.X,
                        Y = item.Y,
                        OrdemServico = item.OrdemDeServico.NumeroOS,      
                        Angulo = item.Angulo                                          
                         
                    });
                }

                var lista = UnitOfWork.PontoEntregaRepository.Get(ponto => ponto.IdOrdemDeServico == idOs && ponto.DataExclusao == null && ponto.ClasseSocial == ClasseSocial.RESIDENCIAL, includeProperties: "Poste, OrdemDeServico").ToList();
                foreach (var item in lista)
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.ReisidenciaRelatorios.Add(new ReisidenciaRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        Classificacao = item.Classificacao,
                        Numero = item.Numero,
                        Complemento1 = item.Complemento1,
                        Complemento2 = item.Complemento2,
                        X = item.X,
                        Y = item.Y,
                        CodPoste = item.Poste.IdPoste,                        
                        Ativo = item.Ativo,                        
                        Angulo = item.Angulo,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        Divisao = item.Divisao,
                        Qtdedem = item.Qtdedem,
                        PosteBox = item.PosteBox

                    });
                }


                var listaComercio = UnitOfWork.PontoEntregaRepository.Get(ponto => ponto.IdOrdemDeServico == idOs && ponto.DataExclusao == null && ponto.ClasseSocial == ClasseSocial.COMERCIAL_P, includeProperties: "Poste, OrdemDeServico").ToList();
                foreach (var item in listaComercio)
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.ComercioRelatorios.Add(new ComercioRelatorio()
                    {
                        ID = item.IdPontoEntrega,
                        Classificacao = item.Classificacao,
                        Numero = item.Numero,
                        Complemento1 = item.Complemento1,
                        Complemento2 = item.Complemento2,
                        X = item.X,
                        Y = item.Y,
                        CodPoste = item.Poste.IdPoste,
                        Ativo = item.Ativo,
                        Angulo = item.Angulo,
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        Divisao = item.Divisao,
                        Qtdedem = item.Qtdedem,
                        PosteBox = item.PosteBox

                    });
                }
                var listaTiep = UnitOfWork.VaosDemandaPosteRepository.Get(ponto => ponto.IdOrdemDeServico == idOs && ponto.DataExclusao == null, includeProperties: "PontoEntrega, OrdemDeServico").ToList();
                foreach (var item in listaTiep)
                {
                 //   item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                    RelatoriosGerais.TiepRelatorios.Add(new TiepRelatorio()
                    {
                        ID = item.IdVaosDemandaPoste,
                        Layer  = item.PontoEntrega.ClasseSocial.ToString(),
                        OrdemServico = item.OrdemDeServico.NumeroOS,
                        X1 = item.X1,
                        Y1 = item.Y1,
                        X2 = item.X2,
                        Y2 = item.Y2,           
                        Ativo = item.Ativo,
                        DemRel = item.PontoEntrega.IdPontoEntrega
                    });
                }


                List<object> postes = new List<object>();
                var postesRepo = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == idOs, includeProperties: "OrdemDeServico").ToList();
                foreach (var item in postesRepo)
                {

                    string data = string.Format("{0:dd/MM/yyyy}", item.data);
                    RelatoriosGerais.PostesRelatorios.Add(new
                    {
                        
                       Codigo = item.IdPoste,
                       Logradouro = "",
                       Equipamento1 = item.equipamento1,
                       Equipamento2 = item.equipamento2,
                       Equipamento3 = item.equipamento3,
                       Aterramento = "",
                       Status = item.Status,
                       Nomedobloco = item.nomedobloco,
                       X = item.X,
                       Y = item.Y,
                       Id_temp = item.id_temp,
                       Ativo = item.ativo,
                       Primario = item.primario,
                       Servico = item.OrdemDeServico.NumeroOS,
                       Lat = "",
                       Longitude = "",
                       Node = "",
                       Proprietario = item.proprietario,
                       Tecnico = "",
                       Data = "",
                       Municipio = "",
                       X_original = "",
                       Y_original = "",
                       Status_edicao = "",
                       Cod_geodatabase	 = item.CodigoGeo,
                       Id_poste_arcitech = "",
                       Quantidade_poste	 = item.quantidade_poste,
                       Idpostecia	 = item.idpostecia,
                       Caracteristica_cia = item.caracteristica_cia,
                       Aterropararaio_cia = item.aterropararaio_cia,
                       Encontrado = item.encontrado,
                       Tipo_poste = item.TipoPoste,
                       Material_poste	 = item.material,
                       Altura_poste	 = item.Altura,
                       Esforco_poste	 = item.Esforco,
                       Tipo_base = item.tipo_base,
                       Para_raio	 = item.para_raio,
                       Estai = item.estai,
                       Observacao = "",
                       Qtde_ramalligacao = item.qtde_ramalligacao,
                       Qtde_ramalservico = item.qtde_ramalservico,
                       Qtde_estai	 = item.qtd_estai,
                       Avaria = item.avaria,
                       Ocupantes	 = item.Ocupante_s,
                       Qtde_ocp = item.qtde_ocp,
                       Qtde_drop = item.qtde_drop,
                       Estai2	 = item.estai2,
                       Qtde_estai2	 = item.qtde_estai2,
                       Lampsemaforo	 = item.lampsemaforo,
                       Tipo_zona = item.tipo_zona,
                    });
                }
                           
            return Json(new ResponseView() { Status = Status.OK, Result = RelatoriosGerais }, JsonRequestBehavior.AllowGet);
        }
                     

        #region Métodos internos de regra de negocio

        private string CalculaPorcentagemFinalizados(int totalPoste, int totalPosteFinalizado)
        {
            int int_Porcentagem = 0;
            if (totalPoste > 0)
                int_Porcentagem = (totalPosteFinalizado * 100) / totalPoste;

            string str_Porcentagem = int_Porcentagem + " %";

            return str_Porcentagem;
        }

        public string RetornaMaterial(string tipo_de_poste)
        {
            try
            {
                string retorno = string.Empty;

                switch (tipo_de_poste)
                {
                    case ("CIRCULAR"):
                        retorno = "CONCRETO";
                        break;

                    case ("MADEIRA"):
                        retorno = "MADEIRA";
                        break;

                    case ("METALICO"):
                        retorno = "METALICO";
                        break;

                    case ("FIBRA"):
                        retorno = "FIBRA";
                        break;

                    case ("SEM_INFORMACÃO"):
                        retorno = "SEM INFORMACAO";
                        break;

                    case ("DUPLOT"):
                        retorno = "CONCRETO";
                        break;

                    case ("QUADRADO"):
                        retorno = "ACO";
                        break;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TipoPoste RetornaTipoPoste(string tipo_de_poste)
        {
            try
            {
                TipoPoste retorno = 0;

                switch (tipo_de_poste)
                {
                    case ("CIRCULAR"):
                        //retorno = 1;
                        retorno = TipoPoste.CIRCULAR;
                        break;

                    case ("DUPLOT"):
                        retorno = TipoPoste.DUPLOT;
                        break;

                    case ("MADEIRA"):
                        //retorno = 3;
                        retorno = TipoPoste.CIRCULAR;
                        break;

                    case ("METALICO"):
                        //retorno = 4;
                        retorno = TipoPoste.TUBULAR;
                        break;

                    case ("FIBRA"):
                        //retorno = 5;
                        retorno = TipoPoste.FIBRA;
                        break;

                    case ("TUBULAR"):
                        retorno = TipoPoste.TUBULAR;
                        break;

                    case ("QUADRADO"):
                        retorno = TipoPoste.QUADRADO;
                        break;

                    case ("ARVORE_P"):
                        retorno = TipoPoste.ARVORE_P;
                        break;

                    case ("ARVORE_M"):
                        retorno = TipoPoste.ARVORE_M;
                        break;

                    case ("ARVORE_G"):
                        retorno = TipoPoste.ARVORE_G;    
                        break;

                    case ("INVASAO_RISCO_01"):
                        retorno = TipoPoste.INVASAO_RISCO_01;
                        break;

                    case ("INVASAO_RISCO_02"):
                        retorno = TipoPoste.INVASAO_RISCO_02;
                        break;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        public ActionResult RelatorioPorColaboradorDataOs()
        {
            return View();
        }

        public ActionResult ListaNomeColaboradorPublicado()
        {
            List<string> users = new List<string>();

            List<PublicacaoOrdemColaborador> colaboradores = new List<PublicacaoOrdemColaborador>();
            colaboradores = UnitOfWork.PublicacaoOrdemColaboradorRepository.Get().ToList();
            string usuario = "";
            foreach (var item in colaboradores)
	        {
                usuario = item.Usuario;
                if(!users.Contains(usuario)){
                    users.Add(usuario);
                }
	        }
            return Json(new ResponseView() { Status = Status.OK, Result = users }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaDatasPublicadas(string user)
        {
            List<PublicacaoOrdemColaborador> dataspublicadas = new List<PublicacaoOrdemColaborador>();
            dataspublicadas = UnitOfWork.PublicacaoOrdemColaboradorRepository.Get(p => p.Usuario == user).ToList();
            List<string> datas = new List<string>();

            string data = "";
            foreach (var item in dataspublicadas)
            {
                data = string.Format("{0:dd/MM/yyyy}", item.Data_publicado);
                if(!datas.Contains(data)){
                    datas.Add(data);
                }
            }
            return Json(new ResponseView() { Status = Status.OK, Result = datas }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaOrdensPublicadas(string user, string data)
        {
            //Dictionary<int, List<string>> osId_osNumero = new Dictionary<int, List<string>>();
            List<string> numeros = new List<string>();
            List<PublicacaoOrdemColaborador> ordens = new List<PublicacaoOrdemColaborador>();

          //  int idOrdem = 0;
            ordens = UnitOfWork.PublicacaoOrdemColaboradorRepository.Get(p => p.Usuario == user && p.Data_publicado == data).ToList();
            foreach (var item in ordens)
            {
                //idOrdem = string.Format("{0:dd//MM/yyyy}", item.Data_publicado);
                if (!numeros.Contains(item.NumeroOs))
                {
                    numeros.Add(item.NumeroOs);
                }
                 
            }
            return Json(new ResponseView() { Status = Status.OK, Result = numeros }, JsonRequestBehavior.AllowGet);
        }
    }
}   


      
        //public ActionResult GetListaDataOrdemServico()
        //{
        //    Dictionary<string, List<int>> data_ids = new Dictionary<string, List<int>>();

        //    List<OrdemDeServico> OrdemDeServicoFim = new List<OrdemDeServico>();
        //    List<DataOrdemExibir> DataOrdemExibir = new List<DataOrdemExibir>();

        //    OrdemDeServicoFim = UnitOfWork.OrdemDeServicoRepository.Get(or => or.DataFinal == null && or.DataPublicacao != null).ToList();
        //    List<DataOrdemExibir> lista = new List<DataOrdemExibir>();
        //    foreach (var item in OrdemDeServicoFim)
        //    {
        //        string dhPublicacao = string.Format("{0:dd/MM/yyyy}", item.DataPublicacao);
        //        int idOrdem =  (int)item.IdOrdemDeServico;
        //        /*lista.Add(new DataOrdemExibir(){                             
        //             ID = idOrdem,
        //             DataOrdem = dhPublicacao
        //         });*/

        //        if (data_ids.ContainsKey(dhPublicacao))
        //        {
        //            data_ids[dhPublicacao].Add(idOrdem);
        //        }
        //        else {
        //            data_ids.Add(dhPublicacao, new List<int>() { idOrdem });
        //        }
        //    }
        //    return Json(new ResponseView() { Status = Status.OK, Result = data_ids }, JsonRequestBehavior.AllowGet);
        //}