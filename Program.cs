using System;
using HW5_GROUP_PROJECT.sand;

SandGrid grid = new();
LookupTable table = new(3);

byte interpet(in SandPixel pixel)
{
    return  pixel.id == PixelId.INVALID?    (byte)0: 
            pixel.id == PixelId.SAND?       (byte)1: 
                                            (byte)2;
}

DateTime time = DateTime.Now;
for(int i = 0; i < 100; i++){
    grid.Update(table, interpet, (byte)(i % 4));
}
Console.WriteLine($"{((DateTime.Now - time)/ 100f).TotalMicroseconds}");
