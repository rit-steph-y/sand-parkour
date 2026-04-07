
using HW5_GROUP_PROJECT.sand;

SandGrid grid = new();
LookupTable table = new(3);

byte interpet(in SandPixel pixel)
{
    return pixel.id == PixelId.INVALID? (byte)0: 
            pixel.id == PixelId.SAND? (byte)1: (byte)2;
}

grid.Update(table, interpet, 0);

// using var game = new  HW5_GROUP_PROJECT.SandGame();
// game.Run();
