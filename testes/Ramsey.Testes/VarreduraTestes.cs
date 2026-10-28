using Conde.Ramsey;
using Xunit;

namespace Conde.Ramsey.Testes;

public class VarreduraTestes
{
    [Theory]
    [InlineData(2, 2L)]
    [InlineData(3, 6L)]
    [InlineData(4, 18L)]
    [InlineData(5, 12L)]
    [InlineData(6, 0L)]
    [InlineData(7, 0L)]
    public void QuantasColoracoesEscapamDoTrioMonocromatico(int vertices, long escapam)
    {
        Assert.Equal(escapam, Varredura.Cega(vertices, 3, 3).Testemunhas);
    }

    [Fact]
    public void OsDoisMotoresContamAsMesmasTestemunhas()
    {
        for (var n = 2; n <= 7; n++)
            Assert.Equal(Varredura.Cega(n, 3, 3).Testemunhas, Varredura.PorVertice(n, 3, 3).Testemunhas);
    }

    [Fact]
    public void TodaTestemunhaAchadaPassaPeloJuiz()
    {
        for (long numero = 0; numero < Grafo.Coloracoes(5); numero++)
        {
            var g = Grafo.DoNumero(5, numero);
            if (!Juiz.EhTestemunha(g, 3, 3)) continue;
            Assert.False(Juiz.TemClique(g, 3));
            Assert.False(Juiz.TemClique(g.Complemento(), 3));
        }
    }

    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(2, 5, 5)]
    [InlineData(3, 3, 6)]
    [InlineData(3, 4, 9)]
    public void OsNumerosDeRamseyQueCabemNumaVarredura(int s, int t, int esperado)
    {
        Assert.Equal(esperado, Varredura.Numero(s, t));
    }

    [Fact]
    public void RamseyEhSimetrico()
    {
        Assert.Equal(Varredura.Numero(3, 4), Varredura.Numero(4, 3));
    }

    [Fact]
    public void EmOitoVerticesAindaExisteTestemunhaDeTresPorQuatro()
    {
        // É o que faz R(3,4) ser 9 e não 8.
        Assert.True(Varredura.PorVertice(8, 3, 4, paraNaPrimeira: true).Testemunhas > 0);
        Assert.Equal(0, Varredura.PorVertice(9, 3, 4, paraNaPrimeira: true).Testemunhas);
    }

    [Fact]
    public void ABuscaPorVerticeEmSeteCustaOMesmoQueEmSeis()
    {
        // Ela nunca chega ao sétimo vértice: em seis já não sobrou nada vivo.
        Assert.Equal(Varredura.PorVertice(6, 3, 3).Nos, Varredura.PorVertice(7, 3, 3).Nos);
    }

    [Fact]
    public void OQueNaoCabeDevolveZeroEmVezDeExplodir()
    {
        Assert.Equal(0, Varredura.NumeroOuZero(4, 4, 10));
        Assert.Equal(6, Varredura.NumeroOuZero(3, 3, 10));
    }

    [Fact]
    public void PassarDoTetoNaVersaoQueNaoPerdoaExplode()
    {
        Assert.Throws<InvalidOperationException>(() => Varredura.Numero(4, 4, 8));
    }

    [Fact]
    public void NoveVerticesNaoCabemNaVarreduraCega()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Cega(9, 3, 3));
    }
}
