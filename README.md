# LearningModbus

In this repo you can find an example of how to write and read data with NModbus package.

## PublishingData 
This project is a worker service project but it is meant to be run on Linux systems. It reads data from some Linux files, so you shouldn't run this on other operating systems.

## ReadingData
This is a console application. It reads data that published with Modbus and make a request to an endpoint in StoringData project.

## StoringData
This is a .Net 6 Minimal API project. It has only one endpoint and that is to store values that sended by ReadingData project. You need to have MSSql for database and
you need to make migrations from package manager console. 

## ConvertType 
This project has onyle one class. In that class has an extension methods to convert float to ushort array and convert ushort array to float.

Work flow basically is like this with all these projects:

- Start PublishingData project on some Linux system, and it reads CPU usage, CPU temperature, memory usage and send it over Modbus TCP.
- Make migrations and make sure database is created.
- Start StoringData API.
- Start ReadingData console application and you will see the data coming from your Linux system. It should also be storing the data to the database.
