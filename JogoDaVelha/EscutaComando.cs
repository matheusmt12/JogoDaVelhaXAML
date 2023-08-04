using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace JogoDaVelha
{
    class EscutaComando
    {
        private TcpClient cliente;
        private MainPage mainPage;

        public EscutaComando(TcpClient cliente, MainPage mainPage)
        {
            this.cliente = cliente;
            this.mainPage = mainPage;
            Thread tipoThread = new Thread(new ThreadStart(escuta));
            tipoThread.Start();
        }

        private void escuta()
        {
            NetworkStream sockStream = cliente.GetStream();
            StreamWriter sw = new StreamWriter(sockStream);
            StreamReader sr = new StreamReader(sockStream);
            sw.WriteLine("1");
            sw.Flush();
            Console.WriteLine("Enviado para o servidor");
            String mensagem = sr.ReadLine();
            Console.WriteLine("Recebeu do servidor: " + mensagem);
            mainPage.AtualizarLabel(mensagem);
            while (true)
            {
                mensagem = sr.ReadLine();
                if (mensagem.Equals("0a"))
                {
                    mainPage.AtualizarLabel(mensagem);
                }
                mainPage.AtualizarLabel(mensagem);
            }
        }
    }

}