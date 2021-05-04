//#pragma warning disable IDE0051 // Usuń nieużywane prywatne składowe
#pragma warning disable IDE0060 // Usuń nieużywany parametr
#pragma warning disable IDE0044 // Dodaj modyfikator tylko do odczytu
#zmiana 123

using System;
using System.Collections.Generic;

namespace Bank_klasy_virtual
{
    interface ILiczenieMajatku
    {
        public double PodliczSaldoFirma();
        public double PodliczSaldoDuzaFirma();
        public double PodliczSaldoOsoba();
        public double PodliczSaldoWaznaOsoba();
    }
    class Konto
    {
        private string nr;
        private double saldo;
        public Konto(string n) { nr = n; saldo = 0; }
        public double GetSaldo() { return saldo; }
        public string GetNr() { return nr; }
        public void Wplac(double kwota) { saldo += kwota; }
        public void Wyplac(double kwota) { saldo -= kwota; }
        /*public bool Wyplac(double kwota)
        {
            if (saldo >= kwota) saldo -= kwota;
            return saldo >= kwota;
        }*/
        public override string ToString() { return $"{GetType().Name}   Nr: {GetNr()}   Saldo: {GetSaldo()}"; }
    }
    abstract class Klient : ILiczenieMajatku
    {
        private List<Konto> listaKont;
        public Klient() { listaKont = new List<Konto>(); }
        public void DodajKonto(Konto kon) { listaKont.Add(kon); }
        public List<Konto> GetKonta() { return listaKont; }
        public override string ToString()
        {
            string napis = "";
            for (int i = 0; i < listaKont.Count; i++)
                napis += $"   {(char)(i + 97)}) { listaKont[i]}\n";
            return napis;
        }
        public virtual double PodliczSaldoFirma() { return 0; }
        public virtual double PodliczSaldoDuzaFirma() { return 0; }
        public virtual double PodliczSaldoOsoba() { return 0; }
        public virtual double PodliczSaldoWaznaOsoba() { return 0; }
        public virtual string Dane() { return ""; }
    }
    class Bank
    {
        private List<Klient> listaKlientow;
        public Bank() { listaKlientow = new List<Klient>(); }
        public void DodajKlienta(Klient klin) { listaKlientow.Add(klin); }
        public List<Klient> GetKlienci() { return listaKlientow; }
        public override string ToString()
        {
            string napis = "";
            for (int i = 0; i < listaKlientow.Count; i++)
                napis += $"Nr.{i} - {listaKlientow[i].GetType().Name} {listaKlientow[i].Dane()}\n{ listaKlientow[i]}\n";
            return napis;
        }
        public double GetMajatek(int opcja)
        { 
            double wynik = 0;
            foreach (Klient c in listaKlientow)
            {
                if (opcja == 0 | opcja == 1)
                    wynik += c.PodliczSaldoFirma();
                if (opcja == 0 | opcja == 2)
                    wynik += c.PodliczSaldoOsoba();
                if (opcja == 3)
                    wynik += c.PodliczSaldoDuzaFirma() + c.PodliczSaldoWaznaOsoba();
                if (opcja == 4)
                    wynik += c.PodliczSaldoOsoba() - c.PodliczSaldoWaznaOsoba();
            }
            return wynik;
        }
        /*public double GetMajatekWszystkich()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
            {
                wynik += c.PodliczSaldoFirma();
                wynik += c.PodliczSaldoOsoba();
            }
            return wynik;
        }
        public double GetMajatekWszystkichFirm()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow) { wynik += c.PodliczSaldoFirma(); }
            return wynik;
        }
        public double GetMajatekWszystkichOsob()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow) { wynik += c.PodliczSaldoOsoba(); }
            return wynik;
        }
        public double GetMajatekDuzychFirmWaznychOsob()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
            {
                wynik += c.PodliczSaldoDuzaFirma();
                wynik += c.PodliczSaldoWaznaOsoba();
            }
            return wynik;
        }
        public double GetMajatekTYlkoZwyklychOsob()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
            {
                wynik += c.PodliczSaldoOsoba();
                wynik -= c.PodliczSaldoWaznaOsoba();
            }
            return wynik;
        }*/
        public double GetMajatekKonkretnejOsoby(string imie, string nazwisko, string pesel)
        {
            Osoba k_porowanie = new Osoba(imie, nazwisko, pesel);
            int index = listaKlientow.IndexOf((Klient)k_porowanie);
            double wynik = 0;
            if (index < listaKlientow.Count & index >= 0)
                foreach (Konto k in listaKlientow[index].GetKonta())
                    wynik += k.GetSaldo();
            else
                Console.WriteLine("Nie znaleziono Osoby o podanych danych");
            return wynik;
        }
        public double GetMajatekKonkretnejFirmy(string nazwa, string KRS)
        {
            Firma f_porowanie = new Firma(nazwa, KRS);
            int index = listaKlientow.IndexOf((Firma)f_porowanie);
            double wynik = 0;
            if (index < listaKlientow.Count & index >= 0)
                foreach (Konto k in listaKlientow[index].GetKonta())
                    wynik += k.GetSaldo();
            else
                Console.WriteLine("Nie znaleziono Firmy o podanych danych");
            return wynik;
        }
    }
    class Firma : Klient
    {
        private string nazwa;
        private string KRS;
        public Firma(string n, string k) { nazwa = n; KRS = k; }
        public string GetNazwa() { return nazwa; }
        public string GetKRS() { return KRS; }
        public void ZmienNazweFirmy(string n) { nazwa = n; }
        public override double PodliczSaldoFirma()
        {
            double wynik = 0;
            foreach (Konto f in GetKonta())
                wynik += f.GetSaldo();
            return wynik;
        }
        public override string Dane() { return $"{nazwa} {KRS}"; }
        public bool Equals(Firma inna)
        {
            return (inna != null) && (nazwa.Equals(inna.nazwa)) && (KRS.Equals(inna.KRS));
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Firma))
                return false;
            else
                return Equals(obj as Firma);
        }
        public override int GetHashCode() { return HashCode.Combine(nazwa, KRS); }
    }
    class DuzaFirma : Firma
    {
        public DuzaFirma(string n, string k) : base(n, k) { }
        public override double PodliczSaldoDuzaFirma()
        {
            double wynik = 0;
            foreach (Konto f in GetKonta())
                wynik += f.GetSaldo();
            return wynik;
        }
    }
    class Osoba : Klient
    {
        private string imie, nazwisko, PESEL;
        public Osoba(string i, string n, string p) { imie = i; nazwisko = n; PESEL = p; }
        public string GetImie() { return imie; }
        public string GetNazwisko() { return nazwisko; }
        public string GetPesel() { return PESEL; }
        public override double PodliczSaldoOsoba()
        {
            double wynik = 0;
            foreach (Konto f in GetKonta())
                wynik += f.GetSaldo();
            return wynik;
        }
        public override string Dane() { return $"{imie} {nazwisko} {PESEL}"; }
        public bool Equals(Osoba inna)
        {
            return (inna != null) && (imie.Equals(inna.imie)) && (nazwisko.Equals(inna.nazwisko)) && (PESEL.Equals(inna.PESEL));
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Osoba))
                return false;
            else
                return Equals(obj as Osoba);
        }
        public override int GetHashCode() { return HashCode.Combine(imie, nazwisko, PESEL); }
    }
    class WaznaOsoba : Osoba
    {
        public WaznaOsoba(string i, string n, string p) : base(i, n, p) { }
        public override double PodliczSaldoWaznaOsoba()
        {
            double wynik = 0;
            foreach (Konto f in GetKonta())
                wynik += f.GetSaldo();
            return wynik;
        }
    }
}


