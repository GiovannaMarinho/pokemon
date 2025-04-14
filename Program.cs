using System;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace MySQLConnectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lendo config do appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtenção da string de conexão do appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");

            // Definição das imagens de fundo, botões e estrelas
            string[] backgroundImage = 
            {
                "./assets/bg-normal.png",
                "./assets/bg-sky.png"
            };
            string[] star = 
            {
                "./assets/star.svg",
                "./assets/normal.svg"
            };
            string botaoDireito = "./assets/slideRight.svg";
            string botaoEsquerdo = "./assets/slideLeft.svg";

            // Conexão ao banco de dados MySQL
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexão bem-sucedida ao banco de dados!");

                    string query = "SELECT * FROM Cards";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    
                    MySqlDataReader reader = cmd.ExecuteReader();

                    var imagens = new System.Collections.Generic.List<string>();
                    var imagensShinny = new System.Collections.Generic.List<string>();
                    var nomes = new System.Collections.Generic.List<string>();

                    // Leitura dos dados da tabela "Cards"
                    while (reader.Read())
                    {
                        imagens.Add(reader["ImagemNormal"]?.ToString() ?? "");
                        imagensShinny.Add(reader["ImagemShinny"]?.ToString() ?? "");
                        nomes.Add(reader["Nome"]?.ToString() ?? "");
                    }

                    reader.Close();
                    
                    // Geração do HTML com as imagens e dados
                    GenerateHtml(imagens, imagensShinny, backgroundImage, botaoDireito, botaoEsquerdo, nomes, star);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro na conexão: " + ex.Message);
                }
            }
        }

        // Função para gerar o HTML
        static void GenerateHtml(
            System.Collections.Generic.List<string> imagens, 
            System.Collections.Generic.List<string> imagensShinny, 
            string[] backgroundImage, 
            string botaoDireito, 
            string botaoEsquerdo, 
            System.Collections.Generic.List<string> nomes, 
            string[] star)
        {
            // Convertendo listas de imagens e nomes em formato JSON
            string cardImagens = "[" + string.Join(", ", imagens.ConvertAll(img => $"'{img}'")) + "]";
            string cardImagensShinny = "[" + string.Join(", ", imagensShinny.ConvertAll(img => $"'{img}'")) + "]";
            string cardNomes = "[" + string.Join(", ", nomes.ConvertAll(nome => $"'{nome}'")) + "]";

            // Montando o conteúdo HTML
            string htmlContent = $@"
            <!DOCTYPE html>
            <html lang='pt-br'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Hall da Fama</title>
                <style>
                    * {{
                        padding: 0;
                        margin: 0;
                    }}
                    body {{
                        background-image: url('{backgroundImage[0]}');
                        background-size: cover;
                        background-position: center;
                        transition: background-image 0.5s ease-in-out;
                    }}
                    .star {{
                        position: absolute;
                        right: 100px;
                        top: 50px;
                    }}
                    .all {{
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        gap: 40px;
                    }}
                    button {{
                        background: none;
                        border: none;
                        cursor: pointer;
                    }}
                    button:hover {{
                        transform: scale(1.1);
                        animation: 3s;
                    }}
                    .card_name {{
                        height: 100vh;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        flex-direction: column;
                        gap: 20px;
                    }}
                    .card {{
                        width: 450px;
                    }}
                    .img_button {{
                        width: 100px;
                    }}
                    h1 {{
                        text-align: center;
                        font-size: 36px;
                        font-weight: 700;
                        font-family: 'Poppins';
                    }}
                    .link {{
                        position: absolute;
                        right: 100px;
                        bottom: 50px;
                        display: inline-block;
                        width: 90px;
                        height: 40px;
                        background: #313131;
                        border-radius: 50px;
                        color: #fff;
                        font-size: 16px;
                        text-align: center;
                        line-height: 40px;
                        text-decoration: none;
                        font-family: 'Poppins';
                    }}
                    .link:hover {{
                        transform: scale(1.1);
                        animation: 3s;
                    }}
                </style>
                <link rel='preconnect' href='https://fonts.googleapis.com'>
                <link rel='preconnect' href='https://fonts.gstatic.com' crossorigin>
                <link href='https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap' rel='stylesheet'>
            </head>
            <body class='toggle'>
                <button id='star' class='star'>
                    <img src='{star[0]}' alt='Botão modo estrela'>
                </button>
                <section class='all'>
                    <button onclick='changeImageLeft()'>
                        <img id='switchLeft' class='img_button' src='{botaoEsquerdo}' alt='Botão para esquerda'>
                    </button>
                    <div class='card_name'>
                        <img class='card toggle' src='{imagens[0]}' alt='Card'>
                        <h1 id='nomesCard' class='toggle'>{nomes[0]}</h1>
                    </div>
                    <button onclick='changeImageRight()'>
                        <img id='switchRight' class='img_button' src='{botaoDireito}' alt='Botão para direita'>
                    </button>
                    <a class='link' href='./upload.html'>Upload</a>
                </section>
                <script>
                    const imagens = {cardImagens};
                    const imagensShinny = {cardImagensShinny};
                    const nomes = {cardNomes};
                    let indiceAtual = 0; 
                    function changeImageRight() {{
                        indiceAtual = (indiceAtual + 1) % imagens.length;
                        document.querySelector('.card').src = imagens[indiceAtual];
                        document.getElementById('nomesCard').innerText = nomes[indiceAtual];
                    }};
                    function changeImageLeft() {{
                        indiceAtual = (indiceAtual - 1 + imagens.length) % imagens.length;
                        document.querySelector('.card').src = imagens[indiceAtual];
                        document.getElementById('nomesCard').innerText = nomes[indiceAtual];
                    }}
                    document.getElementById('star').addEventListener('click', function() {{
                        let body = document.body;
                        let card = document.querySelector('.card');
                        let starButton = document.getElementById('star').querySelector('img');
                        let name = document.getElementById('nomesCard')

                        let buttonLeft = document.querySelector('#switchLeft');
        	            let buttonRight = document.querySelector('#switchRight');

                        if (body.classList.contains('sky-mode')) {{
                            body.classList.remove('sky-mode');
                            body.style.backgroundImage = ""url('./assets/bg-normal.png')""; 
                            card.src = imagens[indiceAtual]; 
                            starButton.src = './assets/star.svg'; 
                            name.style.color = 'black';
                            buttonLeft.style.filter = 'invert(0%)';  
                            buttonRight.style.filter = 'invert(0%)'; 
                        }} else {{
                            body.classList.add('sky-mode');
                            body.style.backgroundImage = ""url('./assets/bg-sky.png')""; 
                            card.src = imagensShinny[indiceAtual];
                            starButton.src = './assets/normal.svg';
                            name.style.color = 'white';
                            buttonLeft.style.filter = 'invert(100%)';  
                            buttonRight.style.filter = 'invert(100%)'; 
                        }}
                    }});
                </script>
            </body>
            </html>";

            // Escrevendo o conteúdo HTML em um arquivo
            File.WriteAllText("carrossel.html", htmlContent);
            Console.WriteLine("Arquivo HTML gerado com sucesso.");
        }
    }
}
