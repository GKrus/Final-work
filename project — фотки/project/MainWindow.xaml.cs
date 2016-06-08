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
using Microsoft.Win32;

namespace project
{
    public partial class MainWindow : Window
    {
        double sharp;
        double side;
        double up;
        double back;
        double stif;
        double filling;
        double flags;
        ThreePointLights light;
        SunLight sun_light;
        int i;
        Line line;
        Point point;
        Point point0;
       
        public MainWindow()
        {
            InitializeComponent();
            i = 0;
            light = new ThreePointLights();
            nose_com.IsReadOnly = true;
            //var studio_light = new DefaultLights();
            //studio_light.Brightness = 0.3;
            //studio_light.Position = new Point3D(-5, -5, 5);
            
            //viewPort3d.Children.Add(new Teapot());
            sun_light = new SunLight();
            viewPort3d.Children.Add(sun_light);

            ModelVisual3D device3D = new ModelVisual3D();
            device3D.Content = Display3d(MODEL_PATH);
            // Add to view port
            viewPort3d.Children.Add(device3D);
            viewPort3d.Visibility = Visibility.Hidden;

            //var cube = new CubeVisual3D();
            //cube.Fill = Brushes.Red;
            //viewPort3d.Children.Add(cube);
        }

//---------------------------------------------------------------------------------------------------------------------

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
            up = (forehead.Value*2 + eyes.Value + cheelbones.Value + face_width.Value*2 + sharp*2) / 8;//ok?
            back = ears.Value;//ok
            stif = (sharp*1.3 + skin.Value*2) / 3.3;//ok
            filling = (sharp + cheelbones.Value) / 2;//ok?
            flags = cheelbones.Value;//ok
            side = face_length.Value;//ok

            light.KeyLightBrightness = 0.8;
            light.KeyLightSideAngle = 30;//ok
            light.KeyLightAngle = 115;//ok
            light.KeyToFillLightRatio = 1 / 1;
            light.FillLightAngle = 310;
            light.FillLightSideAngle = 310;
            light.RimLightAngle = 30;
            light.KeyToRimLightRatio = 1 / 1.5;
            light.ShowLights = true;

            if (back > 5) light.KeyToFillLightRatio = 1 / 100;
            else light.KeyToFillLightRatio = 1 / 0.9;
           
            if (filling > 5)
            {
                light.KeyLightBrightness = 0.7;
                light.KeyToRimLightRatio = 1 / 1;
            }
            if (side > 5)
            {
                light.KeyLightSideAngle = light.KeyLightSideAngle - side * 2;
            }
            else
            {
                light.KeyLightSideAngle = light.KeyLightSideAngle + 10 - side * 2;
            }
            if (up > 5)
            {
                light.KeyLightAngle -= up * 2;
            }
            else
            {
                light.KeyLightAngle += 10 - up * 2;
            }
            //MessageBox.Show("Ok");
        }
//---------------------------------------------------------------------------------------------------------------------
        
