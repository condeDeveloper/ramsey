using System.Diagnostics;

namespace Conde.Ramsey;

/// <summary>
/// As medidas do README. Os testes conferem o que estes métodos devolvem; o
/// medidor só imprime.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaTresTres(
        int Vertices, long Coloracoes, long Testemunhas, double Fracao, string Exemplo);

    public readonly record struct LinhaMotor(
        int Vertices, long Coloracoes, long NosPorVertice, double Vezes, long Testemunhas, bool Bate);

    public readonly record struct LinhaNumero(
        int S, int T, int Ramsey, int Teto, long NosAteAchar, double Segundos);

    public readonly record struct LinhaGoodman(
        int Vertices, long PelaFormula, long PelaVarredura, long Quantas, long Maximo);

    /// <summary>
    /// A pergunta original: seis pessoas numa festa, e três que se conhecem ou
    /// três estranhos. Em cinco pessoas dá para escapar; em seis não.
    /// </summary>
    public static List<LinhaTresTres> TresPorTres()
    {
        var linhas = new List<LinhaTresTres>();
        for (var n = 2; n <= 7; n++)
        {
            var contagem = Varredura.Cega(n, 3, 3);
            linhas.Add(new LinhaTresTres(
                n, contagem.Coloracoes, contagem.Testemunhas,
                (double)contagem.Testemunhas / contagem.Coloracoes, contagem.Exemplo));
        }
        return linhas;
    }

    /// <summary>
    /// A varredura cega contra a busca por vértice: o mesmo número de
    /// testemunhas, custos que não se parecem.
    /// </summary>
    public static List<LinhaMotor> Motores()
    {
        var linhas = new List<LinhaMotor>();
        for (var n = 2; n <= 7; n++)
        {
            var cega = Varredura.Cega(n, 3, 3);
            var porVertice = Varredura.PorVertice(n, 3, 3);
            linhas.Add(new LinhaMotor(
                n, cega.Coloracoes, porVertice.Nos,
                (double)cega.Coloracoes / porVertice.Nos,
                porVertice.Testemunhas,
                cega.Testemunhas == porVertice.Testemunhas));
        }
        return linhas;
    }

    /// <summary>
    /// Os números de Ramsey que ainda cabem numa varredura. R(4,4) = 18 não cabe
    /// e não vai caber: são 2^153 colorações.
    /// </summary>
    public static List<LinhaNumero> Numeros()
    {
        var linhas = new List<LinhaNumero>();
        foreach (var (s, t, teto) in new[] { (2, 2, 4), (2, 5, 8), (3, 3, 8), (3, 4, 10), (3, 5, 11), (4, 4, 10) })
        {
            var relogio = Stopwatch.StartNew();
            var numero = Varredura.NumeroOuZero(s, t, teto);
            relogio.Stop();
            var nos = numero == 0 ? 0 : Varredura.PorVertice(numero, s, t, paraNaPrimeira: true).Nos;
            linhas.Add(new LinhaNumero(s, t, numero, teto, nos, relogio.Elapsed.TotalSeconds));
        }
        return linhas;
    }

    /// <summary>A fórmula de Goodman contra a varredura, enquanto a varredura cabe.</summary>
    public static List<LinhaGoodman> Goodmans()
    {
        var linhas = new List<LinhaGoodman>();
        for (var n = 3; n <= 7; n++)
        {
            var (minimo, quantas, histograma) = Goodman.PorVarredura(n);
            linhas.Add(new LinhaGoodman(n, Goodman.Minimo(n), minimo, quantas, histograma.Length - 1));
        }
        return linhas;
    }

    /// <summary>
    /// A testemunha de cinco vértices, que é o pentágono: cada um conhece os dois
    /// vizinhos e é estranho aos dois de frente.
    /// </summary>
    public static (long Testemunhas, bool TodasSaoPentagono) OPentagono()
    {
        long testemunhas = 0;
        var todasSaoCiclo = true;

        for (long numero = 0; numero < Grafo.Coloracoes(5); numero++)
        {
            var grafo = Grafo.DoNumero(5, numero);
            if (!Juiz.EhTestemunha(grafo, 3, 3)) continue;
            testemunhas++;

            // Num pentágono todo vértice tem exatamente dois vizinhos.
            for (var v = 0; v < 5; v++)
                if (System.Numerics.BitOperations.PopCount((uint)grafo.Vizinhos(v)) != 2)
                    todasSaoCiclo = false;
        }

        return (testemunhas, todasSaoCiclo);
    }
}
