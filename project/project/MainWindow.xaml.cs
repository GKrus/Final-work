using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double sharp;
        double side;
        double up;
        double back;
        double stif;
        double filling;
        double flags;
       

        public MainWindow()
        {
            InitializeComponent();

           // var viewPort3d = new HelixViewport3D();
            

            //viewPort3d.Children.Add(new Teapot());

            ModelVisual3D device3D = new ModelVisual3D();
            device3D.Content = Display3d(MODEL_PATH);
            // Add to view port
            viewPort3d.Children.Add(device3D);

            //var cube = new CubeVisual3D();
            //cube.Fill = Brushes.Red;
            //viewPort3d.Children.Add(cube);
        }

        //------------------------------------------------------------------------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //расчет коэфицоентов 
            double max = 0;
            if (nose.Value > chin.Value) max = nose.Value;
            else max = chin.Value;
            if (eyes.Value > max) max = eyes.Value;
            if (lips.Value > eyes.Value) max = eyes.Value;
            if (skin.Value > lips.Value) max = skin.Value;
            if (max - nose.Value < 5 && max - chin.Value < 5 && max - eyes.Value < 5 && max - lips.Value < 5 && max - skin.Value < 5)
            {
                sharp = (nose.Value + chin.Value + eyes.Value + lips.Value + skin.Value) / 5;
                //if small
            } 
            else
            {
                sharp = max - (nose.Value + chin.Value + eyes.Value + lips.Value + skin.Value) / 10;
                //sharp = (sl1 + sl2 + sl3 + sl4 + sl5) / 5;
            } // sharp -- ok
            up = (forehead.Value*2 + eyes.Value + cheelbones.Value + face_width.Value*2) / 6;//ok?
            back = ears.Value;//ok
            stif = (sharp*1.3 + skin.Value*2) / 3.3;//ok
            filling = (sharp + cheelbones.Value) / 2;//ok?
            flags = cheelbones.Value;//ok
            side = face_length.Value;//ok
            var lights = new DirectionalHeadLight();

            lights.Color = Colors.White;
            lights.Brightness = 1;
            lights.Position = new Point3D(5, -5, 5);
            //viewPort3d.Children.Add(lights);
            //side = Slider.NameProperty.Get
            //string i = Convert.ToString(sharp);
            //MessageBox.Show(i);

            //------------------------------------------------------------------------------------------------------


        }
        // имопрт модели 

        private const string MODEL_PATH = "Female Head.obj";
         //<summary>
         //Display 3D Model
         //</summary>
         //<param name="model">Path to the Model file</param>
         //<returns>3D Model Content</returns>
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(model);

                
                var axis_z = new Vector3D(1, 0, 0);
                var axis_y = new Vector3D(0, 1, 0);
                var angle_z = 120;
                var angle_y = 180;

                var matrix = device.Transform.Value;
                matrix.Rotate(new Quaternion(axis_y, angle_y));
                matrix.Rotate(new Quaternion(axis_z, angle_z));

                device.Transform = new MatrixTransform3D(matrix);


            }
            catch (Exception e)
            {
                 //Handle exception in case can not find the 3D model file
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }
       

        private void def_light_Click(object sender, RoutedEventArgs e)
        {
            var light = new ThreePointLights();

            //light.FillLightSideAngle = 180;
            //light.FillLightAngle = 0;
            light.KeyLightSideAngle = 30;//ok
            light.KeyLightAngle = 115;//ok
            light.KeyToFillLightRatio = 1 / 2;
            light.FillLightAngle = 115;
            light.FillLightSideAngle = 0;

            light.KeyToRimLightRatio = 1 / 1000;
            light.ShowLights = true;
            //light.Brightness = 1;
            //light.Position = new Point3D(-5, -5, 5);
            viewPort3d.Children.Add(light);
        }
    }

    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Create a model group
            var modelGroup = new Model3DGroup();

            // Create a mesh builder and add a box to it
            var meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddBox(new Point3D(0, 0, 1), 1, 2, 0.5);
            meshBuilder.AddBox(new Rect3D(0, 0, 1.2, 0.5, 1, 0.4));

            // Create a mesh from the builder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);

            // Create some materials
            var greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);
            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);
            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = greenMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(-2, 0, 0), Material = redMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(2, 0, 0), Material = blueMaterial, BackMaterial = insideMaterial });

            // Set the property, which will be bound to the Content property of the ModelVisual3D (see MainWindow.xaml)
            this.Model = modelGroup;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public Model3D Model { get; set; }
    }
}
