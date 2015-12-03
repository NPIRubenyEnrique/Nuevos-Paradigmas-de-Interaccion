//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// Modificado por:
// Ruben Peralta Diaz
// Enrique Garcia Gonzalez
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
        // Variables predeterminadas del ejemplo de skeleton basics
        // Para controlar los distintos parametros del programa: sensor, dimension de la pantalla...
        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

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

        // Variables auxiliares para la ejecucion
        // Posiciones para los recuadros de los botones
        public Point bot_siguiente;
        public Point bot_anterior;
        public Point bot_pausa1;
        public Point bot_pausa2;
        public Point bot_reiniciar;
        public Point bot_ayuda;

        // Variable para controlar la etapa en la que nos encontramos
        public int etapa = 0;
        public bool pausa = false;
        

        // Variables para contar los frames que estamos sobre un boton
        public int id_jug_bot = -1;
        public bool contando_sig = false;
        public bool contando_ant = false;
        public bool contando_pau1 = false;
        public bool contando_pau2 = false;
        public bool contando_rei = false;
        public bool contando_ayu = false;
        public int frame_ini, frame_act;

        // Varibles que almacenan 
        public Pala pala1 = new Pala(5, RenderHeight / 2, 80);
        public Pala pala2= new Pala(RenderWidth-(5+10), RenderHeight / 2, 80);
        public Pelota ball = new Pelota(new Point( RenderHeight/2, RenderWidth/2));
        public Jugador jugador1 = new Jugador();
        public Jugador jugador2 = new Jugador();
        public double velx, vely;

        // Varianle para identificaar con que esqueleto estmos trabjando
        int skeleton_id = 0;

        // Variables para el tamanio del campo de juego
        float bordeY1 = RenderHeight / 6, //80,
            bordeY2 = 5 * RenderHeight / 6, //400, 
            bordeX = RenderWidth; //640;

        //-----------------------------------------------------------------
        //
        // Clase creada para representar a los jugadores
        //
        //--------------------------------------------------------------------

        public class Jugador
        {
            //          Atributos         //
            //----------------------------//

            int puntuacion;

            //          Metodos           //
            //----------------------------//

            public Jugador()
            {

                puntuacion = 0;

            }


            // Metodo que aumenta la puntuacion de un jugdor
            public void Marcar()
            {
                puntuacion++;
            }

            // Metodo que devuelve la puntuacion de un jugador

            public int GetPuntuacion()
            {
                return puntuacion;
            }



        };


        //-----------------------------------------------------------------
        //
        // Clase creada para representar las palas de cada jugador 
        //
        //--------------------------------------------------------------------

        public class Pala
        {

            //          Atributos         //
            //----------------------------//

            private int tamanio;
            private double posicionx;
            private double posiciony;


            //          Metodos           //
            //----------------------------//

            // Constructor de la clase Pala
            public Pala(double posx, double posy, int tam)
            {
                posicionx = posx;
                posiciony = posy;
                tamanio = tam;
            }


            // Cambia la posicion en el eje y de la pala
            public void CambiarPos(double nueva_pos)
            {
                posiciony = nueva_pos;

                if ((posiciony) < (RenderHeight / 6)) posiciony = (RenderHeight / 6);
                if ((posiciony + tamanio) > ((5 * RenderHeight) / 6)) posiciony = ((5 * RenderHeight) / 6) - tamanio;

            }


            // Cambia el tamanio de la pala
            public void CambiarTamanio(int ntam)
            {
                tamanio = ntam;
            }

            // Devuelve la posicion del centro de la Pala
            public Point GetPos()
            {
                return new Point(posicionx, posiciony);
            }

            // Devuelve el tamanio de la pala
            public int GetTam()
            {
                return tamanio;
            }


            // Metodo que dibuja la pala
            public void Dibujar(DrawingContext dc)
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(posicionx, posiciony, 10, tamanio));
            }

        };


        //-----------------------------------------------------------------
        //
        // Clase creada para representar la pelota del juego
        //
        //--------------------------------------------------------------------

        public class Pelota
        {
            //          Atributos         //
            //----------------------------//

            private Point pos;  // Posicion actual de la bola
            private double vx;  // Velocidad en x
            private double vy;  // Velocidad en y
            private int tamanio;         // Tamanio de la Pelota

            //          Metodos           //
            //----------------------------//

            // Constructor de la clase pelota
            public Pelota(Point pos_i, int tam = 10)
            {
                tamanio = tam;
                pos = pos_i;
                vx = 5;
                vy = 5;
            }

            // Metodo que inicializa la velocidad de la pelota

            public void Empezar(double velx, double vely)
            {
                vx = velx;
                vy = vely;
            }


            // Metodo que atctualizaa la posicion dependiendo de la velocidad
            public void ActualizarPos()
            {
                pos.X += vx;
                pos.Y += vy;
            }


            // Metodo que atctualizaa la velocidad de la pelota
            public void ActualizarVel(double x, double y)
            {
                vx = x;
                vy = y;
            }


            // Metodo que dibuja la pelota
            public void Dibujar(DrawingContext dc)
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(pos.Y, pos.X, tamanio, tamanio));
            }

            // Metodo que cambia la velocidad de la pelota cuando choca con una pala
            public void ChoqueX()
            {
                vx = (-1)*vx;
            }

            // Metodo que cambia la velocidad de la pelota cuando choca con una pared
            public void ChoqueY()
            {
                vy = (-1)*vy;
            }

            // Metodos get

            public int GetTam()
            {
                return tamanio;
            }

            public Point GetPos()
            {
                return pos;
            }

            public Point GetVel()
            {
                return new Point(vx,vy);
            }
        };



        //-----------------------------------------------------------------
        //
        // Metodos que calcula si ha colisionado la Pelota con una pala
        //
        //------------------------------------------------------------------

        public void ColisionPelota(Pelota ball, Pala p1, Pala p2)
        {

            // Comprobamos que la pelota no se choca con la pala 1
            if ((ball.GetPos().Y <= (p1.GetPos().X+10)) &&
                ((ball.GetPos().X >= p1.GetPos().Y) && (ball.GetPos().X <= (p1.GetPos().Y + p1.GetTam()))))
            {
                ball.ChoqueY();
            }

            // Comprobamos que la pelota no se choca con la pala 2

            if ((ball.GetPos().Y + ball.GetTam() > (p2.GetPos().X)) &&
                 ((ball.GetPos().X > p2.GetPos().Y) && (ball.GetPos().X < (p2.GetPos().Y + p2.GetTam()))))
            {
                ball.ChoqueY();
            }

        }

        // Metodo que calcula si la pelota ha chocado con alguno de los bordes horizontales
        public void ChoquePared(Pelota ball)
        {
            if ((ball.GetPos().X) <= bordeY1)
                ball.ChoqueX();
            if ((ball.GetPos().X + ball.GetTam()) >= bordeY2)
                ball.ChoqueX();
        }

        // Metodo que calcula si se ha marcdo un punto
        public int PuntoMarcado(Pelota ball, Pala p1, Pala p2)
        {
            if (ball.GetPos().Y < 0)
                return 2; // Marca el jugador 2

            if ((ball.GetPos().Y + ball.GetTam()) > bordeX)
                return 1; // Marca el jugador 1

            return 0; // Nadie ha marcado
        }

         // Metodo que calcula si un boton esta siendo pulsado

        bool tocaBoton(Point pos_mano, Point pos, Point tam)
        {
            if((pos_mano.X>=pos.X) && (pos_mano.X <= (pos.X+tam.X))
                && (pos_mano.Y >= pos.Y) && pos_mano.Y <= (pos.Y + tam.Y))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            bot_siguiente = new Point(375, 295);
            bot_anterior = new Point(115, 295);
            bot_pausa1 = new Point(35, 45);
            bot_pausa2 = new Point(515, 45);
            bot_reiniciar = new Point(265, 45);
            bot_ayuda = new Point(85, 422);
            InitializeComponent();
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
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                    frame_act = skeletonFrame.FrameNumber; // Capturamos el número de frame en el que estamos
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        //RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {



                            /****************************** IMPLEMENTACION DEL JUEGO *********************/
                            Point manoDer = new Point();
                            manoDer = SkeletonPointToScreen(skel.Joints[JointType.HandRight].Position);
                            
                            if (etapa == 0)
                            {

                                menu.Visibility = System.Windows.Visibility.Visible;
                                Inicial1_1.Visibility = System.Windows.Visibility.Visible;
                                Inicial1_2.Visibility = System.Windows.Visibility.Visible;
                                Inicial2_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial2_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_3.Visibility = System.Windows.Visibility.Hidden;
                                anterior.Visibility = System.Windows.Visibility.Hidden;
                                siguiente.Visibility = System.Windows.Visibility.Visible;
                                ayuda.Visibility = System.Windows.Visibility.Hidden;
                                siguiente.Content = "SIGUIENTE";
                                puntuacionDos.Content = "0";
                                puntuacionUno.Content = "0";
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(375, 295, 150, 40));

                                // Boton Siguiente

                                if (tocaBoton(manoDer, bot_siguiente, new Point(150, 40))) // Toca el boton 'siguiente'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(375, 295, 150, 40));
                                    if (contando_sig == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_sig = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {

                                        if ((frame_ini + 45) <= (frame_act)) // Hemos presionado el boton 'siguiente'
                                        {
                                            etapa += 1;
                                            contando_sig = false;
                                            id_jug_bot = -1;
                                        }
                                    }
                                }


                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_sig) contando_sig = true;
                                    else
                                    {
                                        contando_sig = false;
                                        id_jug_bot = -1;
                                    }
                                }
                            }

                            if (etapa == 1)
                            {

                                Inicial1_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial1_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial2_1.Visibility = System.Windows.Visibility.Visible;
                                Inicial2_2.Visibility = System.Windows.Visibility.Visible;
                                Inicial3_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_3.Visibility = System.Windows.Visibility.Hidden;
                                anterior.Visibility = System.Windows.Visibility.Visible;
                                siguiente.Visibility = System.Windows.Visibility.Visible;
                                siguiente.Content = "SIGUIENTE";
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(115, 295, 150, 40));
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(375, 295, 150, 40));

                                // Boton Siguiente

                                if (tocaBoton(manoDer, bot_siguiente, new Point(150, 40))) // Toca el boton 'siguiente'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(375, 295, 150, 40));
                                    if (contando_sig == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_sig = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 45) <= (frame_act)) // Hemos presionado el boton 'siguiente'
                                        {
                                            id_jug_bot = -1;
                                            etapa += 1;
                                            contando_sig = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_sig) contando_sig = true;
                                    else
                                    {
                                        contando_sig = false;
                                        id_jug_bot = -1;
                                    }
                                }

                                // Boton Anterior

                                if (tocaBoton(manoDer, bot_anterior, new Point(150, 40))) // Toca el boton 'siguiente'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(115, 295, 150, 40));
                                    if (contando_ant == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_ant = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 45) <= (frame_act)) // Hemos presionado el boton 'anterior'
                                        {
                                            id_jug_bot = -1;
                                            etapa -= 1;
                                            contando_ant = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_ant) contando_sig = true;
                                    else
                                    {
                                        id_jug_bot = -1;
                                        contando_ant = false;
                                    }
                                }
                            }

                            if (etapa == 2)
                            {

                                Inicial1_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial1_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial2_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial2_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_1.Visibility = System.Windows.Visibility.Visible;
                                Inicial3_2.Visibility = System.Windows.Visibility.Visible;
                                Inicial3_3.Visibility = System.Windows.Visibility.Visible;
                                anterior.Visibility = System.Windows.Visibility.Visible;
                                siguiente.Visibility = System.Windows.Visibility.Visible;
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(115, 295, 150, 40));
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(375, 295, 150, 40));
                                siguiente.Content = "EMPEZAR";

                                // Boton Siguiente

                                if (tocaBoton(manoDer, bot_siguiente, new Point(150, 40))) // Toca el boton 'siguiente'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(375, 295, 150, 40));
                                    if (contando_sig == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_sig = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 45) <= (frame_act)) // Hemos presionado el boton 'siguiente'
                                        {
                                            id_jug_bot = -1;
                                            etapa += 1;
                                            contando_sig = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_sig) contando_sig = true;
                                    else
                                    {
                                        contando_sig = false;
                                        id_jug_bot = -1;
                                    }
                                }

                                // Boton Anterior

                                if (tocaBoton(manoDer, bot_anterior, new Point(150, 40))) // Toca el boton 'siguiente'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(115, 295, 150, 40));
                                    if (contando_ant == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_ant = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 45) <= (frame_act)) // Hemos presionado el boton 'anterior'
                                        {
                                            id_jug_bot = -1;
                                            etapa -= 1;
                                            contando_ant = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_ant) contando_sig = true;
                                    else
                                    {
                                        id_jug_bot = -1;
                                        contando_ant = false;
                                    }
                                }
                            }
                            if (etapa == 3)
                            {

                                menu.Visibility = System.Windows.Visibility.Hidden;
                                Inicial1_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial1_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial2_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial2_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_1.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_2.Visibility = System.Windows.Visibility.Hidden;
                                Inicial3_3.Visibility = System.Windows.Visibility.Hidden;
                                anterior.Visibility = System.Windows.Visibility.Hidden;
                                siguiente.Visibility = System.Windows.Visibility.Hidden;
                                puntuacionUno.Visibility = System.Windows.Visibility.Visible;
                                puntuacionDos.Visibility = System.Windows.Visibility.Visible;

                                pausa1.Visibility = System.Windows.Visibility.Visible;
                                pausa2.Visibility = System.Windows.Visibility.Visible;
                                reiniciar.Visibility = System.Windows.Visibility.Visible;
                                ayuda.Visibility = System.Windows.Visibility.Visible;


                                // Dibujar el fondo negro
                                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                                // Fondo blanco fuera de la zona de juego
                                dc.DrawRectangle(Brushes.White, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight / 6.0));
                                dc.DrawRectangle(Brushes.White, null, new Rect(0.0, RenderHeight - RenderHeight / 6.0, RenderWidth, RenderHeight / 6.0));

                                // Dibujamos un marco para diferenciar la pantalla
                                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, 2));
                                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, 2, RenderHeight));
                                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, RenderHeight - 2, RenderWidth, 2));
                                dc.DrawRectangle(Brushes.Black, null, new Rect(RenderWidth - 2, 0.0, 2, RenderHeight));

                                // Linea discontinua central
                                float ini = RenderHeight / 6, tam_ = 4;
                                while ((ini + tam_) <= RenderHeight - (RenderHeight / 6))
                                {
                                    dc.DrawRectangle(Brushes.White, null, new Rect((RenderWidth / 2) - (tam_ / 2), ini, tam_, tam_));
                                    ini += 2 * tam_;
                                }

                                // Pausa jugador 1
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(bot_pausa1.X, bot_pausa1.Y, 90, 30));
                                // Pausa jugador 2
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(bot_pausa2.X, bot_pausa2.Y, 90, 30));
                                // Reiniciar
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(bot_reiniciar.X, bot_reiniciar.Y, 110, 30));

                                // Ayuda
                                dc.DrawRectangle(Brushes.LightGreen, null, new Rect(bot_ayuda.X, bot_ayuda.Y, 40, 30));


                                // Identificacion del Skeleton
                                // El jugador que se encuentre en la mitad derecha controla esa pala y viceversa

                                if (SkeletonPointToScreen(skel.Joints[JointType.Head].Position).X < (RenderWidth / 2))
                                    skeleton_id = 1;
                                else
                                    skeleton_id = 2;

                                if (skeleton_id == 1) // Actualizamos la posicion del jugador 1
                                    pala1.CambiarPos(SkeletonPointToScreen(skel.Joints[JointType.HandRight].Position).Y);

                                if (skeleton_id == 2) // Actualizamos la posicion del jugador 2
                                    pala2.CambiarPos(SkeletonPointToScreen(skel.Joints[JointType.HandRight].Position).Y);

                                // Comprobamos si choca arriba o abajo
                                ChoquePared(ball);

                                // Comprobamos si choca con alguna de las palas de juego
                                ColisionPelota(ball, pala1, pala2);

                                // Comprobamos si algun jugador ha marcado
                                int marcado = PuntoMarcado(ball, pala1, pala2); 

                                if (marcado > 0)
                                {
                                    // Actualizamos la puntuacion
                                    if (marcado == 1)
                                    {
                                        jugador1.Marcar();
                                        puntuacionUno.Content = jugador1.GetPuntuacion().ToString();
                                    }

                                    if (marcado == 2)
                                    {
                                        jugador2.Marcar();
                                        puntuacionDos.Content = jugador2.GetPuntuacion().ToString();
                                    }

                                    ball = new Pelota(new Point(RenderHeight / 2, RenderWidth / 2), ball.GetTam());
                                }
                                
                                // Dibujamos las palas de juego
                                pala1.Dibujar(dc);
                                pala2.Dibujar(dc);


                                // Actualizamos la posicion de la pelota
                                ball.ActualizarPos();
                                ball.Dibujar(dc); // La dibujamos


                                // Funcionalidad de los botones
                                // Boton de pausa del jugador 1
                                if (tocaBoton(manoDer, bot_pausa1, new Point(90, 30))) // Toca el boton 'Pausar'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(bot_pausa1.X, bot_pausa1.Y, 90, 30));
                                    if (contando_pau1 == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_pau1 = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 40) <= (frame_act)) // Hemos presionado el boton 'Pausar'
                                        {
                                            id_jug_bot = -1;
                                            if (pausa==true)
                                            {
                                                ball.ActualizarVel(velx, vely);
                                                pausa = false;
                                            }
                                            else
                                            {
                                                ball.ActualizarVel(0.0, 0.0);
                                                velx = ball.GetVel().X;
                                                vely = ball.GetVel().Y;
                                                pausa = true;
                                            }
                                            contando_pau1 = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_pau1) contando_pau1 = true;
                                    else
                                    {
                                        id_jug_bot = -1;
                                        contando_pau1 = false;
                                    }
                                }

                                // Boton de pausa del jugador 2
                                if (tocaBoton(manoDer, bot_pausa2, new Point(90, 30))) // Toca el boton 'Pausar'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(bot_pausa2.X, bot_pausa2.Y, 90, 30));
                                    if (contando_pau2 == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_pau2 = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 40) <= (frame_act)) // Hemos presionado el boton 'Pausar'
                                        {
                                            id_jug_bot = -1;
                                            if (pausa==true)
                                            {
                                                ball.ActualizarVel(velx, velx);
                                                pausa = false;
                                            }
                                            else
                                            {
                                                ball.ActualizarVel(0.0, 0.0);
                                                velx = ball.GetVel().X;
                                                vely = ball.GetVel().Y;
                                                pausa = true;
                                            }
                                            contando_pau2 = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_pau2) contando_pau2 = true;
                                    else
                                    {
                                        id_jug_bot = -1;
                                        contando_pau2 = false;
                                    }
                                }

                                // Boton de reinicio
                                if (tocaBoton(manoDer, bot_reiniciar, new Point(110, 30))) // Toca el boton 'Reiniciar'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(bot_reiniciar.X, bot_reiniciar.Y, 110, 30));
                                    if (contando_rei == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_rei = true;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 60) <= (frame_act)) // Hemos presionado el boton 'Reiniciar'
                                        {
                                            id_jug_bot = -1;
                                            jugador1 = new Jugador();
                                            jugador2 = new Jugador();
                                            puntuacionUno.Content = jugador1.GetPuntuacion().ToString();
                                            puntuacionDos.Content = jugador2.GetPuntuacion().ToString();
                                            ball = new Pelota(new Point(RenderHeight/2, RenderWidth/2), ball.GetTam());
                                            contando_rei = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_rei) contando_rei = true;
                                    else
                                    {
                                        id_jug_bot = -1;
                                        contando_rei = false;
                                    }
                                }

                                // Boton de ayuda
                                if (tocaBoton(manoDer, bot_ayuda, new Point(40, 30))) // Toca el boton 'Ayuda'
                                {
                                    dc.DrawRectangle(Brushes.Green, null, new Rect(bot_ayuda.X, bot_ayuda.Y, 40, 30));
                                    if (contando_ayu == false)
                                    {
                                        id_jug_bot = skel.TrackingId;
                                        contando_ayu = true;
                                        volver.Visibility = System.Windows.Visibility.Visible;
                                        frame_ini = frame_act;
                                    }
                                    else
                                    {
                                        if ((frame_ini + 30) <= (frame_act)) // Hemos presionado el boton 'Ayuda'
                                        {
                                            id_jug_bot = -1;
                                            jugador1 = new Jugador();
                                            jugador2 = new Jugador();
                                            puntuacionUno.Content = jugador1.GetPuntuacion().ToString();
                                            puntuacionDos.Content = jugador2.GetPuntuacion().ToString();
                                            ball = new Pelota(new Point(RenderWidth / 2, RenderHeight / 2), ball.GetTam());
                                            volver.Visibility = System.Windows.Visibility.Hidden;
                                            etapa = 0;
                                            contando_ayu = false;
                                            
                                        }
                                    }
                                }
                                else
                                {
                                    if ((id_jug_bot != skel.TrackingId) && contando_ayu) contando_ayu = true;
                                    else
                                    {
                                        id_jug_bot = -1;
                                        contando_ayu = false;
                                        volver.Visibility = System.Windows.Visibility.Hidden;
                                    }
                                }


                            }

                            // Dibujamos la mano por encima de todo lo demas
                            dc.DrawEllipse(Brushes.Black, null, SkeletonPointToScreen(skel.Joints[JointType.HandRight].Position), 10, 10);

                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(Brushes.Black, null, SkeletonPointToScreen(skel.Joints[JointType.HandRight].Position), 10, 10);
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
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
