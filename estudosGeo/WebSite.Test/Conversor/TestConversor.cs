using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.BLL.Utils.Geocoding;

namespace WebSite.Test.Conversor
{
    [TestClass]
    public class TestConversor
    {
        [TestMethod]
        public void Conversor_UTM_To_LatLon()
        {
            LatLon ll = new ConverterUtmToLatLon(Datum.WGS_84, NorteOuSul.S, 23).Convert(613362.38, 7801565.459);
            UTM utm = new ConverterLatLonToUtm(Datum.WGS_84, NorteOuSul.S, 23).Convert(ll.Lat, ll.Lon);
        }
    }
}
