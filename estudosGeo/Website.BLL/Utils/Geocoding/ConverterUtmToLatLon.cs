using System;
namespace Website.BLL.Utils.Geocoding
{
    public class ConverterUtmToLatLon : BaseConverter
    {
        public ConverterUtmToLatLon(Datum datum, int norteOuSul, int zona) : base(datum, norteOuSul, zona) { }

        /// <summary>
        /// Converte a UTM para Lat/Long - A Coordenada 'Northing' deve ser passada 
        /// para  método com o sétimo digito.
        /// </summary>
        /// <param name="Easting"></param>
        /// <param name="Northing"></param>
        /// <returns></returns>
        public LatLon Convert(double Easting, double Northing)
        {
            //NorteOuSul.S == 1
            if (norteOuSul == 1)
            {
                Northing = 10000000 - Northing;
            }

            //Datum Constants - O valor é pego atravez do datums
            double aux = (b / a);
            double ec = Math.Sqrt(1 - (aux * aux));
            double ec_2 = ec * ec;
            double e1sq = ec_2 / (1 - ec_2);
            double k0 = 0.9996;

            double ec_4 = ec_2 * ec_2;
            double e = 500000 - Easting;

            //Calcular Footprint latitude
            double arc_lenght = Northing / k0;
            double mu = arc_lenght / (a * (1 - ec_2 / 4 - 3 * ec_4 / 64 - 5 * ec_4 * ec_2 / 256));
            double ei = (1 - Math.Sqrt(1 - ec_2)) / (1 + Math.Sqrt(1 - ec_2));
            double ei_2 = ei * ei;
            double ca = 3 * ei / 2 - 27 * ei_2 * ei / 32;
            double cb = 21 * ei_2 / 16 - 55 * ei_2 * ei_2 / 32;
            double ccc = 151 * ei_2 * ei / 96;
            double cd = 1097 * ei_2 * ei_2 / 512;
            //Footprint lat
            double phi1 = mu + ca * Math.Sin(2 * mu) + cb * Math.Sin(4 * mu) + ccc * Math.Sin(6 * mu) + cd * Math.Sin(8 * mu);

            //Constants for Formulas
            double Q0 = e1sq * Math.Cos(phi1) * Math.Cos(phi1);
            double t0 = Math.Tan(phi1) * Math.Tan(phi1);
            double n0 = a / Math.Sqrt(1 - ((ec_2 * Math.Sin(phi1)) * Math.Sin(phi1)));
            double r0 = a * (1 - ec_2) / Math.Pow(1 - (ec_2 * Math.Sin(phi1) * Math.Sin(phi1)), 1.5);
            double dd0 = e / (n0 * k0);

            //Coefficients for Calculating Latitude
            double Fact1 = n0 * Math.Tan(phi1) / r0;
            double Fact2 = dd0 * dd0 / 2;
            double Fact3 = (5 + 3 * t0 + 10 * Q0 - 4 * Q0 * Q0 - 9 * e1sq) * Math.Pow(dd0, 4) / 24;
            double Fact4 = (61 + 90 * t0 + 298 * Q0 + 45 * t0 * t0 - 252 * e1sq - 3 * Q0 * Q0) * Math.Pow(dd0, 6) / 720;

            //Coefficients for Calculating Longitude
            double LoFact1 = dd0;
            double LoFact2 = (1 + 2 * t0 + Q0) * Math.Pow(dd0, 3) / 6;
            double LoFact3 = (5 - 2 * Q0 + 28 * t0 - 3 * Q0 * Q0 + 8 * e1sq + 24 * t0 * t0) * Math.Pow(dd0, 5) / 120;

            //Delta Long
            double DeltaLong = (LoFact1 - LoFact2 + LoFact3) / Math.Cos(phi1);
            double DeltaLongDec = DeltaLong * 180 / Math.PI;

            LatLon result;
            //NorteOuSul.S == 1
            if (norteOuSul == 1)
            {
                result = new LatLon() { Lat = (-180 * (phi1 - Fact1 * (Fact2 + Fact3 + Fact4)) / Math.PI), Lon = (zonaCentLong - DeltaLongDec) };
            }
            else
            {
                result = new LatLon() { Lat = (180 * (phi1 - Fact1 * (Fact2 + Fact3 + Fact4)) / Math.PI), Lon = (zonaCentLong - DeltaLongDec) };
            }

            return result;
        }
    }
}
