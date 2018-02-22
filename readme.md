Desafio: Gerar os arquivos XML a partir de PDF
================================================

Exemplo: Documento _p40.pdf_ ([download](ConsoleApp1/samples/p40.pdf))

Nesse documento, existem 6 atos (total de 8 ao somar um incompleto e uma retificação), que são na ordem:
- Portaria No 13
- Ato declaratório executivo No 96
- Ato declaratório executivo No 29
- Ato declaratório executivo No 15
- Ato declaratório executivo No 74
- Ato declaratório executivo No 33

Aparentemente os números (13, 96, 29, 15, 74, 33) estão fora de ordem, mas na verdade eles pertencem a diferentes superintendências:
- Superintendência regional da 6ª região fiscal
- Superintendência regional da 7ª região fiscal
- Superintendência regional da 8ª região fiscal

Há uma hierarquia entre Superintendências, Delegacias e o Atos. Assim, podemos entender melhor a organização:
- Superintendência regional da 6ª região fiscal / Delegacia da receita federal do Brasil em Belo Horizonte / Portaria No 13
- Superintendência regional da 6ª região fiscal / Delegacia da receita federal do Brasil em Poços de Calda / Ato declaratório executivo No 96
- (Superintendência regional da 7ª região fiscal / Delegacia da receita federal do Brasil em Vitória / Retificação)
- Superintendência regional da 8ª região fiscal / Alfândega da receita federal do Brasil em São Paulo / Ato declaratório executivo No 29
- Superintendência regional da 8ª região fiscal / Delegacia da receita federal do Brasil em Campinas / Ato declaratório executivo No 15
- Superintendência regional da 8ª região fiscal / Delegacia da receita federal do Brasil em Piracicaba / Ato declaratório executivo No 74
- Superintendência regional da 8ª região fiscal / Delegacia da receita federal do Brasil em São José dos Campos / Seção de Administração Aduaneira / Ato declaratório executivo No 33

Esses são os 6 atos completos presentes no documento e a retificação.
Cada arquivo XML tem uma hierarquia, título, caput (breve sumário), assinatura com cargo opcional. Então seria algo assim:

```xml
    <XML>

    <HIERARQUIA>
    <H1>SUPERINTENDÊNCIA REGIONAL DA 6ª REGIÃO FISCAL</H1>
    <H2>DELEGACIA DA RECEITA FEDERAL DO BRASIL EM BELO HORIZONTE</H2>
    </HIERARQUIA>

    <TITULO> PORTARIA No- 13, DE 10 DE NOVEMBRO DE 2017 </TITULO>

    <CAPUT>Exclui pessoa jurídica do REFIS.</CAPUT>

    <CORPO>
    A DELEGACIA DA RECEITA FEDERAL DO BRASIL
    EM BELO HORIZONTE/MG, tendo em vista a competência delegada
    pela Resolução do Comitê Gestor do REFIS nº 37, de 31 de
    agosto de 2011, por sua vez constituído pela Portaria Interministerial
    MF/MPAS nº 21, de 31 de janeiro de 2000, no uso da competência
    estabelecida no § 1º do art. 1º da Lei nº 9.964, de 10 de abril de 2000,
    e no inciso IV do art. 2º do Decreto nº 3.431, de 24 de abril de 2000,
    tendo em vista o disposto no inciso XIV do art. 79 da Lei nº 11.941,
    de 27 de maio de 2009, resolve:
    Art. 1o Excluir do Programa de Recuperação Fiscal - REFIS,
    a PEDIDO, a pessoa jurídica DEPÓSITO ARAÚJO MATERIAIS DE
    CONSTRUÇÃO, CNPJ: 17.345.570/0001-16, conforme registrado no
    processo administriativo n° 10695.001452/2017-48
    Art. 2o Esta Portaria entra em vigor na data de sua publicação.
    </CORPO>

    <ASSINATURA>
    <NOME>MARIO JOSÉ DEHON SÃO THIAGO SANTIAGO</NOME>
    <CARGO>Delegado da Receita Federal do Brasil em Belo Horizonte/MG</CARGO>
    </ASSINATURA>
    </XML>
```

Obs: Há um diretório com samples do p40, p42 e p44. Com certeza, p44 é o mais fácil!

# Por onde começar?

* Sample: https://github.com/DXBrazil/GetStarted-iText
* Biblioteca: iText (https://itextpdf.com/)

Existem as versões iText 5 e 7, que são bem diferentes entre si. Inicialmente trabalhamos com a versão 5 no repositório privado(https://github.com/DXBrazil/CasaCivil-PDF2XML). Porém, depois trocamos para a versão 7, que suporta .NET Core. Há diferenças significativas na API.
 
Assim, Podemos escrever nosso primeiro código (simples!):

```csharp
        static string ReadTextFromPdf(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                return PdfTextExtractor.GetTextFromPage(page);
            }
        }  
```` 

Entretanto, o resultado não é interessante... compare com o anexo p40.pdf.

```
40
1
ISSN 1677-7042
Nº 218, terça-feira, 14 de novembro de 2017
O montante relativo à atualizaçao monetária e juros con- DELEGACIA DA RECEITA FEDERAL DO BRASIL Parágrafo único O regime especial de substituiçao tributária
tratuais, vinculado à indenizaçao por dano patrimonial, deverá ser nao se aplica ao IPI devido no desembaraço aduaneiro de produtos de
EM POÇOS DE CALDAS
computado na apuraçao da base de cálculo da contribuiçao procedência estrangeira.
...
...
```

Por isso, é necessário customizar a leitura através de Listeners:

```csharp
        static void ReadTextFromListener(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                var parser = new PdfCanvasProcessor(new UserTextListener());

                parser.ProcessPageContent(page);
            }
        }
```

Tem vários exemplos no código: https://github.com/DXBrazil/GetStarted-iText