namespace Bank_klasy_is
{
    class Konto
    {
        private string nr;
        private double saldo;
        public Konto(string n) { nr = n; saldo = 0; }
        public double GetSaldo() { return saldo; }
        public string GetNr() { return nr; }
        public void Wplac(double kwota) { saldo += kwota; }
        public void Wyplac(double kwota) { saldo -= kwota; }
        /*public bool Wyplac(double kwota)
        {
            if (saldo >= kwota) saldo -= kwota;
            return saldo >= kwota;
        }*/
        public override string ToString() { return $"{GetType().Name}   Nr: {GetNr()}   Saldo: {GetSaldo()}"; }
    }
    abstract class Klient
    {
        private List<Konto> listaKont;
        public Klient() { listaKont = new List<Konto>(); }
        public void DodajKonto(Konto kon) { listaKont.Add(kon); }
        public List<Konto> GetKonta() { return listaKont; }
        public override string ToString()
        {
            string napis = "";
            for (int i = 0; i < listaKont.Count; i++)
                napis += $"   {(char)(i+97)}) { listaKont[i]}\n";
            return napis;
        }
    }
    class Bank
    {
        private List<Klient> listaKlientow;
        public Bank() { listaKlientow = new List<Klient>(); }
        public void DodajKlienta(Klient klin) { listaKlientow.Add(klin); }
        public List<Klient> GetKlienci() { return listaKlientow; }
        public override string ToString()
        {
            string napis = "";
            /*for (int i = 0; i < listaKlientow.Count; i++)
                napis += $"Nr.{i} - {listaKlientow[i].GetType().Name}\n{ listaKlientow[i]}\n";*/
            for (int i = 0; i < listaKlientow.Count; i++)
            {
                Klient k = listaKlientow[i];
                napis += $"Nr.{i} - {k.GetType().Name}: " + 
                    ((k is Firma) ? $"{((Firma)k).GetNazwa()}     KRS: {((Firma)k).GetKRS()}" 
                    : $"{((Osoba)k).GetImie()} {((Osoba)k).GetNazwisko()}     Pesel: {((Osoba)k).GetPesel()}") + $"\n{ k}\n";
            }
            return napis;
        }
        public bool Warunek(Klient c, int opcja)
        {
            if (opcja == 0)
                return true;
            else if (opcja == 1)
                return c is Firma;
            else if (opcja == 2)
                return c is Osoba;
            else if (opcja == 3)
                return c is DuzaFirma | c is WaznaOsoba;
            else if (opcja == 4)
                return c is Osoba & !(c is WaznaOsoba);
            else
                return false;
        }
        public double GetMajatek(int opcja)
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
                if (Warunek(c, opcja))
                    foreach (Konto k in c.GetKonta())
                        wynik += k.GetSaldo();
            return wynik;
        }
        /*public double GetMajatekWszystkich() 
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
                foreach(Konto k in c.GetKonta())
                    wynik += k.GetSaldo();
            return wynik; 
        }
        public double GetMajatekWszystkichFirm()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
                if (c is Firma)
                    foreach (Konto k in c.GetKonta())
                        wynik += k.GetSaldo();
            return wynik;
        }
        public double GetMajatekWszystkichOsob()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
                if (c is Osoba)
                    foreach (Konto k in c.GetKonta())
                        wynik += k.GetSaldo();
            return wynik;
        }
        public double GetMajatekDuzychFirmWaznychOsob()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
                if (c is DuzaFirma | c is WaznaOsoba)
                    foreach (Konto k in c.GetKonta())
                        wynik += k.GetSaldo();
            return wynik;
        }
        public double GetMajatekTYlkoZwyklychOsob()
        {
            double wynik = 0;
            foreach (Klient c in listaKlientow)
                if (c is Osoba & !(c is WaznaOsoba))
                    foreach (Konto k in c.GetKonta())
                        wynik += k.GetSaldo();
            return wynik;
        }*/
        public double GetMajatekKonkretnejOsoby(string imie, string nazwisko, string pesel)
        {
            Osoba k_porowanie = new Osoba(imie, nazwisko, pesel);
            int index = listaKlientow.IndexOf((Klient)k_porowanie);
            double wynik = 0;
            if (index < listaKlientow.Count & index >= 0)
                foreach (Konto k in listaKlientow[index].GetKonta())
                    wynik += k.GetSaldo();
            else
                Console.WriteLine("Nie znaleziono Osoby o podanych danych");
            return wynik;
        }
        public double GetMajatekKonkretnejFirmy(string nazwa, string KRS)
        {
            Firma f_porowanie = new Firma(nazwa, KRS);
            int index = listaKlientow.IndexOf((Firma)f_porowanie);
            double wynik = 0;
            if (index < listaKlientow.Count & index >= 0)
                foreach (Konto k in listaKlientow[index].GetKonta())
                    wynik += k.GetSaldo();
            else
                Console.WriteLine("Nie znaleziono Firmy o podanych danych");
            return wynik;
        }
    }
    class Firma : Klient
    {
        private string nazwa;
        private string KRS;
        public Firma(string n, string k) { nazwa = n; KRS = k; }
        public string GetNazwa() { return nazwa; }
        public string GetKRS() { return KRS; }
        public void ZmienNazweFirmy(string n) { nazwa = n; }
        public override string ToString()
        {
            return "\nPOTEZNA FIRMA\n" + base.ToString();
        }
        public bool Equals(Firma inna)
        {
            return (inna != null) && (nazwa.Equals(inna.nazwa)) && (KRS.Equals(inna.KRS));
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Firma))
                return false;
            else
                return Equals(obj as Firma);
        }
        public override int GetHashCode() { return HashCode.Combine(nazwa, KRS); }
    }
    class DuzaFirma : Firma
    {
        public DuzaFirma(string n, string k) : base(n, k) { }
    }
    class Osoba : Klient
    {
        private string imie, nazwisko, PESEL;
        public Osoba(string i, string n, string p) { imie = i; nazwisko = n; PESEL = p; }
        public string GetImie() { return imie; }
        public string GetNazwisko() { return nazwisko; }
        public string GetPesel() { return PESEL; }
        public bool Equals(Osoba inna)
        {
            return (inna != null) && (imie.Equals(inna.imie)) && (nazwisko.Equals(inna.nazwisko)) && (PESEL.Equals(inna.PESEL));
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Osoba))
                return false;
            else
                return Equals(obj as Osoba);
        }
        public override int GetHashCode() { return HashCode.Combine(imie, nazwisko, PESEL); }
    }
    class WaznaOsoba : Osoba
    {
        public WaznaOsoba(string i, string n, string p) : base(i, n, p) { }
    }
}


