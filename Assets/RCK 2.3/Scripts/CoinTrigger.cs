using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; 



public class CoinTrigger : MonoBehaviour
{
    
   // SerialPort arduino = new SerialPort("COM5" , 9600);
    //public MeshRenderer[] coins;
    //SerialPort arduino = new SerialPort("COM5" , 9600);
    
    public string c;
    public string coinname;
    public string c2;
   // public CarMovement a;
    private void OnTriggerEnter(Collider other) {

        

        //if (CoinCollected != null) {

            gameObject.SetActive(false);
            
            coinname=gameObject.name;
            c=coinname.Substring(5,1);
            c2=c;
            
            CarMovement.arduino.Write(c2);
            
          //  a.setCoinNum(c2);
           // coinCollected.CoinCollected(c2);
        
        //}  
    }
        public string getNum(){
            return c2;
            
        }

}