        private const string MODEL_PATH = "Female Head.obj";

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
            if (i <= 0)
            {            
                viewPort3d.Children.Remove(sun_light);
                light.KeyLightBrightness = 0.8;
                light.KeyLightSideAngle = 30;//ok
                light.KeyLightAngle = 115;//ok
                light.KeyToFillLightRatio = 1 / 1;
                light.FillLightAngle = 310;
                light.FillLightSideAngle = 310;
                light.RimLightAngle = 30;
                light.KeyToRimLightRatio = 1 / 1.7;
                light.ShowLights = true;
                viewPort3d.Children.Add(light);
                i++;
            }
            else 
            {
                light.KeyLightBrightness = 0.8;
                light.KeyLightSideAngle = 30;//ok
                light.KeyLightAngle = 115;//ok
                light.KeyToFillLightRatio = 1 / 1;
                light.FillLightAngle = 310;
                light.FillLightSideAngle = 310;
                light.RimLightAngle = 30;
                light.KeyToRimLightRatio = 1 / 1.5;
                light.ShowLights = true;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nose.Value = 10;
            chin.Value = 10;
            eyes.Value = 10;
            lips.Value = 10;
            skin.Value = 10;
            forehead.Value = 10;
            cheelbones.Value = 10;
            ears.Value = 10;
            face_length.Value = 10;
            face_width.Value = 10;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            nose_tick.Visibility = Visibility.Visible;
            nose_com.Visibility = Visibility.Visible;
            nose_com.IsEnabled = true;

            if (nose.Value < 3)
                nose_com.Text = "С этим носом будет минимум проблем";
            else
            {
                nose_com.Text = "Обратите внимание на тени от носа, они могут Вас не обрадовать";
                if (nose.Value > 6) nose_com.Text = "Тени от носа Вас не обрадуют, вероятно";
            }
            //MessageBox.Show("Ok");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            nose_tick.Visibility = Visibility.Visible;
            nose_com.Visibility = Visibility.Visible;
            nose_com.IsEnabled = true;

            sharp_tick.Visibility = Visibility.Visible;
            sharp_com.Visibility = Visibility.Visible;
            sharp_com.IsEnabled = true;
            up_tick.Visibility = Visibility.Visible;
            up_com.Visibility = Visibility.Visible;
            up_com.IsEnabled = true;
            back_tick.Visibility = Visibility.Visible;
            back_com.Visibility = Visibility.Visible;
            back_com.IsEnabled = true;
            stif_tick.Visibility = Visibility.Visible;
            stif_com.Visibility = Visibility.Visible;
            stif_com.IsEnabled = true;
            flags_tick.Visibility = Visibility.Visible;
            flags_com.Visibility = Visibility.Visible;
            flags_com.IsEnabled = true;

            if (nose.Value < 3)
                nose_com.Text = "С этим носом будет минимум проблем";
            else
            {
                nose_com.Text = "Обратите внимание на тени от носа, они могут Вас не обрадовать";
                if (nose.Value > 6) nose_com.Text = "Тени от носа Вас не обрадуют, вероятно";
            }

            if (sharp < 3)
                sharp_com.Text = "Вы можете использоваит любые углы падения света";
            else
            {
                sharp_com.Text = "Не рекомендуется использовать резкие углы постановки света";
                if (sharp > 6) sharp_com.Text = "Резкие углы постановки света неуместны";
            }

            if (up < 3)
                up_com.Text = "Использование верхнего света не доставит неудобств";
            else
            {
                up_com.Text = "Не рекомендуется использовать верхний свет";
                if (up > 6) up_com.Text = "Верхний свет может значительно испортить вашу модель";
            }

            if (back < 3)
                back_com.Text = "Конровой свет не испортит Вашу модель";
            else
            {
                back_com.Text = "Следует внимательно отнестись к работе контрового света";
                if (back > 6) back_com.Text = "Контровой свет приченит неудобства";
            }

            if (stif < 3)
                stif_com.Text = "Вы вольны использовать жесткий свет";
            else
            {
                stif_com.Text = "Отнеситесь к жесткости света внимательно";
                if (stif > 6) stif_com.Text = "Жесткий свет окажет негативное влияние";
            }

            if (flags < 3)
                flags_com.Text = "Черные флаги помогут в отрисовке скул";
            else
            {
                flags_com.Text = "Флаги следует устанавливать по вкусу";
                if (flags > 6) flags_com.Text = "Белые флаги будут уместны";
            }

            if (flags < 3)
                flags_com.Text = "Черные флаги помогут в отрисовке скул";
            else
            {
                flags_com.Text = "Флаги следует устанавливать по вкусу";
                if (flags > 6) flags_com.Text = "Белые флаги будут уместны";
            }

            if (nose.Value > 5) 
            {
                side_tick.Visibility = Visibility.Visible;
                side_com.Visibility = Visibility.Visible;
                side_com.IsEnabled = true;
                side_com.Text = "Обратите внимание на влияние бокового света (вытягивание лица и пр.)";
            }
            else
            {
                side_tick.Visibility = Visibility.Hidden;
                side_com.Visibility = Visibility.Hidden;
                side_com.IsEnabled = false;
            }
            
            //MessageBox.Show("Ok");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            nose.Value = 0;
            chin.Value = 0;
            eyes.Value = 0;
            lips.Value = 0;
            skin.Value = 0;
            forehead.Value = 0;
            cheelbones.Value = 0;
            ears.Value = 0;
            face_length.Value = 0;
            face_width.Value = 0;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "JPEG Photos (*.jpg)|*.jpg|PNG Photos (*.png)|*.png | All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();         
   
            BitmapImage bm1 = new BitmapImage();
            bm1.BeginInit();

            if (openFileDialog.FileName != "")
            {
                bm1.UriSource = new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute);
                bm1.CacheOption = BitmapCacheOption.OnLoad;
                bm1.EndInit();
                photo_sd.Source = bm1;
                border_sd.Visibility = Visibility.Visible;
                draw_sd.Children.Remove(line);

            }
            
           

        }

        private void upload_fr_Click(object sender, RoutedEventArgs e)
        {


            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "JPEG Photos (*.jpg)|*.jpg|PNG Photos (*.png)|*.png | All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            BitmapImage bm1 = new BitmapImage();
            bm1.BeginInit();

            if (openFileDialog.FileName != "")
            {
                bm1.UriSource = new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute);
                bm1.CacheOption = BitmapCacheOption.OnLoad;
                bm1.EndInit();
                photo_fr.Source = bm1;
                border.Visibility = Visibility.Visible;
                draw_fr.Children.Remove(line);
                //border_sel.Visibility = Visibility.Visible;
               
            }
            // MessageBox.Show("Файл не выбран!");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (viewPort3d.Visibility != Visibility.Visible)
            {
                viewPort3d.Visibility = Visibility.Visible;
                draw_fr.Visibility = Visibility.Hidden;
                draw_sd.Visibility = Visibility.Hidden;
                photo_fr.Visibility = Visibility.Hidden;
                photo_sd.Visibility = Visibility.Hidden;
                upload_fr.Visibility = Visibility.Hidden;
                upload_sd.Visibility = Visibility.Hidden;
                nose_sel.Visibility = Visibility.Hidden;
                eyes_sel.Visibility = Visibility.Hidden;
                chin_sel.Visibility = Visibility.Hidden;
                cheelbones_sel.Visibility = Visibility.Hidden;
                forehead_sel.Visibility = Visibility.Hidden;
                ears_sel.Visibility = Visibility.Hidden;
                lips_sel.Visibility = Visibility.Hidden;
                face_width_sel.Visibility = Visibility.Hidden;
                face_length_sel.Visibility = Visibility.Hidden;
                bo1.Visibility = Visibility.Hidden;
                bo2.Visibility = Visibility.Hidden;
                change.Content = "Перейти к фото";
            }
            else
            {
                viewPort3d.Visibility = Visibility.Hidden;
                photo_fr.Visibility = Visibility.Visible;
                photo_sd.Visibility = Visibility.Visible;
                draw_fr.Visibility = Visibility.Visible;
                draw_sd.Visibility = Visibility.Visible;
                upload_fr.Visibility = Visibility.Visible;
                upload_sd.Visibility = Visibility.Visible;
                nose_sel.Visibility = Visibility.Visible;
                eyes_sel.Visibility = Visibility.Visible;
                chin_sel.Visibility = Visibility.Visible;
                cheelbones_sel.Visibility = Visibility.Visible;
                forehead_sel.Visibility = Visibility.Visible;
                ears_sel.Visibility = Visibility.Visible;
                lips_sel.Visibility = Visibility.Visible;
                face_width_sel.Visibility = Visibility.Visible;
                face_length_sel.Visibility = Visibility.Visible;
                bo1.Visibility = Visibility.Visible;
                bo2.Visibility = Visibility.Visible;
                change.Content = "Перейти к 3D";
            }
        }

        private void draw_fr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            point = e.GetPosition(draw_fr); 
        }

        private void draw_fr_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (chin_sel.IsChecked == true)//ok 
            {
                draw_fr.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_fr);
                line.Stroke = Brushes.Red;
                line.X1 = point.X;
                line.Y1 = point.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_fr.Children.Add(line);
                if (Math.Abs(point.X - point2.X) / 15 < 5) chin.Value = Math.Abs(point.X - point2.X) / 15;
                else chin.Value = (Math.Abs(point.X - point2.X) / 15) + 1;
            }

            //all down -- none ok

            if (lips_sel.IsChecked == true)//ok
            {
                draw_fr.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_fr);
                line.Stroke = Brushes.Red;
                line.X1 = point.X;
                line.Y1 = point.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_fr.Children.Add(line);
                if (Math.Abs(point.Y - point2.Y) / 2 < 5) lips.Value = (Math.Abs(point.Y - point2.Y) / 2) - 1;
                else lips.Value = (Math.Abs(point.Y - point2.Y) / 2);
            }
            
            if (forehead_sel.IsChecked == true)//ok
            {
                draw_fr.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_fr);
                line.Stroke = Brushes.Red;
                line.X1 = point.X;
                line.Y1 = point.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_fr.Children.Add(line);

                if ((Math.Abs(point.Y - point2.Y) / 10) < 5) forehead.Value = (Math.Abs(point.Y - point2.Y) / 10) - 1;
                else forehead.Value = Math.Abs(point.Y - point2.Y) / 10;
            }
            if (ears_sel.IsChecked == true)//ok
            {
                draw_fr.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_fr);
                line.Stroke = Brushes.Red;
                line.X1 = point.X;
                line.Y1 = point.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_fr.Children.Add(line);
                ears.Value = Math.Abs(point.X - point2.X)/4;
            }
            if (face_width_sel.IsChecked == true)//ok
            {
                draw_fr.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_fr);
                line.Stroke = Brushes.Red;
                line.X1 = point.X;
                line.Y1 = point.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_fr.Children.Add(line);
                if ((Math.Abs(point.X - point2.X) / 30) > 5)  face_width.Value = (Math.Abs(point.X - point2.X) / 30) + 2;
                else face_width.Value = (Math.Abs(point.X - point2.X) / 30) - 2;
            }
            if (face_length_sel.IsChecked == true)//ok
            {
                draw_fr.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_fr);
                line.Stroke = Brushes.Red;
                line.X1 = point.X;
                line.Y1 = point.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_fr.Children.Add(line);
                if (Math.Abs(point.Y - point2.Y) > 6) face_length.Value =( Math.Abs(point.Y - point2.Y) / 40) + 1.5;
                else face_length.Value = (Math.Abs(point.Y - point2.Y) / 40) ;
            }
            }
      
        private void draw_fr_MouseMove(object sender, MouseEventArgs e)
        {

        }

