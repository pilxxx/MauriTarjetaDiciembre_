using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class EstacionBici
    {
        // lo que cuesta usar la bici por día
        private decimal tarifa = 1777.50m;
        
        // cuánto te multan si te pasás de tiempo
        private decimal multa = 1000m;
        
        // tiempo máximo que podés tener la bici (en minutos)
        private int tiempoMaximo = 60;
        
        // gardo cuándo cada tarjeta retiró la bici
        private Dictionary<int, DateTime> retiros;
        
        // Guardo cuántas multas debe cada tarjeta
        private Dictionary<int, decimal> multas;
        
        // guardo el último día que usó el servicio cada tarjeta
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
            
            // si cambió el día, borro las multas viejas
            if (ultimoDia.ContainsKey(id))
            {
                if (ultimoDia[id].Date != cuando.Date)
                {
                    multas[id] = 0;
                }
            }
            
            // calculo cuánto tiene que pagar
            decimal aPagar = tarifa;
            
            // si tiene multas pendientes, se las sumo
            if (multas.ContainsKey(id))
            {
                aPagar = aPagar + multas[id];
            }
            
            // intento cobrarle
            if (!tarjeta.DescontarSaldo(aPagar))
            {
                return false; // no le alcanza la plata xd
            }
            
            // si pudo pagar, registro que retiró la bici
            retiros[id] = cuando;
            ultimoDia[id] = cuando;
            
            // como pagó, borro las multas
            multas[id] = 0;
            
            return true;
        }
        
        public void DevolverBici(Tarjeta tarjeta, DateTime cuando)
        {
            int id = tarjeta.ID;
            
            // me fijo si tiene una bici
            if (!retiros.ContainsKey(id))
            {
                return; // No tiene nada para devolver
            }
            
            // calculo cuánto tiempo tuvo la bici
            DateTime cuandoRetiro = retiros[id];
            TimeSpan tiempoUsado = cuando - cuandoRetiro;
            int minutos = (int)tiempoUsado.TotalMinutes;
            
            // si se pasó del tiempo, le pongo una multa
            if (minutos > tiempoMaximo)
            {
                if (!multas.ContainsKey(id))
                {
                    multas[id] = 0;
                }
                
                multas[id] = multas[id] + multa;
            }
            
            // saco la bici del diccionario
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