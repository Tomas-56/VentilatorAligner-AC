using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentilatorAlignerAC
{
    class CalculationSheetModel 
    {
        private double testovacieZavazie_Mt;
        private double vibracieBezZavazia_V0;
        private double vibracieNaBodeJeden_V1;
        private double vibracieNaBodeDva_V2;
        private double vibracieNaBodeTri_V3;
        public double uholPoziciaVyvazku_a;
        public double hmotnostZavazia;


        public CalculationSheetModel(double testovacieZavazie_Mt, double vibracieBezZavazia_V0, double vibracieNaBodeJeden_V1, double vibracieNaBodeDva_V2, double vibracieNaBodeTri_V3)
        {
            this.testovacieZavazie_Mt = testovacieZavazie_Mt;
            this.vibracieBezZavazia_V0 = vibracieBezZavazia_V0;
            this.vibracieNaBodeJeden_V1 = vibracieNaBodeJeden_V1;
            this.vibracieNaBodeDva_V2 = vibracieNaBodeDva_V2;
            this.vibracieNaBodeTri_V3 = vibracieNaBodeTri_V3;
            double[] arraySuradnicPriesecnikovkruhov = new double[4];

            double[] arrayVibraci = ZoradenieKruznicPodlaVelkosti(vibracieNaBodeJeden_V1, vibracieNaBodeDva_V2, vibracieNaBodeTri_V3).hodnoty;

            double[] arrayIndexov = ZoradenieKruznicPodlaVelkosti(vibracieNaBodeJeden_V1, vibracieNaBodeDva_V2, vibracieNaBodeTri_V3).index;

            double[] arrayarraySuradnicStredovKruhov = VypocetSuradnicStredovKruznic(arrayIndexov, vibracieBezZavazia_V0);

            double vzdialenostMedziKruznicami = VypocetVzdialenostiMedziDvomakruznicami(arrayarraySuradnicStredovKruhov);
            if (VypocetCiSaKruhyPretinaju(vzdialenostMedziKruznicami, arrayVibraci))
            {
                double bodA = VypocetBoduA(arrayVibraci, vzdialenostMedziKruznicami);

                double bodH = VypocetBoduH(arrayVibraci, bodA);

                arraySuradnicPriesecnikovkruhov = VypocetSuradnicPriesecnikovkruhov(arrayarraySuradnicStredovKruhov, vzdialenostMedziKruznicami, bodA, bodH);


                this.uholPoziciaVyvazku_a = VypocetUhluAlfa(arrayIndexov, arraySuradnicPriesecnikovkruhov).uholAlfa;
                hmotnostZavazia = VypocetZavazia(testovacieZavazie_Mt, vibracieBezZavazia_V0, VypocetUhluAlfa(arrayIndexov, arraySuradnicPriesecnikovkruhov).dlzkaVt);
                hmotnostZavazia = Math.Round(hmotnostZavazia, 2);
                uholPoziciaVyvazku_a = Math.Round(uholPoziciaVyvazku_a);

            }
            else
            {
                this.uholPoziciaVyvazku_a = 0;
            }

        }




        private static double VypocetDlzkyStrany(double prepona, double uhol)
        {
            double strana = prepona * uhol;
            return strana;
        }

        private static double Negacia(double vstup)
        {
            double dvojnasobok = 2 * vstup;
            return vstup - dvojnasobok;
        }

        private static double AbsolutnaHodnota(double vstup)
        {
            return Math.Abs(vstup);
        }




        private static (double[] index, double[] hodnoty) ZoradenieKruznicPodlaVelkosti(double bodV1, double bodV2, double bodV3)
        {
            double[] arrayIndexov = { 1, 2, 3 };
            double[] arrayVelkostiVibraci = { bodV1, bodV2, bodV3 };



            if (bodV1 != bodV2 && bodV1 != bodV3 && bodV2 != bodV3)
            {
                for (int i = 0; i < arrayVelkostiVibraci.Length; i++)
                {
                    for (int j = i + 1; j < arrayVelkostiVibraci.Length; j++)
                    {
                        double tempIndex;
                        double tempHodnota;

                        if (arrayVelkostiVibraci[i] < arrayVelkostiVibraci[j])
                        {
                            tempHodnota = arrayVelkostiVibraci[j];
                            arrayVelkostiVibraci[j] = arrayVelkostiVibraci[i];
                            arrayVelkostiVibraci[i] = tempHodnota;

                            tempIndex = arrayIndexov[j];
                            arrayIndexov[j] = arrayIndexov[i];
                            arrayIndexov[i] = tempIndex;
                        }
                    }
                }

                return (arrayIndexov, arrayVelkostiVibraci);

            }

            for (int i = 0; i > arrayIndexov.Length; i++) { arrayIndexov[i] = 0; arrayVelkostiVibraci[i] = 0; }

            return (arrayIndexov, arrayVelkostiVibraci);

        }

        private static double[] VypocetSuradnicStredovKruznic(double[] arrayIndexov, double vibracieBezZavazia_V0)
        {
            const double sinusUhla30 = 0.5;
            const double sinusUhl60 = 0.86602540348;

            double OsX = 0;
            double OsY = 0;

            double[] arraySuradnicStredovKruhov = new double[4];
            int pocitadloDoDva = 0;


            for (int i = 0; i < 3; i += 2)
            {

                switch (arrayIndexov[pocitadloDoDva])
                {
                    case 1:
                        OsX = 0;
                        OsY = vibracieBezZavazia_V0; break;
                    case 2:
                        OsX = Negacia(VypocetDlzkyStrany(vibracieBezZavazia_V0, sinusUhl60));
                        OsY = Negacia(VypocetDlzkyStrany(vibracieBezZavazia_V0, sinusUhla30)); break;
                    case 3:
                        OsX = VypocetDlzkyStrany(vibracieBezZavazia_V0, sinusUhl60);
                        OsY = VypocetDlzkyStrany(vibracieBezZavazia_V0, sinusUhla30); break;
                }
                pocitadloDoDva++;
                arraySuradnicStredovKruhov[i] = OsX; arraySuradnicStredovKruhov[i + 1] = OsY;
            }
            pocitadloDoDva = 0;
            return arraySuradnicStredovKruhov;

        }
        // [k1X][k1Y][k2X][k2Y]
        private static double VypocetVzdialenostiMedziDvomakruznicami(double[] arraySuradnicStredovKruhov)
        {
            double vzdialenostMedziKruznicami;
            bool vsetkyHodnotyHodnotySaRovnajuNule = true;
            foreach (double element in arraySuradnicStredovKruhov) { if (element != 0) { vsetkyHodnotyHodnotySaRovnajuNule = false; } }
            if (vsetkyHodnotyHodnotySaRovnajuNule == false)
            {
                double vzdialenostMedziX = arraySuradnicStredovKruhov[2] - arraySuradnicStredovKruhov[0];
                double vzdialenostMedziY = arraySuradnicStredovKruhov[3] - arraySuradnicStredovKruhov[1];

                vzdialenostMedziKruznicami = Math.Sqrt(vzdialenostMedziX * vzdialenostMedziX + vzdialenostMedziY * vzdialenostMedziY);

                return vzdialenostMedziKruznicami;
            }
            vzdialenostMedziKruznicami = 0;
            return vzdialenostMedziKruznicami;

        }

        private static bool VypocetCiSaKruhyPretinaju(double vzdialenostMedziKruznicami, double[] arrayVibracii)
        {
            if (vzdialenostMedziKruznicami >= arrayVibracii[0] - arrayVibracii[1] &&
                vzdialenostMedziKruznicami <= arrayVibracii[0] + arrayVibracii[1]) { return true; };

            return false;
        }

        //Vzdialenost Rx sa pocita s Y suradnicami , Ry sa pocita s X suradnicami
        //[Priesecnik k1X][Priesecnik k1Y][Priesecnik k2X][Priesecnik k2Y]

        private static double[] VypocetSuradnicPriesecnikovkruhov(double[] arraySuradnicStredovKruhov, double vzdialenostMedziKruznicami, double bodA, double bodH)
        {
            double[] arraySuradnicPriesecnikovkruhov = new double[4];
            double vzdialenostRx = VypocetVzdialenostR("X", vzdialenostMedziKruznicami, bodH, arraySuradnicStredovKruhov[1], arraySuradnicStredovKruhov[3]);
            double vzdialenostRy = VypocetVzdialenostR("Y", vzdialenostMedziKruznicami, bodH, arraySuradnicStredovKruhov[0], arraySuradnicStredovKruhov[2]);


            double bodPx = VypocetBoduP(vzdialenostMedziKruznicami, bodA, arraySuradnicStredovKruhov[0], arraySuradnicStredovKruhov[2]);
            double bodPy = VypocetBoduP(vzdialenostMedziKruznicami, bodA, arraySuradnicStredovKruhov[1], arraySuradnicStredovKruhov[3]);

            arraySuradnicPriesecnikovkruhov[0] = bodPx + vzdialenostRx;
            arraySuradnicPriesecnikovkruhov[1] = bodPy + vzdialenostRy;

            arraySuradnicPriesecnikovkruhov[2] = bodPx - vzdialenostRx;
            arraySuradnicPriesecnikovkruhov[3] = bodPy - vzdialenostRy;

            return arraySuradnicPriesecnikovkruhov;
        }

        private static (double uholAlfa, double dlzkaVt) VypocetUhluAlfa(double[] arrrayIndexov, double[] arraySuradnicPriesecnikovkruhov)
        {
            double uholAlfa = 0;
            double dlzkaVt;
            bool priesecnikJedna = false;

            double uholPriesecnikuJedna = Math.Atan2(arraySuradnicPriesecnikovkruhov[0], arraySuradnicPriesecnikovkruhov[1]);
            double uholPriesecnikuDva = Math.Atan2(arraySuradnicPriesecnikovkruhov[2], arraySuradnicPriesecnikovkruhov[3]);

            uholPriesecnikuJedna *= (180 / Math.PI);
            uholPriesecnikuDva *= (180 / Math.PI);

            switch (arrrayIndexov[2])
            {
                case 3:
                    if (uholPriesecnikuJedna >= 0 && uholPriesecnikuJedna <= 180 || uholPriesecnikuJedna <= -120 && uholPriesecnikuJedna >= -180) { uholAlfa = uholPriesecnikuJedna; priesecnikJedna = true; }
                    else { uholAlfa = uholPriesecnikuDva; priesecnikJedna = false; }
                    break;
                case 2:
                    if (uholPriesecnikuJedna <= 0 && uholPriesecnikuJedna >= -180 || uholPriesecnikuJedna >= 120 && uholPriesecnikuJedna <= 180) { uholAlfa = uholPriesecnikuJedna; priesecnikJedna = true; }
                    else { uholAlfa = uholPriesecnikuDva; priesecnikJedna = false; }
                    break;
                case 1:
                    if (uholPriesecnikuJedna >= 0 && uholPriesecnikuJedna <= 120 || uholPriesecnikuJedna <= 0 && uholPriesecnikuJedna >= -120) { uholAlfa = uholPriesecnikuJedna; priesecnikJedna = true; }
                    else { uholAlfa = uholPriesecnikuDva; priesecnikJedna = false; }
                    break;

            }
            if (priesecnikJedna)
            {
                dlzkaVt = VypocetDlzkyVt(arraySuradnicPriesecnikovkruhov[0], arraySuradnicPriesecnikovkruhov[1]);

            }
            else
            {
                dlzkaVt = VypocetDlzkyVt(arraySuradnicPriesecnikovkruhov[2], arraySuradnicPriesecnikovkruhov[3]);

            }



            if (uholAlfa < 0) { uholAlfa = AbsolutnaHodnota(uholAlfa); }
            else if (uholAlfa > 0) { uholAlfa = 360 - uholAlfa; }
            else { uholAlfa = 0; }

            return (uholAlfa, dlzkaVt);

        }
        private static double VypocetDlzkyVt(double suradnicaXKruznice, double suradnicaYKruznice)
        {
            double dlzkaVt = Math.Sqrt(suradnicaXKruznice * suradnicaXKruznice + suradnicaYKruznice * suradnicaYKruznice);
            return dlzkaVt;
        }

        private static double VypocetZavazia(double testovacieZavazie_Mt, double vibracieBezZavazia_V0, double dlzkaVt)
        {
            double hmotnostZavazia = testovacieZavazie_Mt * vibracieBezZavazia_V0;
            hmotnostZavazia /= dlzkaVt;
            return hmotnostZavazia;
        }

        private static double VypocetBoduP(double vzdialenostMedziKruznicami, double bodA, double suradnica1, double suradnica2)
        {
            double bodP = suradnica1 + bodA * (suradnica2 - suradnica1) / vzdialenostMedziKruznicami;
            return bodP;
        }

        private static double VypocetBoduA(double[] arrayVibracii, double vzdialenostMedziKruznicami)
        {
            double bodA = (arrayVibracii[0] * arrayVibracii[0] - arrayVibracii[1] * arrayVibracii[1]
                + vzdialenostMedziKruznicami * vzdialenostMedziKruznicami) / (2 * vzdialenostMedziKruznicami);

            return bodA;
        }

        private static double VypocetBoduH(double[] arrayVibracii, double bodA)
        {
            double bodH = Math.Sqrt(arrayVibracii[0] * arrayVibracii[0] - bodA * bodA);
            return bodH;
        }

        private static double VypocetVzdialenostR(string vypocetPreX, double vzdialenostMedziKruznicami, double bodH, double suradnicaKruznice1, double suradnicaKruznice2)
        {
            double vzdialenostR;

            if (vypocetPreX.Equals("X"))
            {
                vzdialenostR = -(suradnicaKruznice2 - suradnicaKruznice1) * (bodH / vzdialenostMedziKruznicami);
            }
            else
            {
                vzdialenostR = (suradnicaKruznice2 - suradnicaKruznice1) * (bodH / vzdialenostMedziKruznicami);
            }
            return vzdialenostR;
        }
    }
}
