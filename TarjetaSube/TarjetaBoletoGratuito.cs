using System;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        private int viajesGratuitosHoy;
        private DateTime ultimaFechaViaje;
        private const int maxViajesGratuitosDia = 2;
        
        public TarjetaBoletoGratuito() : base()
        {
            viajesGratuitosHoy = 0;
            ultimaFechaViaje = DateTime.MinValue;
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
            DateTime hoy = DateTime.Now.Date;
            
            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }
            
            if (viajesGratuitosHoy < maxViajesGratuitosDia)
            {
                return 0;
            }
            
            return valorPasaje;
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