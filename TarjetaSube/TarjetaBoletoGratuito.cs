using System;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        // Cuántos viajes gratis hizo hoy
        private int viajesGratisHoy;
        
        // Qué día fue el último viaje
        private DateTime ultimoViaje;
        
        public TarjetaBoletoGratuito() : base()
        {
            viajesGratisHoy = 0;
            ultimoViaje = DateTime.MinValue;
        }
        
        public override bool PuedeViajar(DateTime cuando)
        {
            // No puede viajar fines de semana
            if (cuando.DayOfWeek == DayOfWeek.Saturday || cuando.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            
            // Solo puede viajar de 6 a 22hs
            if (cuando.Hour < 6 || cuando.Hour >= 22)
            {
                return false;
            }
            
            return true;
        }
        
        public override decimal CalcularMonto(decimal precio)
        {
            DateTime hoy = DateTime.Now.Date;
            
            // Si es otro día, reseteo
            if (ultimoViaje.Date != hoy)
            {
                viajesGratisHoy = 0;
            }
            
            // Si todavía no usó 2 viajes gratis, viaja gratis
            if (viajesGratisHoy < 2)
            {
                return 0;
            }
            
            // Si ya usó los 2, paga completo
            return precio;
        }
        
        public override Boleto PagarCon(Colectivo colectivo, DateTime cuando)
        {
            // Me fijo si puede viajar
            if (!PuedeViajar(cuando))
            {
                return null;
            }

            string linea = colectivo.ObtenerLinea();
            decimal precio = colectivo.ObtenerValorPasaje();
            
            // Me fijo si es trasbordo
            bool trasbordo = PuedeHacerTrasbordo(linea, cuando);
            
            decimal cuantoPaga;
            
            if (trasbordo)
            {
                cuantoPaga = 0;
            }
            else
            {
                cuantoPaga = CalcularMonto(precio);
            }

            // Si va gratis y no es trasbordo, registro el viaje
            if (cuantoPaga == 0 && !trasbordo)
            {
                DateTime hoy = cuando.Date;
                
                if (ultimoViaje.Date != hoy)
                {
                    viajesGratisHoy = 0;
                }
                
                viajesGratisHoy++;
                ultimoViaje = cuando;
                
                return new Boleto(linea, 0, ObtenerSaldo(), false, "Boleto Gratuito", ID);
            }

            // Si no, le cobro
            if (!DescontarSaldo(cuantoPaga))
            {
                return null;
            }

            // Registro el viaje para trasbordo
            if (!trasbordo)
            {
                RegistrarViajeParaTrasbordo(linea, cuando);
            }

            // Registro que hizo un viaje (aunque pagó)
            DateTime hoy2 = cuando.Date;
            if (ultimoViaje.Date != hoy2)
            {
                viajesGratisHoy = 0;
            }
            viajesGratisHoy++;
            ultimoViaje = cuando;

            return new Boleto(linea, cuantoPaga, ObtenerSaldo(), trasbordo, "Boleto Gratuito", ID);
        }
        
        public int ObtenerViajesGratuitosHoy()
        {
            DateTime hoy = DateTime.Now.Date;
            
            if (ultimoViaje.Date != hoy)
            {
                return 0;
            }
            
            return viajesGratisHoy;
        }
        
        protected override string ObtenerTipoTarjeta()
        {
            return "Boleto Gratuito";
        }
    }
}