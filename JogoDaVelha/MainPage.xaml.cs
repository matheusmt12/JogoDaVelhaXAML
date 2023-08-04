using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace JogoDaVelha;

public partial class MainPage : ContentPage
{
    private TcpClient cliente = new TcpClient();
    Thread tipoThread;
    static String simbolo;
    static String x;
    static String y;
    private String tpPartida;
    private CancellationTokenSource cancellationTokenSource;
    public MainPage()
    {
        InitializeComponent();
    }
    public void BtnConectarPartidaPublica(object sender, EventArgs e)
    {
        //count++;

        //if (count == 1)
        //	CounterBtn.Text = $"Clicked {count} time";
        //else
        //	CounterBtn.Text = $"Clicked {count} times";


        /*Comado para navegar para outra página */
        //Navigation.PushAsync(new NewPage1());

        //SemanticScreenReader.Announce(CounterBtn.Text);

        try
        {
            cliente.Connect(txtIpServidor.Text, 5000);
            tipoThread = new Thread(new ThreadStart(escuta));
            tpPartida = "1";
            tipoThread.Start();
            CounterBtn.IsEnabled = false;
            CounterBtn.IsVisible = false;
            btnEntrarPartida.IsEnabled = false;
            btnEntrarPartida.IsEnabled = false;
            btnPrivada.IsEnabled = false;
            txtCodPartida.IsEnabled = false;

        }
        catch
        {
            JogoVelha.Text = "Ouve algum problema no servidor";
        }

    }

    public void AtualizarLabel(String mensagem)
    {
        Device.InvokeOnMainThreadAsync(() =>
        {
            JogoVelha.Text = mensagem;
            
        });
    }

    public void AtualizarButton(Button button, String simbolo)
    {
        Device.InvokeOnMainThreadAsync(() =>
        {
            button.Text = simbolo;
            button.TextColor = Microsoft.Maui.Graphics.Color.Parse("White");
        });
    }

    public void BtnPartidaPrivada(object sender, EventArgs e)
    {
        try
        {
            cliente.Connect(txtIpServidor.Text, 5000);
            tpPartida = "2";
            tipoThread = new Thread(new ThreadStart(escuta));
            tipoThread.Start();
            CounterBtn.IsEnabled = false;
            CounterBtn.IsVisible = false;
            btnEntrarPartida.IsEnabled = false;
            btnEntrarPartida.IsEnabled = false;
            btnPrivada.IsEnabled = false;
            txtCodPartida.IsEnabled = false;
        }
        catch
        {
            AtualizarLabel("Ouve algum problema no servidor");
        }

    }

    public void BtnEntrarPrivada(object sender, EventArgs e)
    {
        try
        {
            if (!txtCodPartida.Text.Equals(""))
            {
                cliente.Connect(txtIpServidor.Text, 5000);
                //esc = new EscutaComando(cliente, this);
                tpPartida = "3";
                tipoThread = new Thread(new ThreadStart(escuta));
                tipoThread.Start();
                CounterBtn.IsEnabled = false;
                CounterBtn.IsVisible = false;
                btnEntrarPartida.IsEnabled = false;
                btnEntrarPartida.IsEnabled = false;
                btnPrivada.IsEnabled = false;
                txtCodPartida.IsEnabled = false;
            }
            else
            {
                AtualizarLabel("Gigite o código da partida");
            }

        }
        catch
        {
            AtualizarLabel("Ouve algum problema no servidor");
        }

    }
    public void OnEntryTextChanged(object sender, EventArgs e)
    {


    }


    public void OnEntryNumPrivada(object sender, EventArgs e)
    {

    }

