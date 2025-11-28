/*
using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class LimitacionFranquiciaCompletaTest
    {
        [Test]
        public void BoletoGratuito_NoPermiteMasDeDosViajesGratisPorDia()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Lunes 10:00
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Boleto primerViaje = colectivo.PagarCon(tarjeta, fecha);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta, fecha);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta, fecha);
            
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNotNull(tercerViaje);
            
            Assert.AreEqual(0, primerViaje.ImportePagado);
            Assert.AreEqual(0, segundoViaje.ImportePagado);
            Assert.AreEqual(1580, tercerViaje.ImportePagado); // Tercer viaje cobra completo
            
            Assert.AreEqual(3, tarjeta.ObtenerViajesGratuitosHoy());
        }
        
        [Test]
        public void BoletoGratuito_ViajesPosterioresAlSegundoCobraCompleto()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(10000);
            Colectivo colectivo = new Colectivo("K");
            
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            
            colectivo.PagarCon(tarjeta, fecha); // Viaje 1: gratis
            colectivo.PagarCon(tarjeta, fecha); // Viaje 2: gratis
            
            decimal saldoDespuesDosGratis = tarjeta.ObtenerSaldo();
            
            Boleto tercerViaje = colectivo.PagarCon(tarjeta, fecha); // Viaje 3: paga completo
            Boleto cuartoViaje = colectivo.PagarCon(tarjeta, fecha); // Viaje 4: paga completo
            
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            
            Assert.AreEqual(saldoInicial, saldoDespuesDosGratis); // No descuenta primeros dos
            Assert.AreEqual(1580, tercerViaje.ImportePagado);
            Assert.AreEqual(1580, cuartoViaje.ImportePagado);
            Assert.AreEqual(saldoInicial - 3160, saldoFinal); // Descontó 2 viajes completos
        }
        
        [Test]
        public void BoletoGratuito_PuedeViajarGratisVerificaCorrectamente()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("144");
            
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Assert.IsTrue(tarjeta.PuedeViajarGratis());
            
            colectivo.PagarCon(tarjeta, fecha);
            Assert.IsTrue(tarjeta.PuedeViajarGratis());
            
            colectivo.PagarCon(tarjeta, fecha);
            Assert.IsFalse(tarjeta.PuedeViajarGratis()); // Ya usó los 2 viajes
        }
        
        [Test]
        public void BoletoGratuito_ContadorViajesGratuitosFuncionaCorrectamente()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("133");
            
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Assert.AreEqual(0, tarjeta.ObtenerViajesGratuitosHoy());
            
            colectivo.PagarCon(tarjeta, fecha);
            Assert.AreEqual(1, tarjeta.ObtenerViajesGratuitosHoy());
            
            colectivo.PagarCon(tarjeta, fecha);
            Assert.AreEqual(2, tarjeta.ObtenerViajesGratuitosHoy());
            
            colectivo.PagarCon(tarjeta, fecha);
            Assert.AreEqual(3, tarjeta.ObtenerViajesGratuitosHoy());
        }
        
        [Test]
        public void BoletoGratuito_SinSaldo_TercerViajeNoPermitido()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("27");
            
            DateTime fecha = new DateTime(2024, 11, 4, 10, 0, 0);
            
            Boleto primerViaje = colectivo.PagarCon(tarjeta, fecha);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta, fecha);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta, fecha);
            
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNull(tercerViaje); // Sin saldo, no puede pagar el tercero
        }
    }
}
*/