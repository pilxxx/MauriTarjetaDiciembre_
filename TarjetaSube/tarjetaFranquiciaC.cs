using System;

namespace TarjetaSube
{
    public class TarjetaFranquiciaCompleta : Tarjeta
    {
        public TarjetaFranquiciaCompleta() : base()
        {
        }

        public virtual decimal CalcularDescuento(decimal monto)
        {
            return 0;
        }

        public virtual bool PuedeViajarEnEsteHorario()
        {
            DateTime ahora = DateTime.Now;
            
            if (ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            
            if (ahora.Hour < 6 || ahora.Hour >= 22)
            {
                return false;
            }
            
            return true;
        }

        // Sobrecarga para tests con fecha simulada
        public virtual bool PuedeViajarEnEsteHorario(DateTime fecha)
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