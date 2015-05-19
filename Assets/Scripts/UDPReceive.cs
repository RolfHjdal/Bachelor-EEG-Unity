/*
 * UDP-Receive
 * IP = 127.0.0.1
 * Port = 8051
 */
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPReceive : MonoBehaviour {
	Thread receiveThread;
	UdpClient client;
	
	//Initalization
	private int port;

	private int currentCognitivPower;
	private string currentCognitivAction;

	public enum cognitivAction{COG_PULL, COG_PUSH, COG_LIFT, COG_DROP, COG_LEFT, COG_RIGHT, 
		COG_ROTATE_LEFT, COG_ROTATE_RIGHT, COG_ROTATE_CLOCKWISE, COG_ROTATE_COUNTERCLOCKWISE, 
		COG_ROTATE_FORWARD, COG_ROTATE_REVERSE, COG_DISAPPEAR, COG_NEUTRAL};

	private static void Main()
	{
		UDPReceive receiveObj = new UDPReceive();
		receiveObj.init(); 
	}
	
	void Start () {
		init();
	}

	private void init(){
		//print("UDPSend.init()");
		
		//Define port
		port = 8051;
		
		//Status
		print("Sending to 127.0.0.1: " + port);
		print("Test-Sending to this port:  nc-u 127.0.0.1 "+port+"");
		
		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	//Splits an input string
	public void ParseInput(string input){
		string[] output = input.Split (':');
		currentCognitivPower = Convert.ToInt32 (output[1]);
		currentCognitivAction = output[0];
	}

	//Receive thread
	private void ReceiveData(){
				client = new UdpClient (port);
				while (true) { 
						for (;;) {
								try {
										//Receive bytes.
										IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
										byte[] data = client.Receive (ref anyIP);
										string UDPInput = System.Text.Encoding.ASCII.GetString(data);
										ParseInput(UDPInput);
										print (UDPInput);
									} catch (Exception err) {
										print (err.ToString ()); 
									}
						}
				}
		}


	//Get functions for external access
	public int getCurrentCognitivPower(){
		return currentCognitivPower;
		}         

	public string getCurrentCognitivAction(){
		return currentCognitivAction;
	}
		
		void OnDisalbe(){
			if(receiveThread!= null)
				receiveThread.Abort();
			client.Close();
		}
	}	
	