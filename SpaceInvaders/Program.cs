using System;
using System.Threading;

namespace SpaceInvaders
{
    internal class Program
    {
        #region ekran początkowy
        static void ekran_początkowy()
        {
            Console.Write(" Witaj pilocie! \n \n");
            Thread.Sleep(800);
            Console.Write("Staw czoła najeźdźcom i wróć jako bohater. \n \n");
            Thread.Sleep(800);
            Console.Write("Kontrolki: \n \n");
            Thread.Sleep(500);
            Console.Write("a - ruch w lewo statkiem \n");
            Thread.Sleep(500);
            Console.Write("d - ruch w prawo statkiem \n");
            Thread.Sleep(500);
            Console.Write("m - strzal laserem \n \n");
            Thread.Sleep(800);
            Console.Write("Powodzenia. \n \n");
            Thread.Sleep(800);
            Console.Write("Wcisnij dowolny klawisz aby zaczac.");
            Console.ReadKey();
        }
        #endregion

        #region wyswielt swiat
        static void wyswietl_swiat(int poleX, int wynik, int poleY, char[,] swiat)
        {
            Console.Clear();
            Console.Write("Wynik:    " + wynik + "\n");
            for (int x = 0; x < poleX; x++)
            {
                Console.Write("|");
                for (int y = 0; y < poleY; y++)
                {
                    Console.Write(swiat[x, y]);
                }
                Console.Write("|");
                Console.Write("\n");
            }
        }
        #endregion

        #region ekran końcowy
        static void ekran_koncowy(bool zwyciestwo,int wynik, int liczbaWrogow, bool przegrana, int i)
        {
            if (zwyciestwo == true)
            {
                Console.Write("Gratulacje !!! \n \n");
                Thread.Sleep(1000);
                Console.Write("Wynik: " + wynik);
                Thread.Sleep(1000);
                int bonus = liczbaWrogow * 20 - i;
                Console.Write("\n \n Bonus: " + bonus);
                Thread.Sleep(1000);
                Console.Write("\n \n Maksymalny wynik: " + (wynik + bonus));
                Thread.Sleep(1000);
                Console.ReadKey();
            }
            else if (przegrana == true)
            {
                Console.Write("\n \n Przegrales !!!.");
                Thread.Sleep(1000);
                Console.Write("\n \n Jestesmy zgubieni.");
                Thread.Sleep(1000);
                Console.Write("\n \n Koncowy wynik: " + wynik);
                Console.ReadKey();
            }
        }
        #endregion

