// Done on some minute don't be bad please xD
//

#include "stdafx.h"
#include "main.h"

int main(int argc, char* argv[])
{
	int result;
	string sFileName = "";

	std::cout << "-----------------------------------------------\n";
	std::cout << "|             Ini Encrypt/Decrypt             |\n";
	std::cout << "|               Grand Fantasia                |\n";
	std::cout << "|                 Version 1.0                 |\n";
	std::cout << "-----------------------------------------------\n";
	std::cout << "|                by davedevils                |\n";
	std::cout << "-----------------------------------------------\n";
	std::cout << " \n";

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

	string file = filename;
	std::fstream infile;
	infile.open(file.c_str(), std::ios::in | std::ios::binary);
	if (!infile.is_open())
	{
		printf("File not found : %s \n", filename.c_str());
		return 3;
	}

	if (file.find(".bin", file.length() - 4) != std::string::npos) 
	{
		file.erase(file.length() - 4);
		ret = 0;
	}
	else
	{
		file.append(".bin");
		ret = 1;
	}

	ifstream fOriginalFile(filename.c_str(), ios::binary | ios::ate);
	ifstream::pos_type pos = fOriginalFile.tellg();

	int size = (int)pos;
	if (size < 0)
	{
		printf("File not found : %s \n", filename.c_str());
		fOriginalFile.close();
		return 3;
	}

	unsigned char* Data = new unsigned char[size];
	unsigned char* DataFinal = new  unsigned char[size];

	fOriginalFile.seekg(0, ios::beg);

	fOriginalFile.read((char*)Data, size);

	fOriginalFile.close();
	unsigned long v6[261];
	char dest[8];
	strncpy(dest, (const char*)"easyfun", 8u);
	RC4_set_key(v6, 8, (int)dest);
	RC4(v6, size, Data, Data);

	std::fstream SaveFile;
	SaveFile.open(file.c_str(), std::fstream::out | fstream::binary | std::fstream::trunc);
	SaveFile.write((char*)Data, size);
	SaveFile.close();

	return ret;
}

unsigned long* __cdecl RC4_set_key(unsigned long* a1, int a2, int a3)
{
	int v3; 
	unsigned long* result; 
	int v5; 
	int v6; 
	int v7; 
	char v8;
	int v9; 
	int v10;
	int v11;
	int v12;
	int v13;
	int v14;
	int v15;
	int v16;
	unsigned __int8 v17;
	char v18;
	char v19;
	int v20; 
	int v21; 
	int v22; 

	v3 = 0;
	*a1 = 0;
	a1[1] = 0;
	do
	{
		a1[v3 + 2] = v3;
		++v3;
	} while (v3 != 256);
	result = a1;
	v17 = 0;
	v15 = 0;
	do
	{
		v5 = result[2];
		v20 = 1;
		v21 = 0;
		if (v15 + 1 != a2)
		{
			v21 = v15 + 1;
			v20 = v15 + 2;
		}
		v6 = (unsigned __int8)(*(unsigned char*)(a3 + v15) + v5 + v17);
		result[2] = a1[v6 + 2];
		a1[v6 + 2] = v5;
		v7 = result[3];
		v8 = *(unsigned char*)(a3 + v21);
		v22 = 1;
		v19 = v8;
		v9 = 0;
		if (a2 != v20)
		{
			v9 = v20;
			v22 = v20 + 1;
		}
		v10 = (unsigned __int8)(v19 + v7 + v6);
		result[3] = a1[v10 + 2];
		a1[v10 + 2] = v7;
		v11 = result[4];
		v18 = v11;
		v12 = 0;
		v16 = 1;
		if (a2 != v22)
		{
			v12 = v22;
			v16 = v22 + 1;
			v11 = result[4];
		}
		v13 = (unsigned __int8)(*(unsigned char*)(a3 + v9) + v18 + v10);
		result[4] = a1[v13 + 2];
		a1[v13 + 2] = v11;
		v14 = result[5];
		v17 = v14 + v13 + *(unsigned char*)(a3 + v12);
		v15 = a2 == v16 ? 0 : v16;
		result[5] = a1[v17 + 2];
		result += 4;
		a1[v17 + 2] = v14;
	} while (result != a1 + 256);
	return result;
}


State::State(unsigned long key[], int length)
{
	for (int k = 0; k < 256; k++)
	{
		s[k] = k;
	}

	j = 0;

	for (i = 0; i < 256; i++)
	{
		j = (j + s[i] + key[i % length]) % 256;
		swap(i, j);
	}

	i = j = 0;
}

void State::swap(int a, int b)
{
	unsigned char temp = s[i];
	s[i] = s[j];
	s[j] = temp;
}

unsigned char State::getbyte(void)
{
	i = (i + 1) % 256;
	j = (j + s[i]) % 256;
	swap(i, j);
	int index = (s[i] + s[j])  % 256;
	return s[index];
}



