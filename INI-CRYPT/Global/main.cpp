// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "main.h"

int main(int argc, char* argv[])
{
	int result;
	string sFileName = "";

	std::cout << "-----------------------------------------------\n" ;
	std::cout << "|         AuraKingdom Ini Encrypt/Decrypt     |\n";
	std::cout << "|                     TW v3                   |\n";
	std::cout << "|                   Version 1.0               |\n";
	std::cout << "-----------------------------------------------\n";
	std::cout << "             Created by DaveDevil's \n";
	std::cout << "        Special thanks to AKDev's group\n";
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
				result = FileCryptDecrypt(sFileName, 0);
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
	unsigned char * DataFinal = new  unsigned char[size];

	fOriginalFile.seekg(0, ios::beg);

	fOriginalFile.read(Data, size);

	fOriginalFile.close();
	// If data is encrypt
	if (std::memcmp(Data, "<encode>", 8) == 0 && lockdown != 2)
	{
		size = size - 8;
		printf("Trying to Decrypt ... \n");
		memmove(&Data[0], &Data[8], size);
		size = Unpack(Data, size, "ghfdsjdl");

		if (Data[size - 1] == 0x00)
			size = size - 1;

		memcpy(DataFinal, Data, size);
		ret = 0;
	}
	else if((std::memcmp(Data, "<encode>", 8) == 0 ||lockdown == 0 || lockdown == 2)
	{
		printf("Trying to Encrypt ... \n");

		size = Pack(Data, size, "ghfdsjdl");
		DataFinal = new unsigned char[size+9];
		memmove(&DataFinal[8], &Data[0], size);
		memcpy(&DataFinal[0], "<encode>", 8);
		ret = 1;
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
		xtmp = (num3 & 0x0fffd) | 2;
		xtmp = ((xtmp * (xtmp ^ 1)) >> 8);
		Datatmp[index] = xtmp ^ Data[index];

		num1 = crctable[(num1 & 0xff) ^ Data[index]] ^ (num1 >> 8);
		num2 += num1 & 0xff;
		num2 = (num2 * 0x8088405L) + 1;
		num3 = crctable[((num2 >> 24) & 0xFF) ^ (num3 & 0xFF)] ^ (num3 >> 8);

		index++;
	}

	memmove(Data, Datatmp, size);
	delete(Datatmp);

	return size;
}