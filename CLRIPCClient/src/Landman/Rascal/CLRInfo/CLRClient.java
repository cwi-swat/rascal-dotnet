package Landman.Rascal.CLRInfo;

import java.io.*;
import java.net.*;

import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationRequest;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationRequest.Builder;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationResponse;


public class CLRClient {

	/**
	 * @param args
	 * @throws IOException 
	 * @throws UnknownHostException 
	 */
	public static void main(String[] args) throws UnknownHostException, IOException {
		 Socket clientSocket = new Socket("localhost", 5555);
		 DataOutputStream outToServer = new DataOutputStream(clientSocket.getOutputStream());
		 DataInputStream inFromServer = new DataInputStream(clientSocket.getInputStream());
		 Builder req = InformationRequest.newBuilder();
		 req.setAssembly("/usr/lib/mono/2.0/System.dll");
		 InformationRequest actualRequest = req.build();
		 byte[] data = actualRequest.toByteArray();
		 outToServer.writeInt(data.length);
		 outToServer.write(data);
		 int resultSize = inFromServer.readInt();
		 byte[] result = new byte[resultSize];
		 int bytesRead = 0;
		 while (bytesRead < resultSize) {
			 bytesRead += inFromServer.read(result, bytesRead, resultSize - bytesRead);
		 }
		 FileOutputStream x = new FileOutputStream("/home/davy/rec.prot");
		 x.write(result);
		 x.close();
		 //FileInputStream x = new FileInputStream("/home/davy/message.prot");
		 InformationResponse actualResult = InformationResponse.parseFrom(result);
		 System.out.print( actualResult.getTypes(0).toString());
	}

}
