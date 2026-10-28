using Conde.Ramsey;

var comando = args.Length > 0 ? args[0] : "tudo";

if (comando is "festa" or "tudo") Festa();
if (comando is "pentagono" or "tudo") Pentagono();
if (comando is "motores" or "tudo") Motores();
if (comando is "numeros" or "tudo") Numeros();
if (comando is "goodman" or "tudo") Goodmans();

static void Festa()
{
    Console.WriteLine();
    Console.WriteLine("SEIS PESSOAS NUMA FESTA: tres que se conhecem ou tres estranhos. Todas as coloracoes.");
    Console.WriteLine();
    Console.WriteLine($"  {"pessoas",8} {"coloracoes",12} {"escapam",10} {"fracao",9}  uma que escapa");

    foreach (var l in Medidas.TresPorTres())
        Console.WriteLine($"  {l.Vertices,8} {l.Coloracoes,12:n0} {l.Testemunhas,10:n0} {l.Fracao,8:0.000%}  {(l.Exemplo.Length == 0 ? "-- nenhuma --" : l.Exemplo)}");

    Console.WriteLine();
}

static void Pentagono()
{
    Console.WriteLine();
    var (testemunhas, pentagonos) = Medidas.OPentagono();
    Console.WriteLine("AS QUE ESCAPAM COM CINCO PESSOAS:");
    Console.WriteLine();
    Console.WriteLine($"  coloracoes que escapam     {testemunhas}");
    Console.WriteLine($"  todas sao o pentagono      {(pentagonos ? "sim" : "nao")}");
    Console.WriteLine();
}

static void Motores()
{
    Console.WriteLine();
    Console.WriteLine("A VARREDURA CEGA contra A BUSCA POR VERTICE: mesma resposta, custo diferente.");
    Console.WriteLine();
    Console.WriteLine($"  {"vertices",9} {"coloracoes",12} {"nos por vertice",16} {"razao",8} {"escapam",9} {"bate",6}");

    foreach (var l in Medidas.Motores())
        Console.WriteLine($"  {l.Vertices,9} {l.Coloracoes,12:n0} {l.NosPorVertice,16:n0} {l.Vezes,7:0.0}x {l.Testemunhas,9:n0} {(l.Bate ? "sim" : "NAO"),6}");

    Console.WriteLine();
}

static void Numeros()
{
    Console.WriteLine();
    Console.WriteLine("OS NUMEROS DE RAMSEY que ainda cabem numa varredura.");
    Console.WriteLine();
    Console.WriteLine($"  {"R(s,t)",9} {"valor",12} {"nos ate provar",16} {"segundos",9}");

    foreach (var l in Medidas.Numeros())
        Console.WriteLine($"  {"R(" + l.S + "," + l.T + ")",9} {(l.Ramsey == 0 ? "passa de " + l.Teto : l.Ramsey.ToString()),12} {l.NosAteAchar,16:n0} {l.Segundos,9:0.00}");

    Console.WriteLine();
}

static void Goodmans()
{
    Console.WriteLine();
    Console.WriteLine("QUANTOS TRIANGULOS DE UMA COR SO: a formula de Goodman contra a varredura.");
    Console.WriteLine();
    Console.WriteLine($"  {"vertices",9} {"pela formula",13} {"pela varredura",15} {"coloracoes no minimo",21} {"maximo possivel",16}");

    foreach (var l in Medidas.Goodmans())
        Console.WriteLine($"  {l.Vertices,9} {l.PelaFormula,13:n0} {l.PelaVarredura,15:n0} {l.Quantas,21:n0} {l.Maximo,16:n0}");

    Console.WriteLine();
}
