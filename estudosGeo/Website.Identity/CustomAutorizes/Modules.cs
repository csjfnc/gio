using System;

namespace Website.Identity.CustomAutorizes
{
    public enum Modules
    {
       MODULO, USUARIOS, MOBILE, ORDEM_DE_SERVICO, REDE, CONFIGURACAO, ADMINISTRACAO, RELATORIOS
    }

    public static class ExtendsModulesString
    {
        /// <summary>
        /// Retorna a String da Enum.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetString(this Modules module)
        {
            return Enum.GetName(module.GetType(), module);
        }
    }    
}