namespace Zadanie8_bank
{
    //using Bank_klasy_is;
    using Bank_klasy_virtual;
    class Program
    {
        static private void WcisnijKlawisz()
        {
            Console.Write("\nNacisnij dowolny klawisz...");
            Console.ReadKey();
        }
        static private string WczytajStr(string komunikat)
        {
            Console.Write(komunikat);
            string s = Console.ReadLine();
            return s;
        }
        static private int WczytajInt(string komunikat)
        {
            while (true)
            {
                Console.Write(komunikat);
                string str = Console.ReadLine();
                if (int.TryParse(str, out int numer))
                    return numer;
                else
                    Console.WriteLine($"{str} nie jest numerem");
            }
        }
        static private void StworzKlientowBanku(Bank b)
        {
            Konto k1 = new Konto("01");
            Konto k2 = new Konto("02");
            Konto k3 = new Konto("03");
            Konto k4 = new Konto("04");
            Konto k5 = new Konto("05");
            Konto k6 = new Konto("06");
            Konto k7 = new Konto("07");
            Konto k8 = new Konto("08");
            Konto k9 = new Konto("09");
            Konto k10 = new Konto("10");
            k1.Wplac(1);
            k2.Wplac(20);
            k3.Wplac(300);
            k4.Wplac(4000);
            k5.Wplac(50000);
            k6.Wplac(600000);
            k7.Wplac(7000000);
            k8.Wplac(80000000);
            k9.Wplac(900000000);
            k10.Wplac(1000000000);
            Osoba o1 = new Osoba("Adam", "White", "11111111111");
            o1.DodajKonto(k1);
            Osoba o2 = new Osoba("John", "Black", "22222222222");
            o2.DodajKonto(k2);
            WaznaOsoba wo = new WaznaOsoba("Bill", "Gates", "nr_peselu_utajniony");
            wo.DodajKonto(k3);
            wo.DodajKonto(k4);
            wo.DodajKonto(k5);
            Firma f1 = new Firma("pizzeria Vita", "123456789");
            f1.DodajKonto(k6);
            f1.DodajKonto(k7);
            Firma f2 = new Firma("piekarnia Bolka", "987654321");
            f2.DodajKonto(k8);
            DuzaFirma df1 = new DuzaFirma("Facebook", "123123123");
            df1.DodajKonto(k9);
            DuzaFirma df2 = new DuzaFirma("Apple", "321321321");
            df2.DodajKonto(k10);
            b.DodajKlienta((Klient)o1);
            b.DodajKlienta((Klient)o2);
            b.DodajKlienta((Klient)wo);
            b.DodajKlienta((Klient)f1);
            b.DodajKlienta((Klient)f2);
            b.DodajKlienta((Klient)df1);
            b.DodajKlienta((Klient)df2);
            /*Konto k11 = new Konto("11");
            Konto k12 = new Konto("12");
            k11.Wplac(10);
            k12.Wplac(30);
            Osoba o3 = new Osoba("I", "N", "P");
            o3.DodajKonto(k11);
            o3.DodajKonto(k12);
            b.DodajKlienta((Klient)o3);
            Konto k13 = new Konto("13");
            Konto k14 = new Konto("14");
            Konto k15 = new Konto("15");
            k13.Wplac(10000000000);
            k14.Wplac(20000000000);
            k15.Wplac(50000000000);
            DuzaFirma df3 = new DuzaFirma("Kompania Wschodnioindyjska", "X");
            df3.DodajKonto(k13);
            df3.DodajKonto(k14);
            df3.DodajKonto(k15);
            b.DodajKlienta((Klient)df3);*/
        }
        static private void WczytajDaneDodajKlienta(Bank b, int t)
        {
            if (t == 1 | t == 2)
            {
                string n = WczytajStr("Podaj nazwe firmy: ");
                string k = WczytajStr("Podaj KRS firmy: ");
                if (t == 1)
                    DodajKlientaFirma(b, n, k);
                else
                    DodajKlientaDuzaFirma(b, n, k);
            }
            else if (t == 3 | t == 4)
            {
                string i = WczytajStr("Podaj imie osoby: ");
                string n = WczytajStr("Podaj nazwisko osoby: ");
                string p = WczytajStr("Podaj pesel osoby: ");
                if (t == 3)
                    DodajKlientaOsoba(b, i, n, p);
                else
                    DodajKlientaWaznaOsoba(b, i, n, p);
            }
            else
                Console.WriteLine("Nie ma takiej opcji\n");
        }
        static private void DodajKlientaFirma(Bank b, string n, string k)
        {
            Firma f = new Firma(n, k);
            b.DodajKlienta(f);
        }
        static private void DodajKlientaDuzaFirma(Bank b, string n, string k)
        {
            DuzaFirma df = new DuzaFirma(n, k);
            b.DodajKlienta(df);
        }
        static private void DodajKlientaOsoba(Bank b, string i, string n, string p)
        {
            Osoba o = new Osoba(i, n, p);
            b.DodajKlienta(o);
        }
        static private void DodajKlientaWaznaOsoba(Bank b, string i, string n, string p)
        {
            WaznaOsoba wo = new WaznaOsoba(i, n, p);
            b.DodajKlienta(wo);
        }
        static private void DodajKontoKlientowi(Bank b, int index)
        {
            if (index < 0 || index > b.GetKlienci().Count-1)
            {
                Console.WriteLine("Index przekroczyl dostepny zakres");
                WcisnijKlawisz();
                return;
            }
            if (b.GetKlienci()[index].GetKonta().Count == 3)
            {
                Console.WriteLine($"\nKlient o indexie {index} ma juz 3 konta i nie mozna dodac nowego.\n");
                WcisnijKlawisz();
                return;
            }
            string nr_konta = WczytajStr("Podaj nr nowego konta: ");
            Konto kont = new Konto(nr_konta);
            b.GetKlienci()[index].DodajKonto(kont);
        }

