import java.io.BufferedReader;
import java.io.InputStreamReader;
import scheduler.scheduling.policies.*;
import java.util.LinkedList;
import scheduler.processing.*;
import scheduler.*;

public class ProcessScheduler {
    public static void main(String[] args) throws Exception {
        Generador g = null;
        Procesador p = null;
        // variables del programa
        BufferedReader tec = new BufferedReader(new InputStreamReader(System.in));
        int rangoF = 0, rangoI = 0;
        int aritht = 0, iot = 0, condt = 0, loopt = 0, quantumt = 0;
        String s = "";
        boolean quit = true;
        // variables de tiempo ingresadas por el usuario
        double arith = 0, io = 0, cond = 0, loop = 0, quantum = 0;
        String rango = "";
        String politica = "";
        Double Rangoinicial = 0.0, Rangofinal = 0.0;

        // Se obtienen los datos ingresados por el usuario.
        try {
            while (quit == true) {
                // comprabamos si correra en uno o dos procesadores
                // dual
                if (args[0].equals("-dual")) {
                    // asignamos los valores para los tiempos segun el usuario los ingrese
                    try {
                        arith = Double.parseDouble(args[3]);
                        aritht = (int) (arith * 1000);
                        io = Double.parseDouble(args[4]);
                        iot = (int) (io * 1000);
                        cond = Double.parseDouble(args[5]);
                        condt = (int) (cond * 1000);
                        loop = Double.parseDouble(args[6]);
                        loopt = (int) (loop * 1000);
                        if (args.length > 7) {
                            quantum = Double.parseDouble(args[7]);
                            quantumt = (int) (quantum * 1000);
                        }
                    } catch (Exception e) {
                        System.out.println("Por favor ingrese valores numericos para los tiempos.");
                    }

                    // Se obtienen los rangos de tiempo
                    for (int i = 0; i <= args[2].length(); i++) {
                        if (args[2].charAt(i) == '-') {
                            Rangoinicial = Double.parseDouble(args[2].substring(0, i));
                            Rangofinal = Double.parseDouble(args[2].substring(i + 1, args[2].length()));
                            break;
                        }
                    }
                    // Rangos convertidos a milisegundos
                    rangoF = (int) (Rangofinal * 1000);
                    rangoI = (int) (Rangoinicial * 1000);

                    // obtememos del usuario la politica (en el caso que sea dual)
                    switch (args[1]) {
                        case "-fcfs":
                            politica = "First Come First Served";
                            g = new Generador("fcfs", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(2, "fcfs");
                            p.start();
                            break;
                        case "-lcfs":
                            politica = "Last Come First Served";
                            g = new Generador("lcfs", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(2, "lcfs");
                            p.start();

                            break;
                        case "-rr":
                            politica = "Round Robin";
                            g = new Generador("rr", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(2, "rr",quantumt );
                            p.start();
                            break;
                        case "-pp":
                            politica = "Priority Policy";
                            g = new Generador("pp", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(2, "pp");
                            p.start();
                            break;
                        default:
                            System.out.println("Por favor ingrese una politica correcta.");
                    }

                    s = tec.readLine();
                    s = s.toLowerCase();
                    if (s.equals("q")) {
                        if (Procesador.politicausada() == "First-Come First-Served") {
                            System.out.println("\nLa cantidad de procesos que se atendieron fue de: "
                                    + Procesador.totalProcesosFCFS());
                            System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                    + Procesador.restantesFCFS());
                        } else if (Procesador.politicausada() == "Last-Come First-Served") {
                            System.out.println("\nLa cantidad de procesos que se atendieron fue de: "
                                    + Procesador.totalProcesosLCFS());
                            System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                    + Procesador.restantesLCFS());
                        } else if (Procesador.politicausada() == "Priority Policy") {
                            System.out.println(
                                    "\nLa cantidad de procesos que se atendieron fue de: " + Procesador.totalProcesosPP());
                            System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                    + Procesador.restantesPP());
                        } else if (Procesador.politicausada() == "Round-Robin") {
                            System.out.println(
                                    "\nLa cantidad de procesos que se atendieron fue de: " + Procesador.totalProcesosRR());
                            System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                    + Procesador.restantesRR());
                        }
                        System.out.println("El tiempo promedio de atencion por proceso es de: " + Procesador.promedio());
                        System.out.println("Politica utilizada: " + Procesador.politicausada());
                        System.exit(0);
                    }
                    
//------------------------------------------------TERMINA DUAL-------------------------------------------------------------------
                } else {
                    // asignamos los valores para los tiempos segun el usuario los ingrese
                    try {
                        arith = Double.parseDouble(args[2]);
                        io = Double.parseDouble(args[3]);
                        cond = Double.parseDouble(args[4]);
                        loop = Double.parseDouble(args[5]);

                        aritht = (int) (arith * 1000);
                        iot = (int) (io * 1000);
                        condt = (int) (cond * 1000);
                        loopt = (int) (loop * 1000);

                        if (args.length > 6) {
                            quantum = Double.parseDouble(args[6]);
                            quantumt = (int) (quantum * 1000);
                        }
                    } catch (Exception e) {
                        System.out.println("Por favor ingrese valores numericos para los tiempos.");
                    }

                    // Se obtienen los rangos de tiempo
                    for (int i = 0; i <= args[1].length(); i++) {
                        if (args[1].charAt(i) == '-') {
                            Rangoinicial = Double.parseDouble(args[1].substring(0, i));
                            Rangofinal = Double.parseDouble(args[1].substring(i + 1, args[1].length()));
                            break;
                        }
                    }
                    rangoF = (int) (Rangofinal * 1000);
                    rangoI = (int) (Rangoinicial * 1000);

                    // obtememos del usuario la politica (en caso que no sea dual)
                    switch (args[0]) {
                        case "-fcfs":
                            politica = "First Come First Served";
                            g = new Generador("fcfs", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(1, "fcfs");
                            p.start();
                            break;
                        case "-lcfs":
                            politica = "Last Come First Served";
                            g = new Generador("lcfs", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(1, "lcfs");
                            p.start();

                            break;
                        case "-rr":
                            politica = "Round Robin";
                            g = new Generador("rr", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(1, "rr", quantumt);
                            p.start();
                            break;

                        case "-pp":
                            politica = "Priority Policy";
                            g = new Generador("pp", rangoF, rangoI, aritht, condt, iot, loopt);
                            g.start();
                            p = new Procesador(1, "pp");
                            p.start();
                            break;
                        default:
                            System.out.println("Por favor ingrese una politica correcta.");
                    }

                }

                s = tec.readLine();
                s = s.toLowerCase();
                if (s.equals("q")) {
                    if (Procesador.politicausada() == "First-Come First-Served") {
                        System.out.println("\nLa cantidad de procesos que se atendieron fue de: "
                                + Procesador.totalProcesosFCFS());
                        System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                + Procesador.restantesFCFS());
                    } else if (Procesador.politicausada() == "Last-Come First-Served") {
                        System.out.println("\nLa cantidad de procesos que se atendieron fue de: "
                                + Procesador.totalProcesosLCFS());
                        System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                + Procesador.restantesLCFS());
                    } else if (Procesador.politicausada() == "Priority Policy") {
                        System.out.println(
                                "\nLa cantidad de procesos que se atendieron fue de: " + Procesador.totalProcesosPP());
                        System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                + Procesador.restantesPP());
                    } else if (Procesador.politicausada() == "Round-Robin") {
                        System.out.println(
                                "\nLa cantidad de procesos que se atendieron fue de: " + Procesador.totalProcesosRR());
                        System.out.println("La cantidad de procesos que quedaron sin atencion fue de: "
                                + Procesador.restantesRR());
                    }
                    System.out.println("El tiempo promedio de atencion por proceso es de: " + Procesador.promedio());
                    System.out.println("Politica utilizada: " + Procesador.politicausada());
                    System.exit(0);
                }
            }

        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Por favor ingrese argumentos en la linea de comandos.");
        }

    }
}