void gethexdigit(char in, unsigned char& out)
{
	if (in >= '0' && in <= '9')
	{
		out += in - '0';
	}
	else if (in >= 'a' && in <= 'f')
	{
		out += in - 'a' + 10;
	}
	else
	{
		std::cout << "Hex key contains letter outside range 0-9 or a-z: " << in << std::endl;
		exit(EXIT_FAILURE);
	}
}



int gethexkey(unsigned long data[], std::string key)
{
	if (key.length() % 2) //key must be of even length if it's hex
	{
		std::cout << "Hex key must have an even number of characters" << std::endl;
		exit(EXIT_FAILURE);
	}

	if (key.length() > 1024)
	{
		std::cout << "Hex key cannot be longer than 512 characters long" << std::endl;
		exit(EXIT_FAILURE);
	}

	unsigned char byte;
	size_t i;

	for (i = 0; i < key.length(); i++)
	{
		gethexdigit(key[i], byte);
		byte <<= 4;
		i++;
		gethexdigit(key[i], byte);
		data[(i - 1) / 2] = byte;
	}
	return i / 2;
}

int gettextkey(unsigned long data[], std::string key)
{
	if (key.length() > 512)
	{
		std::cout << "ASCII key must be 256 characters or less" << std::endl;
		exit(EXIT_FAILURE);
	}

	size_t i;

	for (i = 0; i < key.length(); i++)
	{
		data[i] = key[i];
	}

	return i;
}

