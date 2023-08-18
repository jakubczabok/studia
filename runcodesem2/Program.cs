internal class Program
{
    public static bool CzyWyplataWyzsza(IPremiowalny pracownik, double wartosc)
    {
        if (pracownik!=null)
        return (pracownik.ObliczPremie() > wartosc) ;
        else return false;
    }

    public static bool CzyWyplataNizsza(IPremiowalny pracownik, double wartosc)
    {
        return (pracownik.ObliczPremie() < wartosc);
    }

    public class Pracownik : IPremiowalny
    {
        protected string nazwisko;
        protected int rokZatrudnienia;
        protected double wynagrodzenie;

        public Pracownik(string nazwisko)
        {
            this.nazwisko = nazwisko;
            this.rokZatrudnienia = 2023;
            this.wynagrodzenie = 500;
        }

        public override string ToString()
        {
            return $"{nazwisko} {2023 - rokZatrudnienia} {wynagrodzenie}";
        }

        public void Zmien(int wynagrodzenie, int rok)
        {
            this.wynagrodzenie = wynagrodzenie;
            this.rokZatrudnienia = rok;
        }

        public virtual bool OtrzymujPremie()
        {
            return true;
        }

        public virtual double ObliczPremie()
        {
            if ((2023 - rokZatrudnienia) < 10)
            {
                return 0;
            }
            else
            {
                return wynagrodzenie * 0.8;
            }
        }
    }

    public interface IPremiowalny
    {
        public bool OtrzymujPremie();
        public double ObliczPremie();
    }

    public class Brygadzista : Pracownik
    {
        int poziom;
        public Brygadzista(string nazwisko):base(nazwisko)
        {
            base.rokZatrudnienia = 2023;
            this.poziom = 1;
        }

        public int Poziom
        {
             set { poziom = value; }
        }

        public override string ToString()
        {
            return $"{nazwisko} {2023 - rokZatrudnienia} {wynagrodzenie} {poziom}";
        }

        public override bool OtrzymujPremie()
        {
            return true;
        }

        public override double ObliczPremie()
        {
            if ((2023 - rokZatrudnienia) < 10)
            {
                return 0;
            }
            else if (poziom > 1)
            {
                return wynagrodzenie * 1.2;
            }
            else
            {
                return wynagrodzenie * 0.8;
            }
        }
    }
    public delegate bool SprawdzPremieHandler(IPremiowalny pracownik, double wartosc);

    public class Premie
    {
        Pracownik[] pracownicy;
        string nazwa;

        public Premie(string nazwa)
        {
            this.pracownicy = new Pracownik[] { null };
            this.nazwa = nazwa;
        }

        public void Dodaj(Pracownik pracownik)
        {
            if (pracownicy != null)
            {
                Array.Resize(ref pracownicy, pracownicy.Count() + 1);
                pracownicy[pracownicy.Count() - 1] = pracownik;
            }
            else
            {
                pracownicy = new Pracownik[1] { pracownik };
            }
        }

        public int ZliczPremie()
        {
            return pracownicy.Count();
        }

        public double WartoscPremii()
        {
            double suma = 0;
            for (int i = 0; i < pracownicy.Count(); i++)
            {
                suma += pracownicy[i].ObliczPremie();
            }
            return suma;
        }

        public int Zlicz(SprawdzPremieHandler kryterium, double wartosc)
        {
            int suma = 0;
            for (int i = 0; i < pracownicy.Count(); i++)
            {
               if (kryterium(pracownicy[i],wartosc))
                {
                    suma += 1;
                }
            }
            return suma;
        }


    }
    private static void Main(string[] args)
    {
        Pracownik p1 = new Pracownik("Czabok");
        p1.Zmien(10000, 2022);
        Pracownik p2 = new Pracownik("Nowak");
        p2.Zmien(1000, 2008);
        Brygadzista b1 = new Brygadzista("JJ");
        b1.Zmien(5000, 2022);
        Brygadzista b2 = new Brygadzista("Sochan");
        b2.Zmien(1000, 2008);
        Brygadzista b3 = new Brygadzista("Nigga");
        b3.Zmien(1000, 2007);
        b3.Poziom = 2;
        Premie premie = new Premie("premie");
        premie.Dodaj(p1);
        premie.Dodaj(p2);
        premie.Dodaj(b1);
        premie.Dodaj(b2);
        premie.Dodaj(b3);
        Console.WriteLine(premie.Zlicz(CzyWyplataWyzsza,1000));
    }
}