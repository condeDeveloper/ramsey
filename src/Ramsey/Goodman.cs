namespace Conde.Ramsey;

/// <summary>
/// Saber que o triângulo monocromático existe é uma coisa; saber quantos são é
/// outra. Goodman achou em 1959 uma fórmula que dá o mínimo sem olhar para
/// coloração nenhuma, e aqui ela é confrontada com a varredura.
/// </summary>
public static class Goodman
{
    /// <summary>
    /// O mínimo de triângulos monocromáticos em K_n, pela fórmula de Goodman. A
    /// conta muda conforme o resto da divisão de n por 4, que é o tipo de detalhe
    /// que só aparece quando se tenta escrever a coisa toda numa linha.
    /// </summary>
    public static long Minimo(int vertices)
    {
        if (vertices < 1) throw new ArgumentOutOfRangeException(nameof(vertices));
        long n = vertices;
        return (n % 4) switch
        {
            0 => n * (n - 2) * (n - 4) / 24,
            1 => n * (n - 1) * (n - 5) / 24,
            3 => (n + 1) * (n - 3) * (n - 4) / 24,
            _ => n * (n - 2) * (n - 4) / 24,
        };
    }

    /// <summary>
    /// O mínimo achado na marra: todas as colorações de K_n, o número de
    /// triângulos monocromáticos de cada uma, e o menor deles. A fórmula não é
    /// usada aqui.
    /// </summary>
    public static (long Minimo, long Quantas, long[] Histograma) PorVarredura(int vertices)
    {
        if (vertices is < 1 or > 7) throw new ArgumentOutOfRangeException(nameof(vertices));

        var total = Grafo.Coloracoes(vertices);
        var maximo = vertices * (vertices - 1) * (vertices - 2) / 6;
        var histograma = new long[maximo + 1];
        var minimo = long.MaxValue;

        for (long numero = 0; numero < total; numero++)
        {
            var quantos = Juiz.TriangulosMonocromaticos(Grafo.DoNumero(vertices, numero));
            histograma[quantos]++;
            if (quantos < minimo) minimo = quantos;
        }

        return (minimo, histograma[minimo], histograma);
    }
}
