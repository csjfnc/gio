
using System;
namespace Website.BLL.Utils.Geocoding
{
    public class ConverterLatLonToUtm : BaseConverter
    {
        public ConverterLatLonToUtm(Datum datum, int norteOuSul, int zona) : base(datum, norteOuSul, zona) { }

        public UTM Convert(double Lat, double Lon)
        {
            Lat = Lat * Math.PI / 180D;

            //Datum Constants - O valor é pego atravez do datums            
            double f = ((a - b) / a);
            double m = Math.Sqrt(a * b);
            double k0 = 0.9996;

            double e = Math.Sqrt(1 - (b * b) / (a * a));
            double e2 = e * e / (1 - e * e);

            //Calcular Footprint latitude
            double n = (a - b) / (a + b);

            double rho = a * (1 - e * e) / (Math.Pow(1 - Math.Pow(e * Math.Sin(Lat), 2), 1.5D));
            double rho1 = a * (1 - e * e);
            double rho2 = (Math.Pow(1 - Math.Pow(e * Math.Sin(Lat), 2), 1.5D));
            double rho3 = 1 - Math.Pow(e * Math.Sin(Lat), 2);
            double rho4 = 1 - Math.Pow(e * Math.Sin(Lat), 2);
            double rho5 = Math.Pow(e * Math.Sin(Lat), 2);
            double nu = a / (Math.Sqrt(1 - Math.Pow(e * Math.Sin(Lat), 2)));
            double n2 = n * n;
            double n4 = n2 * n2;

            double A0 = a * (1 - n + (5 * n2 / 4D) * (1 - n) + (81 * n4 / 64D) * (1 - n));
            double B0 = (3 * a * n / 2D) * (1 - n - (7 * n2 / 8D) * (1 - n) + 55 * n4 / 64D);
            double C0 = (15 * a * n2 / 16D) * (1 - n + (3 * n2 / 4D) * (1 - n));
            double D0 = (35 * a * n * n2 / 48D) * (1 - n + 11 * n2 / 16D);
            double E0 = (315 * a * n4 / 51D) * (1 - n);

            double S = A0 * Lat - B0 * Math.Sin(2 * Lat) + C0 * Math.Sin(4 * Lat) - D0 * Math.Sin(6 * Lat) + E0 * Math.Sin(8 * Lat);
            double ZoneCM = 6 * zona - 183;
            double p = ((Lon - ZoneCM) * Math.PI) / 180D;
            double p2 = p * p;
            double sin_lat = Math.Sin(Lat);
            double cos_lat = Math.Cos(Lat);
            double cos_lat2 = cos_lat * cos_lat;
            double tan_lat = Math.Tan(Lat);
            double tan_lat2 = tan_lat * tan_lat;

            double Ki = S * k0;
            double Kii = nu * sin_lat * cos_lat * k0 / 2;
            double Kiii = ((nu * sin_lat * cos_lat * cos_lat2) / 24D) * (5 - tan_lat * tan_lat + 9 * e2 * cos_lat2 + 4 * e2 * e2 * cos_lat2 * cos_lat2) * k0;
            double Kiv = nu * cos_lat * k0;
            double Kv = cos_lat * cos_lat2 * (nu / 6) * (1 - tan_lat2 + e2 * cos_lat2) * k0;

            UTM result = new UTM() { X = ((float)(500000 + (Kiv * p + Kv * p * p2))), Y = ((float)(Ki + Kii * p2 + Kiii * p2 * p2)) };
            //NorteOuSul.S == 1
            if (norteOuSul == 1)
                result.Y += 10000000F;

            return result;
        }
    }
}
