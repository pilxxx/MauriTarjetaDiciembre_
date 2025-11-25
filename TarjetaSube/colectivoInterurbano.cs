using System;

namespace TarjetaSube
{
    public class ColectivoInterurbano : Colectivo
    {
        public ColectivoInterurbano(string linea) : base(linea)
        {
            valorPasaje = 3000;
        }
    }
}