    public void escuta()
    {




        NetworkStream sockStream = cliente.GetStream();
        StreamWriter sw = new StreamWriter(sockStream);
        StreamReader sr = new StreamReader(sockStream);
        sw.WriteLine(tpPartida);
        sw.Flush();
        Console.WriteLine("Enviado para o servidor");
        String mensagem;
        if (tpPartida.Equals("3"))
        {
            //sw.WriteLine(txtCodPartida.Text);
            sw.Flush();
            mensagem = sr.ReadLine();
            if (mensagem.Equals("Codigo da partida invalido!"))
            {
                AtualizarLabel(mensagem);
                cliente.Close();
                cliente = new TcpClient();
                CounterBtn.IsEnabled = true;
                CounterBtn.IsVisible = true;
                btnEntrarPartida.IsEnabled = true;
                btnEntrarPartida.IsEnabled = true;
                btnPrivada.IsEnabled = true;
                txtCodPartida.IsEnabled = true;
                return;
            }
        }
        else
        {
            mensagem = sr.ReadLine();
        }
        Console.WriteLine("Recebeu do servidor: " + mensagem);
        AtualizarLabel(mensagem);
        while (true)
        {
            mensagem = sr.ReadLine();
            Console.WriteLine("Recebeu do servidor2: " + mensagem);
            if (mensagem.Equals("0a"))
            {
                MainPage.simbolo = sr.ReadLine();
                if (MainPage.simbolo.Equals("X"))
                {
                    Device.InvokeOnMainThreadAsync(() =>
                    {
                        desistirPartida.IsEnabled = true;

                    });
                    AtualizarLabel("Partida iniciada! Realize uma jogada");
                }
                else
                {
                    AtualizarLabel("Partida iniciada! Aguarde a sua vez!");
                }
            }
            else if (mensagem.Equals("1a"))
            {
                String x1 = sr.ReadLine();

                String y1 = sr.ReadLine();
                Button btn = this.FindByName<Button>("btn" + x1 + y1);

                if(MainPage.simbolo.Equals("X"))
                {
                    AtualizarButton(btn, "O");
                }
                else
                {
                    AtualizarButton(btn, "X");
                }
                AtualizarLabel("Sua vez de jogar!!!");
            }
            else if (mensagem.Equals("OK!"))
            {
                Button btn = this.FindByName<Button>("btn" + x + y);
                AtualizarButton(btn, simbolo);
                AtualizarLabel("Jogada realizada com sucesso!");
            }
            else if (mensagem.Equals("2a"))
            {
                AtualizarLabel("Seu adversário desistiu! Você venceu!!!");
                break;
            }
            else if (mensagem.Equals("3a"))
            {
                String simboloVencedor = sr.ReadLine();
                if (simboloVencedor.Equals(simbolo))
                {
                    AtualizarLabel("Você venceu !");
                }
                else
                {
                    String x1 = sr.ReadLine();
                    String y1 = sr.ReadLine();

                    Button btn = this.FindByName<Button>("btn" + x1 + y1);
                    AtualizarButton(btn, simboloVencedor);
                    AtualizarLabel("Você perdeu !");
                }
                break;
            }
            else if (mensagem.Equals("4a"))
            {
                String simboloUltimaJogada = sr.ReadLine();
                if (!simboloUltimaJogada.Equals(simbolo))
                {
                    //recebendo x
                    String x1 = sr.ReadLine();
                    //recebendo y
                    String y1 = sr.ReadLine();

                    Button btn = this.FindByName<Button>("btn" + x1 + y1);
                    AtualizarButton(btn, simboloUltimaJogada);
                }
                AtualizarLabel("Jogo empatado!!!!");

                break;
            }
            else
            {
                AtualizarLabel(mensagem);
            }
        }
    }
    private void btn00_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        x = btn.StyleId[3].ToString();
        y = btn.StyleId[4].ToString();
        NetworkStream sockStream = cliente.GetStream();
        StreamWriter sw = new StreamWriter(sockStream);
        StreamReader sr = new StreamReader(sockStream);
        sw.WriteLine("1a");
        sw.WriteLine(x);
        sw.WriteLine(y);
        sw.Flush();

    }


    public void btnDesistirPartida(object sender, EventArgs e)
    {
        NetworkStream sockStream = cliente.GetStream();
        StreamWriter sw = new StreamWriter(sockStream);
        StreamReader sr = new StreamReader(sockStream);
        sw.WriteLine("2a");
        sw.Flush();
        AtualizarLabel("Voce desistiu da partida");
        cliente.Close();
        
    }
}


