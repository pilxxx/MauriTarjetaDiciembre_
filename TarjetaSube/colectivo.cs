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

        public Boleto PagarCon(Tarjeta tarjeta, DateTime fechaHora)
        {
            // Verificar si puede viajar en este horario
            if (!tarjeta.PuedeViajar(fechaHora))
            {
                return null;
            }

            // Verificar trasbordo
            bool esTrasbordo = tarjeta.PuedeHacerTrasbordo(numeroLinea, fechaHora);
            
            decimal montoACobrar;
            
            if (esTrasbordo)
            {
                montoACobrar = 0;
            }
            else
            {
                montoACobrar = tarjeta.CalcularMonto(valorPasaje);
            }

            // Registrar viaje para boleto gratuito si es gratis y no es trasbordo
            if (tarjeta is TarjetaBoletoGratuito gratuito && montoACobrar == 0 && !esTrasbordo)
            {
                gratuito.RegistrarViaje();
                string tipo = ObtenerTipoTarjeta(tarjeta);
                return new Boleto(numeroLinea, 0, tarjeta.ObtenerSaldo(), false, tipo, tarjeta.ID);
            }

            // Intentar descontar el saldo
            if (!tarjeta.DescontarSaldo(montoACobrar))
            {
                return null;
            }

            // Registrar el viaje para trasbordo
            if (!esTrasbordo)
            {
                tarjeta.RegistrarViajeParaTrasbordo(numeroLinea, fechaHora);
            }

            // Registrar viaje para tarjetas de boleto gratuito que pagaron
            if (tarjeta is TarjetaBoletoGratuito g)
            {
                g.RegistrarViaje();
            }

            string tipoTarjeta = ObtenerTipoTarjeta(tarjeta);
            return new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), esTrasbordo, tipoTarjeta, tarjeta.ID);
        }

        private string ObtenerTipoTarjeta(Tarjeta tarjeta)
        {
            if (tarjeta is TarjetaMedioBoleto)
            {
                return "Medio Boleto";
            }
            else if (tarjeta is TarjetaBoletoGratuito)
            {
                return "Boleto Gratuito";
            }
            else if (tarjeta is TarjetaFranquiciaCompleta)
            {
                return "Franquicia Completa";
            }
            else
            {
                return "Normal";
            }
        }
    }
}