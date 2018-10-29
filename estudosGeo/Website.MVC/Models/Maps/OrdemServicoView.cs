using System;
using System.Collections.Generic;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using System.Linq;
using Website.BLL.Enums;

namespace Website.MVC.Models.Maps
{
    public class OrdemServicoView
    {
        public List<LatLon> PontosPoligono { get; set; }        
        public string NumeroOrdemServico { get; set; }
        public SituacaoOrdemServico Situacao{ get; set; }
        public int NumPoste { get; set; }
        public int NumPosteFinalizado { get; set; }
        public string Colaborador { get; set; }
        public string Observacao { get; set; }
        public Cidade Cidade { get; set; }

        public OrdemServicoView()
        {
            PontosPoligono = new List<LatLon>();
        }

        /// <summary>
        /// Método responsável por criar a estrutura de dados necessária para ser
        /// plotado no mapa do Google Maps.
        /// </summary>
        /// <param name="OrdensDaCidade"></param>
        /// <returns></returns>
        public List<OrdemServicoView> GenerateOSObjects(List<OrdemDeServico> OrdensDaCidade)
        {
            if (OrdensDaCidade == null)
                throw new ArgumentException("o paramêtro 'OrdensDaCidade' não ser null.");

            List<OrdemServicoView> result = new List<OrdemServicoView>();

            #region Criando o Objeto para a plotagem das Ordens de Servico no Mapa

            foreach (OrdemDeServico ordemDeServico in OrdensDaCidade)
            {
                ConverterUtmToLatLon converter = new ConverterUtmToLatLon(ordemDeServico.Cidade.Datum, ordemDeServico.Cidade.NorteOuSul, ordemDeServico.Cidade.Zona);
                OrdemServicoView OSView = new OrdemServicoView();
                OSView.NumeroOrdemServico = ordemDeServico.NumeroOS;
                OSView.Situacao = ordemDeServico.Situacao;
                OSView.Colaborador = ordemDeServico.Usuario != null ? ordemDeServico.Usuario.UserName.ToUpper() : "SEM COLABORADOR";

                foreach (PoligonoOS p in ordemDeServico.PoligonosOS.OrderBy(o => o.Ordem))
                {
                    OSView.PontosPoligono.Add(converter.Convert(p.X1, p.Y1));
                    OSView.PontosPoligono.Add(converter.Convert(p.X2, p.Y2));
                }

                result.Add(OSView);
            }

            #endregion

            List<object> coordenadas = new List<object>();

            return result;
        }

        public List<OrdemServicoView> GenerateOSNumero(List<OrdemDeServico> OrdensDaCidade)
        {
            if (OrdensDaCidade == null)
                throw new ArgumentException("o paramêtro 'OrdensDaCidade' não ser null.");

            List<OrdemServicoView> result = new List<OrdemServicoView>();

            #region Criando o Objeto para a plotagem das Ordens de Servico no Mapa

            foreach (OrdemDeServico ordemDeServico in OrdensDaCidade)
            {
                //ConverterUtmToLatLon converter = new ConverterUtmToLatLon(ordemDeServico.Cidade.Datum, ordemDeServico.Cidade.NorteOuSul, ordemDeServico.Cidade.Zona);
                OrdemServicoView OSView = new OrdemServicoView();
                OSView.NumeroOrdemServico = ordemDeServico.NumeroOS;
                OSView.Situacao = ordemDeServico.Situacao;
             //   OSView.Colaborador = ordemDeServico.Usuario != null ? ordemDeServico.Usuario.UserName.ToUpper() : "SEM COLABORADOR";


                result.Add(OSView);
            }

            #endregion

            List<object> coordenadas = new List<object>();

            return result;
        }

        public object GetOSByNum(OrdemDeServico ordemDeServico)
        {
            string dhCriacao = string.Format("{0:yyyy-MM-dd}", ordemDeServico.DataInicio);
            string dhEncerramento = string.Format("{0:yyyy-MM-dd}", ordemDeServico.DataFinal);
            return new {
                Situacao = ordemDeServico.Situacao,
                DhCriacao = dhCriacao,
                DhEncerramento = dhEncerramento, 
                IdUsuario = ordemDeServico.IdUsuario,
                NumPoste = ordemDeServico.Postes.Where(p => p.DataExclusao == null).Count(),
                NumPosteFinalizado = ordemDeServico.Postes.Where(p => p.DataExclusao == null && p.Finalizado == true).Count(),
                Observacao = ordemDeServico.Observacao,
                Cidade = ordemDeServico.Cidade
            };
        }
    }
}