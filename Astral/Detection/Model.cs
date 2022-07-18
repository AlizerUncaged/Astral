using Astral.Monitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow;
using Tensorflow.NumPy;

using static SharpCV.Binding;

namespace Astral.Detection
{
    public class Model : IService
    {
        private readonly ScreenGrab screenGrab;
        private readonly SharpCV.Net net;
        public Model(Monitor.ScreenGrab screenGrab)
        {
            this.screenGrab = screenGrab;

            screenGrab.Screenshot += ScreenshotReceived;

            /// An System.Runtime.InteropServices.SEHException could mean:
            ///     the model doesn't exist on the path.
            ///     the model is faulty.
            net = cv2.dnn.readNetFromTensorflow(@"./frozen_inference_graph.pb",
                          @"./mask_rcnn_inception_v2_coco_2018_01_28.pbtxt");
        }

        private void ScreenshotReceived(object? sender, SharpCV.Mat e)
        {
            var perfCounter = Stopwatch.StartNew();

            long height = e.shape[0], width = e.shape[1];

            var blob = cv2.dnn.blobFromImage(e,
                1.0, ((int)width, (int)height), swapRB: true, crop: true);

            net.setInput(blob);

            // Super slow, better model == faster.
            NDArray detectionResult = net.forward();

            int found = 0;

            foreach (var detection in detectionResult[0, 0, Slice.All, Slice.All])
            {
                float score = detection[2];
                if (score > 0.4)
                    found++;
            }

            Console.WriteLine($"Found : {found}, Took : {perfCounter.ElapsedMilliseconds}ms");
        }
    }
}
