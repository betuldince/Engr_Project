/*THIS CODE IS FOR UNITY ARDUINO SERIAL COMMUNICATION AND PLAYING UNITY GAME USING ARDUINO JOYSTICK.
PLAYER FIRST CONFIGURE 3*3 LED MATRIX USING JOYSTICK AND SELECTING SOME OF LEDS TO LIGHT UP. THEN
WHEN HE IS READY, HE QUITS CONFIGURATION SETTING BY CLICKING START LED AND GAME STARTS. HE PLAYS 
GAME WITH JOYSTICK AND LIGHTED LEDS TURN OFF WHEN HE COLLECTS NECCECARY ITEMS IN THE MAP IN THE ARDUİNO*/

#include <Wire.h>                /* 9-axis oriaentation sensor libraries*/
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>


#define joyX A0 /* Joystick analog inputs */
#define joyY A1

Adafruit_BNO055 bno = Adafruit_BNO055(55);

int xValue,yValue, x,y, button1, button2, button3, button1Pressed, unitydata, unityledstates[]={0, 0, 0, 0, 0, 0, 0, 0, 0};
double  bno_x, bno_y,  bno_z;
int ledStates[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};   /* to store led state*/

const int led1 = 2;   /*output led pin numbers for 3*3 led matrix*/
const int led2 = 3;
const int led3 = 4;
const int led4 = 5;
const int led5 = 6;
const int led6 = 7;
const int led7 = 8;
const int led8 = 9;
const int led9 = 10;

const int stopled = 12;    /* to show whether player is in led configuration or game play mode*/
const int startled = 11;
const int sw1 = 13;    /*  joystich button1 witch pin */
const int sw2= 17;  /* other buttons */
const int sw3= 18; 

int joystick(int analog_v);  /* function to read joystick data */

void blinkled(int led);   /* when the player is in config. mode, it blinks corresponding led */

  
void setup() {
  Serial.begin(9600);

  /*9-axis sensor initialization**/
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }
  bno.setExtCrystalUse(true);  
  sensors_event_t event; 
  bno.getEvent(&event);


  /*Led configuration*/
  
 
  
  pinMode(led1, OUTPUT);  /* output and input pins*/
  pinMode(led2, OUTPUT);
  pinMode(led3, OUTPUT);
  pinMode(led4, OUTPUT);
  pinMode(led5, OUTPUT);
  pinMode(led6, OUTPUT);
  pinMode(led7, OUTPUT);
  pinMode(led8, OUTPUT);
  pinMode(led9, OUTPUT);
  
  pinMode(stopled, OUTPUT);
  pinMode(startled, OUTPUT);  
  pinMode(sw1, INPUT);
  pinMode(sw2, INPUT);
  pinMode(sw3, INPUT);
  
  digitalWrite(sw1,HIGH);
  digitalWrite(stopled,LOW);
  digitalWrite(startled,HIGH);
  
  
  int i= led5;                   /* shows which led thwe player is currenlty looking*/
  int wait=0;                    /* to stop constantly enter into movement statements when the player holds joystick on any direction */

  

  button1 = digitalRead(sw1);   /* reads joystck button1 state */
  
  while ((button1 == HIGH) || (i != stopled)) {  /*this while loop moves the player ın 3*3 led matrix using joystick data*/
    
  xValue = analogRead(joyX);  /*joystıck data*/
  yValue = analogRead(joyY); 
  x = joystick(xValue);
  y = joystick(yValue);
  

  if (((x == 0) & (y == 0)) & (button1 == HIGH) )  {   /*ıf button1 ıs not pressed, does not change the state and blinks the current led*/
    blinkled(i);
    digitalWrite(i, ledStates[i-2]);    
    wait=0;
  }
  
  
  
  if ((button1 == LOW) & (i != stopled) & (wait == 0)) { /*changes led's statement to opposite of its current statement*/
    if (ledStates[i-2] == 0){
      digitalWrite(i,HIGH);
      ledStates[i-2] = 1;
    }
    else if (ledStates[i-2] == 1){
      digitalWrite(i,LOW);
      ledStates[i-2] = 0;
    }
    wait=1;
  }

    
  if((x != 0) & (wait == 0) & (i != stopled) ) {                 /*if statements are for controlling joystick data in 3*3 led matrix*/
    if (((i== led1) & (x > 0) || ((i == led9)& (x < 0)) ))  {
      i=abs(i-(led9+led1));
    } 
    else {
      if (x>0) {
       i=i-1;
      }
      if (x<0) {
       i=i+1;
      }
    }  
    wait=1;
  }

  if((y != 0) & (wait == 0)) {
    if ((i == led1) & (y < 0)) {
      i=abs(i-(led9+led1));
    } 
    else if ((i == led9)& (y > 0)) {
      i= stopled;
    }
    else if ((i == stopled) & (y < 0)) {
      i= led9;      
    }
    else if ((i == stopled) & (y > 0)) {
      i= stopled;      
    } 
    else {
      if (y>0) {
        if (i+3 > led9) {
          i = i-5;
        }
        else {
          i=i+3;
        }
      }
      if (y<0) {
        if (i-3 < led1) {
          i = i+5;
        }
        else {
          i=i-3;
        }
      }
     }  
    wait=1; 
    } 
 
   button1 = digitalRead(sw1); 
  } 
  
  

   
  for (i=2; i<11;  i++) {        /*this part just for good animation after the configuration is done*/
    digitalWrite(i, LOW);
    delay(50); 
    digitalWrite(i, ledStates[i-2]);
    unityledstates[i-2] =  ledStates[i-2];
    delay(50);       
  }
  digitalWrite(stopled,HIGH);
  digitalWrite(startled,HIGH);  
  delay(50);
  digitalWrite(startled,LOW);

  for (int i=0; i<9; i++){
    unityledstates[i]=ledStates[i];
  }

   
}



