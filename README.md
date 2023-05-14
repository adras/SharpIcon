# SharpIcon
This library aims to be an implementation of https://en.wikipedia.org/wiki/ICO_(file_format) to read .ico/.cur files

Since the embedded image in .ico files can be bmp, png the goal is to simply provide a raw byte[] of that data
so that it can be processed by e.g. ImageSharp

Current state is WorkInProgress

# TODO
(*) Read information in image header
() Complete the implementation of the data structs
() Read raw image data
