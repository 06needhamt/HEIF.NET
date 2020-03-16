#include "decode.h"

LIBHEIF_NATIVE_CALL int __stdcall GetNumberOfImagesInFile(const char* fileName) {
	struct heif_context* ctx = heif_context_alloc();
	heif_context_read_from_file(ctx, fileName, NULL);

	int imageCount = heif_context_get_number_of_top_level_images(ctx);
	heif_context_free(ctx);

	return imageCount;
}

LIBHEIF_NATIVE_CALL int __stdcall GetImageIds(const char* fileName, int* ids, int count) {
	struct heif_context* ctx = heif_context_alloc();
	heif_context_read_from_file(ctx, fileName, NULL);

	int IdCount = heif_context_get_list_of_top_level_image_IDs(ctx, ids, count);

	heif_context_free(ctx);

	return IdCount;
}

LIBHEIF_NATIVE_CALL int __stdcall DecodeImage(const char* fileName, int imageId, uint8_t* data) {
	struct heif_context* ctx = heif_context_alloc();
	heif_context_read_from_file(ctx, fileName, NULL);

	struct heif_image_handle* handle;
	heif_context_get_image_handle(ctx, (heif_item_id)imageId, &handle);

	struct heif_image* image;
	struct heif_decoding_options* options = heif_decoding_options_alloc();
	heif_decode_image(handle, &image, heif_colorspace_RGB, heif_chroma_interleaved_RGB, options);

	int width = heif_image_handle_get_width(handle);
	int height = heif_image_handle_get_height(handle);

	int stride;
	data = heif_image_get_plane(image, heif_channel_interleaved, &stride);
	int bytes = width * height * stride;
	
	heif_decoding_options_free(options);
	heif_image_release(image);
	heif_image_handle_release(handle);
	heif_context_free(ctx);

	return bytes;
}