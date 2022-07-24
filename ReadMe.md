
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
> Full instructions for installing required dependencies at https://github.com/maalik0786/FastYolo although different CUDA versions **should** work, tested with CUDA 11.0 and it worked.
#### 🚀 YoloV5
> The implementation of YoloV5 uses ML.NET, for GPU support follow the official instructions at https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/install-gpu-model-builder the implementation also works on CPU.
## 🚄 Speed
🔬 A **GPU** is favored for faster predictions, while a CPU can still do predictions.

🌐 If the input is through network, it is preferred that both computers be on the same lan network to avoid high latency; nevertheless, another machine on different network _(via the internet)_ may create latency and delays.

🦥 The most playable delay for both input and predictions at the moment is **10 predictions per second** 🔮 or 100ms delay (both input and predictions), which is not a big issue with newer hardware and decent settings.

⚡ Below is a list of graphics cards and prediction speed *(predictions/second).*

| GPU      | 960x540 | 1280x720| 1920x1080|
|----------|---------|--------|--------|
| **GTX 1050** | 10*p/sec* | 7*p/sec* | 5*p/sec* |
| **MX 330**   |         |        | 2*p/sec* |