using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class HeliMovement : MonoBehaviour
{
    public string[] v ;
    private Rigidbody myRigidbody;
    private int apply_force;
    private int steer_forward;
    private int steer_right;
    private int steer_left;
    private int steer_back;
    private int turn_right;
    private int turn_left;
    public float motorPower;
    private int arduino_mode;
    private Vector3 rot;
    SerialPort arduino;
    private string datas; 
    // Start is called before the first frame update
    void Start()
    {
        //motorPower = 320000;
        arduino_mode = 0;
        apply_force=0;
        steer_forward=0;
        steer_right=0;
        steer_left =0;
        steer_back = 0;
        turn_right=0;
        turn_left=0;
        rot = transform.rotation.eulerAngles;

        myRigidbody = transform.GetComponent<Rigidbody>();
        //Debug.Log(myRigidbody.velocity.magnitude);
        //transform.Rotate(new Vector3(0,90,0));
        
    }
    void getActionFromUser(){
        if(Input.GetKeyDown(KeyCode.U)){
            apply_force = 1;
            //Debug.Log("Here!!");
        }
        
        if(Input.GetKeyUp(KeyCode.U)){
            apply_force = 0;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)){
           steer_forward = 1;
        }
        
        if(Input.GetKeyUp(KeyCode.UpArrow)){
           steer_forward = 0;
        }
        
        if(Input.GetKeyDown(KeyCode.RightArrow)){
           steer_right = 1;
        }
        
        if(Input.GetKeyUp(KeyCode.RightArrow)){
           steer_right = 0;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
           steer_left = 1;
        }
        
        if(Input.GetKeyUp(KeyCode.LeftArrow)){
           steer_left = 0;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
           steer_back = 1;
        }
        
        if(Input.GetKeyUp(KeyCode.DownArrow)){
           steer_back = 0;
        }
        if(Input.GetKeyDown(KeyCode.A)){
           turn_left = 1;
        }
        
        if(Input.GetKeyUp(KeyCode.A)){
           turn_left = 0;
        }
        if(Input.GetKeyDown(KeyCode.D)){
           turn_right = 1;
        }
        
        if(Input.GetKeyUp(KeyCode.D)){
           turn_right = 0;
        }
        if(Input.GetKeyDown(KeyCode.C)){
            arduino_mode = 1;
            arduino = new SerialPort("COM5" , 9600);
            arduino.Open();
        }
    }
    void getActionViaArduino(){
		datas = arduino.ReadLine();
        if (datas.Length==0)return;
        Debug.Log(datas);
		v= datas.Split(',');
        int jX = int.Parse(v[0]);
        int jY = int.Parse(v[1]);
        //float jButton = float.Parse(v[2]);12,13,14
        float bx = float.Parse(v[12]);
        float by = float.Parse(v[13]);
        float bz = float.Parse(v[14]); 
        if (jX >0f){
            turn_right=1;
            turn_left=0;
        }else if(jX < 0f){
            turn_right=0;
            turn_left=1;
        }else{
            turn_right=0;
            turn_left=0;
        }
        if (jY >0f){
            apply_force = 1;
        }else{
            apply_force=0;
        }

        Debug.Log("\n\n");
        Debug.Log(bx);
        Debug.Log(by);
        Debug.Log(bz);
        
        //CHANGE CONDS TO GET PROPER ACTION
        bool forward_cond = bx>0;
        bool backward_cond = bx>0;
        bool right_cond = bx>0;
        bool left_cond =bx>0 ;
        if (forward_cond){
            steer_forward = 1;
            steer_back = 0;
        }else if(backward_cond){
            steer_forward = 0;
            steer_back = 1;
        }else{
            steer_forward = 0;
            steer_back = 0;
        }
        if (right_cond){
            steer_right = 1;
            steer_left = 0;
        }else if(left_cond){
            steer_right = 0;
            steer_left = 1;
        }else{
            steer_right = 0;
            steer_left = 0;
        }

    }
    void getAction(){
        if (arduino_mode==1){
            getActionViaArduino();
            return;
        }
        getActionFromUser();
    }
    // Update is called once per frame
    void Update()
    {
        getAction();
        if (steer_right==1 && transform.right.y> -0.5){
            transform.Rotate(Vector3.back*0.1f);
        }
        if (steer_left==1&&transform.right.y< 0.5){
            transform.Rotate(Vector3.forward*0.1f);
        }
        if(steer_forward==1 &&transform.forward.y> -0.5){
            transform.Rotate(Vector3.right*0.1f);
        }
        if(steer_back==1&&transform.forward.y< 0.5){
            transform.Rotate(Vector3.left*0.1f);
        }
        if(turn_right==1){
            transform.Rotate(Vector3.up*0.1f);
        }
        if(turn_left==1){
            transform.Rotate(-Vector3.up*0.1f);
        }
        if(apply_force==1){
                myRigidbody.AddForce(transform.up * motorPower);
        }
        
    }
}
