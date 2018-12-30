#include <Ultrasonic.h>

#define TRIGGER_PIN  13
#define ECHO_PIN     12

Ultrasonic ultrasonic(TRIGGER_PIN, ECHO_PIN);
bool active = false;

void setup()
{
  Serial.begin(9600);
}

void loop()
{
  char data = Serial.read();
  switch(data)
  {
    case 'o':
      active = true;
      Serial.print("open");
      delay(100);
      return;
    case 'c':
      active = false;
      Serial.print("closed"); 
      break;
  }

  if(active == true)
  {
    GetSignal();
  }
}

void GetSignal()
{
      float cmMsec, inMsec;
      long microsec = ultrasonic.timing();
    
      cmMsec = ultrasonic.convert(microsec, Ultrasonic::CM);
      inMsec = ultrasonic.convert(microsec, Ultrasonic::IN);

      String result = "";
      result += cmMsec;
      Serial.println(result);
      
      //Serial.print("MS: ");
      //Serial.print(microsec);
      //Serial.print(", CM: ");
      //Serial.print(cmMsec);
      //Serial.print(", IN: ");
      //Serial.println(inMsec);
      delay(10);
}
