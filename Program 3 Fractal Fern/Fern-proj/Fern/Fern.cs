/////////////////////////////////////////
///
/// Jacob Tocila Fractal Fern Project
/// October 24, 2023
/// CS 212
/// Total work time ~3.5 hours
/// 
/////////////////////////////////////////

// Random factors used:
// 1. Random berry size in x and y direction
// 2. Random berry color
// 3. Random tendril color for each line segment
// 4. Random background color

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FernNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     * Written as sample C# code for a CS 212 assignment -- October 2011.
     * 
     * Bugs: WPF and shape objects are the wrong tool for the task 
     */   
    class Fern
    {
        // I had a lot of fun adjusting these variables, they make the fern appear much more consistent
        private static int BERRYMIN = 3;
        // private static int TENDRILS = 32;
        private static int TENDRILMIN = 4;
        private static double DELTATHETA = 0.02;
        private static double SEGLENGTH = 2.9;
        private static Random Random = new Random();
        // LR = alternate fern leaves left and right from stem
        private static int LR = 1;

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {
            /*
            // this if-else block gives a random wallpaper of the given three images in the folder
            // I was hoping to be able to make the wallpaper random, but I was unable to do that...
            //int randomIntBackground = Random.Next(0, 256);
            int randomIntBackground = Random.Next(0, 2);
            if (randomIntBackground == 0)
            {
                ImageBrush myImageBrush = new ImageBrush();
                //myImageBrush.ImageSource = new BitmapImage(new Uri("wallpaper1.jpg", UriKind.Relative));
                canvas.Background = myImageBrush;
            }
            else
            {
                ImageBrush myImageBrush = new ImageBrush();
                //myImageBrush.ImageSource = new BitmapImage(new Uri("wallpaper2.jpg", UriKind.Relative));
                canvas.Background = myImageBrush;
            }
            */

            // set wallpaper to a random solid color
            Random random = new Random();
            byte randomRed = (byte)random.Next(140, 255);
            byte randomGreen = (byte)random.Next(140, 255);
            byte randomBlue = (byte)random.Next(140, 255);
            SolidColorBrush randomColor = new SolidColorBrush(Color.FromRgb(randomRed, randomGreen, randomBlue));
            canvas.Background = randomColor;
            // reset old fern
            canvas.Children.Clear();

            // draw a new fern at the center of the canvas with given parameters
            tendril((int)(canvas.Width / 2), (int)(canvas.Height), size / 1.75, redux / 1.25, turnbias, Math.PI, canvas);       
        }

        /*
         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
         * regularly-spaced directions out of the cluster.
         */
        private void cluster(int x, int y, double size, double redux, double turnbias, double direction, int LR, Canvas canvas)
        {
            
                // compute the angle of the outgoing tendril - changed this equation, original was too strange?
                // I needed help from a friend on adjusting the theta equation, they were able to get a nice one
                double theta = (Math.PI * 10 * Random.NextDouble()/180 - Math.PI * 80 / 180);
                Random BerrySize = new Random();
                tendril(x, y, size, redux, turnbias, direction, canvas);
                // draw the tendril left or right depending on the LR value
                // this I needed help on. Creating a new variable helped control how the fern would form new clusters
                if (LR == 1)
                {
                    tendril(x, y, size / 1.5, redux, turnbias, direction - theta, canvas);
                }
                else
                {
                    tendril(x, y, size / 1.5, redux, turnbias,direction + theta, canvas);
                }
                if (size > BERRYMIN)
                    berry2(x, y, BerrySize.Next(5,8) , canvas);
                
                    

            }
        

        /*
         * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
         * and draws a cluster at the other end if the line is big enough.
         */
        private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            int x2=x1, y2=y1;
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                direction += (random.NextDouble() > turnbias) ? -1 * DELTATHETA : DELTATHETA;
                x1 = x2; y1 = y2;
                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));
                //byte red = (byte) (100+size/2);
                byte red = (byte)(Random.Next(0, 64));
                //byte green = (byte)(220 - size / 3);
                byte green = (byte)(100 + Random.Next(0, 100));
                byte blue = (byte)(Random.Next(0, 64));
                //if (size>120) red = 138; green = 108;
                // I wanted the tendril to be slightly random in color every time it's generated
                //Random TendrilColor = new Random();
                line(x1, y1, x2, y2, red, green, blue, 1 + size / 40, canvas);
            }
            LR = LR * -1;
            if (size > TENDRILMIN)
                cluster(x2, y2, size / redux, redux, turnbias, direction, LR, canvas);
            if (size < BERRYMIN)
            {
                int whichBerry = Random.Next(2);
                if (whichBerry == 0)
                {
                    berry1(x2, y2, 3, canvas);
                }
                else
                {
                    berry2(x2, y2, 3, canvas);
                }
            }
        }

        /*
         * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
         */
        private void berry1(int x, int y, double radius, Canvas canvas)
        {

            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, (byte)(Random.Next(128, 256)), 0, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            // Added randomness to berry dimensions. May get odd shapes
            myEllipse.Width = (Random.Next(2, 4)) * radius;
            myEllipse.Height = (Random.Next(2, 4)) * radius;
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }


        private void berry2(int x, int y, double sideLength, Canvas canvas)
        {
            Polygon mySquare = new Polygon();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, (byte)(Random.Next(128, 256)), 0, 0);
            mySquare.Fill = mySolidColorBrush;
            mySquare.StrokeThickness = 1;
            mySquare.Stroke = Brushes.Black;
            mySquare.HorizontalAlignment = HorizontalAlignment.Center;
            mySquare.VerticalAlignment = VerticalAlignment.Center;

            // Create the points for a square
            PointCollection squarePoints = new PointCollection();
            double halfSide = sideLength / 1.5;
            squarePoints.Add(new Point(x - halfSide, y - halfSide));
            squarePoints.Add(new Point(x + halfSide, y - halfSide));
            squarePoints.Add(new Point(x + halfSide, y + halfSide));
            squarePoints.Add(new Point(x - halfSide, y + halfSide));

            mySquare.Points = squarePoints;
            //mySquare.SetCenter(x, y);

            canvas.Children.Add(mySquare);
        }


        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            // Added random thickness to each line segment in tendril
            myLine.StrokeThickness = thickness + 2;
            //myLine.StrokeThickness = thickness + Random.Next(0, 4);
            canvas.Children.Add(myLine);
        }
    }
}

/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}

