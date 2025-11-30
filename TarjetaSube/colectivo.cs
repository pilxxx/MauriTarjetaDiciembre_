using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        protected string numeroLinea;
        protected decimal valorPasaje;

        public Colectivo(string linea)
        {
            numeroLinea = linea;
            valorPasaje = 1580;
        }

        public string ObtenerLinea()
        {
            return numeroLinea;
        }

        public decimal ObtenerValorPasaje()
        {
            return valorPasaje;
        }
    }
}