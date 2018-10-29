using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.DAL.UnitOfWork;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using System.Linq;
using System.Collections.Generic;

namespace WebSite.Test.Website.DAL
{
    [TestClass]
    public class UnitOfWorkTeste
    {
        private UnitOfWork unitOfWork;
        private string NUMOS = "TESTAPPOS";
        private string NOME_CIDADE = "CIDADE_TESTE";
        private string NOME_CIDADE2 = "CIDADE_TESTE2";

        [TestInitialize]
        public void Init()
        {
            unitOfWork = new UnitOfWork();
        }

        [TestCleanup]
        public void End()
        {
            unitOfWork.Dispose();
        }

        [TestMethod]
        public void CRUD_Cidade()
        {
            Cidade cidade = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE).FirstOrDefault();
            if (cidade != null)
            {
                unitOfWork.CidadeRepository.Delete(cidade);
                unitOfWork.Save();
            }

            Cidade newCidade = new Cidade() { Nome = NOME_CIDADE, Datum = Datum.WGS_84, NorteOuSul = NorteOuSul.N, SetimoDigito = 7, Zona = 23 };
            unitOfWork.CidadeRepository.Insert(newCidade);
            unitOfWork.Save();

            newCidade = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE).FirstOrDefault();
            Assert.IsNotNull(newCidade);

