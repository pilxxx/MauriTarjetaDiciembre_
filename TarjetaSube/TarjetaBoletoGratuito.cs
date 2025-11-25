using System;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        private int viajesGratuitosHoy;
        private DateTime ultimaFechaViaje;
        private const int MAX_VIAJES_GRATUITOS_DIA = 2;

        public TarjetaBoletoGratuito() : base()
        {
            viajesGratuitosHoy = 0;
            ultimaFechaViaje = DateTime.MinValue;
        }

        public virtual decimal CalcularDescuento(decimal monto)
        {
            DateTime hoy = DateTime.Now.Date;

            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }

            if (viajesGratuitosHoy >= MAX_VIAJES_GRATUITOS_DIA)
            {
                return monto;
            }

            return 0;
        }

        public bool PuedeViajarGratis()
        {
            DateTime hoy = DateTime.Now.Date;

            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }

            return viajesGratuitosHoy < MAX_VIAJES_GRATUITOS_DIA;
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

        public void RegistrarViaje()
        {
            DateTime hoy = DateTime.Now.Date;

            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }

            viajesGratuitosHoy++;
            ultimaFechaViaje = DateTime.Now;
        }

        public void RegistrarViajeGratuito()
        {
            RegistrarViaje();
        }

        public int ObtenerViajesGratuitosHoy()
        {
            DateTime hoy = DateTime.Now.Date;

            if (ultimaFechaViaje.Date != hoy)
            {
                return 0;
            }

            return viajesGratuitosHoy;
        }
    }
}