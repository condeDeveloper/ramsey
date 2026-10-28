using Conde.Ramsey;
using Xunit;

namespace Conde.Ramsey.Testes;

public class GrafoTestes
{
    [Fact]
    public void OGrafoNasceSemAresta()
    {
        var g = new Grafo(5);
        Assert.Equal(0, g.Arestas);
        Assert.False(g.TemAresta(0, 1));
    }

    [Fact]
    public void LigarEhNosDoisSentidos()
    {
        var g = new Grafo(3);
        g.Liga(0, 2);
        Assert.True(g.TemAresta(0, 2));
        Assert.True(g.TemAresta(2, 0));
        Assert.Equal(1, g.Arestas);
    }

    [Fact]
    public void DesligarDesfaz()
    {
        var g = new Grafo(3);
        g.Liga(0, 1);
        g.Desliga(0, 1);
        Assert.Equal(0, g.Arestas);
    }

    [Fact]
    public void LacoNaoEhAresta()
    {
        Assert.Throws<ArgumentException>(() => new Grafo(3).Liga(1, 1));
    }

    [Fact]
    public void OComplementoTrocaAsCores()
    {
        var g = new Grafo(4);
        g.Liga(0, 1);
        var c = g.Complemento();

        Assert.False(c.TemAresta(0, 1));
        Assert.True(c.TemAresta(0, 2));
        Assert.Equal(6 - 1, c.Arestas);
    }

    [Fact]
    public void OComplementoDoComplementoEhOGrafoDeVolta()
    {
        for (long numero = 0; numero < 64; numero++)
        {
            var g = Grafo.DoNumero(4, numero);
            Assert.Equal(g.ToString(), g.Complemento().Complemento().ToString());
        }
    }

    [Fact]
    public void CadaNumeroDaUmGrafoDiferente()
    {
        var vistos = new HashSet<string>();
        for (long numero = 0; numero < 64; numero++)
            Assert.True(vistos.Add(Grafo.DoNumero(4, numero).ToString()));
        Assert.Equal(64, vistos.Count);
    }

    [Theory]
    [InlineData(2, 2L)]
    [InlineData(3, 8L)]
    [InlineData(4, 64L)]
    [InlineData(5, 1_024L)]
    [InlineData(6, 32_768L)]
    [InlineData(7, 2_097_152L)]
    public void QuantasColoracoesExistem(int vertices, long quantas)
    {
        Assert.Equal(quantas, Grafo.Coloracoes(vertices));
    }

    [Fact]
    public void ACopiaNaoCompartilhaAresta()
    {
        var g = new Grafo(3);
        var copia = g.Copia();
        copia.Liga(0, 1);
        Assert.Equal(0, g.Arestas);
    }

    [Fact]
    public void DezessetVerticesNaoCabem()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Grafo(17));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Grafo(0));
    }
}
