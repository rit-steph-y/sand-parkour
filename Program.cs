using System;
using System.Threading;
using HW5_GROUP_PROJECT.sand;

SandGrid grid = new();
LookupTable table = new(3);
table.Build((tl, tr, bl, br) =>
{
    bool bl_available = bl == 0;
    bool br_available = br == 0;
    ReplaceSandGroup group = new();
    if (bl_available && tl == 1)
    {
        bl_available = false;
        (group.TopLeft, group.BottomLeft) = (group.BottomLeft, group.TopLeft);
    }
    if (br_available && tr == 1)
    {
        br_available = false;
        (group.TopRight, group.BottomRight) = (group.BottomRight, group.TopRight);
    }
    if (br_available && tl == 1)
    {
        br_available = false;
        (group.TopLeft, group.BottomRight) = (group.BottomRight, group.TopLeft);
    }
    if (bl_available && tr == 1)
    {
        bl_available = false;
        (group.TopRight, group.BottomLeft) = (group.BottomLeft, group.TopRight);
    }
    return group;
});

byte interpet(in SandPixel pixel)
{
    return  pixel.id == PixelId.INVALID?    (byte)2: 
            pixel.id == PixelId.SAND?       (byte)1: 
                                            (byte)0;
}

uint xRange = 800;
uint yRange = 800;

Random r = new(0);
for(uint x = 0; x < xRange; x++)
{
    for(uint y = 0; y < yRange; y++)
    {
        grid.SetPixel(new(x + 1,y + 1), r.Next(2) == 0? PixelId.AIR: PixelId.SAND);
    }
}

DateTime time = DateTime.Now;
for(int i = 0; i < 1000; i++){
    // Thread.Sleep(100);
    
    // for(uint y = 0; y < 34; y++)
    // {
    //     for(uint x = 0; x < 34; x++)
    //     {
    //         switch (grid.GetPixel(new(x, y)).id){
    //             case PixelId.AIR:
    //                 Console.Write(" ");
    //                 continue;
    //             case PixelId.INVALID:
    //                 Console.Write("x");
    //                 continue;
    //             case PixelId.SAND:
    //                 Console.Write("S");
    //                 continue;
    //         }
    //     }
    //     Console.WriteLine();
    // }
    grid.Update(table, interpet, (byte)(i % 4));
}
Console.WriteLine($"{((DateTime.Now - time)/ 1000f).TotalMicroseconds}");
