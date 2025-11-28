/*
using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class FranquiciaTest
    {
        [Test]
        public void FranquiciaCompletaSiemprePuedePagar()
        {
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();
            Colectivo colectivo = new Colectivo("102");
            
            // Simular lunes 10:00
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0); // Lunes
            
            Boleto primerViaje = colectivo.PagarCon(tarjeta, fecha);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta, fecha);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta, fecha);
            
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNotNull(tercerViaje);
            
            decimal saldo = tarjeta.ObtenerSaldo();
            Assert.AreEqual(0, saldo);
        }
        
        [Test]
        public void MedioBoletoCobraMitadDelPasaje()
        {
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(5000);
            
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            
            Colectivo colectivo = new Colectivo("144");
            
            // Simular lunes 10:00
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Boleto boleto = colectivo.PagarCon(tarjeta, fecha);
            
            Assert.IsNotNull(boleto);
            
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            decimal montoDescontado = saldoInicial - saldoFinal;
            
            decimal tarifaNormal = 1580;
            decimal mitadTarifa = tarifaNormal / 2;
            
            Assert.AreEqual(mitadTarifa, montoDescontado);
            Assert.AreEqual(790, boleto.ImportePagado);
        }
        
        [Test]
        public void MedioBoleto_DosPrimerosViajesMitad_TerceroCompleto()
        {
            // CORRECCIÓN: Este test verifica que los dos primeros viajes del día
            // cobran mitad, pero el tercero cobra completo
            
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(10000);
            
            Colectivo colectivo = new Colectivo("K");
            
            // Simular lunes 10:00
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            decimal saldo1 = tarjeta.ObtenerSaldo();
            Boleto boleto1 = colectivo.PagarCon(tarjeta, fecha);
            decimal saldo2 = tarjeta.ObtenerSaldo();
            
            Boleto boleto2 = colectivo.PagarCon(tarjeta, fecha);
            decimal saldo3 = tarjeta.ObtenerSaldo();
            
            Boleto boleto3 = colectivo.PagarCon(tarjeta, fecha);
            decimal saldo4 = tarjeta.ObtenerSaldo();
            
            // Primeros dos viajes: mitad (790)
            Assert.AreEqual(790, boleto1.ImportePagado);
            Assert.AreEqual(790, boleto2.ImportePagado);
            
            // Tercer viaje: completo (1580)
            Assert.AreEqual(1580, boleto3.ImportePagado);
            
            // Verificar descuentos
            Assert.AreEqual(790, saldo1 - saldo2); // Primer viaje
            Assert.AreEqual(790, saldo2 - saldo3); // Segundo viaje
            Assert.AreEqual(1580, saldo3 - saldo4); // Tercer viaje
        }
        
        [Test]
        public void BoletoGratuitoNoCubraSaldo()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            
            Colectivo colectivo = new Colectivo("102");
            
            // Simular lunes 10:00
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Boleto boleto = colectivo.PagarCon(tarjeta, fecha);
            
            Assert.IsNotNull(boleto);
            
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            Assert.AreEqual(saldoInicial, saldoFinal);
            Assert.AreEqual(0, boleto.ImportePagado);
        }
        
        [Test]
        public void FranquiciaCompletaNoCubraSaldo()
        {
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();
            
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            
            Colectivo colectivo = new Colectivo("144");
            
            // Simular lunes 10:00
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Boleto boleto = colectivo.PagarCon(tarjeta, fecha);
            
            Assert.IsNotNull(boleto);
            
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            Assert.AreEqual(saldoInicial, saldoFinal);
            Assert.AreEqual(0, boleto.ImportePagado);
        }
    }
}
*/