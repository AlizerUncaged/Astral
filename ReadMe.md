<div>
  <img width="220" height="210" align="left" src="https://i.ibb.co/Yjk6WKb/Logo.png" alt="Astral"/>
  <br>
  <h1>Astral</h1>
  <p>Astral is a machine learning AI for game automation. Astral recognizes and observes game objects in the same way as any other human would, using the Yolo algorithm, powerful GPUs, a trained model, and a screen as input.</p>
</div>
<br/>
<br/>

## 🏗️ Dependencies
### 🕵️ Installing Detector Dependencies
#### ⚡ FastYolo
> Full instructions for installing required dependencies at https://github.com/maalik0786/FastYolo although different CUDA versions **should** work.
#### 🚀 YoloV5
> The implementation of YoloV5 uses ML.NET, for GPU support follow the official instructions at https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/install-gpu-model-builder the implementation also works on CPU.
## 🚄 Speed
🔬 When it comes to detecting objects, the GPU is around ten times faster than the CPU; 10 predictions per second (100ms delay) is already fast, and a mid-range GPU can get you to 20 predictions per second (50ms delay) on 1280x720 resolution.

🌐 If the input is through network, it is preferred that both computers be on the same lan network to avoid high latency; nevertheless, another machine on different network may create latency and delays.

🦥 The most playable delay for both input and predictions at the moment is 10 predictions per second 🔮 or 100ms delay (both input and predictions), which is not a big issue with newer hardware and decent settings.