using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;

namespace Website.MVC.Models.Maps
{
    public class MetodosVaoView
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public List<VaoView> VaosToVaosViewByOs(List<VaoPrimario> Vaos, OrdemDeServico Os)
        {
            List<VaoView> retorno = new List<VaoView>();

            if (Vaos != null && Vaos.Count > 0)
            {
                foreach (VaoPrimario vao in Vaos)
                {
                    ConverterUtmToLatLon converter = new ConverterUtmToLatLon(Os.Cidade.Datum, Os.Cidade.NorteOuSul, Os.Cidade.Zona);

                    retorno.Add(new VaoView
                    {
                        IdVaoPrimario = vao.IdVaoPrimario,
                        LatitudeA = converter.Convert(vao.X1, vao.Y1).Lat,
                        LongitudeA = converter.Convert(vao.X1, vao.Y1).Lon,
                        LatitudeB = converter.Convert(vao.X2, vao.Y2).Lat,
                        LongitudeB = converter.Convert(vao.X2, vao.Y2).Lon,
                    });
                }
            }

            return retorno;
        }

        public List<VaoView> VaosToVaosViewByCidade(List<OrdemDeServico> Ordens)
        {
            List<VaoView> retorno = new List<VaoView>();

            foreach (OrdemDeServico os_corrrente in Ordens)
            {
                List<VaoPrimario> VaosOs = UnitOfWork.VaoPrimarioRepository.Get(v => v.IdOrdemDeServico == os_corrrente.IdOrdemDeServico).ToList();

                if (VaosOs != null && VaosOs.Count > 0)
                {
                    foreach (VaoPrimario vao in VaosOs)
                    {
                        ConverterUtmToLatLon converter = new ConverterUtmToLatLon(os_corrrente.Cidade.Datum, os_corrrente.Cidade.NorteOuSul, os_corrrente.Cidade.Zona);

                        retorno.Add(new VaoView
                        {
                            IdVaoPrimario = vao.IdVaoPrimario,
                            LatitudeA = converter.Convert(vao.X1, vao.Y1).Lat,
                            LongitudeA = converter.Convert(vao.X1, vao.Y1).Lon,
                            LatitudeB = converter.Convert(vao.X2, vao.Y2).Lat,
                            LongitudeB = converter.Convert(vao.X2, vao.Y2).Lon,
                        });
                    }
                }
            }

            return retorno;
        }
    }
}