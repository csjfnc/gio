using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.MVC.Models.Util
{
    public class EnumConvert
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<T>().Select(
                e => new SelectListItem() { Text = Enum.GetName(typeof(T), e), Value = (Convert.ToInt32(e)).ToString() })).ToList();
        }
    }
}