//-----------------------------------------------------------------------------------------------------------------------------
       
        private void draw_sd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            point0 = e.GetPosition(draw_sd);
        }

        private void draw_sd_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (nose_sel.IsChecked == true)//ok
            {
                draw_sd.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_sd);
                line.Stroke = Brushes.Red;
                line.X1 = point0.X;
                line.Y1 = point0.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_sd.Children.Add(line);
                if (point0.Y - point2.Y != 0)
                nose.Value = Math.Abs(point0.X - point2.X) / 7;
            }
            if (eyes_sel.IsChecked == true)//ok
            {
                draw_sd.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_sd);
                line.Stroke = Brushes.Red;
                line.X1 = point0.X;
                line.Y1 = point0.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_sd.Children.Add(line);
                if (point0.Y - point2.Y != 0)
                    eyes.Value =20/( Math.Abs(point0.Y - point2.Y) / 4);
                 //eyes.Value =20/(Math.Sqrt(Math.Pow(point0.Y - point2.Y, 2) + Math.Pow(point0.X - point2.X, 2)) / 4);
            }
            if (cheelbones_sel.IsChecked == true)//ok
            {
                draw_sd.Children.Remove(line);
                line = new Line();
                Point point2 = e.GetPosition(draw_sd);
                line.Stroke = Brushes.Red;
                line.X1 = point0.X;
                line.Y1 = point0.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;
                draw_sd.Children.Add(line);
                if (point0.Y - point2.Y != 0)
                cheelbones.Value =25/(Math.Sqrt(Math.Pow(point0.Y - point2.Y, 2) + Math.Pow(point0.X - point2.X, 2)) / 7);
                MessageBox.Show("Советуем уделить больше вниманию корректировки этого параметра в правой панели.");
            }
        }

        private void draw_sd_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void border_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            border.Visibility = Visibility.Hidden;
            border_sel.Visibility = Visibility.Hidden;
        }

        private void border_sd_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            border_sd.Visibility = Visibility.Hidden;
            border_sel.Visibility = Visibility.Hidden;
        }

        private void border_sel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

    }

}