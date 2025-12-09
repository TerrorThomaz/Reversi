using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(220, 220);

// met een Bitmap kun je een plaatje opslaan in het geheugen
Bitmap plaatje = new Bitmap(200, 200);

// je kunt de losse pixels van het plaatje manipuleren
plaatje.SetPixel(10, 10, Color.Red);

// maar om complexere figuren te tekenen heb je een Graphics nodig
Graphics tekenaar = Graphics.FromImage(plaatje);
tekenaar.FillEllipse(Brushes.Blue, 30, 40, 100, 50);

// een Label kan ook gebruikt worden om een Bitmap te laten zien
Label afbeelding = new Label();
scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(10, 10);
afbeelding.Size = new Size(200, 200);
afbeelding.BackColor = Color.White;
afbeelding.Image = plaatje;

Application.Run(scherm);