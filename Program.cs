using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace MySQLConnectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
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

            string connectionString = "Server=localhost;Database=cards_pokemon;User=root;Password=LulyRosa0106;";

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

                    while (reader.Read())
                    {
                        imagens.Add(reader["ImagemNormal"]?.ToString() ?? "");
                        imagensShinny.Add(reader["ImagemShinny"]?.ToString() ?? "");
                        nomes.Add(reader["Nome"]?.ToString() ?? "");
                    }

                    reader.Close();
                    
                    GenerateHtml(imagens, imagensShinny, backgroundImage, botaoDireito, botaoEsquerdo, nomes, star);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro na conexão: " + ex.Message);
                }
            }
        }

        static void GenerateHtml(
                                    System.Collections.Generic.List<string> imagens, 
                                    System.Collections.Generic.List<string> imagensShinny, 
                                    string[] backgroundImage, 
                                    string botaoDireito, 
                                    string botaoEsquerdo, 
                                    System.Collections.Generic.List<string> nomes, 
                                    string[] star)
        {
            string cardImagens = "[" + string.Join(", ", imagens.ConvertAll(img => $"'{img}'")) + "]";
            string cardImagensShinny = "[" + string.Join(", ", imagensShinny.ConvertAll(img => $"'{img}'")) + "]";
            string cardNomes = "[" + string.Join(", ", nomes.ConvertAll(nome => $"'{nome}'")) + "]";

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
                        background-image: url('{backgroundImage[0]}');
                    }}
                    .star{{
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
                </script>
            </body>
            </html>";

            File.WriteAllText("carrossel.html", htmlContent);
            Console.WriteLine("Arquivo HTML gerado com sucesso.");
        }
    }
}
