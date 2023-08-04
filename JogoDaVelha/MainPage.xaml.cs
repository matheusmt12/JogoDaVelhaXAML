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

namespace JogoDaVelha;

public partial class MainPage : ContentPage
{
    private TcpClient cliente = new TcpClient();
    Thread tipoThread;
    static String x;
    static String y;
    private String tpPartida;
    public MainPage()
    {
        InitializeComponent();
    }

    private void BtnConectarPartidaPublica(object sender, EventArgs e)
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

    private void BtnPartidaPrivada(object sender, EventArgs e)
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
            JogoVelha.Text = "Ouve algum problema no servidor";
        }

    }

    private void BtnEntrarPrivada(object sender, EventArgs e)
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
                JogoVelha.Text = "Digite o código da partida";
            }

        }
        catch
        {
            JogoVelha.Text = "Ouve algum problema no servidor";
        }

    }
    private void OnEntryTextChanged(object sender, EventArgs e)
    {


    }


    private void OnEntryNumPrivada(object sender, EventArgs e)
    {

    }

    private void escuta()
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
        }
    }
}

