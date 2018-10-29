using System;

namespace Website.BLL.Utils.Geocoding
{
    public class BaseConverter
    {
        protected Datum datum;
        protected int norteOuSul;
        protected int zona, zonaCentLong;
        protected readonly double a, b, coeff = 1000000.0;

        public BaseConverter(Datum _datum, int _norteSul, int _zona)
        {
            //Setup Converter
            this.datum = _datum;
            this.norteOuSul = _norteSul;
            this.zona = _zona;
            this.zonaCentLong = 6 * this.zona - 183;

            //Set a and b by Datum value
            switch (_datum)
            {
                case Datum.WGS_84:
                    a = 6378137.0D;
                    b = 6356752.314245D;
                    break;
                default:
                    a = 6378137.0D;
                    b = 6356752.314245D;
                    break;
            }
        }
    }
}
