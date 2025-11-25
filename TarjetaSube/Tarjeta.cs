using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private static int contadorID = 1;
        public int ID { get; private set; }
        
        protected decimal saldo;
        private static readonly List<decimal> montosPermitidos = new List<decimal> {
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
        };
        
        private const decimal LIMITE_NEGATIVO = -1200m;
        private const decimal LIMITE_MAXIMO = 56000m;
        private decimal saldoPendiente;

        // Para boleto de uso frecuente
        private int viajesDelMes;
        private DateTime ultimoMesRegistrado;

        // Para trasbordo
        private DateTime ultimoViajeParaTrasbordo;
        private string ultimaLineaViajada;

        public Tarjeta()
        {
            ID = contadorID++;
            saldo = 0;
            saldoPendiente = 0;
            viajesDelMes = 0;
            ultimoMesRegistrado = DateTime.Now;
            ultimoViajeParaTrasbordo = DateTime.MinValue;
            ultimaLineaViajada = "";
        }

        public virtual void RegistrarViajeParaTrasbordo(string lineaColectivo, DateTime fechaHora)
        {
            ultimaLineaViajada = lineaColectivo;
            ultimoViajeParaTrasbordo = fechaHora;
        }

        public virtual bool PuedeHacerTrasbordo(string lineaColectivo, DateTime fechaHora)
        {
            // No puede ser la misma línea
            if (ultimaLineaViajada == lineaColectivo) return false;

            // Debe ser dentro de 1 hora
            TimeSpan tiempoTranscurrido = fechaHora - ultimoViajeParaTrasbordo;
            if (tiempoTranscurrido.TotalMinutes > 60) return false;

            // Lunes a sábado de 7 a 22
            if (fechaHora.DayOfWeek == DayOfWeek.Sunday) return false;
            if (fechaHora.Hour < 7 || fechaHora.Hour >= 22) return false;

            return true;
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

        public decimal ObtenerSaldoPendiente()
        {
            return saldoPendiente;
        }

        public int ObtenerViajesDelMes()
        {
            DateTime ahora = DateTime.Now;

            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }

            return viajesDelMes;
        }

        public decimal CalcularDescuentoUsoFrecuente(decimal montoBase)
        {
            DateTime ahora = DateTime.Now;

            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }

            viajesDelMes++;

            if (viajesDelMes >= 1 && viajesDelMes <= 29)
            {
                return montoBase;
            }
            else if (viajesDelMes >= 30 && viajesDelMes <= 59)
            {
                return montoBase * 0.80m;
            }
            else if (viajesDelMes >= 60 && viajesDelMes <= 80)
            {
                return montoBase * 0.75m;
            }
            else
            {
                return montoBase;
            }
        }

        public bool CargarSaldo(decimal monto)
        {
            if (!montosPermitidos.Contains(monto))
            {
                return false;
            }

            if (saldoPendiente > 0)
            {
                AcreditarCarga();
            }

            decimal nuevoSaldo = saldo + monto;

            if (nuevoSaldo > LIMITE_MAXIMO)
            {
                decimal excedente = nuevoSaldo - LIMITE_MAXIMO;
                saldo = LIMITE_MAXIMO;
                saldoPendiente += excedente;
                return true;
            }

            saldo = nuevoSaldo;
            return true;
        }

        public void AcreditarCarga()
        {
            if (saldoPendiente > 0)
            {
                decimal espacioDisponible = LIMITE_MAXIMO - saldo;

                if (espacioDisponible > 0)
                {
                    if (saldoPendiente <= espacioDisponible)
                    {
                        saldo += saldoPendiente;
                        saldoPendiente = 0;
                    }
                    else
                    {
                        saldo += espacioDisponible;
                        saldoPendiente -= espacioDisponible;
                    }
                }
            }
        }

        public virtual bool DescontarSaldo(decimal monto)
        {
            decimal saldoResultado = saldo - monto;

            if (saldoResultado < LIMITE_NEGATIVO)
            {
                return false;
            }

            saldo = saldoResultado;

            if (saldoPendiente > 0)
            {
                AcreditarCarga();
            }

            return true;
        }
    }
}