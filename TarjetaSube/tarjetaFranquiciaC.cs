using System;

namespace TarjetaSube
{
    public class TarjetaFranquiciaCompleta : Tarjeta
    {
        public TarjetaFranquiciaCompleta() : base()
        {
        }

        public override bool PuedeViajar(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            
            if (fecha.Hour < 6 || fecha.Hour >= 22)
            {
                return false;
            }
            
            return true;
        }

        public override decimal CalcularMonto(decimal valorPasaje)
        {
            return 0;
        }

        public override bool DescontarSaldo(decimal monto)
        {
            if (ObtenerSaldoPendiente() > 0)
            {
                AcreditarCarga();
            }
            return true;
        }
    }
}