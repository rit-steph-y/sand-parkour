
using System;
using HW5_GROUP_PROJECT.sand;

// SandGridComponent component = new SandGridComponent(null);

// DateTime time = DateTime.Now;
// for(int i = 0; i < 1000; i++)
// {
//     component.Update();
// }
// Console.WriteLine($"avg: {((DateTime.Now - time)/ 1000f).TotalMilliseconds}ms per tick");

using var game = new  HW5_GROUP_PROJECT.SandGame();
game.Run();