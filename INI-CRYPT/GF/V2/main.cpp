#include "stdafx.h"
#include "main.h"
#include <openssl/rc4.h>

std::string decode_rc4(const std::string&);
std::string encode_rc4(const std::string&);

int main(int argc, char* argv[])
{
	int result;
	string sFileName = "";

	std::cout << "-----------------------------------------------\n" ;
	std::cout << "|            Ini Patch Encrypt/Decrypt        |\n";
	std::cout << "|               GrandFantasia v2              |\n";
	std::cout << "|                   Version 2.0               |\n";
	std::cout << "-----------------------------------------------\n";
	std::cout << "             Created by DaveDevil's \n";
	std::cout << "-----------------------------------------------\n";
	if (argc == 1)
	{
		std::cout << "No input file ...\n";
		system("pause");
		exit(0);
	}
	else
	{
		int i = 1;
		while (i < argc)
		{
			sFileName = argv[i];

			if (sFileName == "1" || sFileName == "2")
			{
				break;
			}

			if (argv[argc] == "1")
			{
				result = FileCryptDecrypt(sFileName, 1);
			}
			else if (argv[argc] == "2")
			{
				result = FileCryptDecrypt(sFileName, 2);
			}
			else
			{
				result = FileCryptDecrypt(sFileName, 1);
			}

			switch (result)
			{
			case 0:
				std::cout << sFileName << " has been Decrypted !\n";
				break;
			case 1: 
				std::cout << sFileName << " has been encrypted !\n";
				break;
			case 2:
				std::cout << sFileName << " not found ...\n";
				break;
			}

			i++;
		}

	}


	printf("\n");
	system("pause");
	return 1;
}

int FileCryptDecrypt(string filename , int lockdown)
{

	//if lockdown == 1 - Decrypt only
	// if lockdown == 2 encrypt only
	int ret = 2;
	ifstream fOriginalFile(filename.c_str(), ios::binary | ios::ate);
	ifstream::pos_type pos = fOriginalFile.tellg();

	int size = (int)pos;

	//lockdown = 1;

	if (size < 0)
	{
		printf("File not found : %s \n", filename.c_str());
		fOriginalFile.close();
		return 3;
	}

	char * Data = new char[size];
	char * DataFinal = new char[size];

	fOriginalFile.seekg(0, ios::beg);

	fOriginalFile.read(Data, size);

	fOriginalFile.close();
	// If data is encrypt

	if(Data[0] == 0x42 || Data[1] == 0x5A || lockdown == 0 || lockdown == 2)
	{
		printf("Trying to Encrypt ... \n");

		//size = Pack(Data, size, "easyfun");

		
		DataFinal = (char *)encode_rc4(Data).c_str();
		
		//memcpy(DataFinal, Data, size);

		ret = 1;
	}
	else if (lockdown != 2)
	{
		//size = size - 8;
		printf("Trying to Decrypt ... \n");
		//memmove(&Data[0], &Data[8], size);
		//size = Unpack(Data, size, "easyfun");
		/*
		if (Data[size - 1] == 0x00)
			size = size - 1;
		*/
		DataFinal = (char*)decode_rc4(Data).c_str();
		//memcpy(DataFinal, Data, size);

		ret = 0;
	}

	
	delete(Data);

	if (ret != 2)
	{
		std::fstream SaveFile;
		string savefilename = filename;
		SaveFile.open(savefilename.c_str(), std::fstream::out | fstream::binary | std::fstream::trunc);
		/*
		if (ret == 0 && DataFinal[0] != 0xEF && DataFinal[1] != 0xBB)
		{
			unsigned char bom[] = { 0xEF,0xBB,0xBF };
			SaveFile.write((char*)bom, sizeof(bom));
		}
		*/
		SaveFile.write((char *)DataFinal, size);
		SaveFile.close();
	}

	delete(DataFinal);
	
	return ret;
}



