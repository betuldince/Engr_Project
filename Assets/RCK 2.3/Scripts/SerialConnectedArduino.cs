
/////////////////SERIAL CONNECTION TEST UNITY PROJECT v1.0////////////////


//This Scripts used for sending arduino data to unity3D using Serial communication. Arduino sends pitch, yawn and roll measurements from the sensor.

using System.Collections;   
using UnityEngine;
using System.IO.Ports;  //to use IO Port library to connect with serial communication channel

public class SerialConnectedArduino : MonoBehaviour
{
    SerialPort arduino = new SerialPort("COM5" , 9600); // Sets the SerialPort for arduino. The boud rate and the serial port name should be same with arduino.
	
	
	public Rigidbody rb;                // define a rigidbody object (we can add velocities to rigidbodies using AddForce)
	public float sensivity = 0.1f;  // sensivity is used to adjust the speed of the object in unity.
	public string datas;              //variable of speed data received from serial port
	public float deltax;             //position change
	public string[] v;              //speed vector in string 
	
	
	void Start()
    {
        arduino.Open(); //initiate the Serial Stream

    }

	

    // Update is called once per frame
	void Update()
	{
	datas = arduino.ReadLine();      // read the arduino data from serial port
	string[] v= datas.Split(',');   //split the data between ',' and put on a string to get individiual data values
	rb.AddForce(0, 0, float.Parse(v[0]) * sensivity* Time.deltaTime, ForceMode.VelocityChange);   //Add velocity in y-direction
	rb.AddForce(float.Parse(v[1])*sensivity* Time.deltaTime, 0, 0, ForceMode.VelocityChange);    //Add velocity in x-direction
	}



}