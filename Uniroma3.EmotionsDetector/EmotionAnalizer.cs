namespace Uniroma3.EmotionsDetector
{
    using Microsoft.Kinect.Toolkit.FaceTracking;
    using System;
    using System.Collections.Generic;

    public class EmotionAnalizer
    {
        private static readonly int SETUP_VALUES = 50;

        private static readonly float SMILE_GAP = 0.5F;
        private static readonly float BROW_UP_GAP = 0.2F;
        private static readonly double BROW_LOW_GAP = 150;

        private List<float> smileSetupArray;
        private List<float> browUpSetupArray;
        private List<double> browLowRSetupArray;
        private List<double> browLowLSetupArray;

        private float smileBasicValue;
        private float browUpBasicValue;
        private double browLowRBasicValue;
        private double browLowLBasicValue;

        private bool isSetupComplete;
        public bool IsSetupComplete
        {
            get
            {
                return this.isSetupComplete;
            }
        }

        public EmotionAnalizer()
        {
            this.isSetupComplete = false;
            this.smileSetupArray = new List<float>();
            this.browUpSetupArray = new List<float>();
            this.browLowRSetupArray = new List<double>();
            this.browLowLSetupArray = new List<double>();
        }

        internal String setup(FaceTrackFrame frame)
        {
            if (this.smileSetupArray.Count < SETUP_VALUES)
            {
                this.smileSetupArray.Add(frame.GetAnimationUnitCoefficients()[2]);
                this.browUpSetupArray.Add(frame.GetAnimationUnitCoefficients()[5]);
                this.browLowRSetupArray.Add(this.pointDistance(frame.GetShapePoints()[89],frame.GetShapePoints()[25]));
                this.browLowLSetupArray.Add(this.pointDistance(frame.GetShapePoints()[89], frame.GetShapePoints()[35]));
                return (".");
            }
            else
            {
                this.evaluateInitalValues();
                this.isSetupComplete = true;
                return ("\r\nSetup Complete!\r\n");
            }
        }

        private void evaluateInitalValues()
        {
            this.smileSetupArray.Sort();
            this.smileBasicValue = this.smileSetupArray[this.smileSetupArray.Count/2];
            this.browUpSetupArray.Sort();
            this.browUpBasicValue = this.browUpSetupArray[this.browUpSetupArray.Count/2];
            this.browLowRSetupArray.Sort();
            this.browLowRBasicValue = this.browLowRSetupArray[this.browLowLSetupArray.Count/2];
            this.browLowLSetupArray.Sort();
            this.browLowLBasicValue = this.browLowLSetupArray[this.browLowRSetupArray.Count/2];
        }
        
        private double pointDistance(PointF pf1 , PointF pf2)
        {
            return Math.Pow((pf1.X - pf2.X), 2) + Math.Pow((pf1.Y - pf2.Y), 2); 
        }

        /// <summary>
        /// Prova i vari tipi di emozioni alla ricerca di quello esatto
        /// </summary>
        internal String analizeEmotion(FaceTrackFrame frame)
        {
            if (frame.GetAnimationUnitCoefficients()[5] > this.browUpBasicValue + BROW_UP_GAP)
            {
                return ("Surprise!");
            }
            else if (frame.GetAnimationUnitCoefficients()[2] > this.smileBasicValue + SMILE_GAP)
            {
                return ("Joy!");
            }
            else
            {
                double distDx = this.pointDistance(frame.GetShapePoints()[89], frame.GetShapePoints()[25]);
                double distSx = this.pointDistance(frame.GetShapePoints()[89], frame.GetShapePoints()[35]);
                if (distDx + BROW_LOW_GAP < browLowRBasicValue || distSx + BROW_LOW_GAP < browLowLBasicValue)
                {
                    return ("Disgust!");
                }
                else
                {
                    return ("No action");
                }
            }
        }
    }
}