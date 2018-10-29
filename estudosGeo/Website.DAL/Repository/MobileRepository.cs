using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Website.BLL.Entities;
using Website.BLL.Utils.Security;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class MobileRepository : IMobileRepository
    {
        private MobileContext Context;
        private DbSet<Mobile> DbSet;

        public MobileRepository(MobileContext context)
        {
            Context = context;
            DbSet = context.Set<Mobile>();
        }

        public Mobile GenerateToken(string imei, string idUsuario)
        {
            if (string.IsNullOrEmpty(imei))
                throw new ArgumentNullException("O parâmetro imei passado está inválido.");

            Mobile mobile = DbSet.FirstOrDefault(t => t.Imei == imei);

            if (mobile == null)
                throw new Exception("MobileRepository.GenerateToken - Não foi encontrado o IMEI cadastrado na Base de dados.");

            try
            {
                mobile.AuthToken = AESCrypt.Encrypt(idUsuario + ":" + Guid.NewGuid().ToString());
                mobile.IssuedOn = DateTime.Now;
                mobile.ExpiresOn = DateTime.Now.AddMonths(1);

                //Criando o token
                DbSet.Attach(mobile);
                Context.Entry(mobile).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("MobileRepository.GenerateToken - Erro ao gerar o Token para o IMEI: " + imei, ex);
            }

            return mobile;
        }

        public void Insert(Mobile mobile)
        {
            DbSet.Add(mobile);
            Context.SaveChanges();
        }

        public void Update(Mobile mobile)
        {
            DbSet.Attach(mobile);
            Context.Entry(mobile).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public Mobile GetByImei(string imei)
        {
            return DbSet.FirstOrDefault(t => t.Imei == imei && !t.IsBlock);
        }

        public Mobile GetById(long id)
        {
            return DbSet.FirstOrDefault(t => t.Id == id);
        }

        public bool ExistImei(string imei)
        {
            return DbSet.FirstOrDefault(t => t.Imei == imei && !t.IsBlock) != null;
        }

        public IEnumerable<Mobile> GetAll()
        {
            IQueryable<Mobile> query = DbSet;
            return query.ToList();
        }

        public void Delete(long id)
        {
            Mobile mobile = DbSet.FirstOrDefault(t => t.Id == id);
            if (mobile == null) return;

            DbSet.Remove(mobile);
            Context.SaveChanges();
        }

        public bool ValidateImeiAndToken(string imei, string token)
        {
            Mobile mobile = DbSet.FirstOrDefault(t => t.Imei == imei && !t.IsBlock && t.AuthToken == token);
            return (mobile != null) ? true : false;
        }        
    }
}