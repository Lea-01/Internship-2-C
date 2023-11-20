using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

Proizvod nekiProizvod = new Proizvod("Limun", new DateTime(2023,12,24), 43.0);
while (true)
{
    Console.WriteLine("1 - Artikli\n2 - Radnici\n3 - Racuni\n0 - Izlaz iz aplikacije");
    var pocetniIzbornik = Console.ReadLine();
    Console.Clear();
    if (pocetniIzbornik == "0")
        break;
    int.TryParse(Console.ReadLine(), out int odluka);
    switch (odluka)
    {
        case 1:
            Artikli();
            break;
        case 2:
            Employees();
            break;
        case 3:
            Bills();
            break;
        default:
            Console.WriteLine("Pogrešno uneseno! Pokušajte ponovno.");
            break;
    }

}
static void Employees()
{
    while (true)
    { 
        Console.WriteLine("1 - Unos radnika\n2 - Brisanje radnika\n3 - Ispis\n-1 - Vraćanje na početni izbornik");
        int.TryParse(Console.ReadLine(), out int odluka);
        if (odluka == -1)
            break;
        switch (odluka)
        {
            case 1:
                Console.WriteLine("Unesite ime i prezime radnika");
                var nameSurname = Console.ReadLine();
                Console.WriteLine("Unesite datum rođenja radnika(DD/MM/YYYY): ");
                DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth);
                Radnik newWorker = new Radnik(nameSurname, dateOfBirth);
                newWorker.unesiRadnika(nameSurname, dateOfBirth);
                break;
            case 2:
                Console.WriteLine("a - Brisanje artikla po imenu\nb - Brisanje svih radnika starijih od 65 ");
                var deleteChoice = Console.ReadLine();
                switch(deleteChoice)
                {
                    case "a":
                        Console.WriteLine("Unesite ime i prezime radnika");
                        var workerName = Console.ReadLine();
                        Radnik someWorker = new Radnik("", new DateTime());
                        someWorker.brisanjeImenom(workerName);
                        break;
                    case "b":
                        Radnik worker = new Radnik("", new DateTime());
                        worker.brisanjeStarih();
                        break;
                }
                break;
            case 3:
                Console.WriteLine("a - svih radnika\nb - svih radnika kojima je rođendan u tekućem mjesecu");
                var printWorkers = Console.ReadLine();
                switch (printWorkers)
                {
                    case "a":
                        Radnik worker = new Radnik("", new DateTime());
                        worker.ispisRadnika();
                        break;
                    case "b":
                        Radnik someWorker = new Radnik("", new DateTime());
                        someWorker.ispisPoRodendanima();
                        break;
                }
                break;
        }
    }
}
static void Artikli()
{
    while (true)
    {
        Console.WriteLine("1 - Unos artikla\n2 - Brisanje artikla\n3 - Uređivanje artikla\n4 - Ispis\n-1 - Vraćanje na početni izbornik");
        int.TryParse(Console.ReadLine(), out int odluka);
        if (odluka == -1)
            break;
        switch (odluka)
        {
            case 1:
                Console.WriteLine("Unesite ime artikla: ");
                var naziv = Console.ReadLine();
                Console.WriteLine("Unesite količinu artikla: ");
                bool k = int.TryParse(Console.ReadLine(), out int kolicina);
                Console.WriteLine("Unesite datum isteka artikla(DD/MM/YYYY): ");
                bool d = DateTime.TryParse(Console.ReadLine(), out DateTime datumIsteka);
                Console.WriteLine("Unesite cijenu artikla: ");
                bool c = double.TryParse(Console.ReadLine(), out double cijena);
                if (c && d && k)
                {
                    Proizvod nekiProizvod = new Proizvod(naziv, datumIsteka, cijena);
                    Trgovina trgovina = new Trgovina();
                    trgovina.neprodano(nekiProizvod, kolicina);
                }
                else
                    Console.WriteLine("Pogrešno je unesen jedan podataka. Pokušajte ponovno!");
                break;
            case 2:
                Console.WriteLine("a - Brisanje artikla po imenu\nb - Brisanje svih proizvoda kojima je istekao rok trajanja ");
                var izborBrisanja = Console.ReadLine();
                switch (izborBrisanja)
                {
                    case "a":
                        Console.WriteLine("Unesite naziv artikla:");
                        var ime = Console.ReadLine();
                        Trgovina trgovina = new Trgovina();
                        trgovina.brisanjeNazivom(ime);
                        break;
                    case "b":
                        Trgovina novaTrgovina = new Trgovina();
                        novaTrgovina.brisanjeDatumom();
                        break;
                    default:
                        Console.WriteLine("Pogrešno uneseno")
                            ; break;
                }
                break;
            case 3:
                Console.WriteLine("a - Zasebno proizvoda po imenu\nb - Popopust/poskupljenje na sve proizvode unutar trgovine ");
                var izborUredivanja = Console.ReadLine();
                switch (izborUredivanja)
                {
                    case "a":
                        Console.WriteLine("Unesite naziv artikla: ");
                        var name = Console.ReadLine();
                        Console.WriteLine("a - datum isteka\nb - kolicinu\nc - cijenu ");
                        var urediArtikl = Console.ReadLine();
                        switch (urediArtikl)
                        {
                            case "a":
                                Console.WriteLine("Unesite datum isteka artikla(DD/MM/YYYY): ");
                                bool date = DateTime.TryParse(Console.ReadLine(), out DateTime datum);
                                Proizvod foundProizvod = Proizvod.FindProizvodByNaziv(Proizvod.proizvodiList, name);

                                if (foundProizvod != null)
                                {
                                    foundProizvod.updateDatum(datum);
                                }
                                break;
                            case "b":
                                Console.WriteLine("Unesite novu količinu tog artikla: ");
                                int.TryParse(Console.ReadLine(), out int newQuantity);
                                Proizvod thisProduct = Proizvod.FindProizvodByNaziv(Proizvod.proizvodiList, name);
                                Trgovina store = new Trgovina();
                                store.neprodano(thisProduct, newQuantity);
                                break;
                            case "c":
                                Console.WriteLine("Unesite novu cijenu tog artikla: ");
                                double.TryParse(Console.ReadLine(), out double newPrice);
                                Proizvod priceProduct = Proizvod.FindProizvodByNaziv(Proizvod.proizvodiList, name);

                                if (priceProduct != null)
                                {
                                    priceProduct.updateCijena(newPrice);
                                }
                                break;
                        }
                        break;

                    case "b":
                        Console.WriteLine("Napišite 'pojeftiniti' za smanjenje svih cijena, a 'poskupiti' za povećanje svih cijena");
                        var popust = Console.ReadLine();
                        switch (popust)
                        {
                            case "pojeftiniti":
                                Console.WriteLine("Napišite broj koliko posto želite smanjiti cijene:");
                                int.TryParse(Console.ReadLine(), out int decrease);
                                foreach (var obj in Proizvod.proizvodiList)
                                {
                                    obj.updateCijena((1 - decrease / 100) * obj.getCijena());
                                }
                                break;
                            case "poskupiti":
                                Console.WriteLine("Napišite broj koliko posto želite povećati cijene:");
                                int.TryParse(Console.ReadLine(), out int increase);
                                foreach (var obj in Proizvod.proizvodiList)
                                {
                                    obj.updateCijena((1 + increase / 100) * obj.getCijena());
                                }
                                break;
                        }
                        break;
                }
                break;
            case 4:
                Console.WriteLine("a. Ispis svih artikala\nb. Svih artikala sortirano po imenu\nc. Svih artikala sortirano po datumu silazno\nd. Svih artikala sortirano po datumu uzlazno\ne. Svih artikala sortirano po količini\nf. Najprodavaniji artikl\ng. Najmanje prodavan artikl");
                var outputChoice = Console.ReadLine();
                switch (outputChoice)
                {
                    case "a":
                        Trgovina store = new Trgovina();
                        foreach (var item in store.ostaliProizvodi)
                        {
                            var daysLeft = (DateTime.Now - item.Key.datumIsteka).Days;
                            if (daysLeft > 0)
                                Console.WriteLine($"{item.Key.naziv} ({item.Value} - {item.Key.getCijena} - {daysLeft} dana do isteka)");
                            else
                                Console.WriteLine($"{item.Key.naziv} ({item.Value} - {item.Key.getCijena} - {daysLeft} dana od isteka)");
                        }
                        break;
                    case "b":
                        Proizvod.proizvodiList.Sort();
                        Console.WriteLine(Proizvod.proizvodiList);
                        break;
                    case "c":
                        for (int i = 0; i < Proizvod.proizvodiList.Count - 1; i++)
                        {
                            for (int j = i; j < Proizvod.proizvodiList.Count; j++)
                            {
                                if (DateTime.Compare(Proizvod.proizvodiList[i].datumIsteka, Proizvod.proizvodiList[j].datumIsteka) < 0)
                                {
                                    var t = Proizvod.proizvodiList[j];
                                    Proizvod.proizvodiList[j] = Proizvod.proizvodiList[i];
                                    Proizvod.proizvodiList[i] = t;
                                }
                            }
                        }
                        Console.WriteLine(Proizvod.proizvodiList);
                        break;
                    case "d":
                        for (int i = 0; i < Proizvod.proizvodiList.Count - 1; i++)
                        {
                            for (int j = i; j < Proizvod.proizvodiList.Count; j++)
                            {
                                if (DateTime.Compare(Proizvod.proizvodiList[i].datumIsteka, Proizvod.proizvodiList[j].datumIsteka) < 0)
                                {
                                    var t = Proizvod.proizvodiList[j];
                                    Proizvod.proizvodiList[j] = Proizvod.proizvodiList[i];
                                    Proizvod.proizvodiList[i] = t;
                                }
                            }
                        }
                        Console.WriteLine(Proizvod.proizvodiList);
                        break;
                    case "e":
                        Trgovina newstore = new Trgovina();
                        var sortedDict = newstore.ostaliProizvodi.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                        foreach (var item in sortedDict)
                        {
                            Console.WriteLine($"{item.Key.naziv}");
                        }
                        break;
                    case "f":
                        var bought = new Dictionary<Proizvod, int>();
                        foreach (var item in Racun.bills)
                        {
                            foreach (var product in item.proizvodi)
                            {
                                bought[product.Key] += product.Value;
                            }
                        }
                        var max = bought.Values.Min();
                        foreach (var item in bought)
                        {
                            if (item.Value == max)
                            {
                                Console.WriteLine(item.Key.naziv);
                            }
                        }
                        break;
                    case "g":
                        var sold = new Dictionary<Proizvod, int>();
                        foreach (var item in Racun.bills)
                        {
                            foreach (var product in item.proizvodi)
                            {
                                sold[product.Key] += product.Value;
                            }
                        }
                        var min = sold.Values.Min();
                        foreach (var item in sold)
                        {
                            if (item.Value == min)
                            {
                                Console.WriteLine(item.Key.naziv);
                            }
                        }
                        break;

                }
                break;

            }

    
    }
}
static void Bills()
{
    while (true)
    {
        Console.WriteLine("1 - Unos novog računa\n2 - Ispis računa\n-1 - Vraćanje na početni izbornik");
        int.TryParse(Console.ReadLine(), out int odluka);
        if (odluka == -1)
            break;
        switch (odluka)
        {
            case 1:
                Racun newBill = new Racun();
                foreach (var product in Proizvod.proizvodiList)
                {
                    Console.WriteLine(product.naziv);
                }
                Console.WriteLine("Unesite ime proizvoda, a ako ste gotovi unesite 0");
                var item = Console.ReadLine();
                while (item != "0")
                {
                    Console.WriteLine("Unesite količinu proizvoda:");
                    int.TryParse(Console.ReadLine(), out int numberWanted);
                    Proizvod billProduct = Proizvod.FindProizvodByNaziv(Proizvod.proizvodiList, item);
                    newBill.dodajProizvod(billProduct, numberWanted);
                    Console.WriteLine("Unesite ime proizvoda, a ako ste gotovi unesite 0");
                    item = Console.ReadLine();
                }
                newBill.zakljucajRacun();
                break;
            case 2:
                Console.WriteLine("a. Ispis svih računa\ni - ispis racuna po id-u");
                var outputChoice = Console.ReadLine();
                switch (outputChoice)
                {
                    case "a":
                        foreach(var bill in Racun.bills)
                        {
                            Console.WriteLine($"{bill.id} - {bill.vrijemeIzdavanja} - {bill.ukupanIznos}");
                        }
                        break;
                    case "b":
                        Console.WriteLine("Upišite id");
                        int.TryParse(Console.ReadLine(), out int adress);
                        Racun foundBill = Racun.FindBillById(Racun.bills, adress);
                        foundBill.ispisRacuna(foundBill);
                        break;
                }
                break;

        }
    }
}
public class Proizvod
{
    public string naziv;
    public DateTime datumIsteka;
    private double cijena;
    public static List<Proizvod> proizvodiList = new List<Proizvod>();
    public Proizvod(string naziv, DateTime datumIsteka, double cijena)
    {
        this.naziv = naziv;
        this.datumIsteka = datumIsteka;
        this.cijena = cijena;
        proizvodiList.Add(this);
    }
    public string getNaziv()
    {
        return this.naziv;
    }
    public DateTime getDatumIsteka()
    {
        return this.datumIsteka;
    }
    public double getCijena()
    {
        return this.cijena;
    }
    public void updateCijena(double cijena)
    {
        this.cijena = cijena;
    }
    public void updateDatum(DateTime datumIsteka)
    {
        this.datumIsteka = datumIsteka;
    }
    public static Proizvod FindProizvodByNaziv(List<Proizvod> proizvodiList, string naziv)
    {
        return proizvodiList.Find(p => p.naziv == naziv);
    }
}
public class Racun
{
    public int id;
    static public int lastId;
    public Dictionary<Proizvod, int> proizvodi = new Dictionary<Proizvod,int>();
    public DateTime vrijemeIzdavanja;
    public double ukupanIznos;
    public double iznosProizvoda;
    public static List<Racun> bills = new List<Racun>();
    public Racun()
    {
        this.id = ++lastId;
    }
    public void dodajProizvod(Proizvod proizvod, int kolicina)
    {
        proizvodi[proizvod] = kolicina;
        this.iznosProizvoda = (double)kolicina * proizvod.getCijena();
    }
    public void iznosRacuna()
    {
        var iznos = 0.0;
        foreach (var proizvod_kolicina in proizvodi)
        {
            iznos += (double)proizvod_kolicina.Value * proizvod_kolicina.Key.getCijena();
        }
        this.ukupanIznos = iznos;
    }
    public void zakljucajRacun()
    {
        this.vrijemeIzdavanja = DateTime.Now;
        bills.Add(this);
    }
    public static Racun FindBillById(List<Racun> bills, int id)
    {
        return bills.Find(p => p.id == id);
    }

