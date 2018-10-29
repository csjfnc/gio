using System;
using System.Collections.Generic;
using System.Linq;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.BLL.Utils.Validacoes
{
    public static class ExtensionPolygon
    {
        public static bool ValidatePolygon(this ICollection<PoligonoOS> list)
        {
            List<UTM> pontos = new List<UTM>();
            foreach (PoligonoOS item in list)
            {
                pontos.Add(new UTM() { X = item.X1, Y = item.Y1 });
                pontos.Add(new UTM() { X = item.X2, Y = item.Y2 });
            }

            return pontos.GroupBy(p => new { p.X, p.Y }).Any(g => g.Count() != 2);
        }

        public static List<PoligonoOS> OrderPolygon(this ICollection<PoligonoOS> list)
        {
            List<PoligonoOS> ordered = list.ToList();
            int current = 0;

            while (list.Count != current)
            {
                ordered[current].Ordem = current;
                for (int i = (current + 1); i < ordered.Count; i++)
                {
                    // Check if is next line
                    if ((ordered[current].X2 == ordered[i].X1) && (ordered[current].Y2 == ordered[i].Y1))
                    {
                        ordered.MoveElement(i, current + 1);
                        break;
                    }
                }
                current++; // Add interator
            }

            return ordered;
        }

        private static void MoveElement<PoligonoOS>(this List<PoligonoOS> list, int fromIndex, int toIndex)
        {
            if (!(fromIndex > 0 && fromIndex <= list.Count))
            {
                throw new ArgumentException("From index is invalid");
            }
            if (!(toIndex > 0 && toIndex <= list.Count))
            {
                throw new ArgumentException("To index is invalid");
            }

            if (fromIndex == toIndex) return;

            PoligonoOS element = list[fromIndex];

            if (fromIndex > toIndex)
            {
                list.RemoveAt(fromIndex);
                list.Insert(toIndex, element);
            }
            else
            {
                list.Insert(toIndex + 1, element);
                list.RemoveAt(fromIndex);
            }
        }
    }
}