using Conde.Ramsey;
using Xunit;

namespace Conde.Ramsey.Testes;

/// <summary>Todo número do README está preso aqui.</summary>
public class MedidasTestes
{
    [Theory]
    [InlineData(2, 2L, 2L)]
    [InlineData(3, 8L, 6L)]
    [InlineData(4, 64L, 18L)]
    [InlineData(5, 1_024L, 12L)]
    [InlineData(6, 32_768L, 0L)]
    [InlineData(7, 2_097_152L, 0L)]
    public void CadaLinhaDaFesta(int vertices, long coloracoes, long escapam)
    {
        var linha = Medidas.TresPorTres().Single(l => l.Vertices == vertices);
        Assert.Equal(coloracoes, linha.Coloracoes);
        Assert.Equal(escapam, linha.Testemunhas);
    }

    [Fact]
    public void AFracaoQueEscapaDespencaAteZero()
    {
        var linhas = Medidas.TresPorTres();
        Assert.Equal(1.000, linhas.Single(l => l.Vertices == 2).Fracao, 3);
        Assert.Equal(0.281, linhas.Single(l => l.Vertices == 4).Fracao, 3);
        Assert.Equal(0.0117, linhas.Single(l => l.Vertices == 5).Fracao, 4);
        Assert.Equal(0.0, linhas.Single(l => l.Vertices == 6).Fracao, 10);
    }

    [Fact]
    public void AsDozeQueEscapamComCincoSaoTodasOPentagono()
    {
        var (testemunhas, todasSaoPentagono) = Medidas.OPentagono();
        Assert.Equal(12, testemunhas);
        Assert.True(todasSaoPentagono);
    }

    [Theory]
    [InlineData(2, 3L, 2L)]
    [InlineData(3, 11L, 6L)]
    [InlineData(4, 59L, 18L)]
    [InlineData(5, 347L, 12L)]
    [InlineData(6, 731L, 0L)]
    [InlineData(7, 731L, 0L)]
    public void CadaLinhaDosMotores(int vertices, long nosPorVertice, long testemunhas)
    {
        var linha = Medidas.Motores().Single(l => l.Vertices == vertices);
        Assert.Equal(nosPorVertice, linha.NosPorVertice);
        Assert.Equal(testemunhas, linha.Testemunhas);
        Assert.True(linha.Bate);
    }

    [Fact]
    public void AVantagemDaPodaSoApareceQuandoHaOQuePodar()
    {
        var linhas = Medidas.Motores();
        // Em quatro vértices a busca com poda abre QUASE o mesmo que a varredura
        // cega: quase tudo ainda é testemunha, então não há o que cortar.
        Assert.Equal(1.1, linhas.Single(l => l.Vertices == 4).Vezes, 1);
        Assert.Equal(2868.9, linhas.Single(l => l.Vertices == 7).Vezes, 1);
    }

    [Theory]
    [InlineData(2, 2, 2, 3L)]
    [InlineData(2, 5, 5, 31L)]
    [InlineData(3, 3, 6, 731L)]
    [InlineData(3, 4, 9, 6_478_595L)]
    public void CadaNumeroDeRamseyMedido(int s, int t, int valor, long nos)
    {
        var linha = Medidas.Numeros().Single(l => l.S == s && l.T == t);
        Assert.Equal(valor, linha.Ramsey);
        Assert.Equal(nos, linha.NosAteAchar);
    }

    [Fact]
    public void OsQueNaoCabemAparecemComoNaoCabem()
    {
        foreach (var (s, t) in new[] { (3, 5), (4, 4) })
        {
            var linha = Medidas.Numeros().Single(l => l.S == s && l.T == t);
            Assert.Equal(0, linha.Ramsey);
            Assert.True(linha.Teto > 0);
        }
    }

    [Fact]
    public void DeTresPorTresParaTresPorQuatroOCustoSobeOitoMilVezes()
    {
        var numeros = Medidas.Numeros();
        var tresTres = numeros.Single(l => l is { S: 3, T: 3 }).NosAteAchar;
        var tresQuatro = numeros.Single(l => l is { S: 3, T: 4 }).NosAteAchar;
        Assert.True(tresQuatro > 8_000 * tresTres);
    }

    [Theory]
    [InlineData(3, 0L, 6L, 1L)]
    [InlineData(4, 0L, 18L, 4L)]
    [InlineData(5, 0L, 12L, 10L)]
    [InlineData(6, 2L, 1_760L, 20L)]
    [InlineData(7, 4L, 11_340L, 35L)]
    public void CadaLinhaDeGoodman(int vertices, long minimo, long quantas, long maximo)
    {
        var linha = Medidas.Goodmans().Single(l => l.Vertices == vertices);
        Assert.Equal(minimo, linha.PelaFormula);
        Assert.Equal(minimo, linha.PelaVarredura);
        Assert.Equal(quantas, linha.Quantas);
        Assert.Equal(maximo, linha.Maximo);
    }

    [Fact]
    public void AFormulaDeGoodmanNuncaDiscordaDaVarredura()
    {
        foreach (var linha in Medidas.Goodmans())
            Assert.Equal(linha.PelaVarredura, linha.PelaFormula);
    }

    [Fact]
    public void OMinimoDeGoodmanContinuaSubindoOndeAVarreduraNaoAlcanca()
    {
        Assert.Equal(8, Goodman.Minimo(8));
        Assert.Equal(20, Goodman.Minimo(10));
        Assert.Equal(240, Goodman.Minimo(20));
        Assert.Throws<ArgumentOutOfRangeException>(() => Goodman.Minimo(0));
    }
}
