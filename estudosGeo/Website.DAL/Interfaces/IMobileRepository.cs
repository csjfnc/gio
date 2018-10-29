using System.Collections.Generic;
using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IMobileRepository
    {
        /// <summary>
        /// Função que gera um token único para o IMEI do dispositivo Mobile.
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        Mobile GenerateToken(string imei, string idUsuario);

        /// <summary>
        /// Função que valida o Token existente no BD.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        bool ValidateImeiAndToken(string imei, string token);

        /// <summary>
        /// Função que insere um novo dispositivo mobile.
        /// </summary>
        /// <param name="mobile"></param>
        void Insert(Mobile mobile);

        /// <summary>
        /// Função que atualiza as informações do dispositivo mobile
        /// </summary>
        /// <param name="mobile"></param>
        void Update(Mobile mobile);

        /// <summary>
        /// Função que busca no BD o dispositivo cadastrado.
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        Mobile GetByImei(string imei);

        /// <summary>
        /// Função que busca no BD o dispositivo cadastrado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Mobile GetById(long id);

        /// <summary>
        /// Função que verifica se o Imei Existe no BD.
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        bool ExistImei(string imei);

        /// <summary>
        /// Função que busca no BD todos os dispositivos cadastrados.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Mobile> GetAll();

        /// <summary>
        /// Deleta o dispositivo Mobile do BD.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(long id);
    }
}
