using Conde.Ramsey;
using Xunit;

namespace Conde.Ramsey.Testes;

public class JuizTestes
{
    private static Grafo Pentagono()
    {
        var g = new Grafo(5);
        for (var v = 0; v < 5; v++) g.Liga(v, (v + 1) % 5);
        return g;
    }

    [Fact]
    public void OTrianguloTemCliqueDeTres()
    {
        var g = new Grafo(3);
        g.Liga(0, 1);
        g.Liga(1, 2);
        g.Liga(0, 2);
        Assert.True(Juiz.TemClique(g, 3));
        Assert.False(Juiz.TemClique(g, 4));
    }

    [Fact]
    public void OGrafoVazioTemIndependenteDeTudo()
    {
        var g = new Grafo(6);
        Assert.True(Juiz.TemIndependente(g, 6));
        Assert.False(Juiz.TemClique(g, 2));
    }

    [Fact]
    public void OPentagonoNaoTemTrianguloNemTrioDeEstranhos()
    {
        var g = Pentagono();
        Assert.False(Juiz.TemClique(g, 3));
        Assert.False(Juiz.TemIndependente(g, 3));
        Assert.True(Juiz.EhTestemunha(g, 3, 3));
    }

    [Fact]
    public void OsDoisCaminhosParaCliqueConcordamEmTodoGrafoDeCinco()
    {
        for (long numero = 0; numero < Grafo.Coloracoes(5); numero++)
        {
            var g = Grafo.DoNumero(5, numero);
            for (var k = 1; k <= 5; k++)
                Assert.Equal(Juiz.TemClique(g, k), Juiz.TemCliquePorMascara(g, k));
        }
    }

    [Fact]
    public void OCliqueDevolvidoEhCliqueDeVerdade()
    {
        for (long numero = 0; numero < Grafo.Coloracoes(5); numero++)
        {
            var g = Grafo.DoNumero(5, numero);
            var clique = Juiz.PrimeiroClique(g, 3);
            if (clique is null) continue;

            Assert.Equal(3, clique.Length);
            Assert.Equal(3, clique.Distinct().Count());
            for (var i = 0; i < 3; i++)
                for (var j = i + 1; j < 3; j++)
                    Assert.True(g.TemAresta(clique[i], clique[j]));
        }
    }

    [Fact]
    public void OPentagonoTemZeroTriangulosDeUmaCorSo()
    {
        Assert.Equal(0, Juiz.TriangulosMonocromaticos(Pentagono()));
    }

    [Fact]
    public void NoGrafoCompletoTodoTrianguloEhMonocromatico()
    {
        var g = new Grafo(6);
        for (var a = 0; a < 6; a++)
            for (var b = a + 1; b < 6; b++)
                g.Liga(a, b);
        Assert.Equal(20, Juiz.TriangulosMonocromaticos(g));
    }

    [Fact]
    public void NoGrafoVazioTambemTodoTrianguloEhMonocromatico()
    {
        Assert.Equal(20, Juiz.TriangulosMonocromaticos(new Grafo(6)));
    }

    [Fact]
    public void EmSeisVerticesNaoExisteTestemunhaDeTresPorTres()
    {
        for (long numero = 0; numero < Grafo.Coloracoes(6); numero++)
            Assert.False(Juiz.EhTestemunha(Grafo.DoNumero(6, numero), 3, 3));
    }

    [Fact]
    public void TodoGrafoDeSeisTemPeloMenosDoisTriangulosMonocromaticos()
    {
        for (long numero = 0; numero < Grafo.Coloracoes(6); numero++)
            Assert.True(Juiz.TriangulosMonocromaticos(Grafo.DoNumero(6, numero)) >= 2);
    }
}
