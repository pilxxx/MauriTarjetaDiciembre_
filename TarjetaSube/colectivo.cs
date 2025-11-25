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

        public virtual bool PuedePagar(Tarjeta tarjeta)
        {
            // Verificar restricciones horarias usando polimorfismo
            if (tarjeta is TarjetaMedioBoleto medio)
            {
                if (!medio.PuedeViajarEnEsteHorario()) return false;
            }
            else if (tarjeta is TarjetaBoletoGratuito gratuito)
            {
                if (!gratuito.PuedeViajarEnEsteHorario()) return false;
            }
            else if (tarjeta is TarjetaFranquiciaCompleta franquicia)
            {
                if (!franquicia.PuedeViajarEnEsteHorario()) return false;
            }

            return true;
        }

        public virtual decimal CalcularMonto(Tarjeta tarjeta)
        {
            // Calcular monto según tipo de tarjeta usando polimorfismo
            if (tarjeta is TarjetaMedioBoleto medio)
            {
                return medio.CalcularDescuento(valorPasaje);
            }
            else if (tarjeta is TarjetaBoletoGratuito gratuito)
            {
                if (gratuito.PuedeViajarGratis())
                {
                    return 0;
                }
                return valorPasaje;
            }
            else if (tarjeta is TarjetaFranquiciaCompleta franquicia)
            {
                return franquicia.CalcularDescuento(valorPasaje);
            }
            else if (tarjeta.GetType() == typeof(Tarjeta))
            {
                return tarjeta.CalcularDescuentoUsoFrecuente(valorPasaje);
            }

            return valorPasaje;
        }

        public Boleto PagarCon(Tarjeta tarjeta, DateTime? fechaHora = null)
        {
            DateTime fecha = fechaHora ?? DateTime.Now;

            // Verificar si puede pagar
            if (!PuedePagar(tarjeta))
            {
                return null;
            }

            // Verificar trasbordo
            bool esTrasbordo = tarjeta.PuedeHacerTrasbordo(numeroLinea, fecha);
            decimal montoACobrar = esTrasbordo ? 0 : CalcularMonto(tarjeta);

            // Registrar viaje para boleto gratuito
            if (tarjeta is TarjetaBoletoGratuito gratuito && montoACobrar == 0 && !esTrasbordo)
            {
                gratuito.RegistrarViajeGratuito();
                return new Boleto(numeroLinea, 0, tarjeta.ObtenerSaldo(), false);
            }

            // Intentar descontar saldo
            if (!tarjeta.DescontarSaldo(montoACobrar))
            {
                return null;
            }

            // Registrar viaje para trasbordo
            if (!esTrasbordo)
            {
                tarjeta.RegistrarViajeParaTrasbordo(numeroLinea, fecha);
            }

            // Registrar viaje para boleto gratuito (si paga)
            if (tarjeta is TarjetaBoletoGratuito g)
            {
                g.RegistrarViaje();
            }

            return new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), esTrasbordo);
        }
    }
}