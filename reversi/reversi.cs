using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(260, 260);

// met een Bitmap kun je een plaatje opslaan in het geheugen
Bitmap plaatje = new Bitmap(200, 200);


Button knop = new Button();
scherm.Controls.Add(knop);
knop.Location = new Point(10, 10);
knop.Size = new Size(10, 10);
// een Label kan ook gebruikt worden om een Bitmap te laten zien
Label afbeelding = new Label();
scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(40, 40);
afbeelding.Size = new Size(200, 200);
afbeelding.BackColor = Color.White;
afbeelding.Image = plaatje;

byte[,] matrix(int n)
{
    byte[,] matrix = new byte [n, n];
    int middle = (int)(n / 2);
    int color = 0;

    for(int i = -1; i < 1;i++)
    {
        for (int j = -1; j < 1; j++)
        {
            color++;
            int base_color = (color % 2) +1;
            if (i == 0) base_color = 3 - base_color;
            matrix[middle + i, middle + j] = (byte)base_color;


        }
        
    }
    


    return matrix;
}
byte[,] myArray = matrix(8);


void teken_raster(object o, EventArgs ea)
{
    afbeelding.Invalidate();
    int n = myArray.GetLength(0);
    Graphics graphics = Graphics.FromImage(plaatje);
    Pen pen = new Pen(Color.Blue,1);
    int raster_size = (int)(afbeelding.Width-pen.Width)/n;
    Brush player_1 = new SolidBrush(Color.Red);
    Brush player_2 = new SolidBrush(Color.Blue);
    for (int i = 1; i <= n; i++) {
        for (int j = 1; j <= n; j++) { 
            graphics.DrawRectangle(pen, i*raster_size-raster_size, j*raster_size-raster_size, raster_size, raster_size);
            
    } }
    for (int i = 0;  i<= myArray.GetLength(0)-1; i++)
    {
        for (int j =0; j <= myArray.GetLength(1)-1; j++)
        {
            if (myArray[i, j] == 1)
            {
                graphics.FillEllipse(player_1, i * raster_size , j * raster_size, raster_size, raster_size);
            }
            if (myArray[i, j] == 2)
            {
                graphics.FillEllipse(player_2, i * raster_size, j * raster_size, raster_size, raster_size);
            }
        }
    }
}
int Beurt = 0;

void zet(Object o, MouseEventArgs ea)
{
    Beurt += 1;
    
    afbeelding.Invalidate();
    int raster_size = (int)(afbeelding.Width) / myArray.GetLength(0);
    int x = ea.X / raster_size;
    int y = ea.Y / raster_size;
    if (Beurt % 2 == 0) {myArray[x, y] = 1;}
    if(Beurt % 2 == 1) { myArray[x, y] = 2;}
    teken_raster(null, EventArgs.Empty);
}


afbeelding.MouseClick += zet;
afbeelding.MouseClick += teken_raster;

knop.Click += teken_raster;
Application.Run(scherm);