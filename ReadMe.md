

<div>
  <img width="220" height="210" align="left" src="https://i.ibb.co/Yjk6WKb/Logo.png" alt="Astral"/>
  <br>
  <h1>Astral</h1>
  <p>Astral is a machine learning AI for game automation. Astral recognizes and observes game objects in the same way as any other human would, using the Yolo algorithm, powerful GPUs, a trained model, and a screen as input.</p>
</div>
<br/>
<br/>

## 💻Requirements
+ <img src="https://tinyurl.com/59snr2c5" height="20px"></img> At least Windows 7 SP1, (tested on Windows 10 and 11).
+ 💾 2GB of available RAM.
+ 🖥️ For GPU Support:
	+ <img src="https://cdn.worldvectorlogo.com/logos/nvidia-7.svg" height="20px"></img> At least CUDA 11.0 and cuDNN 8.4.0
	+ <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/Windows_logo_-_2012_%28dark_blue%29.svg/2048px-Windows_logo_-_2012_%28dark_blue%29.svg.png" height="20px"></img> At least Windows 10.
> 💡 Unfortunately, none of the compatible CUDA versions work with Windows 7. For operating systems below Windows 10, they can opt in for CPU.'

> 💡 Unfortunately, AMD GPUs are not supported; for GPU support, an Nvidia GPU with CUDA is required.
>
## 🏗️ Dependencies
### 🕵️ Installing Detector Dependencies
#### ⚡ FastYolo
> Full instructions for installing required dependencies at https://github.com/maalik0786/FastYolo 💡 Although different CUDA versions greater than or 11.0 **should** work, tested with CUDA 11.0 with cuDNN 8.4.0 and it worked.
#### 🚀 YoloV5
> The implementation of YoloV5 uses ML.NET, for GPU support follow the official instructions at https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/install-gpu-model-builder the implementation also works on CPU.

## 🚄 Speed
🔬 A **GPU** is favored for faster predictions, while a CPU can still do predictions.

🌐 If the input is through network, it is preferred that both computers be on the same lan network to avoid high latency; nevertheless, another machine on different network _(via the internet)_ may create latency and delays.

🦥 The most playable delay for both input and predictions at the moment is **10 predictions per second** 🔮 or **100ms delay** (both input and predictions), which is not a big issue with newer GPUs and decent settings.

⚡ Below is a list of graphics cards tested on resolutions and its prediction speed *(predictions/second)*.

> ⚠️ Different factors can cause the GPU to perform better or worse; please take this table with a grain of salt and don't place too much faith in it; **different hardware and environment can result in different results**, such as having different video settings in a game, different drivers, room temperature, or having background processes, for example.

<div align="center">
<table>
    <thead>
        <tr>
            <th>Predictor GPU</th>
            <th>Game GPU</th>
            <th>Game</th>
            <th>Setup</th>
            <th>960x540</th>
            <th>1280x720</th>
            <th>1920x1080</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><b><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/Windows_logo_-_2012_%28dark_blue%29.svg/2048px-Windows_logo_-_2012_%28dark_blue%29.svg.png" height="20px" alt="Windows 10"></img> GTX 1050</b></td>
            <td><b><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/Windows_logo_-_2012_%28dark_blue%29.svg/2048px-Windows_logo_-_2012_%28dark_blue%29.svg.png" height="20px" alt="Windows 10"></img> GTX 1050</b></td>
            <td>CSGO</td>
            <td>🖥️ Same Machine</td>
            <td><b>10 <i> p/sec </i></b></td>
            <td><b>11 <i> p/sec </i></b></td>
            <td><b>12 <i> p/sec </i></b></td>
        </tr>
        <tr>
            <td><b><img src="https://tinyurl.com/ys8hp77y" height="20px" alt="Windows 11"></img> MX 330</b></td>
            <td><b><img src="https://tinyurl.com/ys8hp77y" height="20px" alt="Windows 11"></img> MX 330</b></td>
            <td>CSGO</td>
            <td>🖥️ Same Machine</td>
            <td>?</td>
            <td>?</td>
            <td>5 <i> p/sec </i> </td>
        </tr>
    </tbody>
</table>
</div>

> 💡 As you can see, a mid-level gaming desktop GPU from 2016 may get decent results; but, newer GPUs can produce better and quicker predictions.

> 💡 Picture resizing can take some time; if the configurations demand additional image altering, this can slow down the prediction.



## 🛠 Custom Models 
🖼️ Astral uses **Yolo** for object detection, more information on creating models here: https://github.com/AlexeyAB/darknet#how-to-train-tiny-yolo-to-detect-your-custom-objects
> 💡 There are several versions and sizes of Yolo; currently, the C# implementation may function on any model trained, **for the quickest predictions, we choose the smallest Yolo model** size. The larger the model size, the more accurate the prediction, but slower; the Yolo implementation used can be seen at https://github.com/AlexeyAB/darknet.

