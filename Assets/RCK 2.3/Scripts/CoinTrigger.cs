using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; 



public class CoinTrigger : MonoBehaviour
{
    
    SerialPort arduino = new SerialPort("COM5" , 9600);
    public MeshRenderer[] coins;

    public char[] c;
    public string coinname,c2;
    private void OnTriggerEnter(Collider other) {
        
        
        gameObject.SetActive(false);
        coinname=gameObject.name;
        c=coinname.ToCharArray();
        c2=c[5].ToString();
        for (int i=0; i<1000; i++) {
            arduino.WriteLine(c2);
        }
        
       
        
    }


}
