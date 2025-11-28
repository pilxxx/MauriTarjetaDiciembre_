/*
using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TrasbordoTest
    {
        [Test]
        public void Trasbordo_DentroDeUnaHora_LineaDiferente_EsGratis()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo1 = new Colectivo("102");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Lunes 10:00
            DateTime fecha1 = new DateTime(2024, 11, 4, 10, 0, 0);
            Boleto boleto1 = colectivo1.PagarCon(tarjeta, fecha1);
            
            decimal saldoDespuesPrimero = tarjeta.ObtenerSaldo();
            
            // Lunes 10:30 (30 minutos después)
            DateTime fecha2 = new DateTime(2024, 11, 4, 10, 30, 0);
            Boleto boleto2 = colectivo2.PagarCon(tarjeta, fecha2);
            
            decimal saldoDespuesSegundo = tarjeta.ObtenerSaldo();
            
            Assert.IsNotNull(boleto1);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(1580, boleto1.ImportePagado);
            Assert.AreEqual(0, boleto2.ImportePagado); // Trasbordo gratis
            Assert.IsTrue(boleto2.EsTransbordo);
            Assert.AreEqual(saldoDespuesPrimero, saldoDespuesSegundo); // No descuenta en trasbordo
        }
        
        [Test]
        public void Trasbordo_MismaLinea_NoPuedeHacer()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Lunes 10:00
            DateTime fecha1 = new DateTime(2024, 11, 4, 10, 0, 0);
            colectivo.PagarCon(tarjeta, fecha1);
            
            // Lunes 10:30, misma línea
            DateTime fecha2 = new DateTime(2024, 11, 4, 10, 30, 0);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, fecha2);
            
            Assert.IsFalse(boleto2.EsTransbordo); // No es trasbordo, paga normal
            Assert.AreEqual(1580, boleto2.ImportePagado);
        }
        
        [Test]
        public void Trasbordo_MasDeUnaHora_PagaNormal()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo1 = new Colectivo("102");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Lunes 10:00
            DateTime fecha1 = new DateTime(2024, 11, 4, 10, 0, 0);
            colectivo1.PagarCon(tarjeta, fecha1);
            
            // Lunes 11:15 (1 hora 15 minutos después)
            DateTime fecha2 = new DateTime(2024, 11, 4, 11, 15, 0);
            Boleto boleto2 = colectivo2.PagarCon(tarjeta, fecha2);
            
            Assert.IsFalse(boleto2.EsTransbordo);
            Assert.AreEqual(1580, boleto2.ImportePagado); // Paga completo
        }
        
        [Test]
        public void Trasbordo_Domingo_NoPuede()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo1 = new Colectivo("102");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Domingo 10:00
            DateTime fecha1 = new DateTime(2024, 11, 3, 10, 0, 0);
            colectivo1.PagarCon(tarjeta, fecha1);
            
            // Domingo 10:30
            DateTime fecha2 = new DateTime(2024, 11, 3, 10, 30, 0);
            Boleto boleto2 = colectivo2.PagarCon(tarjeta, fecha2);
            
            Assert.IsFalse(boleto2.EsTransbordo); // Domingo no hay trasbordo
            Assert.AreEqual(1580, boleto2.ImportePagado);
        }
        
        [Test]
        public void Trasbordo_Antes7AM_NoPuede()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo1 = new Colectivo("102");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Lunes 6:30
            DateTime fecha1 = new DateTime(2024, 11, 4, 6, 30, 0);
            colectivo1.PagarCon(tarjeta, fecha1);
            
            // Lunes 6:45 (antes de las 7)
            DateTime fecha2 = new DateTime(2024, 11, 4, 6, 45, 0);
            Boleto boleto2 = colectivo2.PagarCon(tarjeta, fecha2);
            
            Assert.IsFalse(boleto2.EsTransbordo);
            Assert.AreEqual(1580, boleto2.ImportePagado);
        }
        
        [Test]
        public void Trasbordo_TodasLasTarjetasPueden()
        {
            // Tarjeta normal
            Tarjeta normal = new Tarjeta();
            normal.CargarSaldo(5000);
            
            // Medio boleto
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            
            Colectivo col1 = new Colectivo("102");
            Colectivo col2 = new Colectivo("K");
            
            DateTime fecha1 = new DateTime(2024, 11, 4, 10, 0, 0); // Lunes 10:00
            DateTime fecha2 = new DateTime(2024, 11, 4, 10, 30, 0); // Lunes 10:30
            
            // Normal
            col1.PagarCon(normal, fecha1);
            Boleto boletoNormal = col2.PagarCon(normal, fecha2);
            Assert.IsTrue(boletoNormal.EsTransbordo);
            
            // Medio boleto
            col1.PagarCon(medio, fecha1);
            Boleto boletoMedio = col2.PagarCon(medio, fecha2);
            Assert.IsTrue(boletoMedio.EsTransbordo);
        }
        
        [Test]
        public void Trasbordo_VariosTrasbordos_DentroDeUnaHora()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(10000);
            
            Colectivo col1 = new Colectivo("102");
            Colectivo col2 = new Colectivo("K");
            Colectivo col3 = new Colectivo("144");
            
            DateTime fecha1 = new DateTime(2024, 11, 4, 10, 0, 0);
            DateTime fecha2 = new DateTime(2024, 11, 4, 10, 20, 0);
            DateTime fecha3 = new DateTime(2024, 11, 4, 10, 40, 0);
            
            Boleto b1 = col1.PagarCon(tarjeta, fecha1);
            Boleto b2 = col2.PagarCon(tarjeta, fecha2);
            Boleto b3 = col3.PagarCon(tarjeta, fecha3);
            
            Assert.AreEqual(1580, b1.ImportePagado);
            Assert.AreEqual(0, b2.ImportePagado);
            Assert.AreEqual(0, b3.ImportePagado);
            Assert.IsTrue(b2.EsTransbordo);
            Assert.IsTrue(b3.EsTransbordo);
        }
    }
}
*/