using Grafi;
using static Grafi.GrafoDiretto;

namespace Grafi
{
    //scopo del programma: implementare le due classi GrafoDiretto e GrafoNonDiretto per la creazione di nodi e archi, con la possibilità di aggiungere nodi, aggiungere archi, ottenere gli archi uscenti e gli archi entranti
    public class GrafoDiretto
    {
        public struct Arco
        {
            public int nodo_partenza;//nodo da dove parte l'arco
            public int nodo_arrivo;//nodo da dove arriva
            public int costo;//costo ovvero la distanza

            public Arco(int nodo_partenza, int nodo_arrivo, int costo)//costruttore della struct dell'arco
            {
                this.nodo_partenza = nodo_partenza;
                this.nodo_arrivo = nodo_arrivo;
                this.costo = costo;
            }
        }

        private int numero_max_nodi;//limite massimo di nodi
        private Dictionary<string, int> nomeToIndice = new Dictionary<string, int>();//utilizzo un dizionario per memorizzare i nodi e la loro posizione all'interno dell'arco
        private List<string> indiceToNome = new List<string>();//utilizzo una lista per memorizzare i nomi dei nodi
        private List<List<Arco>> archiUscenti = new List<List<Arco>>();//uso una lista per memorizzare gli archi uscenti
        private List<List<Arco>> archiEntranti = new List<List<Arco>>();//uso una lista per memorizzare gli archi entranti

        public GrafoDiretto(int numero_max_nodi = 1000)//costruttore della classe GrafoDiretto
        {
            this.numero_max_nodi = numero_max_nodi;
        }

        public int NumeroNodi => indiceToNome.Count;//utilizzo una lambda expression per ottenere il numero di nodi correnti

        public void AggiungiNodo(string nome)//aggiungo il nodo con il nome del nodo
        {
            if (nomeToIndice.ContainsKey(nome))//controllo che non ci sia già un nodo con lo stesso nome
                throw new ArgumentException($"Nodo '{nome}' già presente nel grafo.");

            if (NumeroNodi >= numero_max_nodi)//controllo di non aver superato il limite di nodi istanziabili
                throw new InvalidOperationException("Numero massimo di nodi raggiunto.");

            nomeToIndice[nome] = NumeroNodi;//metto il nodo alla prima posizione libera
            indiceToNome.Add(nome);//aggiungo il nome del nodo alla lista
            archiUscenti.Add(new List<Arco>());//aggiungo una nuova lista di archi uscenti
            archiEntranti.Add(new List<Arco>());//aggiungo una nuova lista di archi entranti
        }
        public virtual void AggiungiArco(string nome_nodo_partenza, string nome_nodo_arrivo, int costo)//aggiungo un arco dato il nome del nodo di arrivo, quello di partenza e il costo totale
        {
            if (!nomeToIndice.ContainsKey(nome_nodo_partenza) || !nomeToIndice.ContainsKey(nome_nodo_arrivo))//controllo che entrambi i nodi, sia di partenza che di arrivo esistano
                throw new ArgumentException("Uno o entrambi i nodi non sono presenti nel grafo.");

            int partenza = nomeToIndice[nome_nodo_partenza];//ottengo il valore intero dell'indice del nodo di partenza
            int arrivo = nomeToIndice[nome_nodo_arrivo];//ottengo il valore intero dell'indice del nodo di arrivo

            var arco = new Arco(partenza, arrivo, costo);//creo l'arco
            archiUscenti[partenza].Add(arco);//metto in archi uscenti l'arco partendo dal primo nodo dell'arco
            archiEntranti[arrivo].Add(arco);//metto in archi entranti l'arco partendo dall'ultimo nodo dell'arco
        }
        public virtual void AggiungiArco(Arco arco)//aggiungo un arco dato un arco di tipo Arco
        {
            if (arco.nodo_partenza >= NumeroNodi || arco.nodo_arrivo >= NumeroNodi)//controllo che che all'interno dell'arco il nodo di partenza e quello di arrivo siano minori rispetto al numero dei nodi totali
                throw new ArgumentException("Indici dei nodi non validi per il grafo corrente.");

            archiUscenti[arco.nodo_partenza].Add(arco);
            archiEntranti[arco.nodo_arrivo].Add(arco);
        }

        public string this[int nodo]//indicizzatore che ritorna il nodo stringa
        {
            get
            {
                if (nodo < 0 || nodo >= NumeroNodi)
                    throw new IndexOutOfRangeException("Indice nodo non valido.");
                return indiceToNome[nodo];
            }
        }

        public int this[string nome_nodo]//indicizzatore che ritorna il nodo intero
        {
            get
            {
                if (!nomeToIndice.ContainsKey(nome_nodo))
                    throw new KeyNotFoundException($"Nodo '{nome_nodo}' non trovato.");
                return nomeToIndice[nome_nodo];
            }
        }

        public IEnumerable<Arco> ArchiUscenti(int nodo)//enumeratore per gli archi uscenti dal nodo dato
        {
            if (nodo < 0 || nodo >= NumeroNodi)
                throw new IndexOutOfRangeException("Indice nodo non valido.");
            return archiUscenti[nodo];
        }

        public IEnumerable<Arco> ArchiEntranti(int nodo)//enumeratore per gli archi uscenti dal nodo dato
        {
            if (nodo < 0 || nodo >= NumeroNodi)
                throw new IndexOutOfRangeException("Indice nodo non valido.");
            return archiEntranti[nodo];
        }
    }

    public class GrafoNonDiretto : GrafoDiretto
    {
        public GrafoNonDiretto(int numero_max_nodi = 1000) : base(numero_max_nodi)//costruttore
        {
        }
        public override void AggiungiArco(string nome_nodo_partenza, string nome_nodo_arrivo, int costo)//metodo per aggiungere gli archi con i dati
        {
            base.AggiungiArco(nome_nodo_partenza, nome_nodo_arrivo, costo);
            base.AggiungiArco(nome_nodo_arrivo, nome_nodo_partenza, costo);
        }
        public override void AggiungiArco(Arco arco)//metodo per aggiungere l'arco già istanziato
        {
            base.AggiungiArco(arco);
            var arcoInvertito = new Arco(arco.nodo_arrivo, arco.nodo_partenza, arco.costo);
            base.AggiungiArco(arcoInvertito);
        }
    }
}