    public void ispisRacuna(Racun id)
    {
        Console.WriteLine($"{id.id} - {id.vrijemeIzdavanja}");
        foreach (var item  in id.proizvodi)
        {
            Console.WriteLine($"{item.Key} - {item.Value}");
        }
        Console.WriteLine(ukupanIznos);
    }
}
public class Radnik
{
    public string imePrezime = "";
    public DateTime datumRodenja;
    public static Dictionary<string, DateTime> radnici = new Dictionary<string, DateTime>();
    public Radnik(string imePrezime, DateTime datumRodenja)
    {
        this.imePrezime = imePrezime;
        this.datumRodenja = datumRodenja;
    }
    public void unesiRadnika(string imePrezime, DateTime datumRodenja)
    {
        radnici[imePrezime] = datumRodenja;
    }

    public void brisanjeImenom(string imePrezime)
    {
        radnici.Remove(imePrezime);
    }
    public void brisanjeStarih()
    {
        foreach (var radnik in radnici)
        {
            int dob = DateTime.Now.Year - radnik.Value.Year;
            if (dob >= 65)
            {
                radnici.Remove(radnik.Key);
            }
        }
    }
    public void ispisRadnika()
    {
        foreach(var radnik in radnici)
        {
            Console.WriteLine($"{radnik.Key} - {radnik.Value}");
        }
    }
    public void ispisPoRodendanima()
    {
        foreach( var radnik in radnici)
        {
            if(radnik.Value.Month == DateTime.Now.Month)
            {
                Console.WriteLine($"{radnik.Key} - {radnik.Value}");
            }
        }
    }
}

