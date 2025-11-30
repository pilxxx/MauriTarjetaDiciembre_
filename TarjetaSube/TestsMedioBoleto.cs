using System;

namespace TarjetaSube
{
    public class TestsMedioBoleto
    {
        public static void Main()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("  TESTS DE MEDIO BOLETO");
            Console.WriteLine("========================================\n");
            
            int pasaron = 0;
            int total = 0;
            
            // TEST 1
            Console.WriteLine("Test 1: Dos viajes a mitad de precio");
            total++;
            
            TarjetaMedioBoleto m1 = new TarjetaMedioBoleto();
            m1.CargarSaldo(5000);
            Colectivo c1 = new Colectivo("152");
            
            DateTime f1 = new DateTime(2025, 11, 27, 8, 0, 0);
            Boleto b1 = m1.PagarCon(c1, f1);
            Boleto b2 = m1.PagarCon(c1, f1.AddMinutes(5));
            
            if (b1 != null && b1.ImportePagado == 790m && b2 != null && b2.ImportePagado == 790m)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine($"Viaje 1: ${b1.ImportePagado}, Viaje 2: ${b2.ImportePagado}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 2
            Console.WriteLine("\nTest 2: Dos mitad, tercero completo");
            total++;
            
            TarjetaMedioBoleto m2 = new TarjetaMedioBoleto();
            m2.CargarSaldo(10000);
            Colectivo c2 = new Colectivo("152");
            
            DateTime f2 = new DateTime(2025, 11, 27, 8, 0, 0);
            Boleto b2_1 = m2.PagarCon(c2, f2);
            Boleto b2_2 = m2.PagarCon(c2, f2.AddMinutes(5));
            Boleto b2_3 = m2.PagarCon(c2, f2.AddMinutes(10));
            
            if (b2_1.ImportePagado == 790m && b2_2.ImportePagado == 790m && b2_3.ImportePagado == 1580m)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine($"Viaje 1: ${b2_1.ImportePagado}");
                Console.WriteLine($"Viaje 2: ${b2_2.ImportePagado}");
                Console.WriteLine($"Viaje 3: ${b2_3.ImportePagado}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 3
            Console.WriteLine("\nTest 3: Con saldo justo (positivo)");
            total++;
            
            TarjetaMedioBoleto m3 = new TarjetaMedioBoleto();
            m3.CargarSaldo(2000);
            Colectivo c3 = new Colectivo("152");
            
            DateTime f3 = new DateTime(2025, 11, 27, 8, 0, 0);
            Boleto b3_1 = m3.PagarCon(c3, f3);
            Boleto b3_2 = m3.PagarCon(c3, f3.AddMinutes(5));
            
            if (b3_1 != null && b3_2 != null && b3_2.SaldoRestante > 0)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine($"Saldo final: ${b3_2.SaldoRestante}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 4
            Console.WriteLine("\nTest 4: Trasbordo tiene prioridad");
            total++;
            
            TarjetaMedioBoleto m4 = new TarjetaMedioBoleto();
            m4.CargarSaldo(5000);
            Colectivo c4a = new Colectivo("152");
            Colectivo c4b = new Colectivo("60");
            
            DateTime f4 = new DateTime(2025, 11, 27, 10, 0, 0);
            Boleto b4_1 = m4.PagarCon(c4a, f4);
            Boleto b4_2 = m4.PagarCon(c4b, f4.AddMinutes(15));
            
            if (b4_1.ImportePagado == 790m && b4_2.ImportePagado == 0m && b4_2.EsTransbordo)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine("Primer viaje: $790 (medio boleto)");
                Console.WriteLine("Segundo viaje: $0 (trasbordo)");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 5
            Console.WriteLine("\nTest 5: Misma línea no es trasbordo");
            total++;
            
            TarjetaMedioBoleto m5 = new TarjetaMedioBoleto();
            m5.CargarSaldo(5000);
            Colectivo c5 = new Colectivo("152");
            
            DateTime f5 = new DateTime(2025, 11, 27, 10, 0, 0);
            Boleto b5_1 = m5.PagarCon(c5, f5);
            Boleto b5_2 = m5.PagarCon(c5, f5.AddMinutes(15));
            
            if (b5_1.ImportePagado == 790m && b5_2.ImportePagado == 790m && !b5_2.EsTransbordo)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine("Ambos pagaron $790 (mismo colectivo)");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 6
            Console.WriteLine("\nTest 6: Más de 60 min no es trasbordo");
            total++;
            
            TarjetaMedioBoleto m6 = new TarjetaMedioBoleto();
            m6.CargarSaldo(5000);
            Colectivo c6a = new Colectivo("152");
            Colectivo c6b = new Colectivo("60");
            
            DateTime f6 = new DateTime(2025, 11, 27, 10, 0, 0);
            Boleto b6_1 = m6.PagarCon(c6a, f6);
            Boleto b6_2 = m6.PagarCon(c6b, f6.AddMinutes(75));
            
            // Como pasan más de 60 min, NO es trasbordo
            // Pero todavía le queda 1 viaje con descuento, así que paga $790
            if (b6_1.ImportePagado == 790m && b6_2.ImportePagado == 790m && !b6_2.EsTransbordo)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine("Primer viaje: $790");
                Console.WriteLine("Segundo viaje: $790 (no trasbordo, usa segundo descuento)");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            Console.WriteLine("\n========================================");
            Console.WriteLine($"RESULTADO: {pasaron}/{total} tests pasaron");
            Console.WriteLine("========================================");
        }
    }
}