using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(700, 700);

// met een Bitmap kun je een plaatje opslaan in het geheugen
Bitmap plaatje = new Bitmap(500, 500);

//aanmaken van speelveld array
byte[,] myArray = matrix(6);

//aangeven welke speler aan de beurt is
int Beurt = 0;

//array voor legale zetten
byte[,] legal_array = legal();

//variabele voor help
bool helpAan = false;

//Pasteller om te kijken of er 2 keer op rij wordt gepast 
int PasTeller = 0;

//hoeveelheid beginstenen
int rood = 2;
int blauw = 2;



//button om een nieuw spel te starten 
Button NieuwSpelKnop = new Button();
scherm.Controls.Add(NieuwSpelKnop);
NieuwSpelKnop.Location = new Point(10, 50);
NieuwSpelKnop.Size = new Size(100, 20);
NieuwSpelKnop.Text = "Nieuw spel";
NieuwSpelKnop.BackColor = Color.White;

//button om mogelijke zetten te laten zien
Button Help = new Button();
scherm.Controls.Add(Help);
Help.Location = new Point(10, 80);
Help.Size = new Size(100, 20);
Help.Text = "Help";
Help.BackColor = Color.White;

//button om te passen
Button Pas = new Button();
scherm.Controls.Add(Pas);
Pas.Location = new Point(10, 110);
Pas.Size = new Size(100, 20);
Pas.Text = "Pas";
Pas.BackColor = Color.White;

//label voor keuze aangeven
Label KeuzeUitleg = new Label();
KeuzeUitleg.Location = new Point(10, 140);
KeuzeUitleg.Size = new Size(100, 20);
KeuzeUitleg.Text = "Grootte: n x n";
scherm.Controls.Add(KeuzeUitleg);


//keuze van de grootte van het speelveld (dropdown)
ComboBox grootteKeuze = new ComboBox();
scherm.Controls.Add(grootteKeuze);
grootteKeuze.Location = new Point(10, 160);
grootteKeuze.Width = 100;
grootteKeuze.DropDownStyle = ComboBoxStyle.DropDownList;

grootteKeuze.Items.Add(4);
grootteKeuze.Items.Add(6);
grootteKeuze.Items.Add(8);
grootteKeuze.Items.Add(10);
grootteKeuze.SelectedIndex = 1;

//Label om score te laten zien
Label scoreLabel = new Label();
scoreLabel.Location = new Point(10, 190);
scoreLabel.Size = new Size(120, 50);
scoreLabel.Font = new Font("Segoe UI", 15, FontStyle.Bold);
scherm.Controls.Add(scoreLabel);


// een Label kan ook gebruikt worden om een Bitmap te laten zien
Label afbeelding = new Label();
scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(150, 50);
afbeelding.Size = new Size(500, 500);
afbeelding.BackColor = Color.White;
afbeelding.Image = plaatje;


//grote label onderaan voor wiens zet het is
Label WiensZet = new Label();
scherm.Controls.Add(WiensZet);
WiensZet.Location = new Point(160, 560);
WiensZet.Size = new Size(480, 100);
WiensZet.Text = "Blauw is aan zet";
WiensZet.TextAlign = ContentAlignment.MiddleCenter;
WiensZet.ForeColor = Color.White;
WiensZet.BackColor = Color.Blue;
WiensZet.Font = new Font("Segoe UI", 30);






//functie voor nieuwspel knop
void NieuwSpel(int grootte)
    {
    myArray = matrix(grootte);
    Beurt = 1;
    PasTeller = 0;
    helpAan = false;
    UpdateScore();
    legal_array = legal();
    teken_raster(null, EventArgs.Empty);
    }


//voor het updaten van de score aan de linkerkant van het scherm
void UpdateScore()
{
    TelStenen();
    scoreLabel.Text = $"Blauw: {blauw}\nRood: {rood}";
}

//checken of de speler een legale zet heeft, want dan mogen ze niet passen
bool HeeftLegaleZet(byte speler)
{
    for (int x = 0; x < myArray.GetLength(0); x++)
        for (int y = 0; y < myArray.GetLength(1); y++)
            if (legal_array[x, y] == speler)
                return true;
    return false;
}



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

