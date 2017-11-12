namespace Uniroma3.EmotionsDetector
{
    using System;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit.FaceTracking;
    using Point = System.Windows.Point;
    using System.Windows.Controls;

    public class SkeletonFaceTracker : IDisposable
    {
        private PointF[] facePoints;

        private FaceTracker faceTracker;

        private bool lastFaceTrackSucceeded;

        private SkeletonTrackingState skeletonTrackingState;

        private int rightFrameCount;

        private EmotionAnalizer analizer;

        private TextBox outputBox;

        private bool pause;
        public bool Pause
        {
            set
            {
                this.pause = value;
            }
        }

        public SkeletonFaceTracker(TextBox outputBox)
        {
            this.outputBox = outputBox;
            this.analizer = new EmotionAnalizer();
            this.rightFrameCount = 0;
            this.pause = true;
        }

        public int LastTrackedFrame { get; set; }

        public void Dispose()
        {
            if (this.faceTracker != null)
            {
                this.faceTracker.Dispose();
                this.faceTracker = null;
                this.pause = true;
            }
        }

        /// <summary>
        ///Disegna i 100 punti rilavati dalla Kinect sul viso
        /// </summary>
        public void DrawFaceModel(DrawingContext drawingContext)
        {
            if (!this.lastFaceTrackSucceeded || this.skeletonTrackingState != SkeletonTrackingState.Tracked)
            {
                return;
            }

            var faceModelGroup = new GeometryGroup();
            for (int i = 0; i < this.facePoints.Length; i++)
            {
                Point p = new Point(this.facePoints[i].X, this.facePoints[i].Y);
                EllipseGeometry eg = new EllipseGeometry(p, 1, 1);
                faceModelGroup.Children.Add(eg);
            }
            drawingContext.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.LightGreen, 1.0), faceModelGroup);
        }

        /// <summary>
        /// Updates the face tracking information for this skeleton
        /// </summary>
        internal void OnFrameReady(KinectSensor kinectSensor, ColorImageFormat colorImageFormat, byte[] colorImage, DepthImageFormat depthImageFormat, short[] depthImage, Skeleton skeletonOfInterest)
        {
            //elabora un frame si e due no
            if (this.rightFrameCount != 0)
            {
                if (this.rightFrameCount == 2)
                {
                    this.rightFrameCount = 0;
                }
                else //this.rightFrameCount == 1
                {
                    this.rightFrameCount++;
                }
                return;
            }
            this.rightFrameCount++;

            this.skeletonTrackingState = skeletonOfInterest.TrackingState;

            if (this.skeletonTrackingState != SkeletonTrackingState.Tracked)
            {
                // nothing to do with an untracked skeleton.
                return;
            }

            if (this.faceTracker == null)
            {
                try
                {
                    this.faceTracker = new FaceTracker(kinectSensor);
                }
                catch (InvalidOperationException)
                {
                    // During some shutdown scenarios the FaceTracker
                    // is unable to be instantiated.  Catch that exception
                    // and don't track a face.
                    System.Diagnostics.Debug.WriteLine("AllFramesReady - creating a new FaceTracker threw an InvalidOperationException");
                    this.faceTracker = null;
                }
            }

            if (this.faceTracker != null)
            {
                FaceTrackFrame frame = this.faceTracker.Track(
                    colorImageFormat, colorImage, depthImageFormat, depthImage, skeletonOfInterest);

                this.lastFaceTrackSucceeded = frame.TrackSuccessful;
                if (this.lastFaceTrackSucceeded)
                {
                    this.facePoints = frame.GetShapePoints();
                }

                if (this.pause)
                {
                    return;
                }

                if (this.analizer.IsSetupComplete)
                {
                    outputBox.AppendText(this.analizer.analizeEmotion(frame));
                    outputBox.AppendText("\r\n");
                    outputBox.ScrollToEnd();
                }
                else
                {
                    outputBox.AppendText(this.analizer.setup(frame));
                    outputBox.ScrollToEnd();

                }
            }
        }
    }
}