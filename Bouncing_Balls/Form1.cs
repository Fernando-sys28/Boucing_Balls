using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Bouncing_Balls
{
    public partial class Form1 : Form
    {
        public int ballsize;

        private Bitmap bmp;
        private Graphics g;
        private int NumBalls=10;
        private Random rand = new Random();
        private List<Balls> balls = new List<Balls>();
        public Form1()
        {
            InitializeComponent();           
            // Create the PictureBox and add it to the form
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;

            // Create some balls
            for (int i = 0; i < NumBalls; i++)
            { 
                
                this.balls.Add(new Balls
                {
                    ballsize = rand.Next(20, 50),
                    X = rand.Next(pictureBox1.Width - ballsize),
                    Y = rand.Next(pictureBox1.Height - ballsize),
                    DX = rand.Next(5, 10),
                    DY = rand.Next(5, 10),
                    Color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))
                });
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           foreach (var ball in this.balls)
            {
                ball.X += ball.DX;
                ball.Y += ball.DY;
            }

                // Check for collision between balls
                for (int i = 0; i < balls.Count; i++)
                {
                    for (int j = i + 1; j < balls.Count; j++)
                    {
                        var ball1 = balls[i];
                        var ball2 = balls[j];

                        // Calculate the distance between the centers of the balls
                        double distance = Math.Sqrt(Math.Pow(ball2.X - ball1.X, 2) + Math.Pow(ball2.Y - ball1.Y, 2));

                        // Calculate the distance that the balls will travel in the next iteration of the loop
                        double travelDistance = ball1.ballsize / 2 + ball2.ballsize / 2;

                        // Check if the balls will collide in the next iteration of the loop
                        if (distance <= travelDistance)
                        {
                            // Move the balls back to their collision point
                            double overlap = travelDistance - distance;
                            double ratio = overlap / distance;
                            double dx = (ball2.X - ball1.X) * ratio;
                            double dy = (ball2.Y - ball1.Y) * ratio;
                            ball1.X -= dx / 2;
                            ball1.Y -= dy / 2;
                            ball2.X += dx / 2;
                            ball2.Y += dy / 2;
                        }
                    }
                }

                // Check for collision with walls
                foreach (var ball in this.balls)
                {
                    if (ball.X < ball.ballsize / 2 || ball.X + ball.ballsize / 2 > pictureBox1.Width)
                    {
                        ball.DX = -ball.DX;
                        ball.X = ball.X < ball.ballsize / 2 ? ball.ballsize / 2 : pictureBox1.Width - ball.ballsize / 2;
                    }

                    if (ball.Y < ball.ballsize / 2 || ball.Y + ball.ballsize / 2 > pictureBox1.Height)
                    {
                        ball.DY = -ball.DY;
                        ball.Y = ball.Y < ball.ballsize / 2 ? ball.ballsize / 2 : pictureBox1.Height - ball.ballsize / 2;
                    }
                pictureBox1.Invalidate();
            }

           // Draw the balls
            drawBalls();
            


        }

        public void drawBalls()
        {
            g.Clear(Color.White);

            // Draw the balls
            foreach (var ball in this.balls)
            {
                g.FillEllipse(new SolidBrush(ball.Color), Convert.ToInt32(ball.X), Convert.ToInt32(ball.Y), ball.ballsize, ball.ballsize);
            }
        }

        
    }
}
