using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class ArvoreController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        /// <summary>
        /// Método que altera o tamanho max do retorno do Json.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
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
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetArvoresByCidade(long IdCidade)
        {
            Cidade cidade_corrente = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == IdCidade).FirstOrDefault();
            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade_corrente.Datum, cidade_corrente.NorteOuSul, cidade_corrente.Zona);
                
            //Lista de Arvores do Banco de Dados.
            //Tranforma a lista de arvores retornadas do banco para um objeto que javascript entende.
            //envia a lista de arvores para a javascript
            List<Arvore> arvores = new List<Arvore>();

            List<ArvoreView> _arvoresView = new List<ArvoreView>();
            /*
            arvores = UnitOfWork.ArvoreRepository.Get(a => a.IdCidade == IdCidade && a.DataExclusao == null).ToList();

            LatLon _coor = null;
            foreach (var arvore_corrente in arvores)
            {
                if (arvore_corrente.Latitude == 0 && arvore_corrente.Longitude == 0)
                {
                    _coor = converter.Convert(arvore_corrente.X, arvore_corrente.Y);
                    arvore_corrente.Latitude = _coor.Lat;
                    arvore_corrente.Longitude = _coor.Lon;
                }

                _arvoresView.Add(new ArvoreView()
                {
                    Fotos = new List<FotoArvoreView>(),
                    IdArvore = arvore_corrente.IdArvore,
                    Latitude = arvore_corrente.Latitude,
                    Longitude = arvore_corrente.Longitude,
                    Porte = arvore_corrente.Porte
                });
            }
            */
            return SendBigJson(_arvoresView, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetArvoresByOs(string OrdemServico)
        {
            //Lista de Arvores do Banco de Dados.
            //Tranforma a lista de arvores retornadas do banco para um objeto que javascript entende.
            //envia a lista de arvores para a javascript
            OrdemDeServico os = UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS.Equals(OrdemServico), includeProperties: "Cidade").FirstOrDefault();
            List<Arvore> arvores = new List<Arvore>();

            List<ArvoreView> _arvoresView = new List<ArvoreView>();
            /*
            if (!os.Equals(new OrdemDeServico()))
            {
                ConverterUtmToLatLon converter = new ConverterUtmToLatLon(os.Cidade.Datum, os.Cidade.NorteOuSul, os.Cidade.Zona);
                
                arvores = UnitOfWork.ArvoreRepository.Get(a => a.IdOrdemDeServico == os.IdOrdemDeServico && a.DataExclusao == null).ToList();

                LatLon _coor = null;
                foreach (var arvore_corrente in arvores)
                {
                    if (arvore_corrente.Latitude == 0 && arvore_corrente.Longitude == 0)
                    {
                        _coor = converter.Convert(arvore_corrente.X, arvore_corrente.Y);
                        arvore_corrente.Latitude = _coor.Lat;
                        arvore_corrente.Longitude = _coor.Lon;
                    }

                    _arvoresView.Add(new ArvoreView() { 
                        Fotos = new List<FotoArvoreView>(),
                        IdArvore = arvore_corrente.IdArvore,
                        Latitude = arvore_corrente.Latitude,
                        Longitude = arvore_corrente.Longitude,
                        Porte =  arvore_corrente.Porte
                    });
                }
            }
            */
            return SendBigJson(_arvoresView, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetArvoresById(long IdArvore)
        {
            //Lista de Arvores do Banco de Dados.
            //Tranforma a lista de arvores retornadas do banco para um objeto que javascript entende.
            //envia a lista de arvores para a javascript
            Arvore arvore = UnitOfWork.ArvoreRepository.Get(a => a.IdArvore == IdArvore && a.DataExclusao == null, includeProperties: "Cidade").FirstOrDefault();
            ArvoreView arvoreview = new ArvoreView();
            if(arvore != null)
            {
                arvoreview.IdArvore = arvore.IdArvore;
                arvoreview.Porte = arvore.Porte;

                ConverterUtmToLatLon converter = new ConverterUtmToLatLon(arvore.Cidade.Datum, arvore.Cidade.NorteOuSul, arvore.Cidade.Zona);
                LatLon _coor = converter.Convert(arvore.X, arvore.Y);
                if (arvore.Latitude == 0 && arvore.Longitude == 0)
                {
                    arvoreview.Latitude = _coor.Lat;
                    arvoreview.Longitude = _coor.Lon;
                }
                else 
                {
                    arvoreview.Latitude = arvore.Latitude;
                    arvoreview.Longitude = arvore.Longitude;
                }


                foreach(FotoArvore foto_corrente in UnitOfWork.FotoArvoreRepository.Get(f => f.IdArvore == arvore.IdArvore && f.DataExclusao == null).ToList())
                {
                    arvoreview.Fotos.Add(new FotoArvoreView()
                    {
                        DataFoto = string.Format("{0:yyyy-MM-dd}", foto_corrente.DataFoto),
                        IdFotoArvore = foto_corrente.IdFotoArvore,
                        NumeroFoto = foto_corrente.NumeroFoto
                    }); 
                }
            }

            return Json(new ResponseView() { Status = Status.OK, Result = arvoreview }, JsonRequestBehavior.AllowGet);
        }
	}
}