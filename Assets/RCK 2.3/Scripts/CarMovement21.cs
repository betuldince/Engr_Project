using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;  //to use IO Port library to connect with serial communication channel


public class CarMovement21 : MonoBehaviour
{
    // Start is called before the first frame update
    public float MotorForce, steerForce;
    private float BrakeForce = 500000f;
    public WheelCollider Fr_L, Fr_R, Bc_L, Bc_R;
	 

    //public GameObject car;
	//Cem
    SerialPort arduino = new SerialPort("COM6" , 9600); // Sets the SerialPort for arduino. The boud rate and the serial port name should be same with arduino.

	//public Rigidbody rb;                // define a rigidbody object (we can add velocities to rigidbodies using AddForce)
	public float sensivity = 0.1f;  // sensivity is used to adjust the speed of the object in unity.
	public string datas;              //variable of speed data received from serial port
	public float deltax;             //position change
	public string[] v;              //speed vector in string 
	
    void Start()
    {
        arduino.Open();
		
		
		
		
		
		
		
    }

    // Update is called once per frame
    void Update()
    {
		
		datas = arduino.ReadLine();
		string[] v= datas.Split(',');
		
		
        if (transform.rotation.eulerAngles.z > 80f && transform.rotation.eulerAngles.z <91f  )
        {
         
                Debug.Log("devrildi");
                //SceneManager.LoadScene("MainMenu");
            
            
        }
        if (transform.rotation.eulerAngles.z > 266f && transform.rotation.eulerAngles.z < 278f)
        {

            Debug.Log("devrildi");
            //SceneManager.LoadScene("MainMenu");


        }
        //if yan cevrilirse Lose ekranı
        //float v = Input.GetAxis("Vertical") * MotorForce;
        //float h = Input.GetAxis("Horizontal") * steerForce;
		Debug.Log(Input.GetAxis("Vertical"));
        float k = float.Parse(v[0]) * MotorForce;
        float h = float.Parse(v[1]) * steerForce;		

        Bc_R.motorTorque = k;
        Bc_L.motorTorque = k;

        Fr_L.steerAngle = h;
        Fr_R.steerAngle = h;
/*
        if (Input.GetKey(KeyCode.Space))
        {
            Bc_R.brakeTorque = BrakeForce;
            Bc_L.brakeTorque = BrakeForce;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Bc_R.brakeTorque = 0;
            Bc_L.brakeTorque = 0;
        }
		*/
        if (float.Parse(v[0]) == 0)
        {
            Bc_R.brakeTorque = BrakeForce;
            Bc_L.brakeTorque = BrakeForce;
        }
        else if(float.Parse(v[0]) != 0)
        {
            Bc_R.brakeTorque = 0;
            Bc_L.brakeTorque = 0;

        }
        if(float.Parse(v[2])==1){
            Debug.Log("ad");
            Bc_R.brakeTorque = BrakeForce*5;
            Bc_L.brakeTorque = BrakeForce*5;   

        }


    }

    

	
	
	
	
	
}