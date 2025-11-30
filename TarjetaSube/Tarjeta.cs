using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private static int contadorID = 1;
        
        private int id;
        private decimal saldo;
        private decimal saldoPendiente;
        private int viajesDelMes;
        private DateTime ultimoMesRegistrado;
        private DateTime ultimoViajeParaTrasbordo;
        private string ultimaLineaViajada;
        
        private const decimal limiteNegativo = -1200m;
        private const decimal limiteMaximo = 56000m;
        
        private static List<decimal> montosPermitidos = new List<decimal> 
        { 
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 
        };
        
        public int ID
        {
            get { return id; }
        }
        
        public Tarjeta()
        {
            id = contadorID;
            contadorID++;
            
            saldo = 0;
            saldoPendiente = 0;
            viajesDelMes = 0;
            ultimoMesRegistrado = DateTime.Now;
            ultimoViajeParaTrasbordo = DateTime.MinValue;
            ultimaLineaViajada = "";
        }
        
        public decimal ObtenerSaldo()
        {
            return saldo;
        }
        
        public decimal ObtenerSaldoPendiente()
        {
            return saldoPendiente;
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
            
            if (nuevoSaldo > limiteMaximo)
            {
                decimal excedente = nuevoSaldo - limiteMaximo;
                saldo = limiteMaximo;
                saldoPendiente = saldoPendiente + excedente;
                return true;
            }
            
            saldo = nuevoSaldo;
            return true;
        }
        
        public void AcreditarCarga()
        {
            if (saldoPendiente > 0)
            {
                decimal espacioDisponible = limiteMaximo - saldo;
                
                if (espacioDisponible > 0)
                {
                    if (saldoPendiente <= espacioDisponible)
                    {
                        saldo = saldo + saldoPendiente;
                        saldoPendiente = 0;
                    }
                    else
                    {
                        saldo = saldo + espacioDisponible;
                        saldoPendiente = saldoPendiente - espacioDisponible;
                    }
                }
            }
        }
        
        public virtual bool DescontarSaldo(decimal monto)
        {
            decimal saldoResultado = saldo - monto;
            
            if (saldoResultado < limiteNegativo)
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
        
        public virtual bool PuedeViajar(DateTime fecha)
        {
            return true;
        }
        
        public virtual decimal CalcularMonto(decimal valorPasaje)
        {
            return CalcularDescuentoUsoFrecuente(valorPasaje);
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
        
        public void RegistrarViajeParaTrasbordo(string lineaColectivo, DateTime fechaHora)
        {
            ultimaLineaViajada = lineaColectivo;
            ultimoViajeParaTrasbordo = fechaHora;
        }
        
        public bool PuedeHacerTrasbordo(string lineaColectivo, DateTime fechaHora)
        {
            if (ultimaLineaViajada == lineaColectivo)
            {
                return false;
            }
            
            TimeSpan tiempoTranscurrido = fechaHora - ultimoViajeParaTrasbordo;
            if (tiempoTranscurrido.TotalMinutes > 60)
            {
                return false;
            }
            
            if (fechaHora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            
            if (fechaHora.Hour < 7 || fechaHora.Hour >= 22)
            {
                return false;
            }
            
            return true;
        }
        
        public virtual Boleto PagarCon(Colectivo colectivo, DateTime fechaHora)
        {
            if (!PuedeViajar(fechaHora))
            {
                return null;
            }

            string lineaColectivo = colectivo.ObtenerLinea();
            decimal valorPasaje = colectivo.ObtenerValorPasaje();
            
            bool esTrasbordo = PuedeHacerTrasbordo(lineaColectivo, fechaHora);
            
            decimal montoACobrar;
            
            if (esTrasbordo)
            {
                montoACobrar = 0;
            }
            else
            {
                montoACobrar = CalcularMonto(valorPasaje);
            }

            if (!DescontarSaldo(montoACobrar))
            {
                return null;
            }

            if (!esTrasbordo)
            {
                RegistrarViajeParaTrasbordo(lineaColectivo, fechaHora);
            }

            string tipoTarjeta = ObtenerTipoTarjeta();
            return new Boleto(lineaColectivo, montoACobrar, saldo, esTrasbordo, tipoTarjeta, id);
        }
        
        protected virtual string ObtenerTipoTarjeta()
        {
            return "Normal";
        }
    }
}