public class Trgovina
{
    public List<string> artikli = new List<string>();
    public Dictionary<Proizvod, int> ostaliProizvodi = new Dictionary<Proizvod, int>();
    public double iznosTrgovini;
    public Trgovina()
    {
   
    }
    public void neprodano(Proizvod proizvod, int kolicina)
    {
        ostaliProizvodi[proizvod] = kolicina;
    }
    public void updateKolicinu(Proizvod proizvod, int kupljeno)
    {
        ostaliProizvodi[proizvod] -= kupljeno;
    }
    public void updateNeprodano()
    {
        foreach (var proizvod in ostaliProizvodi)
        {
            if (proizvod.Value == 0)
            {
                ostaliProizvodi.Remove(proizvod.Key);
            }
        }
    }
    public void brisanjeNazivom(string ime)
    {
        foreach (var proizvod in ostaliProizvodi)
        {
            if (proizvod.Key.getNaziv() == ime)
            {
                ostaliProizvodi.Remove(proizvod.Key);
            }
        }
    }
    public void brisanjeDatumom()
    {
        foreach (var proizvod in ostaliProizvodi)
        {
            if (DateTime.Compare(proizvod.Key.getDatumIsteka(), DateTime.Now) < 0 )
            {
                ostaliProizvodi.Remove(proizvod.Key);
            }
        }
    }
    public void iznosNeprodanog()
    {
        foreach (var proizvod in ostaliProizvodi)
        {
            iznosTrgovini += proizvod.Key.getCijena() * proizvod.Value;
        }
    }
    
}
