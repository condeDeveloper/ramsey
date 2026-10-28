namespace Conde.Ramsey;

/// <summary>O que uma contagem achou e o que custou.</summary>
public readonly record struct Contagem(int Vertices, long Coloracoes, long Testemunhas, long Nos, string Exemplo);

/// <summary>
/// Dois jeitos de procurar a testemunha: a varredura cega, que monta as 2^C(n,2)
/// colorações uma por uma, e a busca por vértice, que desiste assim que o pedaço
/// já montado estragou.
/// </summary>
public static class Varredura
{
    /// <summary>
    /// Todas as colorações de K_n, uma por uma, contadas pelo juiz. Só vale até
    /// uns 7 vértices: em 8 já são 268 milhões de colorações.
    /// </summary>
    public static Contagem Cega(int vertices, int s, int t)
    {
        if (vertices is < 1 or > 8) throw new ArgumentOutOfRangeException(nameof(vertices));

        var total = Grafo.Coloracoes(vertices);
        long testemunhas = 0;
        var exemplo = string.Empty;

        for (long numero = 0; numero < total; numero++)
        {
            var grafo = Grafo.DoNumero(vertices, numero);
            if (!Juiz.EhTestemunha(grafo, s, t)) continue;
            testemunhas++;
            if (exemplo.Length == 0) exemplo = grafo.ToString();
        }

        return new Contagem(vertices, total, testemunhas, total, exemplo);
    }

    /// <summary>
    /// A busca que monta o grafo vértice a vértice: cada novo vértice escolhe com
    /// quais dos anteriores se liga, e se o pedaço já tem clique de s ou
    /// independente de t, o ramo morre ali.
    /// </summary>
    public static Contagem PorVertice(int vertices, int s, int t, bool paraNaPrimeira = false)
    {
        if (vertices is < 1 or > 16) throw new ArgumentOutOfRangeException(nameof(vertices));

        var grafo = new Grafo(vertices);
        long testemunhas = 0, nos = 0;
        var exemplo = string.Empty;

        bool Passo(int v)
        {
            if (v == vertices)
            {
                testemunhas++;
                if (exemplo.Length == 0) exemplo = grafo.ToString();
                return paraNaPrimeira;
            }

            for (var mascara = 0; mascara < 1 << v; mascara++)
            {
                nos++;
                for (var u = 0; u < v; u++)
                    if ((mascara >> u & 1) != 0)
                        grafo.Liga(u, v);

                var pedaco = Pedaco(grafo, v + 1);
                if (Juiz.EhTestemunha(pedaco, s, t) && Passo(v + 1)) return true;

                for (var u = 0; u < v; u++) grafo.Desliga(u, v);
            }

            return false;
        }

        Passo(0);
        return new Contagem(vertices, Grafo.Coloracoes(vertices), testemunhas, nos, exemplo);
    }

    private static Grafo Pedaco(Grafo grafo, int quantos)
    {
        var saida = new Grafo(quantos);
        for (var a = 0; a < quantos; a++)
            for (var b = a + 1; b < quantos; b++)
                if (grafo.TemAresta(a, b))
                    saida.Liga(a, b);
        return saida;
    }

    /// <summary>
    /// O número de Ramsey R(s,t): o menor n em que NENHUMA coloração de K_n
    /// escapa. A busca sobe de um em um e para no primeiro n sem testemunha.
    /// </summary>
    public static int Numero(int s, int t, int teto = 14)
    {
        for (var n = 1; n <= teto; n++)
            if (PorVertice(n, s, t, paraNaPrimeira: true).Testemunhas == 0)
                return n;
        throw new InvalidOperationException($"R({s},{t}) passa de {teto}");
    }

    /// <summary>
    /// O número de Ramsey, ou zero quando ele passa do teto. É o que separa os
    /// que cabem numa varredura dos que não cabem e não vão caber.
    /// </summary>
    public static int NumeroOuZero(int s, int t, int teto)
    {
        for (var n = 1; n <= teto; n++)
            if (PorVertice(n, s, t, paraNaPrimeira: true).Testemunhas == 0)
                return n;
        return 0;
    }
}
