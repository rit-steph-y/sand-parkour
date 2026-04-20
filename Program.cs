
using System;
using HW5_GROUP_PROJECT.sand;

SandGridComponent component = new SandGridComponent(null);

DateTime time = DateTime.Now;

const int ITERS = 100;

for (int i = 0; i < ITERS; i++)
{
    component.Update();
}
Console.WriteLine($"avg: {((DateTime.Now - time)/ (float) ITERS).TotalMilliseconds}ms per tick");

using var game = new  HW5_GROUP_PROJECT.SandGame();
game.Run();