using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Website.BLL.Enums;
using Website.DAL.UnitOfWork;

namespace Website.MVC.WebApi
{
    public class RelatoriosColaboradorController : ApiController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public long idOs;

        [HttpGet]
        public HttpResponseMessage CriaRelatorioStrands(string data, string os, string nome)
        {           
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,x1,y1,x2,y2,ativo,servico");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var DemandaStrandRepository = UnitOfWork.DemandaStrandRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            string databd = "";
            foreach (var item in DemandaStrandRepository)
            {
               // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    string x1 = item.X1.ToString();
                    x1 = x1.Replace(",", ".");
                    string y1 = item.Y1.ToString();
                    y1 = y1.Replace(",", ".");
                    string x2 = item.X2.ToString();
                    x2 = x2.Replace(",", ".");
                    string y2 = item.Y2.ToString();
                    y2 = y2.Replace(",", ".");

                    writer.WriteLine(
                        item.ID + "," +
                        x1 + "," +
                        y1 + "," +
                        x2 + "," +
                        y2 + "," +
                        item.Ativo + "," +
                        item.OrdemDeServico.NumeroOS);
                }
            }                         
           writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "strands-"+nome+"-"+os+"-"+data+".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioAnotacao(string data, string os,  string nome)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,anotacao,x,y,angulo,ativo,servico");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var AnotacaoRepository = UnitOfWork.AnotacaoRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            string databd = "";
            foreach (var item in AnotacaoRepository)
            {
                // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    string x = item.X.ToString();
                    x = x.Replace(",", ".");
                    string y = item.Y.ToString();
                    y = y.Replace(",", ".");

                    writer.WriteLine(
                        item.IdAnotacao + "," +
                        item.Descricao + "," +
                        x + "," +
                        y + "," +
                        item.Angulo + "," +
                        item.Ativo + "," +
                        item.OrdemDeServico.NumeroOS);
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "anotacoes-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioEdificio(string data, string os, string nome)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,clasificacao,lagradouro,numero,complemento 1,txtcompl1,complemento 2,txtcompl2,complemento 3,txtcompl3,complemento 4,txtcompl4,observacao,cabeamento,node,x,y,cod_poste,tipo_comercio,numero_de_andares,total_apartamentos,nome_edificio,ativo,ano_levantamento,angulo,servico,lat,longitude,poste_box");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.EDIFÍCIO_RES, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            string databd = "";
            foreach (var item in PontoEntregaRepository)
            {
                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                    string x = item.X.ToString();
                    x = x.Replace(",", ".");
                    string y = item.Y.ToString();
                    y = y.Replace(",", ".");
                    writer.WriteLine(
                        item.IdPontoEntrega + "," +
                        item.Classificacao + ",%," +
                        item.Numero + "," +
                        item.Complemento1 + ",%," +
                        "%" + "," +
                        item.Complemento2 + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                            x + "," +
                            y + "," +
                            item.Poste.IdPoste + "," +
                            "%" + "," +
                            item.NumeroAndaresEdificio + "," +
                            item.TotalApartamentosEdificio + "," +
                            item.Ativo + "," +
                            "%" + "," +
                            item.Angulo + "," +
                            item.OrdemDeServico.NumeroOS + "," +
                            "%" + "," +
                            "%" + "," +
                            "%");
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "edificios-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioTerreno(string data, string os, string nome)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,clasificacao,numero,tipo,x,y,cod_poste,ativo,lagradouro,angulo,divisao,servico,node,lat,longitude,poste_box");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.TERRENO, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            string databd = "";
            foreach (var item in PontoEntregaRepository)
            {
                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                    string x = item.X.ToString();
                    x = x.Replace(",", ".");
                    string y = item.Y.ToString();
                    y = y.Replace(",", ".");

                    writer.WriteLine(
                        item.IdPontoEntrega + ","
                        + item.Classificacao + ","
                        + item.Numero + ","
                        + item.TipoImovel + ","
                        + x + ","
                        + y + ","
                        + item.Poste.IdPoste + ","
                        + item.Ativo
                        + ",%,"
                        + item.Angulo + ","
                        + item.Divisao + ","
                        + item.OrdemDeServico.NumeroOS + "," +
                        "%" + "," +
                        "%" + "," +
                        "%" + "," +
                        item.PosteBox);
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "terreno-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioResid(string data, string os, string nome)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.RESIDENCIAL, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            writer.WriteLine("codigo,clasificacao,lagradouro,numero,complemento 1,txtcompl1,complemento 2,txtcompl2,complemento 3,txtcompl3,complemento 4,txtcompl4,observacao,cabeamento,node,x,y,cod_poste,angulo,divisao,qtdedem,ativo,servico,lat,longitude,poste_box");
            string databd = "";
            foreach (var item in PontoEntregaRepository)
            {
                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                    string x = item.X.ToString();
                    x = x.Replace(",", ".");
                    string y = item.Y.ToString();
                    y = y.Replace(",", ".");

                    writer.WriteLine(
                        item.IdPontoEntrega + ","
                        + item.Classificacao + ","
                        + "%" + ","
                        + item.Numero + ","
                        + item.Complemento1 + ","
                        + "%" + ","
                        + item.Complemento2 + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + x + ","
                        + y + ","
                        + item.Poste.IdPoste + ","
                        + item.Angulo + ","
                        + item.Divisao + ","
                        + item.Qtdedem + ","
                        + item.Ativo + ","
                        + item.OrdemDeServico.NumeroOS + ","
                        + "%" + ","
                        + "%" + ","
                        + item.PosteBox);
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "resid-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }
        
        [HttpGet]
        public HttpResponseMessage CriaRelatorioCni(string data, string os, string nome)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);           
            writer.WriteLine("codigo,clasificacao,lagradouro,numero,complemento 1,txtcompl1,complemento 2,txtcompl2,complemento 3,txtcompl3,complemento 4,txtcompl4,observacao,cabeamento,node,x,y,cod_poste,angulo,divisao,qtdedem,ativo,servico,lat,longitude,poste_box");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.COMERCIAL_P, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            string databd = "";
            foreach (var item in PontoEntregaRepository)
            {
                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                    string x = item.X.ToString();
                    x = x.Replace(",", ".");
                    string y = item.Y.ToString();
                    y = y.Replace(",", ".");

                    writer.WriteLine(
                        item.IdPontoEntrega + ","
                        + item.Classificacao + ","
                        + "%" + ","
                        + item.Numero + ","
                        + item.Complemento1 + ","
                        + "%" + ","
                        + item.Complemento2 + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + x + ","
                        + y + ","
                        + item.Poste.IdPoste + ","
                        + item.Angulo + ","
                        + item.Divisao + ","
                        + item.Qtdedem + ","
                        + item.Ativo + ","
                        + item.OrdemDeServico.NumeroOS + ","
                        + "%" + ","
                        + "%" + ","
                        + item.PosteBox);
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "cni-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioTiep(string data, string os, string nome)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);                       
            writer.WriteLine("codigo,x1,y1,x2,y2,ativo,dem_rel,layer,lat1,longitude1,lat2,longitude2,servico,node");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var VaosDemandaPosteRepository = UnitOfWork.VaosDemandaPosteRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico, PontoEntrega").ToList();
            string databd = "";
            foreach (var item in VaosDemandaPosteRepository)
            {
                databd = string.Format("{0:dd/MM/yyyy}", item.DataInclusao);
                if (data.Equals(databd))
                {
                    // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                    string x1 = item.X1.ToString();
                    x1 = x1.Replace(",", ".");

                    string y1 = item.Y1.ToString();
                    y1 = y1.Replace(",", ".");

                    string x2 = item.X2.ToString();
                    x2 = x2.Replace(",", ".");

                    string y2 = item.Y2.ToString();
                    y2 = y2.Replace(",", ".");

                    writer.WriteLine(
                        item.IdVaosDemandaPoste + ","
                        + x1 + ","
                        + y1 + ","
                        + x2 + ","
                        + y2 + ","
                        + item.Ativo + ","
                        + item.IdPontoEntrega + ","
                        + item.PontoEntrega.ClasseSocial + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + "%" + ","
                        + item.OrdemDeServico.NumeroOS + ","
                        + "%");
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "tiep-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioPoste(string data, string os, string nome)
        {
            int a = 8;
            AlturaPoste aa = (AlturaPoste)a;

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo;logradouro;equipamento1;equipamento2;equipamento3;aterramento;status;nomedobloco;x;y;id_temp;ativo;primario;servico;lat;longitude;node;proprietario;tecnico;data;municipio;x_original;y_original;status_edicao;cod_geodatabase;id_poste_arcitech;quantidade_poste;idpostecia;caracteristica_cia;aterropararaio_cia;encontrado;tipo_poste;material_poste;altura_poste;esforco_poste;tipo_base;para_raio;estai;observacao;qtde_ramalligacao;qtde_ramalservico;qtde_estai;avaria;ocupantes;qtde_ocp;qtde_drop;estai2;qtde_estai2;lampsemaforo;tipo_zona");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var PosteRepository = UnitOfWork.PosteRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            string databd = "";
            foreach (var item in PosteRepository)
            {
                databd = string.Format("{0:dd/MM/yyyy}", item.DataCadastro);
                if (data.Equals(databd))
                {
                    item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                    string x = item.X.ToString();
                 //   x = x.Replace(",", ".");
                    string y = item.Y.ToString();
                   // y = y.Replace(",", ".");

                    string data1 = string.Format("{0:dd/MM/yyyy}", item.data);

                    AlturaPoste alturaPoste = (AlturaPoste) item.Altura;
                    Esforco esforcoPoste = (Esforco) item.Esforco;

                    string altura = GetAlturaPoste(alturaPoste);
                    string esforco = GetAEsforcoPoste(esforcoPoste);

                    writer.WriteLine(
                        item.IdPoste + ";"
                        + "%" + ";"
                        + item.equipamento1 + ";"
                        + item.equipamento2 + ";"
                        + item.equipamento3 + ";"
                        + "%" + ";"
                        + item.Status + ";"
                        + item.nomedobloco + ";"
                        + x + ";"
                        + y + ";"
                        + "%" + ";"
                        + item.ativo + ";"
                        + item.primario + ";"
                        + item.OrdemDeServico.NumeroOS + ";"
                        + "%" + ";"
                        + "%" + ";"
                        + "%" + ";"
                        + "%" + ";"
                        + item.OrdemDeServico.Usuario.UserName + ";"
                        + databd + ";"
                        + "%" + ";"
                        + "%" + ";"
                        + "%" + ";"
                        + "%" + ";"
                        + item.CodigoGeo + ";"
                        + "%" + ";"
                        + item.quantidade_poste + ";"
                        + "%" + ";"
                        + item.caracteristica_cia + ";"
                        + item.aterropararaio_cia + ";"
                        + item.encontrado + ";"
                        + item.TipoPoste + ";"
                        + item.material + ";"
                        + altura + ";"
                        + esforco + ";"
                        + item.tipo_base + ";"
                        + item.para_raio + ";"
                        + item.estai + ";"
                        + "%" + ";"
                        + item.qtde_ramalligacao + ";"
                        + item.qtde_ramalservico + ";"
                        + item.qtd_estai + ";"
                        + item.avaria + ";"
                        + item.Ocupante_s + ";"
                        + item.qtde_ocp + ";"
                        + item.qtde_drop + ";"
                        + item.estai2 + ";"
                        + item.qtde_estai2 + ";"
                        + item.lampsemaforo + ";"
                        + item.tipo_zona
                        );
                }
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "postes-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }


        [HttpGet]
        public HttpResponseMessage CriaRelatorioPosteFotos(string data, string os, string nome)
        {
            int a = 8;
            AlturaPoste aa = (AlturaPoste)a;

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("id_poste;numero_foto;data_foto;");
            var id_os = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == os).FirstOrDefault();
            idOs = id_os.IdOrdemDeServico;
            var PosteRepository = UnitOfWork.PosteRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            //var fotosposte = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste)
            string databd = "";
            foreach (var item in PosteRepository)
            {
                var fotos = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste == item.IdPoste).ToList();

                foreach (var numero in fotos)
                {
                    databd = string.Format("{0:dd/MM/yyyy}", item.DataCadastro);
                    if (data.Equals(databd))
                    {
                        item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();
                        string x = item.X.ToString();
                        //   x = x.Replace(",", ".");
                        string y = item.Y.ToString();
                        // y = y.Replace(",", ".");

                        string data1 = string.Format("{0:dd/MM/yyyy}", numero.DataFoto);

                        AlturaPoste alturaPoste = (AlturaPoste)item.Altura;
                        Esforco esforcoPoste = (Esforco)item.Esforco;

                        string altura = GetAlturaPoste(alturaPoste);
                        string esforco = GetAEsforcoPoste(esforcoPoste);

                        writer.WriteLine(
                            numero.IdPoste 
                            + ";"
                            + numero.NumeroFoto
                            + ";"
                            + data1                    
                            );
                    }                    
                }              
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "fotos-postes-" + nome + "-" + os + "-" + data + ".csv" };
            return result;
        }

        private string GetAlturaPoste( AlturaPoste altura ) {

            string retorno = "";
            switch (altura)
            {
                case AlturaPoste._7:
                    retorno = "7";
                    break;
                case AlturaPoste._8:
                    retorno = "8";
                    break;
                case AlturaPoste._9:
                    retorno = "9";
                    break;
                case AlturaPoste._10:
                    retorno = "10";
                    break;
                case AlturaPoste._10_5:
                    retorno = "10.5";
                    break;
                case AlturaPoste._11:
                    retorno = "11";
                    break;
                case AlturaPoste._12:
                    retorno = "12";
                    break;
                case AlturaPoste._13:
                    retorno = "13";
                    break;
                case AlturaPoste._14:
                    retorno = "14";
                    break;
                case AlturaPoste._15:
                    retorno = "15";
                    break;
                case AlturaPoste._16:
                    retorno = "16";
                    break;
                default:
                    retorno = "0";
                    break;
            }

            return retorno;
        }

        private string GetAEsforcoPoste(Esforco esforco)
        {

            string retorno = "";

            switch (esforco)
            {
                case Esforco._200:
                    retorno = "200";
                    break;
                case Esforco._300:
                    retorno = "200";
                    break;
                case Esforco._400:
                    retorno = "200";
                    break;
                case Esforco._450:
                    retorno = "200";
                    break;
                case Esforco._500:
                    retorno = "200";
                    break;
                case Esforco._600:
                    retorno = "200";
                    break;
                case Esforco._700:
                    retorno = "200";
                    break;
                case Esforco._800:
                    retorno = "200";
                    break;
                case Esforco._900:
                    retorno = "200";
                    break;
                case Esforco._1000:
                    retorno = "200";
                    break;
                case Esforco._1100:
                    retorno = "200";
                    break;
                case Esforco._1200:
                    retorno = "200";
                    break;
                case Esforco._1300:
                    retorno = "200";
                    break;
                default:
                    retorno = "SEM";
                    break;
            }
            return retorno;
        }
    }
    
}