unsigned long* __cdecl RC4(unsigned long* a1, unsigned int a2, unsigned char * a3, unsigned char* a4)
{
	// BAAADDDDD implemented
	unsigned long * result; 
	int v5; 
	int v6; 
	unsigned char* v7; 
	int v8; 
	int v9; 
	int v10;
	int v11;
	int v12;
	int v13;
	int v14;
	int v15;
	int v16;
	int v17;
	int v18;
	int v19;
	int v20;
	int v21;
	int v22;
	int v23;
	int v24;
	int v25;
	int v26;
	int v27;
	int v28;
	int v29;
	int v30;
	int v31;
	int v32;
	int v33;
	int v34;
	int v35;
	int v36;
	int v37;
	int v38;
	int v39;
	int v40;
	int v41;
	int v42;
	int v43;
	int v44;
	int v45;
	int v46;
	int v47;
	int v48;
	int v49;
	int v50;
	int v51;
	char v52; 
	char v53; 
	char v54; 
	char v55; 
	char v56; 
	char v57; 
	char v58; 
	char v59; 
	unsigned int v60;
	unsigned int v61; 
	unsigned char* v62; 
	int v63; 
	int v64;

	result = a1;
	v5 = *a1;
	v6 = a1[1];
	v60 = a2 >> 3;
	v61 = a2 >> 3;
	if (a2 >> 3)
	{
		v7 = a4;
		v52 = a1[1];
		v62 = a3;
		do
		{
			v8 = (unsigned __int8)(v5 + 1);
			v9 = a1[v8 + 2];
			v10 = (unsigned __int8)(v9 + v52);
			v11 = a1[v10 + 2];
			v53 = v9 + v52;
			a1[v8 + 2] = v11;
			a1[v10 + 2] = v9;
			v12 = (unsigned __int8)(v8 + 1);
			*v7 = *v62 ^ a1[(unsigned __int8)(v11 + v9) + 2];
			v13 = a1[v12 + 2];
			v14 = (unsigned __int8)(v13 + v53);
			v15 = a1[v14 + 2];
			v54 = v13 + v53;
			a1[v12 + 2] = v15;
			a1[v14 + 2] = v13;
			v16 = (unsigned __int8)(v12 + 1);
			v7[1] = v62[1] ^ a1[(unsigned __int8)(v15 + v13) + 2];
			v17 = a1[v16 + 2];
			v18 = (unsigned __int8)(v17 + v54);
			v19 = a1[v18 + 2];
			v55 = v17 + v54;
			a1[v16 + 2] = v19;
			a1[v18 + 2] = v17;
			v20 = (unsigned __int8)(v16 + 1);
			v7[2] = v62[2] ^ a1[(unsigned __int8)(v19 + v17) + 2];
			v21 = a1[v20 + 2];
			v22 = (unsigned __int8)(v21 + v55);
			v23 = a1[v22 + 2];
			v56 = v21 + v55;
			a1[v20 + 2] = v23;
			a1[v22 + 2] = v21;
			v24 = (unsigned __int8)(v20 + 1);
			v7[3] = v62[3] ^ a1[(unsigned __int8)(v23 + v21) + 2];
			v25 = a1[v24 + 2];
			v26 = (unsigned __int8)(v25 + v56);
			v27 = a1[v26 + 2];
			v57 = v25 + v56;
			a1[v24 + 2] = v27;
			a1[v26 + 2] = v25;
			v28 = (unsigned __int8)(v24 + 1);
			v7[4] = v62[4] ^ a1[(unsigned __int8)(v27 + v25) + 2];
			v29 = a1[v28 + 2];
			v30 = (unsigned __int8)(v29 + v57);
			v31 = a1[v30 + 2];
			v58 = v29 + v57;
			a1[v28 + 2] = v31;
			a1[v30 + 2] = v29;
			v32 = (unsigned __int8)(v28 + 1);
			v7[5] = v62[5] ^ a1[(unsigned __int8)(v31 + v29) + 2];
			v33 = a1[v32 + 2];
			v34 = (unsigned __int8)(v33 + v58);
			v35 = a1[v34 + 2];
			v59 = v33 + v58;
			a1[v32 + 2] = v35;
			a1[v34 + 2] = v33;
			v5 = (unsigned __int8)(v32 + 1);
			v7[6] = v62[6] ^ a1[(unsigned __int8)(v35 + v33) + 2];
			v36 = a1[v5 + 2];
			v6 = (unsigned __int8)(v36 + v59);
			v37 = a1[v6 + 2];
			v52 = v6;
			a1[v5 + 2] = v37;
			a1[v6 + 2] = v36;
			v7[7] = v62[7] ^ LOBYTE(a1[(unsigned __int8)(v37 + v36) + 2]);
			v7 += 8;
			v62 += 8;
			--v61;
		} while (v61);
		a3 += 8 * v60;
		a4 += 8 * v60;
	}
	v63 = a2 & 7;
	if ((a2 & 7) != 0)
	{
		v5 = (unsigned __int8)(v5 + 1);
		v38 = a1[v5 + 2];
		v6 = (unsigned __int8)(v38 + v6);
		v39 = a1[v6 + 2];
		a1[v5 + 2] = v39;
		a1[v6 + 2] = v38;
		*a4 = *a3 ^ LOBYTE(a1[(unsigned __int8)(v39 + v38) + 2]);
		if (v63 != 1)
		{
			v5 = (unsigned __int8)(v5 + 1);
			v40 = a1[v5 + 2];
			v6 = (unsigned __int8)(v40 + v6);
			v41 = a1[v6 + 2];
			a1[v5 + 2] = v41;
			a1[v6 + 2] = v40;
			a4[1] = a3[1] ^ LOBYTE(a1[(unsigned __int8)(v41 + v40) + 2]);
			if (v63 != 2)
			{
				v5 = (unsigned __int8)(v5 + 1);
				v42 = a1[v5 + 2];
				v6 = (unsigned __int8)(v42 + v6);
				v43 = a1[v6 + 2];
				a1[v5 + 2] = v43;
				a1[v6 + 2] = v42;
				a4[2] = a3[2] ^ LOBYTE(a1[(unsigned __int8)(v43 + v42) + 2]);
				if (v63 != 3)
				{
					v5 = (unsigned __int8)(v5 + 1);
					v44 = a1[v5 + 2];
					v6 = (unsigned __int8)(v44 + v6);
					v45 = a1[v6 + 2];
					a1[v5 + 2] = v45;
					a1[v6 + 2] = v44;
					a4[3] = a3[3] ^ LOBYTE(a1[(unsigned __int8)(v45 + v44) + 2]);
					if (v63 != 4)
					{
						v5 = (unsigned __int8)(v5 + 1);
						v46 = a1[v5 + 2];
						v6 = (unsigned __int8)(v46 + v6);
						v47 = a1[v6 + 2];
						a1[v5 + 2] = v47;
						a1[v6 + 2] = v46;
						a4[4] = a3[4] ^ LOBYTE(a1[(unsigned __int8)(v47 + v46) + 2]);
						v64 = v63 - 5;
						if (v64)
						{
							v5 = (unsigned __int8)(v5 + 1);
							v48 = a1[v5 + 2];
							v6 = (unsigned __int8)(v48 + v6);
							v49 = a1[v6 + 2];
							a1[v5 + 2] = v49;
							a1[v6 + 2] = v48;
							a4[5] = a3[5] ^ LOBYTE(a1[(unsigned __int8)(v49 + v48) + 2]);
							if (v64 != 1)
							{
								v5 = (unsigned __int8)(v5 + 1);
								v50 = a1[v5 + 2];
								v6 = (unsigned __int8)(v50 + v6);
								v51 = a1[v6 + 2];
								a1[v5 + 2] = v51;
								a1[v6 + 2] = v50;
								a4[6] = a3[6] ^ a1[(unsigned __int8)(v51 + v50) + 2];
							}
						}
					}
				}
			}
		}
	}
	*a1 = v5;
	a1[1] = v6;
	return result;
}
