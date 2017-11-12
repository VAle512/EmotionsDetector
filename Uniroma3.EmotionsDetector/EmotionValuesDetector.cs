namespace Uniroma3.EmotionsDetector
{
using Microsoft.Kinect.Toolkit.FaceTracking;
using System;

/// <summary>
/// Classe che fornisce metodi per la rilevazione dei valori degli Animation Unit Coefficients
/// </summary>
    public class EmotionValuesDetector
    {
        public static String detectJawLowerer(FaceTrackFrame frame)
        {
            return frame.GetAnimationUnitCoefficients()[1].ToString();
        }

        public static String detectLipStretch(FaceTrackFrame frame)
        {
            return frame.GetAnimationUnitCoefficients()[2].ToString();
        }

        public static String detectBrowLowerer(FaceTrackFrame frame)
        {
            return frame.GetAnimationUnitCoefficients()[3].ToString();
        }

        public static String detectLipCornerDepressor(FaceTrackFrame frame)
        {
            return frame.GetAnimationUnitCoefficients()[4].ToString();
        }

        public static String detectOuterBrowRaiser(FaceTrackFrame frame)
        {
            return frame.GetAnimationUnitCoefficients()[5].ToString();
        }
    }
}