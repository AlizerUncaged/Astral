
# FastYolo

Project: https://github.com/maalik0786/FastYolo

> Alizer's note: I have the Cuda installations v10.1, v11.2 and v11.6, CudNN installation 8.4 and it seems to work.

Yolo Object Detection library for .NET 6. This one requires Cuda 11.7 and CudNN 8.4.1 to be installed to work, it will use your NVidia GPU for processing (which is many times faster than using the CPU version that is disabled in this release). Any GPU from Maxwell upwards is support (Cuda api 5.2+, Maxwell GPUs like GTX 750 and above). Since 11.1 it also supports OpenCV for more advanced features like tracking, shape detection, etc.
			
Includes the .dll files for Yolo Darknet Wrapper, Real-Time Object Detection (yolo core of AlexeyAB/darknet), including opencv_world460.dll and pthreadVC2.dll as needed by the yolo_cpp_dll implementation.

#### Different operating systems should have different cuda versions:
1. Windows: should have Cuda 11.7 and CUDNN 8.3.2+ installed, if not an exception is thrown with detailed installation instructions.
Environment path for cuda 11.7 must be set (installer does this), e.g. CUDA_PATH=C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.7
2. If you get error: Could not load library cudnn_cnn_infer64_8.dll. Error code 126, 
Please follow [this link](https://docs.nvidia.com/deeplearning/cudnn/install-guide/index.html#install-zlib-windows) for proper installation of CUDNN
3. Linux x64:  should have Cuda 11.7 and CUDNN 8.4.1+ installed, if not an exception is thrown with detailed installation instructions.
4. Jetson Nano Arm64:  should have Cuda 10.0 and CUDNN 8 installed, if not an exception is thrown with detailed installation instructions.
5. Jetson Xavier Arm64:  should have Cuda 10.2 and CUDNN 8 installed, if not an exception is thrown with detailed installation instructions.

The nuget installer includes all other needed files, for compiling it yourself, copy cudnn64_8.dll, opencv_world460.dll, pthreadVC2.dll into the FastYolo folder and compile yolo_cpp_dll.dll into it as well.

Current version is for .NET 6, you can check older releases for .NET 5, .NET Core 3.1, .NET 4.6 and lower.
