// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>
#include <string>
#include <fstream>
#include <iostream>
#include <vector>
#include <cstdlib>
#include <cstdio>
#include <cstring>
using namespace std;

#define LOWORD(l) ((unsigned int)(l))
#define HIWORD(l) ((unsigned int)(((unsigned long)(l) >> 16) & 0xFFFF))
#define LOBYTE(w) ((unsigned char)(w))
#define HIBYTE(w) ((unsigned char)(((unsigned int)(w) >> 8) & 0xFF))

#define uint unsigned int
#define _CRT_SECURE_NO_WARNINGS 0


// TODO: reference additional headers your program requires here
