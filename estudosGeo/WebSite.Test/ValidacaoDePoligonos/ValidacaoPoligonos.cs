using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.DAL.UnitOfWork;
using System.Linq;
using Website.BLL.Entities;
using System.Collections.Generic;
using Website.BLL.Utils.Validacoes;

namespace WebSite.Test.ValidacaoDePoligonos
{
    [TestClass]
    public class ValidacaoPoligonos
    {
        private UnitOfWork unitOfWork;

        [TestInitialize]
        public void Init()
        {
            unitOfWork = new UnitOfWork();
        }

        [TestMethod]
        public void ValidarPoligonosNaBase()
        {
            List<OrdemDeServico> AllOS = unitOfWork.OrdemDeServicoRepository.Get(includeProperties: "PoligonosOS").ToList();
            foreach (OrdemDeServico os in AllOS)
            {
                //Verifica se o Poligono
                Assert.IsTrue(os.PoligonosOS.ValidatePolygon());
            }
        }

        [TestMethod]
        public void CorrigindoOsPoligonosValidos()
        {
            List<OrdemDeServico> AllOS = unitOfWork.OrdemDeServicoRepository.Get(includeProperties: "PoligonosOS").ToList();
            foreach (OrdemDeServico os in AllOS)
            {
                //Ordenando o poligono
                os.PoligonosOS = os.PoligonosOS.OrderPolygon();

                foreach (PoligonoOS polig in os.PoligonosOS)
                {
                    unitOfWork.PoligonoOSRepository.Update(polig);
                }
            }
            unitOfWork.Save();
        }
    }
}