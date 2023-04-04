#pragma once

int FileCryptDecrypt(string filename, int lockdown);

class State
{
	unsigned long s[261];
	int i, j;

	void swap(int a, int b);

public:
	unsigned char getbyte(void);
	State(unsigned long key[], int length);
};

void gethexdigit(char in, unsigned char& out);
int gethexkey(unsigned long data[], std::string key);
int gettextkey(unsigned long data[], std::string key);


unsigned long* __cdecl RC4(unsigned long* a1, unsigned int a2, unsigned char* a3, unsigned char* a4);

unsigned long* __cdecl RC4_set_key(unsigned long* a1, int a2, int a3);