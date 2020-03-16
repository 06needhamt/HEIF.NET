#pragma once
#include <libheif\heif.h>

#define LIBHEIF_NATIVE_CALL __declspec(dllexport)

LIBHEIF_NATIVE_CALL int __stdcall GetNumberOfImagesInFile(const char* fileName);
LIBHEIF_NATIVE_CALL int __stdcall GetImageIds(const char* fileName, int* ids, int count);
LIBHEIF_NATIVE_CALL uint8_t* __stdcall DecodeImage(const char* fileName, int imageId, int* size);