            Cidade cidade2 = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE2).FirstOrDefault();
            if (cidade2 != null)
            {
                unitOfWork.CidadeRepository.Delete(cidade2);
                unitOfWork.Save();
            }

            Cidade newCidade2 = new Cidade() { Nome = NOME_CIDADE2, Datum = Datum.WGS_84, NorteOuSul = NorteOuSul.N, SetimoDigito = 9, Zona = 27 };
            unitOfWork.CidadeRepository.Insert(newCidade2);
            unitOfWork.Save();

            newCidade2 = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE2).FirstOrDefault();
            Assert.IsNotNull(newCidade2);

            newCidade.NorteOuSul = NorteOuSul.S;
            unitOfWork.CidadeRepository.Update(newCidade);
            unitOfWork.Save();

            Assert.AreEqual(NOME_CIDADE, unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE).FirstOrDefault().Nome);
        }

        [TestMethod]
        public void CRUD_Ordem_de_servico()
        {
            Cidade cidade = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE).FirstOrDefault();
            Assert.IsNotNull(cidade);

            /// Add OS de Teste
            OrdemDeServico OS = unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault();
            if (OS != null)
            {
                unitOfWork.OrdemDeServicoRepository.Delete(OS);
                unitOfWork.Save();
            }

            OS = new OrdemDeServico() { Cidade = cidade, NumeroOS = NUMOS, DataInicio = DateTime.Now };
            unitOfWork.OrdemDeServicoRepository.Insert(OS);
            unitOfWork.Save();
            Assert.IsNotNull(unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault());

            Usuario us = unitOfWork.UsuarioRepository.Get(u => u.UserLogin == "dev").FirstOrDefault();
            Assert.IsNotNull(us);
            OS.Usuario = us;
            unitOfWork.OrdemDeServicoRepository.Update(OS);
            unitOfWork.Save();

            OS.Usuario = null;
            unitOfWork.OrdemDeServicoRepository.Update(OS);
            unitOfWork.Save();
            Assert.IsNull(unitOfWork.OrdemDeServicoRepository.Get(u => u.NumeroOS == NUMOS).FirstOrDefault().Usuario);

            Cidade cidade2 = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE2).FirstOrDefault();
            Assert.IsNotNull(cidade2);

            //Atualizando a Cidade
            OS = unitOfWork.OrdemDeServicoRepository.Get(u => u.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsNotNull(OS);
            OS.Cidade = cidade2;
            unitOfWork.OrdemDeServicoRepository.Update(OS);
            unitOfWork.Save();

            OS = unitOfWork.OrdemDeServicoRepository.Get(u => u.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsTrue(OS.Cidade.IdCidade == cidade2.IdCidade);
        }

        [TestMethod]
        public void CRUD_Poste()
        {
            OrdemDeServico OS = unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsNotNull(OS);

            Poste poste = unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault();
            if (poste != null)
            {
                unitOfWork.PosteRepository.Delete(poste);
                unitOfWork.Save();
            }

            poste = new Poste() { X = 356390.5, Y = 7409430, OrdemDeServico = OS, DataCadastro = DateTime.Now };
            unitOfWork.PosteRepository.Insert(poste);
            unitOfWork.Save();
            Assert.IsNotNull(unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault());
        }

        [TestMethod]
        public void Add_IMG_Poste()
        {
            OrdemDeServico OS = unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsNotNull(OS);

            Poste poste = unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault();
            poste.Fotos.Add(new FotoPoste() { CodigoGeoBD = 007, NumeroFoto = "007COD", Path = "C:\\Users\\mcaraujo\\Desktop\\imgTeste.png", Poste = poste });
            unitOfWork.PosteRepository.Update(poste);
            unitOfWork.Save();

            FotoPoste fotoPoste = unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault().Fotos.FirstOrDefault();
            Assert.IsNotNull(fotoPoste);
        }

        [TestMethod]
        public void Add_IP_Poste()
        {
            OrdemDeServico OS = unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsNotNull(OS);

            Poste poste = unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault();
            poste.IP.Add(new IP() { TipoBraco = "ABC", TipoLuminaria = "DEF", QtdLuminaria = 007, TipoLampada = "Florecente", Potencia = 1000, CodigoGeoBD = 007 });
            unitOfWork.PosteRepository.Update(poste);            
            unitOfWork.Save();

            IP ip = unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault().IP.FirstOrDefault();
            Assert.IsNotNull(ip);
        }

        [TestMethod]
        public void UP_IP_Poste()
        {
            OrdemDeServico OS = unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsNotNull(OS);

            Poste poste = unitOfWork.PosteRepository.Get(p => p.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault();
            Assert.IsNotNull(poste);

            IP ip = unitOfWork.IPRepository.Get(i => i.Poste.IdPoste == poste.IdPoste, includeProperties: "Poste").FirstOrDefault();
            Assert.IsNotNull(ip);

        }

        [TestMethod]
        public void CRUD_Poligono()
        {
            OrdemDeServico OS = unitOfWork.OrdemDeServicoRepository.Get(os => os.NumeroOS == NUMOS).FirstOrDefault();
            Assert.IsNotNull(OS);

            List<PoligonoOS> poligonosOs = unitOfWork.PoligonoOSRepository.Get(po => po.OrdemDeServico.IdOrdemDeServico == OS.IdOrdemDeServico).ToList();
            if (poligonosOs != null && poligonosOs.Count > 0)
            {
                foreach (PoligonoOS item in poligonosOs)
                    unitOfWork.PoligonoOSRepository.Delete(item);
                unitOfWork.Save();
            }

            for (int i = 0; i < 5; i++)
                unitOfWork.PoligonoOSRepository.Insert(new PoligonoOS() { OrdemDeServico = OS, X1 = 007.00, X2 = 007.00, Y1 = 007.00, Y2 = 007.00 });
            unitOfWork.Save();

            OS = unitOfWork.OrdemDeServicoRepository.Get(o => o.IdOrdemDeServico == OS.IdOrdemDeServico).FirstOrDefault();
            Assert.IsNotNull(OS != null && OS.PoligonosOS.Count > 0);
        }
        
        [TestMethod]
        public void Deletar_Cidades_Teste()
        {
            /// Deletando a Cidade1
            Cidade cid = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE).FirstOrDefault();
            Assert.IsNotNull(cid);
            unitOfWork.CidadeRepository.Delete(cid);
            unitOfWork.Save();
            Assert.IsNull(unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE).FirstOrDefault());

            /// Deletando a Cidade2
            cid = unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE2).FirstOrDefault();
            Assert.IsNotNull(cid);
            unitOfWork.CidadeRepository.Delete(cid);
            unitOfWork.Save();
            Assert.IsNull(unitOfWork.CidadeRepository.Get(c => c.Nome == NOME_CIDADE2).FirstOrDefault());
        }

        [TestMethod]
        public void Logradouro_CRUD()
        {
            /*/Add Logradouro
            Logradouro logradouro = new Logradouro();
            logradouro.Bairro = "centro";
            logradouro.Cep = "102030";
            logradouro.Complemento = "praca";
            logradouro.Nome = "riachoelo";
            logradouro.Tipo = "rua";


            unitOfWork.LogradouroRepository.Insert(logradouro);
            unitOfWork.Save();
            */
            //Update Logradouro e Select Logradouro
            Logradouro l = unitOfWork.LogradouroRepository.Get(lrg => lrg.IdLogradouro == 11).FirstOrDefault();
            Assert.IsNotNull(l);
            l.Complemento = "GinasioPokemom";

            unitOfWork.LogradouroRepository.Update(l);
            unitOfWork.Save();
            Assert.AreEqual("GinasioPokemom", unitOfWork.LogradouroRepository.Get(lrg => lrg.IdLogradouro == 11).FirstOrDefault().Complemento);
            

            //Remove Logradouro 
            Logradouro lg = unitOfWork.LogradouroRepository.Get(lrg => lrg.IdLogradouro == 11).FirstOrDefault();
            unitOfWork.LogradouroRepository.Delete(lg);
            unitOfWork.Save();



        }
    }
}