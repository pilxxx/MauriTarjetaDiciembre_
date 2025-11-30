using System;

namespace TarjetaSube
{
    public class TestsMiBiciTuBici
    {
        public static void Main()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("  TESTS DE MI BICI TU BICI");
            Console.WriteLine("========================================\n");
            
            int pasaron = 0;
            int total = 0;
            
            // TEST 1: Retiro normal
            Console.WriteLine("Test 1: Retiro normal sin problemas");
            total++;
            
            Tarjeta t1 = new Tarjeta();
            t1.CargarSaldo(5000);
            EstacionBici estacion1 = new EstacionBici();
            
            decimal saldoAntes = t1.ObtenerSaldo();
            bool pudoRetirar = estacion1.RetirarBici(t1, new DateTime(2025, 11, 27, 10, 0, 0));
            decimal saldoDespues = t1.ObtenerSaldo();
            
            if (pudoRetirar && (saldoAntes - saldoDespues) == 1777.50m)
            {
                Console.WriteLine("✓ PASO");
                Console.WriteLine($"Saldo antes: ${saldoAntes}, después: ${saldoDespues}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 2: Saldo insuficiente
            Console.WriteLine("\nTest 2: No tiene suficiente saldo");
            total++;
            
            Tarjeta t2 = new Tarjeta();
            t2.CargarSaldo(1000); // Menos de $1777.50
            EstacionBici estacion2 = new EstacionBici();
            
            bool pudoRetirar2 = estacion2.RetirarBici(t2, new DateTime(2025, 11, 27, 10, 0, 0));
            
            if (!pudoRetirar2)
            {
                Console.WriteLine("✓ PASO - Correctamente rechazado");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO - Debería haber rechazado");
            }
            
            // TEST 3: Con multa pendiente y poco saldo
            Console.WriteLine("\nTest 3: Multa pendiente y poco saldo");
            total++;
            
            Tarjeta t3 = new Tarjeta();
            t3.CargarSaldo(3000);
            EstacionBici estacion3 = new EstacionBici();
            
            DateTime momento1 = new DateTime(2025, 11, 27, 10, 0, 0);
            estacion3.RetirarBici(t3, momento1);
            
            DateTime momento2 = momento1.AddMinutes(90); // Se pasa de tiempo
            estacion3.DevolverBici(t3, momento2);
            
            decimal multa = estacion3.ObtenerMultasPendientes(t3);
            bool pudoRetirar3 = estacion3.RetirarBici(t3, momento2.AddMinutes(10));
            
            if (!pudoRetirar3 && multa == 1000m)
            {
                Console.WriteLine("✓ PASO - Rechazado por multa + tarifa > saldo");
                Console.WriteLine($"Multa: ${multa}, Saldo: ${t3.ObtenerSaldo()}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 4: Retiro con multa
            Console.WriteLine("\nTest 4: Retiro pagando multa acumulada");
            total++;
            
            Tarjeta t4 = new Tarjeta();
            t4.CargarSaldo(5000);
            EstacionBici estacion4 = new EstacionBici();
            
            DateTime m1 = new DateTime(2025, 11, 27, 10, 0, 0);
            estacion4.RetirarBici(t4, m1);
            estacion4.DevolverBici(t4, m1.AddMinutes(90));
            
            decimal multaAntes = estacion4.ObtenerMultasPendientes(t4);
            decimal saldoAntes4 = t4.ObtenerSaldo();
            
            estacion4.RetirarBici(t4, m1.AddMinutes(100));
            
            decimal saldoDespues4 = t4.ObtenerSaldo();
            decimal multaDespues = estacion4.ObtenerMultasPendientes(t4);
            
            if (multaAntes == 1000m && (saldoAntes4 - saldoDespues4) == 2777.50m && multaDespues == 0)
            {
                Console.WriteLine("✓ PASO - Pagó tarifa + multa");
                Console.WriteLine($"Cobrado: ${saldoAntes4 - saldoDespues4}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
            }
            
            // TEST 5: Varias multas
            Console.WriteLine("\nTest 5: Varias multas acumuladas");
            total++;
            
            Tarjeta t5 = new Tarjeta();
            t5.CargarSaldo(10000);
            EstacionBici estacion5 = new EstacionBici();
            
            DateTime f = new DateTime(2025, 11, 27, 9, 0, 0);
            
            // Primera bici - se pasa (genera multa)
            estacion5.RetirarBici(t5, f);
            estacion5.DevolverBici(t5, f.AddMinutes(75));
            
            // Segunda bici - paga la multa anterior + tarifa, después se pasa (genera otra multa)
            estacion5.RetirarBici(t5, f.AddMinutes(80));
            estacion5.DevolverBici(t5, f.AddMinutes(160));
            
            // Ahora tiene 1 multa de $1000 (la segunda vez que se pasó)
            decimal multasTotal = estacion5.ObtenerMultasPendientes(t5);
            
            decimal saldoAntes5 = t5.ObtenerSaldo();
            estacion5.RetirarBici(t5, f.AddMinutes(170));
            decimal saldoDespues5 = t5.ObtenerSaldo();
            
            // Debe cobrar tarifa + 1 multa = $2777.50
            if (multasTotal == 1000m && (saldoAntes5 - saldoDespues5) == 2777.50m)
            {
                Console.WriteLine("✓ PASO - 1 multa + tarifa");
                Console.WriteLine($"Multas: ${multasTotal}, Cobrado: ${saldoAntes5 - saldoDespues5}");
                pasaron++;
            }
            else
            {
                Console.WriteLine("✗ FALLO");
                Console.WriteLine($"Se esperaba 1 multa ($1000) y cobro de $2777.50");
                Console.WriteLine($"Pero hubo ${multasTotal} en multas y se cobró ${saldoAntes5 - saldoDespues5}");
            }
            
            Console.WriteLine("\n========================================");
            Console.WriteLine($"RESULTADO: {pasaron}/{total} tests pasaron");
            Console.WriteLine("========================================");
        }
    }
}