int Unpack(char * Data , int size , string key)
{

	std::vector<char> UserKey(key.begin(), key.end());
	unsigned char * Datatmp = new unsigned char[size];
	uint num1 = 0x12345678;
	uint num2 = 0x23456789;
	uint num3 = 0x34567890;

	for (auto tmpkey : UserKey)
	{
		num1 = crctable[(num1 & 0xff) ^ tmpkey] ^ (num1 >> 8);
		num2 += num1 & 0xff;
		num2 = (num2 * 0x8088405L) + 1;
		num3 = crctable[((num2 >> 24) & 0xFF) ^ (num3 & 0xFF)] ^ (num3 >> 8);
	}

	int index = 0;
	int length = size;
	while (index < length)
	{

		uint xtmp = 0;
		xtmp = (num3 & 0x0fffd) | 2;
		xtmp = ((xtmp * (xtmp ^ 1)) >> 8);
		Datatmp[index] = xtmp ^ Data[index];

		int crctableindex = (num1 & 0xff) ^ Datatmp[index];
		num1 = crctable[crctableindex] ^ (num1 >> 8);
		num2 += num1 & 0xff;
		num2 = (num2 * 0x8088405L) + 1;
		num3 = crctable[((num2 >> 24) & 0xFF) ^ (num3 & 0xFF)] ^ (num3 >> 8);

		index++;
	}

	memmove(Data, Datatmp, size);
	delete(Datatmp);
	

	return size;
}

int Pack(char * Data, int size, string key)
{

	std::vector<char> UserKey(key.begin(), key.end());
	unsigned char * Datatmp = new unsigned char[size];
	uint num1 = 0x12345678;
	uint num2 = 0x23456789;
	uint num3 = 0x34567890;

	for (auto tmpkey : UserKey)
	{
		num1 = crctable[(num1 & 0xff) ^ tmpkey] ^ (num1 >> 8);
		num2 += num1 & 0xff;
		num2 = (num2 * 0x8088405L) + 1;
		num3 = crctable[((num2 >> 24) & 0xFF) ^ (num3 & 0xFF)] ^ (num3 >> 8);
	}

	int index = 0;
	int length = size;
	while (index < length)
	{

		uint xtmp = 0;
		
		num3 = ((num3 * (num3 ^ 1)) >> 8);
		Datatmp[index] = num3 ^ Data[index];
		int crctableindex = (num1 & 0xff) ^ Datatmp[index];
		
		//num1 = crctable[(num1 & 0xff) ^ Data[index]] ^ (num1 >> 8);
		num2 = (num2 * 0x8088405L) + 1;
		num1 = crctable[crctableindex] ^ (num1 >> 8);
		num2 -= num1 & 0xff;

		xtmp = (num2 & 0x0fffd) | 2;

		num3 = crctable[((num2 >> 24) & 0xFF) ^ (xtmp & 0xFF)] ^ (xtmp >> 8);
		index++;
	}

	memmove(Data, Datatmp, size);
	delete(Datatmp);

	return size;
}




string decode_rc4(const string& data) {
	RC4_KEY key;

	int len = data.size();
	unsigned char* obuf = (unsigned char*)malloc(len + 1);
	memset(obuf, 0, len + 1);

	unsigned char * local_41c = new unsigned char(8);
	strncpy((char*)local_41c, "easyfun", 8);
	RC4_set_key(&key, 8, local_41c);
	RC4(&key, len, (const unsigned char*)data.c_str(), obuf);

	string decode_data((char*)obuf, len);
	free(obuf);

	return decode_data;
}

string encode_rc4(const string& data) {
	RC4_KEY key;

	int len = data.size();
	unsigned char* obuf = (unsigned char*)malloc(len + 1);
	memset(obuf, 0, len + 1);

	unsigned char* local_41c = new unsigned char(8);
	strncpy((char*)local_41c, "easyfun", 8);
	RC4_set_key(&key, 8, local_41c);
	RC4(&key, len, (const unsigned char*)data.c_str(), obuf);

	string encode_data((char*)obuf, len);
	free(obuf);

	return encode_data;
}