bool InBounds(int r, int c)
{
    return r >= 0 && r < myArray.GetLength(0) &&
           c >= 0 && c < myArray.GetLength(1);
}


 byte[,] legal()
{
    //pakt lengte en breedte van het veld
    int rows = myArray.GetLength(0);
    int cols = myArray.GetLength(1);
    //maak array aan
    byte[,] legal_array = new byte[rows, cols];
    //check voor elke speler
    for (byte player = 1; player <= 2; player++)
    {   
        //zelfde logica als speler maar dan omgekeert
        byte opponent = (player == 1) ? (byte)2 : (byte)1;
        //gaat langs elke cel
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                //als cel leeg is dan slaat hij over
                if (myArray[i, j] != 0)
                    continue;
                //zo niet elke windrichting checken
                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        //als windrichting 0,0 is dan slaat hij over
                        if (dx == 0 && dy == 0) continue;
                        //coordinaat + richting
                        int x = i + dx;
                        int y = j + dy;
                        //als het speler is of 0 of out of bounds dan slaat hij over
                        if (!InBounds(x, y) || myArray[x, y] != opponent)
                            continue;

                        x += dx;
                        y += dy;
                        //terwijl het in bounds is
                        while (InBounds(x, y))
                        {
                            //stop bij 0
                            if (myArray[x, y] == 0)
                                break;
                            //wanneer het speler is
                            if (myArray[x, y] == player)
                            {   //zet in legale zetten array voor wie het legaal is
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
    //zet huidige vak naar speler
    myArray[x, y] = player;
    //zelfde logica als speler maar dan andersom
    byte opponent = (player == 1) ? (byte)2 : (byte)1;
    //voor elke windrichting
    for (int dx = -1; dx <= 1; dx++) for (int dy = -1; dy <= 1; dy++)
        {
            //slaat 0,0 over
            if (dx == 0 && dy == 0) continue;
            //coordinaat = beginpunt + richting
            int Cor_x = x + dx;
            int Cor_y = y + dy;
            //als de coordinaat waar het opkomt niet een tegenstander is dan slaat hij over
            if (!InBounds(Cor_x, Cor_y) || myArray[Cor_x, Cor_y] != opponent) continue;

            //neemt stappen naar voren terwijl het nog tegenstander is
            while (InBounds(Cor_x, Cor_y) && myArray[Cor_x, Cor_y] == opponent)
            {
                Cor_x += dx; Cor_y += dy;
            }
            //als het buiten bounds is of het is de tegenstander of niks dan stopt het
            if (!InBounds(Cor_x, Cor_y) || myArray[Cor_x, Cor_y] != player)
                continue;
            //stap terug naar laatste tegenstander
            Cor_x -= dx; Cor_y -= dy;

            //loopt terug tot begin punt terwijl het alles meeverandert
                    while (Cor_x != x || Cor_y != y)
                    {
                        myArray[Cor_x, Cor_y] = player;
                        Cor_x -= dx; Cor_y -= dy;

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

                if(helpAan && legal_array[i, j] == (Beurt % 2 == 0 ? (byte)1 : (byte)2))
                {
                    int grootteHint = raster_size / 3;
                    int offset = (raster_size - grootteHint) / 2;
                    graphics.FillEllipse(Brushes.Green, i * raster_size + offset, j * raster_size + offset, grootteHint, grootteHint);

                }
            }
        
            
    }

    afbeelding.Image = plaatje;
    afbeelding.Refresh();
}


void zet(Object o, MouseEventArgs ea)
{
    int raster_size = (int)(afbeelding.Width) / myArray.GetLength(0);
    int x = ea.X / raster_size;
    int y = ea.Y / raster_size;

    byte current_player = Beurt % 2 == 0 ? (byte)1 : (byte)2;
    if (legal_array[x, y] != current_player) return;

    afbeelding.Invalidate();
    flipper(x, y, current_player);
    legal_array = legal();

    teken_raster(null, EventArgs.Empty);
    Beurt += 1;

    //zorgen dat de pas teller wordt gereset
    if (PasTeller == 1)
        PasTeller -= 1;
    
    
    

    //zorgen dat het wienszet label de juiste kleur en tekst heeft
    if (Beurt%2 == 0)
    {
        WiensZet.Text = "Rood is aan de beurt";
        WiensZet.BackColor = Color.Red;
    }
    else 
    {
        WiensZet.Text = "Blauw is aan de beurt";
        WiensZet.BackColor = Color.Blue;
    }
    UpdateScore();
}


void TelStenen()
{
    rood = 0;
    blauw = 0;
    
    int n = myArray.GetLength(0);

    for (int x = 0; x < n; x++)
    {
        for (int y = 0; y < n; y++)
        {
            if (myArray[x, y] == 1)
                rood++;
            else if (myArray[x, y] == 2)
                blauw++;
        }
    }
    CheckSpelKlaar();
}


//kijken of er is gewonnen door vol bord
void CheckSpelKlaar()
{
    //kijken hoe groot het raster is 
    int n = myArray.GetLength(0);


    if (rood + blauw == n * n)
    {
        if (rood > blauw)
        {
            WiensZet.Text = "Rood heeft gewonnen!";
            WiensZet.BackColor = Color.Red;
        }
        if (blauw > rood)
        {
            WiensZet.Text = "Blauw heeft gewonnen!";
            WiensZet.BackColor = Color.Blue;
        }
        if (blauw == rood)
        {
            WiensZet.Text = "Het is gelijkspel";
            WiensZet.BackColor = Color.Magenta;
        }
    }
}

//functie om te kunnen passen
void PasHandler(object o,EventArgs e)
{

    //zorgen dat je niet kan passen als je gewoon een zet hebt
    byte speler = Beurt % 2 == 0 ? (byte)1 : (byte)2;
    if (HeeftLegaleZet(speler)) return;

    TelStenen();
    Beurt += 1;
    PasTeller += 1;
    legal_array = legal();
    teken_raster(null, EventArgs.Empty);
    if (PasTeller == 2)
    {
        if (rood > blauw)
        {
            WiensZet.Text = "Rood heeft gewonnen!";
            WiensZet.BackColor = Color.Red;
        }
        if (blauw > rood)
        {
            WiensZet.Text = "Blauw heeft gewonnen!";
            WiensZet.BackColor = Color.Blue;
        }
        if (blauw == rood)
        {
            WiensZet.Text = "Het is gelijkspel";
            WiensZet.BackColor = Color.Magenta;
        }
    }
}







afbeelding.MouseClick += zet;
afbeelding.MouseClick += teken_raster;

//dropdown event handler
grootteKeuze.SelectedIndexChanged += (o, e) =>
{
    int gekozenGrootte = (int)grootteKeuze.SelectedItem;
    NieuwSpel(gekozenGrootte);
};


//pas button
Pas.Click += PasHandler;

//help button
Help.Click += (s, e) =>
{
    helpAan = !helpAan; //verander help waarde
    teken_raster(null, EventArgs.Empty); //teken raster opnieuw
};

//nieuw spel button
NieuwSpelKnop.Click += (o, e) =>
{
    int gekozenGrootte = (int)grootteKeuze.SelectedItem;
    NieuwSpel(gekozenGrootte);
};
//teken het eerste speelveld van 6x6
NieuwSpel(6);

Application.Run(scherm);