using System.Numerics;

namespace Conde.Ramsey;

/// <summary>
/// O juiz pela definição: ele olha TODOS os subconjuntos de vértices do tamanho
/// pedido e pergunta se aquele punhado está todo ligado, ou todo desligado. Não
/// tem poda, não tem esperteza e não sabe o que é Ramsey.
/// </summary>
public static class Juiz
{
    /// <summary>Existe um grupo de k vértices em que todos se conhecem?</summary>
    public static bool TemClique(Grafo grafo, int k)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        return PrimeiroClique(grafo, k) is not null;
    }

    /// <summary>Existe um grupo de k vértices em que ninguém se conhece?</summary>
    public static bool TemIndependente(Grafo grafo, int k)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        return TemClique(grafo.Complemento(), k);
    }

    /// <summary>O primeiro grupo de k vértices todos ligados, ou null.</summary>
    public static int[]? PrimeiroClique(Grafo grafo, int k)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        if (k <= 1) return k == 1 ? [0] : [];

        var escolhidos = new int[k];

        bool Passo(int indice, int comeco)
        {
            if (indice == k) return true;

            for (var v = comeco; v < grafo.Vertices; v++)
            {
                var vale = true;
                for (var i = 0; i < indice && vale; i++)
                    if (!grafo.TemAresta(escolhidos[i], v))
                        vale = false;

                if (!vale) continue;
                escolhidos[indice] = v;
                if (Passo(indice + 1, v + 1)) return true;
            }

            return false;
        }

        return Passo(0, 0) ? escolhidos : null;
    }

    /// <summary>
    /// Quantos triângulos são de uma cor só: três que se conhecem, ou três
    /// estranhos entre si.
    /// </summary>
    public static int TriangulosMonocromaticos(Grafo grafo)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        var total = 0;
        for (var a = 0; a < grafo.Vertices; a++)
            for (var b = a + 1; b < grafo.Vertices; b++)
                for (var c = b + 1; c < grafo.Vertices; c++)
                {
                    var ab = grafo.TemAresta(a, b);
                    var ac = grafo.TemAresta(a, c);
                    var bc = grafo.TemAresta(b, c);
                    if (ab == ac && ac == bc) total++;
                }
        return total;
    }

    /// <summary>
    /// Nem clique de s nem independente de t: é o que um grafo precisa para
    /// testemunhar que o número de Ramsey é maior que o número de vértices dele.
    /// </summary>
    public static bool EhTestemunha(Grafo grafo, int s, int t) =>
        !TemClique(grafo, s) && !TemIndependente(grafo, t);

    /// <summary>
    /// O mesmo clique achado por máscaras, para ser comparado com o jeito burro.
    /// Dois caminhos independentes para a mesma pergunta.
    /// </summary>
    public static bool TemCliquePorMascara(Grafo grafo, int k)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        if (k <= 1) return k <= grafo.Vertices;

        var todos = (1 << grafo.Vertices) - 1;

        bool Passo(int candidatos, int faltam)
        {
            if (faltam == 0) return true;
            if (BitOperations.PopCount((uint)candidatos) < faltam) return false;

            var restantes = candidatos;
            while (restantes != 0)
            {
                var v = BitOperations.TrailingZeroCount(restantes);
                restantes &= restantes - 1;
                if (Passo(candidatos & grafo.Vizinhos(v) & ~((1 << (v + 1)) - 1), faltam - 1)) return true;
            }

            return false;
        }

        return Passo(todos, k);
    }
}