        static private bool MainMenu(Bank b)
        {
            Console.WriteLine("Opcje:\nW - wyswietl wszystkich klientow i wypisz majatki grup\n" +
                "K - Dodaj Klienta\nD - Dodaj Konto dla Klienta\nP - wplac pieniadze na konto\n" +
                "S - Sprawdz majatek konkretnej osoby\nF - Sprawdz majatek konkretnej firmy\nZ - Zakoncz");
            ConsoleKeyInfo cs = Console.ReadKey(true);
            switch (cs.Key)
            {
                case ConsoleKey.W:
                    Console.Clear();
                    Console.WriteLine(b);
                    Console.WriteLine("Majatek wszystkich wynosi: " + b.GetMajatek(0));
                    Console.WriteLine("Majatek wszystkich firm wynosi: " + b.GetMajatek(1));
                    Console.WriteLine("Majatek wszystkich osob wynosi: " + b.GetMajatek(2));
                    Console.WriteLine("Majatek duzych firm i waznych osob wynosi: " + b.GetMajatek(3));
                    Console.WriteLine("Majatek tylko zwyklych osob wynosi: " + b.GetMajatek(4));
                    WcisnijKlawisz();
                    return true;
                case ConsoleKey.K:
                    Console.Clear();
                    int typ = WczytajInt("Kim jest klient:\n1 - Zwykla Firma\n2 - Duza Firma\n3 - Zwykla Osoba\n4 - Wazna Osoba\n");
                    WczytajDaneDodajKlienta(b, typ);
                    DodajKontoKlientowi(b, b.GetKlienci().Count-1);
                    return true;
                case ConsoleKey.D:
                    Console.Clear();
                    Console.WriteLine(b);
                    int index = WczytajInt("Podaj nr. (z listy) klienta, ktoremu chcesz dodac konto: ");
                    DodajKontoKlientowi(b, index);
                    return true;
                case ConsoleKey.P:
                    Console.Clear();
                    Console.WriteLine(b);
                    int wplataindex = WczytajInt("Podaj nr. (z listy) klienta: ");
                    if (wplataindex > b.GetKlienci().Count - 1 | wplataindex < 0)
                    {
                        Console.WriteLine("Nie ma klienta o podanym indexie.");
                        WcisnijKlawisz();
                        return true;
                    }
                    int nrkontaindex = WczytajInt("Podaj index konta klienta (a - 0; b - 1; c - 2): ");
                    if (nrkontaindex > b.GetKlienci()[wplataindex].GetKonta().Count - 1 | nrkontaindex < 0)
                    {
                        Console.WriteLine("Klient nie posiada konta o podanym indexie.");
                        WcisnijKlawisz();
                        return true;
                    }
                    int wplatakwota = WczytajInt("Ile pieniedzy chcesz wplacic: ");
                    b.GetKlienci()[wplataindex].GetKonta()[nrkontaindex].Wplac(wplatakwota);
                    return true;
                case ConsoleKey.S:
                    Console.Clear();
                    string i = WczytajStr("Podaj imie osoby: ");
                    string n = WczytajStr("Podaj nazwisko osoby: ");
                    string p = WczytajStr("Podaj pesel osoby: ");
                    double majatekKonkretnejOsoby = b.GetMajatekKonkretnejOsoby(i, n, p);
                    if(majatekKonkretnejOsoby != 0)
                        Console.WriteLine($"Majatek Osoby o podanych danych wynosi: {majatekKonkretnejOsoby}");
                    WcisnijKlawisz();
                    return true;
                case ConsoleKey.F:
                    Console.Clear();
                    string nazw = WczytajStr("Podaj nazwe firmy: ");
                    string k = WczytajStr("Podaj KRS firmy: ");
                    double majatekKonkretnejFirmy = b.GetMajatekKonkretnejFirmy(nazw,k);
                    if(majatekKonkretnejFirmy != 0)
                        Console.WriteLine($"Majatek Firmy o podanych danych wynosi: {majatekKonkretnejFirmy}");
                    WcisnijKlawisz();
                    return true;
                case ConsoleKey.Z:
                    return false;
                default:
                    return false;
            }
        }
        static void Main(string[] args)
        {
            Bank b1 = new Bank();
            StworzKlientowBanku(b1);
            do
            {
                Console.Clear();
            } while (MainMenu(b1));
        }
    }
}
