# Números de Ramsey

Numa festa com seis pessoas, há sempre três que se conhecem ou três que não se
conhecem. Com cinco, não. Isso é `R(3,3) = 6`, e aqui ele não é citado: as 32.768
maneiras de dividir as quinze duplas de uma festa de seis em "se conhecem" e "não
se conhecem" são montadas uma por uma, e nenhuma escapa.

C# e .NET 8, com o grafo guardado em bits: as arestas são as duplas que se
conhecem e as não-arestas são as que não, o que faz uma coloração de duas cores
caber num grafo só.

```bash
dotnet test testes/Ramsey.Testes/Ramsey.Testes.csproj -c Release    # 71 testes
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release  # todas as tabelas
```

## A festa

```
   pessoas   coloracoes    escapam    fracao  uma que escapa
         2            2          2 100.000%  (sem aresta)
         3            8          6  75.000%  0-1
         4           64         18  28.125%  0-3 1-2
         5        1,024         12   1.172%  0-3 0-4 1-2 1-4 2-3
         6       32,768          0   0.000%  -- nenhuma --
         7    2,097,152          0   0.000%  -- nenhuma --
```

Com quatro pessoas ainda é fácil escapar: 18 das 64 divisões não têm trio nenhum.
Com cinco são 12 em 1.024, **um por cento**. Com seis, zero, e a tabela nunca mais
tem outro número.

As doze que escapam com cinco pessoas são a mesma festa com os nomes trocados:

```
  coloracoes que escapam     12
  todas sao o pentagono      sim
```

Todas as doze são o **pentágono**: cada pessoa conhece exatamente duas, as duas
vizinhas, e é estranha às duas de frente. Doze é o número de jeitos de rotular um
ciclo de cinco pontos. **O único jeito de escapar é esse, e ele é tão apertado que
acrescentar uma pessoa destrói tudo.**

## O custo de provar

```
   vertices   coloracoes  nos por vertice    razao   escapam   bate
          2            2                3     0.7x         2    sim
          3            8               11     0.7x         6    sim
          4           64               59     1.1x        18    sim
          5        1,024              347     3.0x        12    sim
          6       32,768              731    44.8x         0    sim
          7    2,097,152              731  2868.9x         0    sim
```

Dois motores. A **varredura cega** monta as 2^C(n,2) colorações, uma por uma, e
pergunta ao juiz. A **busca por vértice** monta o grafo uma pessoa de cada vez: a
pessoa nova escolhe quem conhece entre as que já chegaram, e se o pedaço já tem
trio, o ramo morre ali.

Duas coisas nessa tabela.

Com quatro pessoas a poda não vale nada: 59 nós contra 64 colorações, **1,1x**.
Quase toda coloração ainda escapa, então não há o que podar. Poda só paga onde já
há o que cortar.

E a linha de sete é igual à de seis: **731 nós, o mesmo número**. A busca por
vértice nunca chega à sétima pessoa, porque em seis já não sobrou um único grafo
vivo para estender. A varredura cega, para dizer a mesma coisa, precisa montar
2.097.152 colorações. É a diferença entre provar e conferir.

## Os números que ainda cabem

```
     R(s,t)        valor   nos ate provar  segundos
     R(2,2)            2                3      0.00
     R(2,5)            5               31      0.00
     R(3,3)            6              731      0.00
     R(3,4)            9        6,478,595      1.64
     R(3,5)  passa de 11                0      0.00
     R(4,4)  passa de 10                0      0.00
```

De `R(3,3)` para `R(3,4)` o custo sobe **oito mil vezes**: de 731 nós para
6.478.595. Três pessoas a mais na pergunta.

`R(4,4) = 18` é conhecido desde 1955 e **não está aqui**, porque a busca não chega
lá. São 2^153 colorações de K18, e nenhuma varredura vai percorrer isso. `R(5,5)`
ninguém sabe: está entre 43 e 46, e Erdős dizia que, se alienígenas ameaçassem
destruir a Terra a menos que descobríssemos `R(5,5)`, deveríamos juntar todos os
computadores do mundo e tentar; mas se pedissem `R(6,6)`, seria melhor tentar
destruir os alienígenas.

