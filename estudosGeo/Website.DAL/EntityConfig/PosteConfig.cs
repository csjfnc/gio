using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class PosteConfig : EntityTypeConfiguration<Poste>
    {
        public PosteConfig()
        {
            HasKey(p => p.IdPoste).Property(os => os.IdPoste).HasColumnName("id_poste").IsRequired();
            Property(p => p.X).HasColumnName("x").IsRequired();
            Property(p => p.Y).HasColumnName("y").IsRequired();
            Property(p => p.DataCadastro).HasColumnName("data_cadastro");
            Property(p => p.DataExclusao).HasColumnName("data_exclusao");
            Property(p => p.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(p => p.IdCidade).HasColumnName("id_cidade");
            Property(p => p.IdLogradouro).HasColumnName("id_logradouro");
            Property(p => p.Finalizado).HasColumnName("finalizado");
            Property(p => p.DataFinalizado).HasColumnName("data_finalizado");
            Property(p => p.CodigoGeo).HasColumnName("codigo_bd_geo");
            Property(p => p.XAtualizacao).HasColumnName("x_atualizacao"); 
            Property(p => p.YAtualizacao).HasColumnName("y_atualizacao");
            Property(p => p.Status).HasColumnName("status_comparativo");

            Property(p => p.Altura).HasColumnName("altura");
            Property(p => p.TipoPoste).HasColumnName("tipo_poste");
            Property(p => p.Esforco).HasColumnName("esforco");
            Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(500);
            Property(p => p.NumeroPosteNaOS).HasColumnName("numero_poste_na_os");

            Property(p => p.IdPoste_1).HasColumnName("id_poste_1");

            Property(p => p.Ocupante_s).HasColumnName("ocupante_s");       
            Property(p => p.Ocupante_d).HasColumnName("ocupante_d");

            //Dados do BD Geosurvey
            Property(p => p.logradouro).HasColumnName("logradouro").HasMaxLength(255);
            Property(p => p.equipamento1).HasColumnName("equipamento1").HasMaxLength(255);
            Property(p => p.equipamento2).HasColumnName("equipamento2").HasMaxLength(255);
            Property(p => p.equipamento3).HasColumnName("equipamento3").HasMaxLength(255);
            Property(p => p.nomedobloco).HasColumnName("nome_do_bloco").HasMaxLength(255);
            Property(p => p.id_temp).HasColumnName("id_temp");
            Property(p => p.ativo).HasColumnName("ativo");
            Property(p => p.primario).HasColumnName("primario").HasMaxLength(255);
            Property(p => p.proprietario).HasColumnName("proprietario").HasMaxLength(255);
            Property(p => p.quantidade_poste).HasColumnName("qtd_poste");
            Property(p => p.idpostecia).HasColumnName("id_potencia");
            Property(p => p.caracteristica_cia).HasColumnName("caracteristica").HasMaxLength(255);
            Property(p => p.aterropararaio_cia).HasColumnName("aterropararraio").HasMaxLength(255);
            Property(p => p.encontrado).HasColumnName("encontrado").HasMaxLength(255);
            Property(p => p.material).HasColumnName("material").HasMaxLength(255);
            Property(p => p.tipo_base).HasColumnName("tipo_base").HasMaxLength(255);
            Property(p => p.para_raio).HasColumnName("para_raio").HasMaxLength(255);
            Property(p => p.estai).HasColumnName("estai").HasMaxLength(255);
            Property(p => p.qtd_estai).HasColumnName("qtd_estai");
            Property(p => p.qtde_ramalservico).HasColumnName("qtd_ramalservico");
            Property(p => p.qtde_ramalligacao).HasColumnName("qtd_religacao");
            Property(p => p.avaria).HasColumnName("avaria").HasMaxLength(255);
            Property(p => p.qtde_ocp).HasColumnName("qtd_ocupantes");
            Property(p => p.qtde_drop).HasColumnName("qtd_drop");
            Property(p => p.estai2).HasColumnName("estai2");
            Property(p => p.qtde_estai2).HasColumnName("qtd_estai2");
            Property(p => p.lampsemaforo).HasColumnName("lampsemaforo");
            Property(p => p.tipo_zona).HasColumnName("tipo_zona");


            Property(p => p.barramento).HasColumnName("barramento");
            Property(p => p.aterramento).HasColumnName("aterramento");
            Property(p => p.estrutura_primaria).HasColumnName("estrutura_primaria");
            Property(p => p.estrutura_secundaria).HasColumnName("estrutura_secundaria");
            Property(p => p.situacao).HasColumnName("situacao");
            Property(p => p.mufla).HasColumnName("mufla");
            Property(p => p.rede_primaria).HasColumnName("rede_primaria");
            Property(p => p.defeito).HasColumnName("defeito");
            Property(p => p.ano).HasColumnName("ano");





            ToTable("poste");

            /// Relacionamento da Classe
            //HasMany(p => p.IP).WithRequired(ip => ip.Poste).WillCascadeOnDelete(false);
            HasMany(p => p.Transformadores).WithRequired(trf => trf.Poste).WillCascadeOnDelete(false);
            HasMany(p => p.PontoDeEntrega).WithRequired(pe => pe.Poste).WillCascadeOnDelete(false);
        }
    }
}