using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(220, 220);

// met een Bitmap kun je een plaatje opslaan in het geheugen
Bitmap plaatje = new Bitmap(200, 200);

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
byte[,] myArray =matrix(4);

Label Test = new Label();
scherm.Controls.Add(Test);
Test.Text = myArray[0,1].ToString();

// een Label kan ook gebruikt worden om een Bitmap te laten zien
Label afbeelding = new Label();
scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(10, 10);
afbeelding.Size = new Size(200, 200);
afbeelding.BackColor = Color.White;
afbeelding.Image = plaatje;

Application.Run(scherm);