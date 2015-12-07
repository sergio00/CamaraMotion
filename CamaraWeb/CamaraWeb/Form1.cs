using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Librerias para la inicializacion del video
using AForge.Video.DirectShow;
using AForge.Video;
//Librerias para la deteccion del movimiento
using AForge.Vision.Motion;

namespace CamaraWeb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Variables para el manejo de inicializacion y finalizacion de video
        private FilterInfoCollection dispositivos;
        private VideoCaptureDevice fuenteDeVideo;
        //Variables para la deteccion de movimiento
        MotionDetector deteccion;
        float nivelDeDeteccion;

        private void Form1_Load(object sender, EventArgs e)
        {
            //Incializa las variables de deteccion
            deteccion = new MotionDetector(new TwoFramesDifferenceDetector(),new MotionBorderHighlighting());
            nivelDeDeteccion = 0;
            //Lista de todos los dispositivos de entrada de video
            dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //Cargar todo los dispositivos al comboBox
            foreach (FilterInfo x in dispositivos)
            {
                comboBox1.Items.Add(x.Name);  
            }
            comboBox1.SelectedIndex = 0;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //Se establece el disposotivo seleccionado como fuente de video
            fuenteDeVideo = new VideoCaptureDevice(dispositivos[comboBox1.SelectedIndex].MonikerString);
            // Se inicializa el control de video
            videoSourcePlayer1.VideoSource = fuenteDeVideo;
            //Inicia la recepcion de imagenes
            videoSourcePlayer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Para finalizar la recepcion de imagenes
            videoSourcePlayer1.SignalToStop();
        }

        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            nivelDeDeteccion = deteccion.ProcessFrame(image);
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //textBox1.Text = nivelDeDeteccion.ToString();
            textBox1.Text = String.Format("{0:00.0000}",nivelDeDeteccion);
        }
    }
}
