using System.Collections.Generic;
using System.IO;
using Website.BLL.Entities;

namespace Website.MVC.Models.Relatorios
{
    public static  class RelatorioFotosPoste
    {
        public static IEnumerable<FotosPosteRelatorioView> Processar(IEnumerable<Poste> Postes)
        {
            List<FotosPosteRelatorioView> posteView = new List<FotosPosteRelatorioView>();

            bool possuiFoto = false;
            foreach (Poste item in Postes)
            {
                foreach (FotoPoste itemFP in item.Fotos)
                {
                    possuiFoto = true;  

                    if (itemFP.NumeroFoto == null || itemFP.NumeroFoto == string.Empty ||
                        (!File.Exists(itemFP.Path)))
                    {
                        FotosPosteRelatorioView objFPRV = new FotosPosteRelatorioView();
                        objFPRV.CodBDGeoPoste = item.CodigoGeo;
                        objFPRV.DiretorioFoto = itemFP.Path;
                        objFPRV.NumeroFoto = itemFP.NumeroFoto;
                        objFPRV.NumeroOS = item.OrdemDeServico.NumeroOS;
                        objFPRV.ExisteHD = "NÂO";

                        posteView.Add(objFPRV);
                    }
                }

                if(!possuiFoto)
                {
                    FotosPosteRelatorioView objFPRV = new FotosPosteRelatorioView();
                    objFPRV.CodBDGeoPoste = item.CodigoGeo;
                    objFPRV.DiretorioFoto = "";
                    objFPRV.NumeroFoto = "";
                    objFPRV.NumeroOS = item.OrdemDeServico.NumeroOS;
                    objFPRV.ExisteHD = "";

                    posteView.Add(objFPRV);
                }
            }

            return posteView;
        }
    }
}