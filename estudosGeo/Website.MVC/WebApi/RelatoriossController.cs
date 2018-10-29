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
    public class RelatoriossController : ApiController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        public HttpResponseMessage CriaRelatorioStrands(int idOs, string data, string os)
        {           
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,x1,y1,x2,y2,ativo,servico");
            var DemandaStrandRepository = UnitOfWork.DemandaStrandRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            foreach (var item in DemandaStrandRepository)
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
                    item.ID + "," + 
                    x1 + "," + 
                    y1 + "," + 
                    x2 + "," + 
                    y2 + "," + 
                    item.Ativo + "," + 
                    item.OrdemDeServico.NumeroOS);
            }                         
           writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "strands-"+os+"-"+data+".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioAnotacao(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,anotacao,x,y,angulo,ativo,servico");
            var AnotacaoRepository = UnitOfWork.AnotacaoRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            foreach (var item in AnotacaoRepository)
            {
                // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();

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
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "anotacoes-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioEdificio(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("codigo,clasificacao,lagradouro,numero,complemento 1,txtcompl1,complemento 2,txtcompl2,complemento 3,txtcompl3,complemento 4,txtcompl4,observacao,cabeamento,node,x,y,cod_poste,tipo_comercio,numero_de_andares,total_apartamentos,nome_edificio,ativo,ano_levantamento,angulo,servico,lat,longitude,poste_box");
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.EDIFÍCIO_RES, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            foreach (var item in PontoEntregaRepository)
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
                    item.Complemento2+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                     x+","+
                     y+","+
                     item.Poste.IdPoste+","+
                     "%"+","+                     
                     item.NumeroAndaresEdificio+","+
                     item.TotalApartamentosEdificio+","+
                     item.Ativo+","+
                     "%"+","+
                     item.Angulo+","+
                     item.OrdemDeServico.NumeroOS+","+
                     "%"+","+
                     "%"+","+
                     "%");
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "edificios-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioTerreno(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("codigo,clasificacao,numero,tipo,x,y,cod_poste,ativo,lagradouro,angulo,divisao,servico,node,lat,longitude,poste_box");
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.TERRENO, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            foreach (var item in PontoEntregaRepository)
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
                    + item.TipoImovel+","
                    + x +","
                    + y +"," 
                    + item.Poste.IdPoste + "," 
                    + item.Ativo 
                    + ",%,"
                    + item.Angulo + "," 
                    + item.Divisao + "," 
                    + item.OrdemDeServico.NumeroOS+","+
                    "%"+","+
                    "%"+","+
                    "%"+","+
                    item.PosteBox);
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "terreno-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioResid(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.RESIDENCIAL, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            writer.WriteLine("codigo,clasificacao,lagradouro,numero,complemento 1,txtcompl1,complemento 2,txtcompl2,complemento 3,txtcompl3,complemento 4,txtcompl4,observacao,cabeamento,node,x,y,cod_poste,angulo,divisao,qtdedem,ativo,servico,lat,longitude,poste_box");
            foreach (var item in PontoEntregaRepository)
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
                    + item.Numero+","
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
                    + item.Ativo +","
                    + item.OrdemDeServico.NumeroOS + ","
                    + "%" + ","
                    + "%" + ","
                    + item.PosteBox);
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "resid-" + os + "-" + data + ".csv" };
            return result;
        }
        
        [HttpGet]
        public HttpResponseMessage CriaRelatorioCni(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
           
            writer.WriteLine("codigo,clasificacao,lagradouro,numero,complemento 1,txtcompl1,complemento 2,txtcompl2,complemento 3,txtcompl3,complemento 4,txtcompl4,observacao,cabeamento,node,x,y,cod_poste,angulo,divisao,qtdedem,ativo,servico,lat,longitude,poste_box");
            var PontoEntregaRepository = UnitOfWork.PontoEntregaRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null && dem.ClasseSocial == ClasseSocial.COMERCIAL_P, includeProperties: "Poste, Cidade, OrdemDeServico").ToList();
            foreach (var item in PontoEntregaRepository)
            {
                // item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                string x = item.X.ToString();
                x = x.Replace(",", ".");
                string y = item.Y.ToString();
                y = y.Replace(",", ".");

                writer.WriteLine(
                    item.IdPontoEntrega + ","
                    + item.Classificacao + ","
                    + "%"+","
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
                    + item.Ativo+","
                    + item.OrdemDeServico.NumeroOS + ","
                    + "%" + ","
                    + "%" + ","
                    + item.PosteBox);
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "cni-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioTiep(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
                       
            writer.WriteLine("codigo,x1,y1,x2,y2,ativo,dem_rel,layer,lat1,longitude1,lat2,longitude2,servico,node");
            var VaosDemandaPosteRepository = UnitOfWork.VaosDemandaPosteRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico, PontoEntrega").ToList();
            foreach (var item in VaosDemandaPosteRepository)
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
                    + item.Ativo+","
                    + item.IdPontoEntrega + ","
                    + item.PontoEntrega.ClasseSocial + ","
                    + "%" + ","
                    + "%" + ","
                    + "%" + ","
                    + "%" + ","
                    +item.OrdemDeServico.NumeroOS+","
                    +"%");
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "tiep-" + os + "-" + data + ".csv" };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage CriaRelatorioPoste(int idOs, string data, string os)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
           
            writer.WriteLine("codigo,logradouro,equipamento1,equipamento2,equipamento3,aterramento,status,nomedobloco,x,y,id_temp,ativo,primario,servico,lat,longitude,node,proprietario,tecnico,data,municipio,x_original,y_original,status_edicao,cod_geodatabase,id_poste_arcitech,quantidade_poste,idpostecia,caracteristica_cia,aterropararaio_cia,encontrado,tipo_poste,material_poste,altura_poste,esforco_poste,tipo_base,para_raio,estai,observacao,qtde_ramalligacao,qtde_ramalservico,qtde_estai,avaria,ocupantes,qtde_ocp,qtde_drop,estai2,qtde_estai2,lampsemaforo,tipo_zona");
            var PosteRepository = UnitOfWork.PosteRepository.Get(dem => dem.IdOrdemDeServico == idOs && dem.DataExclusao == null, includeProperties: "Cidade, OrdemDeServico").ToList();
            foreach (var item in PosteRepository)
            {
                 item.OrdemDeServico.Usuario = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == item.OrdemDeServico.IdUsuario).FirstOrDefault();       
                string x = item.X.ToString();
                x = x.Replace(",", ".");
                string y = item.Y.ToString();
                y = y.Replace(",", ".");

                string data1 = string.Format("{0:dd/MM/yyyy}", item.data);

                writer.WriteLine(
                    item.IdPoste + ","
                    + "%" + ","                    
                    + item.equipamento1 + ","
                    + item.equipamento2 + ","
                    + item.equipamento3 + ","
                    + "%" + ","      
                    + item.Status+","
                    +item.nomedobloco+","
                    + x + ","
                    + y + ","
                    + "%" + ","
                    + item.ativo + ","
                    + item.primario + ","
                    + item.OrdemDeServico.NumeroOS + ","
                    + "%" + ","
                    + "%" + ","
                    + "%" + ","
                    + "%" + ","
                    + item.OrdemDeServico.Usuario.UserName+","
                    + data1+","
                    + "%" + ","
                    + "%" + ","
                    + "%" + ","
                    + "%" + ","
                    + item.CodigoGeo + ","
                    + "%" + ","                    
                    + item.quantidade_poste + ","
                    + "%" + ","
                    + item.caracteristica_cia + ","
                    + item.aterropararaio_cia + ","
                    + item.encontrado + ","
                    + item.TipoPoste + ","
                    + item.material + ","
                    + item.Altura + ","
                    + item.Esforco + ","
                    + item.tipo_base + ","
                    + item.para_raio + ","
                    + item.estai + ","
                    + "%" + ","
                    + item.qtde_ramalligacao + ","
                    + item.qtde_ramalservico + ","
                    + item.qtd_estai + ","
                    + item.avaria + ","
                    + item.Ocupante_s + ","
                    + item.qtde_ocp + ","
                    + item.qtde_drop + ","
                    + item.estai2 + ","
                    + item.qtde_estai2 + ","
                    + item.lampsemaforo + ","
                    + item.tipo_zona
                    );
            }
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "postes-" + os + "-" + data + ".csv" };
            return result;
        }
    }
}
