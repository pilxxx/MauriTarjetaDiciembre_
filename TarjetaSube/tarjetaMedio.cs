using System;

namespace TarjetaSube
{
    public class TarjetaMedioBoleto : Tarjeta
    {
        // Cuántos viajes a mitad de precio hizo hoy
        private int viajesHoy;
        
        // Qué día fue el último viaje
        private DateTime ultimoViaje;
        
        public TarjetaMedioBoleto() : base()
        {
            viajesHoy = 0;
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
            // No hago nada acá, solo devuelvo el precio
            // La lógica está en PagarCon
            if (viajesHoy >= 2)
            {
                return precio;
            }
            
            return precio / 2;
        }
        
        public override Boleto PagarCon(Colectivo colectivo, DateTime cuando)
        {
            // Primero me fijo si puede viajar en este horario
            if (!PuedeViajar(cuando))
            {
                return null;
            }

            string linea = colectivo.ObtenerLinea();
            decimal precio = colectivo.ObtenerValorPasaje();
            
            // Resetear contador si es un día nuevo
            DateTime hoy = cuando.Date;
            if (ultimoViaje.Date != hoy)
            {
                viajesHoy = 0;
            }
            
            // Me fijo si puede hacer trasbordo
            bool trasbordo = PuedeHacerTrasbordo(linea, cuando);
            
            decimal cuantoPaga;
            bool usoDescuento = false;
            
            // El trasbordo es más importante que el medio boleto
            if (trasbordo)
            {
                cuantoPaga = 0;
            }
            else
            {
                // Calcular cuánto paga según los viajes que hizo
                if (viajesHoy >= 2)
                {
                    cuantoPaga = precio; // Paga completo
                }
                else
                {
                    cuantoPaga = precio / 2; // Paga mitad
                    usoDescuento = true;
                }
            }

            // Le cobro
            if (!DescontarSaldo(cuantoPaga))
            {
                return null;
            }

            // Si no es trasbordo, lo registro por si después hace uno
            if (!trasbordo)
            {
                RegistrarViajeParaTrasbordo(linea, cuando);
            }
            
            // Si usó el descuento, cuento el viaje
            if (usoDescuento)
            {
                viajesHoy++;
                ultimoViaje = cuando;
            }

            return new Boleto(linea, cuantoPaga, ObtenerSaldo(), trasbordo, "Medio Boleto", ID);
        }
        
        protected override string ObtenerTipoTarjeta()
        {
            return "Medio Boleto";
        }
    }
}