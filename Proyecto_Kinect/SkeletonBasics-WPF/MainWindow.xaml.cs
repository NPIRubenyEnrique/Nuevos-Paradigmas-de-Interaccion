//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Struct para almacenar un punto 3D necesario tras pisar accidentalmente el original
        public struct Vector3D
        {
            public double X, Y, Z;
        }

        // Valor que controlara el color de fondo activo
        private int fondo = 0;

        SolidColorBrush[] colores = new SolidColorBrush[7];

        private bool inicioReconocido = false;

        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            
                colores[0] = Brushes.Gray;
                colores[1] = Brushes.LightCoral;
                colores[2] = Brushes.LightSalmon;
                colores[3] = Brushes.MediumPurple;
                colores[4] = Brushes.Olive;
                colores[5] = Brushes.Orange;
                colores[6] = Brushes.Black;
            InitializeComponent();
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        /// <summary>
        /// Funcion para comprobar si se ha tocado la bola de inico (posicion inicial)
        /// </summary>
        private bool tocaBola(Point manoDer, Point bola)
        {
            bool toca = false;
            if (Math.Abs(manoDer.X-bola.X)<20.0f)
                if (Math.Abs(manoDer.Y-bola.Y)<20.0f)
                    toca = true;
            return toca;
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];
            Point puntoPosInicial, puntoNextBG, puntoPrevBG;
            puntoPosInicial = new Point(500.0, 200.0);
            puntoNextBG = new Point(480.0, 100.0);
            puntoPrevBG = new Point(450.0, 300.0);

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(colores[fondo], null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                

                

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {

                            this.DrawBonesAndJoints(skel, dc);
                            dc.DrawEllipse(Brushes.Beige, null, SkeletonPointToScreen(skel.Joints[JointType.Head].Position), 30.0, 35.0);
                            dc.DrawEllipse(Brushes.Red, null, puntoPosInicial, 25.0, 25.0);
                            dc.DrawEllipse(Brushes.Yellow, null, puntoNextBG, 25.0, 25.0);
                            dc.DrawEllipse(Brushes.Yellow, null, puntoPrevBG, 25.0, 25.0);

                            // Reconocer la posición del cuerpo:
                            /*Vector3D manoDerecha = new Vector3D();
                            manoDerecha.X = skel.Joints[JointType.HandRight].Position.X;
                            manoDerecha.Y = skel.Joints[JointType.HandRight].Position.Y;
                            manoDerecha.Z = skel.Joints[JointType.HandRight].Position.Z;*/
                            Point manoDerecha = new Point();
                            manoDerecha = SkeletonPointToScreen(skel.Joints[JointType.HandRight].Position);

                            Vector3D manoIzquierda = new Vector3D();
                            manoIzquierda.X = skel.Joints[JointType.HandLeft].Position.X;
                            manoIzquierda.Y = skel.Joints[JointType.HandLeft].Position.Y;
                            manoIzquierda.Z = skel.Joints[JointType.HandLeft].Position.Z;

                            Vector3D codoDerecho = new Vector3D();
                            codoDerecho.X = skel.Joints[JointType.ElbowRight].Position.X;
                            codoDerecho.Y = skel.Joints[JointType.ElbowRight].Position.Y;
                            codoDerecho.Z = skel.Joints[JointType.ElbowRight].Position.Z;

                            Vector3D codoIzquierdo = new Vector3D();
                            codoIzquierdo.X = skel.Joints[JointType.ElbowLeft].Position.X;
                            codoIzquierdo.Y = skel.Joints[JointType.ElbowLeft].Position.Y;
                            codoIzquierdo.Z = skel.Joints[JointType.ElbowLeft].Position.Z;

                            Vector3D rodillaDerecha = new Vector3D();
                            rodillaDerecha.X = skel.Joints[JointType.KneeRight].Position.X;
                            rodillaDerecha.Y = skel.Joints[JointType.KneeRight].Position.Y;
                            rodillaDerecha.Z = skel.Joints[JointType.KneeRight].Position.Z;

                            Vector3D rodillaIzquierda = new Vector3D();
                            rodillaIzquierda.X = skel.Joints[JointType.KneeLeft].Position.X;
                            rodillaIzquierda.Y = skel.Joints[JointType.KneeLeft].Position.Y;
                            rodillaIzquierda.Z = skel.Joints[JointType.KneeLeft].Position.Z;

                            Vector3D pieDerecho = new Vector3D();
                            pieDerecho.X = skel.Joints[JointType.FootRight].Position.X;
                            pieDerecho.Y = skel.Joints[JointType.FootRight].Position.Y;
                            pieDerecho.Z = skel.Joints[JointType.FootRight].Position.Z;

                            Vector3D pieIzquierdo = new Vector3D();
                            pieIzquierdo.X = skel.Joints[JointType.FootLeft].Position.X;
                            pieIzquierdo.Y = skel.Joints[JointType.FootLeft].Position.Y;
                            pieIzquierdo.Z = skel.Joints[JointType.FootLeft].Position.Z;

                            Vector3D hombroDerecho = new Vector3D();
                            hombroDerecho.X = skel.Joints[JointType.ShoulderRight].Position.X;
                            hombroDerecho.Y = skel.Joints[JointType.ShoulderRight].Position.Y;
                            hombroDerecho.Z = skel.Joints[JointType.ShoulderRight].Position.Z;

                            Vector3D hombroIzquierdo = new Vector3D();
                            hombroIzquierdo.X = skel.Joints[JointType.ShoulderLeft].Position.X;
                            hombroIzquierdo.Y = skel.Joints[JointType.ShoulderLeft].Position.Y;
                            hombroIzquierdo.Z = skel.Joints[JointType.ShoulderLeft].Position.Z;

                            if (tocaBola(manoDerecha, puntoPosInicial))
                            {
                                dc.DrawEllipse(Brushes.LightGreen, null, puntoPosInicial, 25.0, 25.0);
                                dc.DrawEllipse(Brushes.LightGreen, null, SkeletonPointToScreen(skel.Joints[JointType.Head].Position), 30.0, 35.0);
                                inicioReconocido = true;
                            }
                            if (inicioReconocido && tocaBola(manoDerecha, puntoNextBG))
                            {
                                fondo = (fondo + 1) % 7;
                                dc.DrawEllipse(Brushes.LightGreen, null, puntoNextBG, 25.0, 25.0);
                                dc.DrawRectangle(colores[fondo], null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                                inicioReconocido = false;
                            }
                            if (inicioReconocido && tocaBola(manoDerecha, puntoPrevBG))
                            {
                                fondo = (fondo - 1) % 7;
                                dc.DrawEllipse(Brushes.LightGreen, null, puntoPrevBG, 25.0, 25.0);
                                dc.DrawRectangle(colores[fondo], null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                                inicioReconocido = false;
                            }

                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);
 
            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;                    
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;                    
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

        /// <summary>
        /// Handles the checking or unchecking of the seated mode combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                }
                else
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                }
            }
        }
    }
}