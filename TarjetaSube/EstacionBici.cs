using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class EstacionBici
    {
        // Lo que cuesta usar la bici por día
        private decimal tarifa = 1777.50m;
        
        // Cuánto te multan si te pasás de tiempo
        private decimal multa = 1000m;
        
        // Tiempo máximo que podés tener la bici (en minutos)
        private int tiempoMaximo = 60;
        
        // Guardo cuándo cada tarjeta retiró la bici
        private Dictionary<int, DateTime> retiros;
        
        // Guardo cuántas multas debe cada tarjeta
        private Dictionary<int, decimal> multas;
        
        // Guardo el último día que usó el servicio cada tarjeta
        private Dictionary<int, DateTime> ultimoDia;
        
        public EstacionBici()
        {
            retiros = new Dictionary<int, DateTime>();
            multas = new Dictionary<int, decimal>();
            ultimoDia = new Dictionary<int, DateTime>();
        }
        
        public bool RetirarBici(Tarjeta tarjeta, DateTime cuando)
        {
            int id = tarjeta.ID;
            
            // Si cambió el día, borro las multas viejas
            if (ultimoDia.ContainsKey(id))
            {
                if (ultimoDia[id].Date != cuando.Date)
                {
                    multas[id] = 0;
                }
            }
            
            // Calculo cuánto tiene que pagar
            decimal aPagar = tarifa;
            
            // Si tiene multas pendientes, se las sumo
            if (multas.ContainsKey(id))
            {
                aPagar = aPagar + multas[id];
            }
            
            // Intento cobrarle
            if (!tarjeta.DescontarSaldo(aPagar))
            {
                return false; // No le alcanza la plata
            }
            
            // Si pudo pagar, registro que retiró la bici
            retiros[id] = cuando;
            ultimoDia[id] = cuando;
            
            // Como pagó, borro las multas
            multas[id] = 0;
            
            return true;
        }
        
        public void DevolverBici(Tarjeta tarjeta, DateTime cuando)
        {
            int id = tarjeta.ID;
            
            // Me fijo si tiene una bici
            if (!retiros.ContainsKey(id))
            {
                return; // No tiene nada para devolver
            }
            
            // Calculo cuánto tiempo tuvo la bici
            DateTime cuandoRetiro = retiros[id];
            TimeSpan tiempoUsado = cuando - cuandoRetiro;
            int minutos = (int)tiempoUsado.TotalMinutes;
            
            // Si se pasó del tiempo, le pongo una multa
            if (minutos > tiempoMaximo)
            {
                if (!multas.ContainsKey(id))
                {
                    multas[id] = 0;
                }
                
                multas[id] = multas[id] + multa;
            }
            
            // Saco la bici del diccionario
            retiros.Remove(id);
        }
        
        public decimal ObtenerMultasPendientes(Tarjeta tarjeta)
        {
            int id = tarjeta.ID;
            
            if (multas.ContainsKey(id))
            {
                return multas[id];
            }
            
            return 0;
        }
    }
}