## Quantos triângulos, e não se existe triângulo

Saber que o trio existe é uma coisa. Goodman perguntou em 1959 quantos são, e
achou uma fórmula que não olha para coloração nenhuma.

```
   vertices  pela formula  pela varredura  coloracoes no minimo  maximo possivel
          3             0               0                     6                1
          4             0               0                    18                4
          5             0               0                    12               10
          6             2               2                 1,760               20
          7             4               4                11,340               35
```

A fórmula e a varredura concordam em todas as linhas que a varredura alcança. E
elas dizem uma coisa mais forte que `R(3,3) = 6`: em seis pessoas não existe **um**
trio monocromático, existem pelo menos **dois**, e isso em todas as 32.768
divisões. O melhor que se consegue fazer é empatar em dois, e 1.760 colorações
conseguem.

A fórmula continua depois que a varredura para: 8 em oito pessoas, 20 em dez, 240
em vinte. O máximo possível em sete pessoas é 35, e o mínimo é 4: **por mais bem
dividida que esteja a festa, um nono dos trios é de uma cor só**.

## O que as medidas me corrigiram

**Escrevi 1.330 onde a fórmula dá 240.** No teste que estica a fórmula de Goodman
para onde a varredura não vai, pus um número de cabeça para `n = 20` e ele estava
errado por um fator de cinco. A conta é `20 × 18 × 16 / 24`. O teste quebrou e me
devolveu o 240, que é o ponto de um teste existir: eu não sabia o valor, e achava
que sabia.

**Achei que a poda pagasse desde o começo.** Com quatro pessoas a busca com poda
abre 59 nós e a varredura cega 64: economia de 8%. A poda não é uma propriedade do
algoritmo, é uma propriedade do problema. Onde quase tudo é resposta, não há o que
cortar, e a estrutura esperta vira peso morto.

**Precisei de dois tetos diferentes para a mesma tabela.** Tentei pôr `R(3,5)` e
`R(4,4)` na tabela como os outros, com um teto generoso. A busca de `R(3,5)` rodou
sete minutos sem terminar. Não dá para fingir que o método vai até onde a
literatura vai: a tabela passou a dizer "passa de 11", que é o que eu de fato
medi, em vez do valor que eu sei de ler.

**O juiz burro precisou de um segundo juiz.** Para ter certeza de que a busca por
clique não tinha um erro escondido, escrevi a mesma pergunta duas vezes: uma por
combinação de vértices e outra por máscara de bits e interseção de vizinhanças. As
duas concordam nas 1.024 colorações de cinco vértices e em cada tamanho de clique
de 1 a 5. Sem isso, "a varredura e o juiz concordam" não valeria nada: os dois
poderiam estar errados do mesmo jeito.

## Os arquivos

| arquivo | o que faz |
|---|---|
| `src/Ramsey/Grafo.cs` | o grafo em bits, o complemento, e a coloração montada a partir de um número |
| `src/Ramsey/Juiz.cs` | clique e independente pela definição, e os mesmos por máscara, para conferir |
| `src/Ramsey/Varredura.cs` | a varredura cega, a busca por vértice e o número de Ramsey |
| `src/Ramsey/Goodman.cs` | a fórmula de 1959 e a contagem de triângulos monocromáticos na marra |
| `src/Ramsey/Medidas.cs` | as cinco tabelas deste README |
| `ferramentas/Medidor` | imprime as tabelas, uma por comando |

## Rodando

```bash
dotnet test testes/Ramsey.Testes/Ramsey.Testes.csproj -c Release

dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- festa
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- pentagono
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- motores
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- numeros
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- goodman
```

C# 12 e .NET 8. Zero dependências fora do xUnit, e só nos testes.

71 testes. Os números deste README estão presos em `MedidasTestes`.
