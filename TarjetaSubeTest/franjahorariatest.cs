using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class FranjaHorariaTest
    {
        [Test]
        public void MedioBoleto_PuedeViajarLunesAViernes6a22()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Lunes 10:00 (debería poder)
            DateTime fechaValida = new DateTime(2024, 11, 4, 10, 0, 0);
            Boleto boleto = colectivo.PagarCon(medio, fechaValida);
            Assert.IsNotNull(boleto);
        }
        
        [Test]
        public void MedioBoleto_NoPuedeViajarSabado()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Sábado 10:00 (no debería poder)
            DateTime fechaSabado = new DateTime(2024, 11, 2, 10, 0, 0);
            Boleto boleto = colectivo.PagarCon(medio, fechaSabado);
            Assert.IsNull(boleto);
        }
        
        [Test]
        public void MedioBoleto_NoPuedeViajarDomingo()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Domingo 10:00 (no debería poder)
            DateTime fechaDomingo = new DateTime(2024, 11, 3, 10, 0, 0);
            Boleto boleto = colectivo.PagarCon(medio, fechaDomingo);
            Assert.IsNull(boleto);
        }
        
        [Test]
        public void MedioBoleto_NoPuedeViajarAntes de6AM()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Lunes 5:00 (no debería poder)
            DateTime fechaTemprana = new DateTime(2024, 11, 4, 5, 0, 0);
            Boleto boleto = colectivo.PagarCon(medio, fechaTemprana);
            Assert.IsNull(boleto);
        }
        
        [Test]
        public void MedioBoleto_NoPuedeViajarDespuesDe22()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Lunes 22:00 (no debería poder)
            DateTime fechaTarde = new DateTime(2024, 11, 4, 22, 0, 0);
            Boleto boleto = colectivo.PagarCon(medio, fechaTarde);
            Assert.IsNull(boleto);
        }
        
        [Test]
        public void BoletoGratuito_RespetaFranjaHoraria()
        {
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("K");
            
            // Lunes 10:00 (debería poder)
            DateTime fechaValida = new DateTime(2024, 11, 4, 10, 0, 0);
            Boleto boletoValido = colectivo.PagarCon(gratuito, fechaValida);
            Assert.IsNotNull(boletoValido);
            
            // Domingo 10:00 (no debería poder)
            DateTime fechaDomingo = new DateTime(2024, 11, 3, 10, 0, 0);
            Boleto boletoInvalido = colectivo.PagarCon(gratuito, fechaDomingo);
            Assert.IsNull(boletoInvalido);
        }
        
        [Test]
        public void FranquiciaCompleta_RespetaFranjaHoraria()
        {
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();
            Colectivo colectivo = new Colectivo("144");
            
            // Viernes 15:00 (debería poder)
            DateTime fechaValida = new DateTime(2024, 11, 8, 15, 0, 0);
            Boleto boletoValido = colectivo.PagarCon(franquicia, fechaValida);
            Assert.IsNotNull(boletoValido);
            
            // Sábado 15:00 (no debería poder)
            DateTime fechaSabado = new DateTime(2024, 11, 2, 15, 0, 0);
            Boleto boletoInvalido = colectivo.PagarCon(franquicia, fechaSabado);
            Assert.IsNull(boletoInvalido);
        }
        
        [Test]
        public void TarjetaNormal_NoTieneRestriccionHoraria()
        {
            Tarjeta normal = new Tarjeta();
            normal.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("27");
            
            // Domingo 23:00 (debería poder, es tarjeta normal)
            DateTime fechaDomingo = new DateTime(2024, 11, 3, 23, 0, 0);
            Boleto boleto = colectivo.PagarCon(normal, fechaDomingo);
            Assert.IsNotNull(boleto);
        }
    }
}