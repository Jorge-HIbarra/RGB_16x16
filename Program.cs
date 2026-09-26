using Raylib_cs;
using System;

Raylib.InitWindow(800, 600, "Dibujo bob");
Raylib.SetTargetFPS(60);

Color[] colors = new Color[]
{
    new Color(255, 255, 0, 255),
    new Color(26, 64, 133, 255),
    new Color(90, 78, 46, 255),
    new Color(225, 252, 200, 255),
    new Color(240, 96, 29, 255)
};

Color colorSeleccionado = Color.RayWhite;

int indexA = -1;
int indexB = -1;

const int palW  = 64;
const int palH  = 64;
const int palX  = 20;
const int palY0 = 30;
const int palGap = 78;

const byte alphaByte = 128;   

const int gridSize = 16;
const int cellSize = 30;
const int gridW = gridSize * cellSize;      
const int gridH = gridSize * cellSize;      
const int gridX = (800 - gridW) / 2;        
const int gridY = 15;

Color[,] grid = new Color[gridSize, gridSize];
for (int r = 0; r < gridSize; r++)
    for (int c = 0; c < gridSize; c++)
        grid[r, c] = Color.RayWhite;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    System.Numerics.Vector2 mouse = Raylib.GetMousePosition();

   
    for (int i = 0; i < colors.Length; i++)
    {
        Rectangle rec = new Rectangle(palX, palY0 + i * palGap, palW, palH);

        if (Raylib.CheckCollisionPointRec(mouse, rec))
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                indexA = i;
                colorSeleccionado = colors[i];
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                indexB = i;
            }
        }
    }


    if (Raylib.CheckCollisionPointRec(mouse, new Rectangle(gridX, gridY, gridW, gridH)))
    {
        int col = (int)((mouse.X - gridX) / cellSize);
        int row = (int)((mouse.Y - gridY) / cellSize);

        if (col >= 0 && col < gridSize && row >= 0 && row < gridSize)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                grid[row, col] = colorSeleccionado;

            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
                grid[row, col] = Color.RayWhite;
        }
    }


    for (int r = 0; r < gridSize; r++)
    {
        for (int c = 0; c < gridSize; c++)
        {
            int x = gridX + c * cellSize;
            int y = gridY + r * cellSize;

            Raylib.DrawRectangle(x, y, cellSize, cellSize, grid[r, c]);
            Raylib.DrawRectangleLines(x, y, cellSize, cellSize, Color.LightGray);
        }
    }
    Raylib.DrawRectangleLines(gridX, gridY, gridW, gridH, Color.Black);

   
    for (int i = 0; i < colors.Length; i++)
    {
        int x = palX;
        int y = palY0 + i * palGap;
        Rectangle rec = new Rectangle(x, y, palW, palH);

        Raylib.DrawRectangleRec(rec, colors[i]);

        bool hover = Raylib.CheckCollisionPointRec(mouse, rec);

        if (i == indexA)
            Raylib.DrawRectangleLinesEx(rec, 4, Color.Black);
        else
            Raylib.DrawRectangleLinesEx(rec, hover ? 3 : 1, hover ? Color.Black : Color.Gray);

        string hex = $"#{colors[i].R:X2}{colors[i].G:X2}{colors[i].B:X2}";
        Raylib.DrawText(hex, x + palW + 8, y + 24, 14, Color.Black);

        if (i == indexA && i == indexB)
        {
            Raylib.DrawText("A", x + 5,  y + 5, 26, Color.Black);
            Raylib.DrawText("B", x + 34, y + 5, 26, Color.Black);
        }
        else if (i == indexA)
        {
            Raylib.DrawText("A", x + 5, y + 5, 26, Color.Black);
        }
        else if (i == indexB)
        {
            Raylib.DrawText("B", x + 5, y + 5, 26, Color.Black);
        }
    }


    Color cA = indexA >= 0 ? colors[indexA] : Color.RayWhite;
    Color cB = indexB >= 0 ? colors[indexB] : Color.RayWhite;

    float af = alphaByte / 255f;
    byte mr = (byte)Math.Round(cA.R * af + cB.R * (1f - af));
    byte mg = (byte)Math.Round(cA.G * af + cB.G * (1f - af));
    byte mb = (byte)Math.Round(cA.B * af + cB.B * (1f - af));
    Color blended = new Color(mr, mg, mb, (byte)255);
    string blendedHex = $"#{blended.R:X2}{blended.G:X2}{blended.B:X2}";

    const int indSize = 36;
    const int indGap  = 20;
    int indTotalW = indSize * 3 + indGap * 2;
    int indX = (800 - indTotalW) / 2;
    int indY = 536;

    int xB = indX + indSize + indGap;
    int xR = indX + 2 * (indSize + indGap);


    Raylib.DrawText("A", indX + 14, indY - 16, 12, Color.Black);
    Raylib.DrawText("B", xB + 14,   indY - 16, 12, Color.Black);
    Raylib.DrawText("A sobre B", xR, indY - 16, 12, Color.Black);

    Raylib.DrawRectangle(indX, indY, indSize, indSize, cA);
    Raylib.DrawRectangleLines(indX, indY, indSize, indSize, Color.Black);

    Raylib.DrawRectangle(xB, indY, indSize, indSize, cB);
    Raylib.DrawRectangleLines(xB, indY, indSize, indSize, Color.Black);

    Raylib.DrawRectangle(xR, indY, indSize, indSize, cB);
    Raylib.DrawRectangle(xR, indY, indSize, indSize, new Color(cA.R, cA.G, cA.B, alphaByte));
    Raylib.DrawRectangleLines(xR, indY, indSize, indSize, Color.Black);

    Raylib.DrawText(blendedHex, xR, indY + indSize + 4, 12, Color.Black);

    Raylib.DrawText("Click izq: pintar", palX, 440, 14, Color.DarkGray);
    Raylib.DrawText("Click der: borrar", palX, 460, 14, Color.DarkGray);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();