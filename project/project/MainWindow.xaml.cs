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
        double sl1;
        double sl2;
        double sl3;
        double sl4;
        double sl5;
        double sl6;
        double sl7;
        double sl8;
        double sl9;
        double sl10;

        public MainWindow()
        {
            InitializeComponent();
            Create3DViewPort();
        }



        //private void Create3DViewPort() { var hVp3D = new HelixViewport3D(); }
        private void Create3DViewPort()
        {
            var hVp3D = new HelixViewport3D();

            var lights = new DefaultLights();
            var teaPot = new Teapot();
            

            hVp3D.Children.Add(lights);
            hVp3D.Children.Add(teaPot);
        }

        //------------------------------------------------------------------------------------------------------

        // example from zip
        //private ModelVisual3D CreateDice()
        //{
        //    var diceMesh = new MeshBuilder();
        //    diceMesh.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
        //    for (int i = 0; i < 2; i++)
        //        for (int j = 0; j < 2; j++)
        //            for (int k = 0; k < 2; k++)
        //            {
        //                var points = new List<Point3D>();
        //                diceMesh.ChamferCorner(new Point3D(i - 0.5, j - 0.5, k - 0.5), 0.1, 1e-6, points);
        //                //foreach (var p in points)
        //                //    b.ChamferCorner(p, 0.03);
        //            }

        //    return new ModelVisual3D { Content = new GeometryModel3D { Geometry = diceMesh.ToMesh(), Material = Materials.White } };
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //расчет коэфицоентов 
            double max = 0;
            if (sl1 > sl2) max = sl1;
            else max = sl2;
            if (sl3 > max) max = sl3;
            if (sl4 > sl3) max = sl4;
            if (sl5 > sl4) max = sl5;
            if (max - sl1 < 5 && max - sl2 < 5 && max - sl3 < 5 && max - sl4 < 5 && max - sl5 < 5)
            {
                sharp = (sl1 + sl2 + sl3 + sl4 + sl5) / 5;
                //if small
            } 
            else
            {
                sharp = max - (sl1 + sl2 + sl3 + sl4 + sl5) / 10;
                //sharp = (sl1 + sl2 + sl3 + sl4 + sl5) / 5;
            } // sharp -- ok
            up = (sl6*2 + sl3 + sl7 + sl9*2) / 6;//ok?
            back = sl8;//ok
            stif = (sharp*1.3 + sl5*2) / 3.3;//ok
            filling = (sharp + sl7) / 2;//ok?
            flags = sl7;//ok
            side = sl10;//ok
            //side = Slider.NameProperty.Get
            string i = Convert.ToString(up);
            MessageBox.Show(i);

            //------------------------------------------------------------------------------------------------------


        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl1 = ((Slider)sender).Value;
            
           
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl2 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl3 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_3(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl4 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_4(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl5 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_5(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl6 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_6(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl7 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_7(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl8 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_8(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl9 = ((Slider)sender).Value;
        }

        private void Slider_ValueChanged_9(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sl10 = ((Slider)sender).Value;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    //создание предметов черезе доп класс
    //http://qiita.com/ousttrue/items/a7ffefeaa4b642a054bd

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