        static void Main(string[] args)
        {
            int poleX = 23;
            int poleY = 40;
            int y, x, yi;
            char[,] swiat = new char[poleX, poleY];
            char statek = 'A';
            char laserStatku = '^';
            char kosmita = 'M';
            char kosmitaObronny = 'O';
            char laserKosmity = 'U';
            char wybuch = 'X';
            int wynik = 0;
            bool zwyciestwo = false;
            bool przegrana = false;
            int gotowoscLasera = 1;
            int gotowoscWroga = 0;
            int liczbaWrogow = 0;


            /*wywołanie ekranu początkowego*/
            Console.SetCursorPosition(30, 5);
            ekran_początkowy();


            #region tworzenie pole gry
            /*tworz pole gry*/
            for (x = 0; x < poleX; x++)
            {
                for (y = 0; y < poleY; y++)
                {
                    if ((x + 1) % 2 == 0 && x < 7 && y > 4 && y < poleY - 5 && y % 2 == 0)
                    {
                        swiat[x, y] = kosmita;
                        liczbaWrogow++;
                    }
                    else if ((x + 1) % 2 == 0 && x >= 7 && x < 9 && y > 4 && y < poleY - 5 && y % 2 == 0)
                    {
                        swiat[x, y] = kosmitaObronny;
                        liczbaWrogow++;
                    }
                    else
                    {
                        swiat[x, y] = ' ';
                    }
                }
            }


            #endregion

            swiat[poleX - 1, poleY / 2] = statek;
            int i = 1;
            char kierunekKosmity = 'l';
            int aktualLiczbaWrogow = liczbaWrogow;

            while (aktualLiczbaWrogow > 0 && zwyciestwo == false && przegrana == false)
            {

                bool upuszczone = false;
                gotowoscLasera++;


                /*wyswielt swiat*/
                wyswietl_swiat(poleX, wynik, poleY, swiat);

                //aktualizuj pozycje rakiet
                #region aktualizuj pozycje rakiet wroga
                for (x = poleX - 1; x >= 0; x--)
                {
                    for (y = 0; y < poleY; y++)
                    {
                        if (i % 5 == 0)
                        {
                            if (x < poleX - 1)
                            {
                                if (swiat[x, y] == laserKosmity)
                                {
                                    if (swiat[x + 1, y] == statek)
                                    {
                                        swiat[x, y] = ' ';
                                        swiat[x + 1, y] = wybuch;
                                        przegrana = true;
                                    }
                                    else if (swiat[x + 1, y] != kosmita && swiat[x + 1, y] != kosmitaObronny)
                                    {
                                        swiat[x + 1, y] = laserKosmity;
                                        swiat[x, y] = ' ';
                                    }
                                    else if (swiat[x + 1, y] == kosmita || swiat[x + 1, y] == kosmitaObronny)
                                    {
                                        swiat[x, y] = ' ';
                                    }
                                }
                            }
                            else
                            {
                                if (swiat[x, y] == laserKosmity)
                                {
                                    swiat[x, y] = ' ';
                                }
                            }
                        }

                    }
                }
                #endregion

                //obsluga dynamiki gry
                #region dynamika gry
                Random rand = new Random();
                for (x = 0; x < poleX; x++)
                {
                    for (y = 0; y < poleY; y++)
                    {
                        if (x < poleX)
                        {
                            if ((i % 3) == 0 && (swiat[x, y] == kosmitaObronny || swiat[x, y] == kosmita) && rand.Next(1, 101) <= 2 && swiat[x + 1, y] != laserStatku)
                            {
                                for (yi = x + 1; yi < poleX; yi++)
                                {
                                    if (swiat[yi, y] == kosmita || swiat[yi, y] == kosmitaObronny)
                                    {
                                        gotowoscWroga = 0;
                                        break;
                                    }
                                    gotowoscWroga = 1;
                                }
                                if (gotowoscWroga == 1)
                                {
                                    swiat[x + 1, y] = laserKosmity;
                                }
                            }
                        }

                        if (x > 0)
                        {
                            if (swiat[x, y] == laserStatku)
                            {
                                swiat[x, y] = ' ';

                                if (swiat[x - 1, y] == kosmita || swiat[x - 1, y] == kosmitaObronny)
                                {
                                    wynik = wynik + 50;

                                    if (swiat[x - 1, y] == kosmitaObronny)
                                    {
                                        swiat[x - 1, y] = kosmita;
                                    }
                                    else
                                    {
                                        aktualLiczbaWrogow--;
                                        swiat[x - 1, y] = wybuch;

                                        if (aktualLiczbaWrogow == 0)
                                        {
                                            zwyciestwo = true;
                                        }
                                    }
                                }
                                else if (swiat[x - 1, y] == laserKosmity)
                                {
                                    swiat[x - 1, y] = wybuch;
                                }
                                else
                                {
                                    swiat[x - 1, y] = laserStatku;
                                }
                            }
                        }
                        else
                        {
                            if (swiat[x, y] == laserStatku)
                            {
                                swiat[x, y] = wybuch;
                            }
                        }

                        if (swiat[x, y] == wybuch)
                        {
                            swiat[x, y] = ' ';
                        }
                    }
                }
                #endregion

                //poruszanie wrogow
                #region obsuga poruszania wrogow
                for (x = 0; x < poleX; x++)
                {
                    if (swiat[x, 0] == kosmita)
                    {
                        kierunekKosmity = 'r';
                        upuszczone = true;
                        break;
                    }
                    else if (swiat[x, poleY - 1] == kosmita)
                    {
                        kierunekKosmity = 'l';
                        upuszczone = true;
                        break;
                    }
                }
                #endregion

                //zmiana pozycji wrogow
                #region obsuga wrogow
                if (i % 5 == 0)
                {
                    if (kierunekKosmity == 'l')
                    {
                        for (x = poleX - 2; x >= 0; x--)
                        {
                            for (y = 0; y < poleY; y++)
                            {
                                if (upuszczone == true && (swiat[x, y] == kosmita || swiat[x, y] == kosmitaObronny))
                                {
                                    swiat[x + 1, y - 1] = swiat[x, y];
                                    swiat[x, y] = ' ';
                                }
                                else if (upuszczone == false && (swiat[x, y] == kosmita || swiat[x, y] == kosmitaObronny))
                                {
                                    swiat[x, y - 1] = swiat[x, y];
                                    swiat[x, y] = ' ';
                                }
                            }
                        }
                    }
                    else if (kierunekKosmity == 'r')
                    {
                        for (x = poleX - 2; x >= 0; x--)
                        {
                            for (y = poleY - 1; y >= 0; y--)
                            {
                                if (upuszczone == true && (swiat[x, y] == kosmita || swiat[x, y] == kosmitaObronny))
                                {
                                    swiat[x + 1, y + 1] = swiat[x, y];
                                    swiat[x, y] = ' ';
                                }
                                else if (upuszczone == false && (swiat[x, y] == kosmita || swiat[x, y] == kosmitaObronny))
                                {
                                    swiat[x, y + 1] = swiat[x, y];
                                    swiat[x, y] = ' ';
                                }
                            }
                        }
                    }
                    for (y = 0; y < poleY; y++)
                    {
                        if (swiat[poleX - 1, y] == kosmita)
                        {
                            zwyciestwo = false;
                            przegrana = true;
                        }
                    }
                }
                #endregion

                /*kontrola gracza*/
                #region obsluga gracza          
                char keyPress;
                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(false);
                    keyPress = key.KeyChar;
                }
                else
                {
                    keyPress = '.';
                }
                if (keyPress == 'a')
                {
                    for (y = 0; y < poleY - 1; y = y + 1)
                    {
                        if (swiat[poleX - 1, y + 1] == statek)
                        {
                            swiat[poleX - 1, y] = statek;
                            swiat[poleX - 1, y + 1] = ' ';
                        }
                    }
                }

                if (keyPress == 'd')
                {
                    for (y = poleY - 1; y > 0; y = y - 1)
                    {
                        if (swiat[poleX - 1, y - 1] == statek)
                        {
                            swiat[poleX - 1, y] = statek;
                            swiat[poleX - 1, y - 1] = ' ';
                        }
                    }
                }
                if (keyPress == 'm' && gotowoscLasera > 2)
                {
                    for (y = 0; y < poleY; y = y + 1)
                    {
                        if (swiat[poleX - 1, y] == statek)
                        {
                            swiat[poleX - 2, y] = laserStatku;
                            gotowoscLasera = 0;
                        }
                    }
                }
                #endregion

                i++;
                Thread.Sleep(50);
            }

            //wyswietl koncowy ekran
            wyswietl_swiat(poleX, wynik, poleY, swiat);

            Thread.Sleep(2000);
            Console.Clear();

            //koncowy ekran
            Console.SetCursorPosition(30, 5);
            ekran_koncowy(zwyciestwo, wynik, liczbaWrogow, przegrana, i);


        }
    }
}