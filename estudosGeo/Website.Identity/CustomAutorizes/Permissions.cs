using System;

namespace Website.Identity.CustomAutorizes
{
    public enum Permissions
    {
        PERMISSAO, ADICIONAR, REMOVER, ATUALIZAR, CONSULTAR        
    }

    public static class ExtendsPermissionsString
    {
        /// <summary>
        /// Retorna a String da Enum.
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static string GetString(this Permissions permissions)
        {
            return Enum.GetName(permissions.GetType(), permissions);
        }
    }
}