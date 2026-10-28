# OpenAC.Net.EscPos

[![NuGet Version](https://img.shields.io/nuget/v/OpenAC.Net.EscPos.svg)](https://www.nuget.org/packages/OpenAC.Net.EscPos/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/OpenAC.Net.EscPos.svg)](https://www.nuget.org/packages/OpenAC.Net.EscPos/)
[![Discord](https://img.shields.io/badge/Chat%20on-Discord-purple.svg)](https://discord.com/invite/brdmJ7Yv6w)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Target Frameworks](https://img.shields.io/badge/.NET%20Framework-4.6.2%20%7C%204.8-blue.svg)](https://dotnet.microsoft.com/)
[![Target Frameworks](https://img.shields.io/badge/.NET%20Core%20%2F%20Standard-netstandard2.0%20%7C%20net8.0%20%7C%20net9.0%20%7C%20net10.0-blue.svg)](https://dotnet.microsoft.com/)

Biblioteca .NET multiplataforma para comunicação, impressão e pré-visualização gráfica de cupons em **impressoras térmicas não fiscais** utilizando o protocolo **ESC/POS** e variações dos principais fabricantes.

Desenvolvida pelo time **OpenAC .Net**, a biblioteca abstrai as particularidades de cada fabricante, oferecendo uma API fluida, suporte a múltiplos meios de conexão (`Serial`, `TCP/IP`, `RAW`, `File`) e processamento de imagens 100% multiplataforma com **SkiaSharp**.

---

## 🚀 Funcionalidades

- **Formatação e Impressão de Texto**:
  - Fontes (`Normal`, `FonteA`, `FonteB`) e tamanhos (`Normal`, `Condensada`, `Expandida`).
  - Alinhamento (`Esquerda`, `Centro`, `Direita`).
  - Estilos combináveis (`Negrito`, `Itálico`, `Sublinhado`, `AlturaDupla`, `Invertido`).
  - Impressão de fragmentos de texto com estilos distintos na mesma linha (`TextSlice`).
  - Suporte a diversas páginas de código (`pc437`, `pc850`, `pc852`, `pc860`, `pcUTF8`, `pc1252`).
- **Códigos de Barras e QR Code**:
  - Códigos de barras: `UPCA`, `UPCE`, `EAN13`, `EAN8`, `CODE39`, `Inter2of5`, `CodaBar`, `CODE93`, `CODE128`, `CODE128a`, `CODE128b`, `CODE128c` e `MSI`.
  - Configuração de altura, largura das barras e posição do texto HRI (`SemTexto`, `Acima`, `Abaixo`, `Ambos`).
  - Impressão de **QR Code** (ideal para NFC-e, SAT e PIX) com controle de tamanho do módulo e nível de correção de erro.
- **Imagens e Logotipos**:
  - Impressão de imagens a partir de `byte[]` com fatiamento automático e redimensionamento proporcional (`ResizeImage`).
  - Impressão e programação de logotipo gravado na memória da impressora (`KC1` / `KC2`).
- **Modo Página (Page Mode)**:
  - Criação de múltiplas regiões independentes com coordenadas (`Esquerda`, `Topo`, `Largura`, `Altura`) e rotação (`EsquerdaParaDireita`, `BaixoParaTopo`, `TopoParaBaixo`, `DireitaParaEsquerda`).
- **Controle de Periféricos e Diagnóstico**:
  - Corte de papel (`Total` ou `Parcial`), avanço de linhas e sinal sonoro (`Beep`).
  - Abertura de gaveta de dinheiro com suporte a inversão de sinal de leitura.
  - Leitura em tempo real do **Status da Impressora** (`EscPosTipoStatus`: tampa aberta, pouco papel, sem papel, gaveta aberta, erro, etc.).
  - Leitura de **Informações da Impressora** (`InformacoesImpressora`: fabricante, modelo, firmware, número de série e guilhotina).
- **Pré-visualização Gráfica (`GerarPreview`)**:
  - Geração de imagem PNG (`byte[]` ou `Stream`) simulando a bobina térmica com todos os comandos presentes no buffer (textos, estilos, códigos de barras, QR Codes, imagens, cortes e regiões em modo página), sem exigir impressora física conectada.

---

## 🖨️ Protocolos e Fabricantes Suportados

A biblioteca suporta os seguintes protocolos configuráveis através do enum `ProtocoloEscPos`:

- **EscPos** (Padrão Epson ESC/POS e compatíveis)
- **EscEpsonP2** (Epson Esc/P2)
- **EscBematech** (Bematech MP-4200 TH, MP-2800 TH, etc.)
- **EscDaruma** (Daruma DR700, DR800, etc.)
- **EscElgin** (Elgin i7, i8, i9)
- **EscDiebold** (Diebold Mecaf / Procomp)
- **EscGPrinter** (GPrinter)
- **EscDatecs** (Datecs)
- **EscZJiang** (ZJiang)
- **EscPosStar** (PosStar)
- **EscSunmi** (Terminais inteligentes Sunmi)
- **EscCustomPos** (Impressoras Custom)

---

## 📦 Instalação

Adicione o pacote via NuGet Package Manager ou CLI:

### .NET CLI:
```bash
dotnet add package OpenAC.Net.EscPos
```

### Package Manager Console:
```powershell
Install-Package OpenAC.Net.EscPos
```

### PackageReference:
```xml
<PackageReference Include="OpenAC.Net.EscPos" Version="1.6.0.1" />
```

---

## 🛠️ Como Usar

### 1. Inicializando e Configurando a Impressora

Você pode instanciar `EscPosPrinter<TConfig>` utilizando qualquer dispositivo do `OpenAC.Net.Devices` (`SerialConfig`, `TCPConfig`, `RawConfig` ou `FileConfig`):

```csharp
using OpenAC.Net.Devices;
using OpenAC.Net.EscPos;
using OpenAC.Net.EscPos.Commom;

// Exemplo via TCP/IP (rede)
using var posPrinter = new EscPosPrinter<TCPConfig>(new TCPConfig
{
    IP = "192.168.0.100",
    Porta = 9100,
    ControlePorta = true
});

posPrinter.Protocolo = ProtocoloEscPos.EscPos;
posPrinter.PaginaCodigo = PaginaCodigo.pc850;
posPrinter.EspacoEntreLinhas = 0;
posPrinter.LinhasEntreCupons = 4;

posPrinter.Conectar();
```

---

### 2. Imprimindo Textos, Códigos de Barras e QR Code

```csharp
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

// Cabeçalho
posPrinter.ImprimirTexto("OPENAC .NET AUTOMAÇÃO", CmdTamanhoFonte.Expandida, CmdAlinhamento.Centro, CmdEstiloFonte.Negrito);
posPrinter.ImprimirTexto("Rua Exemplo, 123 - Centro", CmdAlinhamento.Centro);
posPrinter.ImprimirLinha(dupla: true);

// Textos com múltiplos estilos na mesma linha
posPrinter.ImprimirTexto(
    new TextSlice("ITEM 001  ", CmdEstiloFonte.Negrito),
    new TextSlice("Café Expresso           R$ 8,50"));

posPrinter.ImprimirLinha();
posPrinter.ImprimirTexto("TOTAL R$ 8,50", CmdFonte.Normal, CmdTamanhoFonte.Expandida, CmdAlinhamento.Direita, CmdEstiloFonte.Negrito);
posPrinter.ImprimirLinha();

// Código de Barras EAN-13
posPrinter.ImprimirBarcode("7891234567895", CmdBarcode.EAN13, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);
posPrinter.PularLinhas(1);

// QR Code (NFC-e / SAT / PIX)
posPrinter.ImprimirQrCode(
    "https://openac.net.br",
    tamanho: QrCodeModSize.Normal,
    aAlinhamento: CmdAlinhamento.Centro);

// Cortar papel e descarregar o buffer para a impressora
posPrinter.CortarPapel(parcial: true);
posPrinter.Imprimir();
```

---

### 3. Imprimindo Imagens (`byte[]`)

```csharp
using System.IO;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Extensions;

// Carrega os bytes da imagem e redimensiona mantendo a proporção
byte[] logoBytes = File.ReadAllBytes("logo.png").ResizeImage(300, 300);

posPrinter.ImprimirImagem(logoBytes, CmdAlinhamento.Centro);
posPrinter.CortarPapel(parcial: true);
posPrinter.Imprimir();
```

---

### 4. Gerando Imagem de Pré-visualização (`GerarPreview`)

É possível gerar uma imagem PNG do cupom montado no buffer antes de imprimir (ou mesmo sem impressora conectada):

```csharp
using System.IO;
using OpenAC.Net.EscPos.Commom;

posPrinter.ImprimirTexto("CUPOM DE TESTE", CmdTamanhoFonte.Expandida, CmdAlinhamento.Centro, CmdEstiloFonte.Negrito);
posPrinter.ImprimirLinha(dupla: true);
posPrinter.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", aAlinhamento: CmdAlinhamento.Centro);
posPrinter.CortarPapel(parcial: true);

// Obtém os bytes da imagem PNG (ou grava diretamente em um Stream)
byte[] pngPreview = posPrinter.GerarPreview(limparBuffer: false);
File.WriteAllBytes("preview_cupom.png", pngPreview);
```

---

### 5. Consultando Status e Informações da Impressora

```csharp
// Leitura do status (tampa aberta, pouco papel, sem papel, gaveta aberta, etc.)
EscPosTipoStatus status = posPrinter.LerStatusImpressora();
Console.WriteLine($"Status atual: {status}");

// Leitura das informações de hardware/firmware
InformacoesImpressora? info = posPrinter.LerInfoImpressora();
Console.WriteLine(info?.ToString());
```

---

## 🖥️ Aplicação de Demonstração (Avalonia UI)

O repositório inclui o projeto **`OpenAC.Net.EscPos.Demo`** construído em **Avalonia UI** (`.NET 10`), compatível com Windows, Linux e macOS, permitindo testar todas as conexões, comandos, leitura de status e visualização de cupons em tempo real.

```bash
dotnet run --project src/OpenAC.Net.EscPos.Demo/OpenAC.Net.EscPos.Demo.csproj
```

---

## 🤝 Como Contribuir

Contribuições são muito bem-vindas! Se você deseja relatar um problema, sugerir melhorias ou enviar uma contribuição:

1. Faça um Fork do repositório.
2. Crie uma Branch para a sua feature (`git checkout -b feature/nova-funcionalidade`).
3. Faça o commit das suas alterações (`git commit -m 'Adiciona suporte a novo protocolo'`).
4. Envie para o GitHub (`git push origin feature/nova-funcionalidade`).
5. Abra um Pull Request.

---

## 💖 Ajude o Projeto OpenAC .Net

Se o **OpenAC.Net.EscPos** é útil para o seu negócio ou projeto open source e você deseja apoiar o desenvolvimento contínuo, considere fazer uma contribuição:

- Participe da nossa comunidade no [Discord](https://discord.com/invite/brdmJ7Yv6w).
- Contribua com código, testes e documentação no [GitHub](https://github.com/OpenAC-Net/OpenAC.Net.EscPos).

---

## 📄 Licença

Este projeto é distribuído sob a licença **MIT**. Consulte o arquivo [LICENSE](LICENSE) para mais detalhes.
