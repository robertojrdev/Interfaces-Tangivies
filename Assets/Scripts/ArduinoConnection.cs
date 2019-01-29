using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class ArduinoConnection : MonoBehaviour 
{
    //Setup parameters to connect to Arduino
    public static SerialPort sp = null; //new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);
	private float updatePeriod = 0.0f;

    private Spaceship ship;

    // Use this for initialization
    void Start () 
	{
        if(sp == null || !sp.IsOpen)
            FindArduino();
        //OpenConnection();
        ship = FindObjectOfType<Spaceship>();
    }
 
    void Update()
    {
        if (sp == null)
            return;

        string received;
        //try to receive information from arduino
        try
        {
            received = sp.ReadExisting();
            received = received.Replace('.', ',');
        }
        catch (Exception)
        {
            TryToReconnect();
            return;
        }

        string[] messages = received.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var message in messages)
        {
            if (!string.IsNullOrEmpty(message))
            {
                float result;
                if (float.TryParse(message, out result))
                {
                    if(ship)
                        ship.SetHeightWithArduino(result);
                }
                else
                {
                    Debug.LogWarning(" -> Not parsed");
                }
            }
        }
    }

    private void TryToReconnect()
    {
        StartCoroutine(Reconnect_Routine());
    }

    IEnumerator Reconnect_Routine()
    {
        sp = null;
        while (sp == null)
        {
            Debug.Log("Trying to connect");
            FindArduino();
            yield return new WaitForSeconds(1);
        }
    }

    private void FindArduino()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
            sp.Dispose();
        }

        string[] ports = SerialPort.GetPortNames();

        foreach (var port in ports)
        {
            sp = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);

                if (sp.IsOpen)
                    sp.Close();

                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;


                print("Trying port " + port);


            try
            {
                sp.Open();
                if (sp.IsOpen)
                {
                    sp.WriteLine("o");
                    string result = sp.ReadLine();
                    if (result.Contains("open"))
                    {
                        Debug.Log("port opened " + port + " - Responce: " + result);
                        break;
                    }
                    else
                    {
                        sp.Close();
                        sp.Dispose();
                        sp = null;
                    }
                }
            }
            catch (Exception e)
            {

                Debug.Log("port " + port + "failed /n" + e);
                sp = null;
                continue;
            }
        }

        Debug.Log("Finished search");
    }

    //Function connecting to Arduino
    public void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
                sp.Close();
                print("Closing port, because it was already open!");
                //message = "Closing port, because it was already open!";
            }
            else
            {
                sp.Open();  // opens the connection
                sp.ReadTimeout = 1000;  // sets the timeout value before reporting error
                print("Port Opened!");
                //		message = "Port Opened!";
            }
        }
        else
        {
            if (sp.IsOpen)
            {
                print("Port is already open");
            }
            else
            {
                print("Port == null");
            }
        }
    }

    void OnApplicationQuit() 
    {
        if (sp != null)
        {
            sp.WriteLine("c");
            sp.Close();
            sp.Dispose();
        }
    }
}