using System.Numerics;

namespace Conde.Ramsey;

/// <summary>
/// Um grafo simples de até 16 vértices, guardado como uma lista de vizinhanças em
/// bits. Aqui ele serve para duas coisas ao mesmo tempo: as arestas são as
/// ligações vermelhas e as não-arestas são as azuis, que é como uma coloração de
/// duas cores vira um grafo só.
/// </summary>
public sealed class Grafo
{
    private readonly int[] _vizinhos;

    public Grafo(int vertices)
    {
        if (vertices is < 1 or > 16) throw new ArgumentOutOfRangeException(nameof(vertices));
        Vertices = vertices;
        _vizinhos = new int[vertices];
    }

    public int Vertices { get; }

    /// <summary>Os vizinhos de um vértice, como máscara de bits.</summary>
    public int Vizinhos(int v) => _vizinhos[v];

    public bool TemAresta(int a, int b) => (_vizinhos[a] >> b & 1) != 0;

    public void Liga(int a, int b)
    {
        if (a == b) throw new ArgumentException("laço não é aresta");
        _vizinhos[a] |= 1 << b;
        _vizinhos[b] |= 1 << a;
    }

    public void Desliga(int a, int b)
    {
        _vizinhos[a] &= ~(1 << b);
        _vizinhos[b] &= ~(1 << a);
    }

    public int Arestas
    {
        get
        {
            var total = 0;
            for (var v = 0; v < Vertices; v++) total += BitOperations.PopCount((uint)_vizinhos[v]);
            return total / 2;
        }
    }

    /// <summary>O grafo com as cores trocadas: o que era aresta vira não-aresta.</summary>
    public Grafo Complemento()
    {
        var saida = new Grafo(Vertices);
        var todos = (1 << Vertices) - 1;
        for (var v = 0; v < Vertices; v++)
            saida._vizinhos[v] = ~_vizinhos[v] & todos & ~(1 << v);
        return saida;
    }

    public Grafo Copia()
    {
        var saida = new Grafo(Vertices);
        Array.Copy(_vizinhos, saida._vizinhos, Vertices);
        return saida;
    }

    /// <summary>
    /// O grafo montado a partir do número da coloração: o bit i diz se a i-ésima
    /// aresta possível está presente. É assim que a varredura percorre todas as
    /// 2^C(n,2) colorações sem repetir nenhuma.
    /// </summary>
    public static Grafo DoNumero(int vertices, long numero)
    {
        var grafo = new Grafo(vertices);
        var bit = 0;
        for (var a = 0; a < vertices; a++)
            for (var b = a + 1; b < vertices; b++, bit++)
                if ((numero >> bit & 1) != 0)
                    grafo.Liga(a, b);
        return grafo;
    }

    /// <summary>Quantas colorações existem com n vértices: dois elevado ao número de pares.</summary>
    public static long Coloracoes(int vertices) => 1L << (vertices * (vertices - 1) / 2);

    public override string ToString()
    {
        var pares = new List<string>();
        for (var a = 0; a < Vertices; a++)
            for (var b = a + 1; b < Vertices; b++)
                if (TemAresta(a, b))
                    pares.Add($"{a}-{b}");
        return pares.Count == 0 ? "(sem aresta)" : string.Join(" ", pares);
    }
}
