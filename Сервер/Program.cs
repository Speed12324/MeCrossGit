
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;


class MyTcpListener
{
    
    static StreamWriter sw;
    static bool[] a = new bool[20];
    public static void Main()
    {
        for (int i = 0; i < 20; i++)
        {
            a[i] = false;


        }
        
        TcpListener server = null;
        
        try
        {
            
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Any;

            
            server = new TcpListener(localAddr, port);
            


            server.Start();

            
            Byte[] bytes = new Byte[256];
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
                    byte[] msg = null;
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    
                    if (a[0] == false)
                    {
                        if (data == "CMD")
                        {
                            a[0] = true;
                            data = "Expected commands...";
                        }
                    }
                    else
                    {
                        control(data);

                    }
                    msg = System.Text.Encoding.ASCII.GetBytes(data);



                   Console.WriteLine("Received: {0}", data);












                    stream.Write(msg, 0, msg.Length);
                    if (a[1] == false )
                    {
                        Console.WriteLine("Sent: {0}", data);
                    }
                  
                    
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
    public static void control(string s)
    {
        
        if (s == "CMDEND")
        {
            a[0] = false;
            return;
        }
        else if (s== "STARTBAT")
        {
            Process.Start("Config.bat");
            
            return;
        }
        else if (s == "DELRTE")
        {
            File.Delete("Config.bat");
        }
        else if (s.IndexOf("READ") >= 0)
        {
            s = s.Replace("READ", "");
            sw = new StreamWriter("Config.bat", true);
            sw.WriteLine(s);
            sw.Close();
            return;
        }
        else if (s == "CLEAR")
        {
            sw = new StreamWriter("Config.bat");
            sw.WriteLine("");
            sw.Close();
            return;
        }
        else if (s == "SHOW")
        {
            try
            {

                using (StreamReader sr = new StreamReader("Config.bat"))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        
       
        
        
        
       
        
    


    }
}
