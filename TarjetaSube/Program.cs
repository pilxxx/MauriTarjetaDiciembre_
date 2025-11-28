using System;

namespace TarjetaSube
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("   SISTEMA DE PRUEBA - TARJETA SUBE");
            Console.WriteLine("==============================================\n");

            // TEST 1: Tarjeta Normal
            Console.WriteLine("### TEST 1: TARJETA NORMAL ###");
            Tarjeta tarjetaNormal = new Tarjeta();
            tarjetaNormal.CargarSaldo(10000);
            Console.WriteLine("Saldo inicial: $" + tarjetaNormal.ObtenerSaldo());
            
            Colectivo colectivo152 = new Colectivo("152");
            DateTime fecha1 = new DateTime(2025, 11, 27, 10, 30, 0); // Miércoles 10:30
            
            Boleto boleto1 = colectivo152.PagarCon(tarjetaNormal, fecha1);
            if (boleto1 != null)
            {
                Console.WriteLine("✓ Viaje 1 exitoso");
                Console.WriteLine("  Pagó: $" + boleto1.ImportePagado);
                Console.WriteLine("  Saldo restante: $" + boleto1.SaldoRestante);
            }
            Console.WriteLine();

            // TEST 2: Trasbordo
            Console.WriteLine("### TEST 2: TRASBORDO ###");
            Colectivo colectivo60 = new Colectivo("60");
            DateTime fecha2 = new DateTime(2025, 11, 27, 10, 50, 0); // 20 minutos después
            
            Boleto boleto2 = colectivo60.PagarCon(tarjetaNormal, fecha2);
            if (boleto2 != null)
            {
                Console.WriteLine("✓ Viaje 2 exitoso");
                Console.WriteLine("  Es trasbordo: " + boleto2.EsTransbordo);
                Console.WriteLine("  Pagó: $" + boleto2.ImportePagado);
                Console.WriteLine("  Saldo restante: $" + boleto2.SaldoRestante);
            }
            Console.WriteLine();

            // TEST 3: Medio Boleto
            Console.WriteLine("### TEST 3: MEDIO BOLETO ###");
            TarjetaMedioBoleto medioB = new TarjetaMedioBoleto();
            medioB.CargarSaldo(5000);
            Console.WriteLine("Saldo inicial: $" + medioB.ObtenerSaldo());
            
            DateTime fecha3 = new DateTime(2025, 11, 27, 14, 0, 0); // Miércoles 14:00
            Boleto boleto3 = colectivo152.PagarCon(medioB, fecha3);
            if (boleto3 != null)
            {
                Console.WriteLine("✓ Viaje exitoso");
                Console.WriteLine("  Tipo: " + boleto3.TipoTarjeta);
                Console.WriteLine("  Pagó: $" + boleto3.ImportePagado + " (mitad de $1580)");
                Console.WriteLine("  Saldo restante: $" + boleto3.SaldoRestante);
            }
            
            // Probar medio boleto fuera de horario
            DateTime fecha3b = new DateTime(2025, 11, 30, 14, 0, 0); // Domingo
            Boleto boleto3b = colectivo152.PagarCon(medioB, fecha3b);
            if (boleto3b == null)
            {
                Console.WriteLine("✓ Correctamente rechazado en domingo");
            }
            Console.WriteLine();

            // TEST 4: Boleto Gratuito
            Console.WriteLine("### TEST 4: BOLETO GRATUITO ###");
            TarjetaBoletoGratuito gratuita = new TarjetaBoletoGratuito();
            gratuita.CargarSaldo(5000);
            Console.WriteLine("Saldo inicial: $" + gratuita.ObtenerSaldo());
            
            DateTime fecha4 = new DateTime(2025, 11, 27, 8, 0, 0); // Miércoles 8:00
            
            // Primer viaje gratis
            Boleto boleto4a = colectivo152.PagarCon(gratuita, fecha4);
            if (boleto4a != null)
            {
                Console.WriteLine("✓ Viaje 1 exitoso");
                Console.WriteLine("  Pagó: $" + boleto4a.ImportePagado + " (gratis 1/2)");
                Console.WriteLine("  Viajes gratis hoy: " + gratuita.ObtenerViajesGratuitosHoy());
            }
            
            // Segundo viaje gratis
            Boleto boleto4b = colectivo152.PagarCon(gratuita, fecha4.AddMinutes(30));
            if (boleto4b != null)
            {
                Console.WriteLine("✓ Viaje 2 exitoso");
                Console.WriteLine("  Pagó: $" + boleto4b.ImportePagado + " (gratis 2/2)");
                Console.WriteLine("  Viajes gratis hoy: " + gratuita.ObtenerViajesGratuitosHoy());
            }
            
            // Tercer viaje (debe pagar)
            Boleto boleto4c = colectivo152.PagarCon(gratuita, fecha4.AddMinutes(60));
            if (boleto4c != null)
            {
                Console.WriteLine("✓ Viaje 3 exitoso");
                Console.WriteLine("  Pagó: $" + boleto4c.ImportePagado + " (se acabaron los gratis)");
                Console.WriteLine("  Saldo restante: $" + boleto4c.SaldoRestante);
            }
            Console.WriteLine();

            // TEST 5: Franquicia Completa
            Console.WriteLine("### TEST 5: FRANQUICIA COMPLETA ###");
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();
            franquicia.CargarSaldo(3000);
            Console.WriteLine("Saldo inicial: $" + franquicia.ObtenerSaldo());
            
            DateTime fecha5 = new DateTime(2025, 11, 27, 9, 0, 0); // Miércoles 9:00
            Boleto boleto5 = colectivo152.PagarCon(franquicia, fecha5);
            if (boleto5 != null)
            {
                Console.WriteLine("✓ Viaje exitoso");
                Console.WriteLine("  Tipo: " + boleto5.TipoTarjeta);
                Console.WriteLine("  Pagó: $" + boleto5.ImportePagado + " (gratis)");
                Console.WriteLine("  Saldo restante: $" + boleto5.SaldoRestante);
            }
            Console.WriteLine();

            // TEST 6: Colectivo Interurbano
            Console.WriteLine("### TEST 6: COLECTIVO INTERURBANO ###");
            ColectivoInterurbano interurbano = new ColectivoInterurbano("500");
            Console.WriteLine("Valor pasaje interurbano: $" + interurbano.ObtenerValorPasaje());
            
            Tarjeta tarjeta6 = new Tarjeta();
            tarjeta6.CargarSaldo(10000);
            
            DateTime fecha6 = new DateTime(2025, 11, 27, 11, 0, 0);
            Boleto boleto6 = interurbano.PagarCon(tarjeta6, fecha6);
            if (boleto6 != null)
            {
                Console.WriteLine("✓ Viaje exitoso");
                Console.WriteLine("  Pagó: $" + boleto6.ImportePagado);
                Console.WriteLine("  Saldo restante: $" + boleto6.SaldoRestante);
            }
            Console.WriteLine();

            // TEST 7: Saldo negativo y límites
            Console.WriteLine("### TEST 7: SALDO NEGATIVO ###");
            Tarjeta tarjeta7 = new Tarjeta();
            tarjeta7.CargarSaldo(2000);
            Console.WriteLine("Saldo inicial: $" + tarjeta7.ObtenerSaldo());
            
            DateTime fecha7 = new DateTime(2025, 11, 27, 12, 0, 0);
            Boleto boleto7 = colectivo152.PagarCon(tarjeta7, fecha7);
            if (boleto7 != null)
            {
                Console.WriteLine("✓ Viaje exitoso con saldo negativo");
                Console.WriteLine("  Saldo restante: $" + boleto7.SaldoRestante);
            }
            Console.WriteLine();

            // TEST 8: Saldo pendiente
            Console.WriteLine("### TEST 8: SALDO PENDIENTE ###");
            Tarjeta tarjeta8 = new Tarjeta();
            // Cargar hasta el límite
            tarjeta8.CargarSaldo(30000);
            tarjeta8.CargarSaldo(25000);
            tarjeta8.CargarSaldo(10000); // Esto debería ir a pendiente
            Console.WriteLine("Saldo: $" + tarjeta8.ObtenerSaldo());
            Console.WriteLine("Saldo pendiente: $" + tarjeta8.ObtenerSaldoPendiente());
            
            // Al pagar, debería acreditarse el pendiente
            DateTime fecha8 = new DateTime(2025, 11, 27, 15, 0, 0);
            Boleto boleto8 = colectivo152.PagarCon(tarjeta8, fecha8);
            if (boleto8 != null)
            {
                Console.WriteLine("✓ Viaje exitoso");
                Console.WriteLine("  Saldo después del viaje: $" + tarjeta8.ObtenerSaldo());
                Console.WriteLine("  Saldo pendiente: $" + tarjeta8.ObtenerSaldoPendiente());
            }
            Console.WriteLine();

            Console.WriteLine("==============================================");
            Console.WriteLine("   PRUEBAS COMPLETADAS");
            Console.WriteLine("==============================================");
            Console.WriteLine("\nPresiona Enter para salir...");
            Console.ReadLine();
        }
    }
}
