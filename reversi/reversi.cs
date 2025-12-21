using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

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
byte[,] myArray = matrix(4);
bool InBounds(int r, int c)
{
    return r >= 0 && r < myArray.GetLength(0) &&
           c >= 0 && c < myArray.GetLength(1);
}


 byte[,] legal()
{
    int rows = myArray.GetLength(0);
    int cols = myArray.GetLength(1);

    byte[,] legal_array = new byte[rows, cols];

    for (byte player = 1; player <= 2; player++)
    {
        byte opponent = (player == 1) ? (byte)2 : (byte)1;

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                if (myArray[i, j] != 0)
                    continue;

                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        int x = i + dx;
                        int y = j + dy;

                        if (!InBounds(x, y) || myArray[x, y] != opponent)
                            continue;

                        x += dx;
                        y += dy;

                        while (InBounds(x, y))
                        {
                            if (myArray[x, y] == 0)
                                break;

                            if (myArray[x, y] == player)
                            {
                                legal_array[i, j] |= player; // 1 or 2
                                break;
                            }

                            x += dx;
                            y += dy;
                        }
                    }
            }
    }

    return legal_array;
}

void flipper(int x, int y, byte player)
{

    byte opponent = (player == 1) ? (byte)2 : (byte)1;
    for (int dx = -1; dx <= 1; dx++) for (int dy = -1; dy <= 1; dy++)
        {
            if (dx == 0 && dy == 0) continue;
            int Cor_x = x + dx;
            int Cor_y = y + dy;
            if (!InBounds(Cor_x, Cor_y) || myArray[Cor_x, Cor_y] !=opponent) continue;
            while (InBounds(Cor_x, Cor_y) && myArray[Cor_x,Cor_y]!=player)
            {
                if (myArray[Cor_x, Cor_y] != player)

                {
                    if (myArray[Cor_x, Cor_y] == 0) break;

                    while (Cor_x != x || Cor_y != y)
                    {
                        myArray[Cor_x, Cor_y] = player;
                        Cor_x -= dx; Cor_y -= dy;

                    }


                }




            }
        }
}


void teken_raster(object o, EventArgs ea)
{
    int n = myArray.GetLength(0);

    using (Graphics graphics = Graphics.FromImage(plaatje))
    {
        graphics.Clear(Color.White);

        Pen pen = new Pen(Color.Blue, 1);
        int raster_size = afbeelding.Width / n;
        Brush player_1 = Brushes.Red;
        Brush player_2 = Brushes.Blue;

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                graphics.DrawRectangle(
                    pen,
                    i * raster_size,
                    j * raster_size,
                    raster_size,
                    raster_size
                );

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (myArray[i, j] == 1)
                    graphics.FillEllipse(player_1, i * raster_size, j * raster_size, raster_size, raster_size);
                else if (myArray[i, j] == 2)
                    graphics.FillEllipse(player_2, i * raster_size, j * raster_size, raster_size, raster_size);
            }
    }

    afbeelding.Image = plaatje;
    afbeelding.Refresh();
}

int Beurt = 0;
byte[,] legal_array = legal();
void zet(Object o, MouseEventArgs ea)
{
    int raster_size = (int)(afbeelding.Width) / myArray.GetLength(0);
    int x = ea.X / raster_size;
    int y = ea.Y / raster_size;

    byte current_player = Beurt % 2 == 0 ? (byte)1 : (byte)2;
    if (legal_array[x, y] != current_player) return;

        afbeelding.Invalidate();


        if (Beurt % 2 == 0) {myArray[x, y] = 1;}
        if(Beurt % 2 == 1) { myArray[x, y] = 2;}

        legal_array =legal();
        flipper(x,y, current_player);
    teken_raster(null, EventArgs.Empty);
    Beurt += 1;
}


afbeelding.MouseClick += zet;
afbeelding.MouseClick += teken_raster;

knop.Click += teken_raster;
Application.Run(scherm);