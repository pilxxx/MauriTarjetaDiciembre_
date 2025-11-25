using System;

namespace TarjetaSube
{
    public class TarjetaMedioBoleto : Tarjeta
    {
        public TarjetaMedioBoleto() : base()
        {
        }

        public virtual decimal CalcularDescuento(decimal monto)
        {
            return monto / 2;
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

        // Método para tests con fecha simulada
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
    }
}