void loop() {
   
  /* Get a new sensor event */ 
  sensors_event_t event; 
  bno.getEvent(&event);

  
  bno_x=event.orientation.x; /*gets 9-axis orientation data*/
  bno_y=event.orientation.y;
  bno_z=event.orientation.z;



  xValue = analogRead(joyX);  /*reads joystick data*/
  yValue = analogRead(joyY);
  x = joystick(xValue);
  y = joystick(yValue);
  button1 = digitalRead(sw1);

  /*Serial Data Read from Unity*/
  
    unitydata= Serial.read()-48;
    if((unitydata>0) & (unitydata<10)) { 
      unityledstates[unitydata-1]=0;
      digitalWrite(unitydata+1, LOW);

    }
  


  if (button1 == 1) {
    button1Pressed = 0;         
  }
  else if (button1 == 0) {
    button1Pressed = 1;         
  }  



 //button2= analogRead(sw2); /*other button's data */
 //button3= analogRead(sw3);
  
  //print the values to the serial port
  Serial.print(x);
  Serial.print(",");
  Serial.print(y);
  Serial.print(",");
  Serial.print(button1Pressed);
  Serial.print(",");
  for (int i=0; i<9; i++) {
    Serial.print(unityledstates[i]);
    Serial.print(",");
  }
  //Serial.print(",");
  //Serial.print(button2);
 // Serial.print(",");
 // Serial.print(button3);
  Serial.print(",");
  Serial.print(bno_x, 4);
  Serial.print(",");
  Serial.print(bno_y, 4);
  Serial.print(",");
  Serial.print(bno_z, 4);  
  Serial.print(",");
  Serial.print(unitydata); 
  Serial.println("");  



  
}





void blinkled(int led) {     /*this function blinks the current led*/
  int currenttime;
  int button1 = HIGH;
  int delayLED = 400;
  int beginMillis = millis();
  
  if ((button1 == HIGH) ) {   
    while( ((currenttime =millis()) - beginMillis < delayLED/2 ) & (x == 0) & (y == 0) & (button1 == HIGH) ) {
      
      xValue = analogRead(joyX);
      yValue = analogRead(joyY); 
      x = joystick(xValue);
      y = joystick(yValue);  
      button1 = digitalRead(sw1);        
      digitalWrite(led,HIGH);
    }
    while ( ((currenttime =millis()) - beginMillis < delayLED ) & (x == 0) & (y == 0) & (button1 == HIGH) ) { 
      xValue = analogRead(joyX);
      yValue = analogRead(joyY); 
      x = joystick(xValue);
      y = joystick(yValue);  
      button1 = digitalRead(sw1); 
  
      digitalWrite(led,LOW);
      }
  digitalWrite(led,LOW);
  }
}




int joystick(int analog_v) { /* this function reads the analog joystick data and categorize it in a small interval*/
  
     if (analog_v < 200 ) {
    return 2;
    }
  else if ((200 <= analog_v)& (analog_v < 400)) {
    return 1;
  }
  else if ((400 <= analog_v)& (analog_v < 600)) {
    return 0;
  }
  else if ((600 <= analog_v)& (analog_v < 800)) {
    return -1;
  }
  else {
    return -2;
  }
}
