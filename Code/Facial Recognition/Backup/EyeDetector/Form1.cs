using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Collections;

namespace EyeDetector
{
    public partial class Form1 : Form
    {
        private Capture _capture;
        private HaarCascade _faces;
        private HaarCascade _eyes;
        int xMax;
        int yMax;


        public Form1()
        {
            InitializeComponent();
            _capture = new Capture();
            _faces = new HaarCascade("haarcascade_frontalface_alt_tree.xml");
            _eyes = new HaarCascade("haarcascade_eye.xml");

            Application.Idle += new EventHandler(FrameGrabber);
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            //We are acquiring a new frame
            Image<Bgr, Byte> frame = _capture.QueryFrame();
            //We convert it to grayscale
            Image<Gray, Byte> grayFrame = frame.Convert<Gray, Byte>();
            //Equalization step
            grayFrame._EqualizeHist();

            // We assume there's only one face in the video
            MCvAvgComp[][] facesDetected = grayFrame.DetectHaarCascade(_faces, 1.1, 1, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT, new Size(20, 20));

            if (facesDetected[0].Length == 1)
            {
                MCvAvgComp face = facesDetected[0][0];
                //Set the region of interest on the faces                

                #region Luca Del Tongo Search Roi based on Face Metric Estimation --- based on empirical measuraments on a couple of photos ---  a really trivial heuristic

                // Our Region of interest where find eyes will start with a sample estimation using face metric
                Int32 yCoordStartSearchEyes = face.rect.Top + (face.rect.Height * 3 / 11);
                Point startingPointSearchEyes = new Point(face.rect.X, yCoordStartSearchEyes);
                Point endingPointSearchEyes = new Point((face.rect.X + face.rect.Width), yCoordStartSearchEyes);

                Size searchEyesAreaSize = new Size(face.rect.Width, (face.rect.Height * 2 / 9));
                Point lowerEyesPointOptimized = new Point(face.rect.X, yCoordStartSearchEyes + searchEyesAreaSize.Height);
                Size eyeAreaSize = new Size(face.rect.Width / 2, (face.rect.Height * 2 / 9));
                Point startingLeftEyePointOptimized = new Point(face.rect.X + face.rect.Width / 2, yCoordStartSearchEyes);

                Rectangle possibleROI_eyes = new Rectangle(startingPointSearchEyes, searchEyesAreaSize);
                Rectangle possibleROI_rightEye = new Rectangle(startingPointSearchEyes, eyeAreaSize);
                Rectangle possibleROI_leftEye = new Rectangle(startingLeftEyePointOptimized, eyeAreaSize);

                #endregion

                #region Drawing Utilities
                // Let's draw our search area, first the upper line
                frame.Draw(new LineSegment2D(startingPointSearchEyes, endingPointSearchEyes), new Bgr(Color.White), 3);
                // draw the bottom line
                frame.Draw(new LineSegment2D(lowerEyesPointOptimized, new Point((lowerEyesPointOptimized.X + face.rect.Width), (yCoordStartSearchEyes + searchEyesAreaSize.Height))), new Bgr(Color.White), 3);
                // draw the eyes search vertical line
                frame.Draw(new LineSegment2D(startingLeftEyePointOptimized, new Point(startingLeftEyePointOptimized.X, (yCoordStartSearchEyes + searchEyesAreaSize.Height))), new Bgr(Color.White), 3);

                MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
                frame.Draw("Search Eyes Area", ref font, new Point((startingLeftEyePointOptimized.X - 80), (yCoordStartSearchEyes + searchEyesAreaSize.Height + 15)), new Bgr(Color.Yellow));
                frame.Draw("Right Eye Area", ref font, new Point(startingPointSearchEyes.X, startingPointSearchEyes.Y - 10), new Bgr(Color.Yellow));
                frame.Draw("Left Eye Area", ref font, new Point(startingLeftEyePointOptimized.X + searchEyesAreaSize.Height / 2, startingPointSearchEyes.Y - 10), new Bgr(Color.Yellow));
                #endregion

                grayFrame.ROI = possibleROI_leftEye;
                MCvAvgComp[][] leftEyesDetected = grayFrame.DetectHaarCascade(_eyes, 1.15, 0, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
                grayFrame.ROI = Rectangle.Empty;

                grayFrame.ROI = possibleROI_rightEye;
                MCvAvgComp[][] rightEyesDetected = grayFrame.DetectHaarCascade(_eyes, 1.15, 0, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
                grayFrame.ROI = Rectangle.Empty;

                //If we are able to find eyes inside the possible face, it should be a face, maybe we find also a couple of eyes
                if (leftEyesDetected[0].Length != 0 && rightEyesDetected[0].Length != 0)
                {
                    //draw the face
                    frame.Draw(face.rect, new Bgr(Color.Violet), 2);
                    

                    #region Hough Circles Eye Detection                                       
                    
                    grayFrame.ROI = possibleROI_leftEye;
                    CircleF[] leftEyecircles = grayFrame.HoughCircles(new Gray(180), new Gray(70), 5.0, 10.0, 1, 200)[0];
                    grayFrame.ROI = Rectangle.Empty;
                    foreach (CircleF circle in leftEyecircles)
                    {
                        float x = circle.Center.X + startingLeftEyePointOptimized.X;
                        float y = circle.Center.Y + startingLeftEyePointOptimized.Y;
                        frame.Draw(new CircleF(new PointF(x, y), circle.Radius), new Bgr(Color.RoyalBlue), 4);
                    }

                    grayFrame.ROI = possibleROI_rightEye;
                    CircleF[] rightEyecircles = grayFrame.HoughCircles(new Gray(180), new Gray(70), 5.0, 10.0, 1, 200)[0];
                    grayFrame.ROI = Rectangle.Empty;

                    foreach (CircleF circle in rightEyecircles)
                    {
                        float x = circle.Center.X + startingPointSearchEyes.X;
                        float y = circle.Center.Y + startingPointSearchEyes.Y;
                        frame.Draw(new CircleF(new PointF(x, y), circle.Radius), new Bgr(Color.RoyalBlue), 4);
                    }

                    #endregion

                    //Uncomment this to draw all rectangle eyes
                    //foreach (MCvAvgComp eyeLeft in leftEyesDetected[0])
                    //{
                    //    Rectangle eyeRect = eyeLeft.rect;
                    //    eyeRect.Offset(startingLeftEyePointOptimized.X, startingLeftEyePointOptimized.Y);
                    //    frame.Draw(eyeRect, new Bgr(Color.Red), 2);
                    //}
                    //foreach (MCvAvgComp eyeRight in rightEyesDetected[0])
                    //{
                    //    Rectangle eyeRect = eyeRight.rect;
                    //    eyeRect.Offset(startingPointSearchEyes.X, startingPointSearchEyes.Y);
                    //    frame.Draw(eyeRect, new Bgr(Color.Red), 2);
                    //}   
                 
                }
                imageBoxCapturedFrame.Image = frame;
        
            }
        }


    }
}
