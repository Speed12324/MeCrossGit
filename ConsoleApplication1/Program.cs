using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

class MyTcpListener
{
    
    [STAThread]
    public static void Main(string[] args)
    {
        TcpListener server = null;
        try
        {
            Int32 port = 13000; ;
            if (args.Length != 0)
                Int32.TryParse(args[0], out port);






            IPAddress localAddr = IPAddress.Any;

           
            server = new TcpListener(localAddr, port);

           
            server.Start();

            Byte[] bytes = new Byte[1048576];
            String data = null;

            
            while (true)
            {
                Console.Write("Waiting for a connection... ");

                
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                
                NetworkStream stream = client.GetStream();

                int i;

                
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                   
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    if (Clipboard.GetText(TextDataFormat.Text) != data)
                        Clipboard.SetText(data);



                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

                
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            
            server.Stop();
        }


        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }
}