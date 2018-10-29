using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class PontoEntregaConfig : EntityTypeConfiguration<PontoEntrega>
    {
        public PontoEntregaConfig()
        {
            HasKey(cs => cs.IdPontoEntrega).Property(cs => cs.IdPontoEntrega).HasColumnName("id_ponto_de_entrega").IsRequired();
            Property(cs => cs.IdPoste).HasColumnName("id_poste").IsRequired();
            Property(cs => cs.IdLagradouro).HasColumnName("id_logradouro");
            Property(cs => cs.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(cs => cs.CodigoGeoBD).HasColumnName("codigo_bd_geo");
            Property(cs => cs.X).HasColumnName("x");
            Property(cs => cs.Y).HasColumnName("y");
            Property(cs => cs.X_atualizacao).HasColumnName("lat");
            Property(cs => cs.Y_atualizacao).HasColumnName("lon");
            Property(cs => cs.DataExclusao).HasColumnName("data_exclusao");
            Property(cs => cs.DataInclusao).HasColumnName("data_inclusao");
            Property(cs => cs.Classificacao).HasColumnName("classificacao");
            Property(cs => cs.Complemento1).HasColumnName("complemento1");
            Property(cs => cs.Complemento2).HasColumnName("complemento2");
            Property(cs => cs.TipoImovel).HasColumnName("tipo_imovel");
            Property(cs => cs.NumeroAndaresEdificio).HasColumnName("numero_andares_edificio");
            Property(cs => cs.TotalApartamentosEdificio).HasColumnName("total_apartamentos_edificio");
            Property(cs => cs.NomeEdificio).HasColumnName("nome_edificio");
            //Property(cs => cs.ClasseAtendimento).HasColumnName("classe_atendimento");
            Property(cs => cs.Numero).HasColumnName("numero_logradouro").HasMaxLength(10);
            Property(cs => cs.ClasseSocial).HasColumnName("classe_social");
            Property(cs => cs.IdCidade).HasColumnName("id_cidade");
            Property(cs => cs.Ativo).HasColumnName("ativo");
            Property(cs => cs.AnoLevantamentoEdificio).HasColumnName("ano_levantamento_edificio");
            Property(cs => cs.Angulo).HasColumnName("angulo");
            Property(cs => cs.Divisao).HasColumnName("divisao");
            Property(cs => cs.PosteBox).HasColumnName("poste_box");
            Property(cs => cs.QtdBlocos).HasColumnName("qtd_blocos");
            Property(cs => cs.QtdSalas).HasColumnName("qtd_salas");
            Property(cs => cs.QtdDomicilio).HasColumnName("qtd_domicilio");
            Property(cs => cs.TipoComercio).HasColumnName("tipo_comercio");
            Property(cs => cs.Ocorrencia).HasColumnName("ocorrencia");
            //Property(cs => cs.Excluido).HasColumnName("excluido");

            //Property(cs => cs.TipoConstrucao).HasColumnName("tipo_construcao");
            // Property(cs => cs.Logradouro).HasColumnName("logradouro").HasMaxLength(1000);
            // Property(cs => cs.Medidor).HasColumnName("medidor").HasMaxLength(45);
            // Property(cs => cs.Fase).HasColumnName("fase").HasMaxLength(10);
            // Property(cs => cs.EtLigacao).HasColumnName("et_ligacao").HasMaxLength(45);
            // Property(cs => cs.Observacao).HasColumnName("observacao").HasMaxLength(500);


            //Property(cs => cs.Status).HasColumnName("status");

            ToTable("ponto_de_entrega");

            // Relacionamento da Classe
            // HasMany(cs => cs.Medidor).WithRequired(p => p.PontoEntrega).WillCascadeOnDelete(false);
        }
    }
}
