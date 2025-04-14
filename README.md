🃏 Projeto - Hall da Fama Pokémon
Este projeto é uma aplicação simples que conecta ao MySQL, busca dados de Cards Pokémon e gera uma página HTML interativa com um carrossel de navegação entre os cards.

Além disso, o projeto conta com uma página upload.html para cadastro de novos cards.

💡 Funcionalidades
✅ Conexão com banco de dados MySQL
✅ Carregamento dinâmico de imagens e nomes de cards
✅ Alternância entre modo Normal e Shiny
✅ Navegação pelo carrossel via botões
✅ Upload de novos cards

🗂️ Estrutura de Pastas
css
Copiar
Editar
├── assets/
│   ├── bg-normal.png
│   ├── bg-sky.png
│   ├── star.svg
│   ├── normal.svg
│   ├── slideRight.svg
│   └── slideLeft.svg
├── Program.cs
├── carrossel.html
├── upload.html
├── pokemon.csproj
└── README.md
⚙️ Pré-requisitos
.NET 8.0 SDK

MySQL Server

MySql.Data (já incluso no csproj)

💻 Como rodar
Clone o repositório:

bash
Copiar
Editar
git clone https://github.com/seu-usuario/seu-projeto.git
cd seu-projeto
Configure a string de conexão no Program.cs:

csharp
Copiar
Editar
string connectionString = "Server=localhost;Database=cards_pokemon;User=root;Password=SUASENHA;";
Execute o projeto:

bash
Copiar
Editar
dotnet build
dotnet run
Se tudo estiver correto, você verá:

css
Copiar
Editar
Conexão bem-sucedida ao banco de dados!
Arquivo HTML gerado com sucesso.
O arquivo carrossel.html será criado no diretório raiz.

🖼️ Como usar
Abra o carrossel.html em seu navegador.

Use as setas para navegar entre os cards.

Clique no botão 🌟 para alternar entre o modo Normal e Shiny.

Para adicionar novos cards, abra upload.html, preencha as informações e envie.

📦 Dependências
MySql.Data - Conector C# para MySQL.

Já adicionado no seu .csproj:

xml
Copiar
Editar
<PackageReference Include="MySql.Data" Version="9.1.0" />
🧠 Observações
O projeto pressupõe que você já tenha as imagens salvas no banco de dados e referenciadas corretamente.

As imagens dos botões e fundos precisam estar dentro da pasta /assets.

🚀 Melhorias futuras
Implementar upload real de imagens no banco.

Adicionar transições suaves no carrossel.

Criar paginação caso o número de cards seja muito grande.

🏆 Créditos
Projeto desenvolvido por Giovanna Marinho.