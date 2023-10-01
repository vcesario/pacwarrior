using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;

namespace topdown1;

public static class GameText
{
    public static FontSystem Font_OpenSansBold { get; private set; }
    public static FontSystem Font_OpenSans { get; private set; }

    public static void Initialize()
    {
        Font_OpenSans = new FontSystem();
        Font_OpenSans.AddFont(File.ReadAllBytes("Assets/Fonts/OpenSans-Regular.ttf"));

        Font_OpenSansBold = new FontSystem();
        Font_OpenSansBold.AddFont(File.ReadAllBytes("Assets/Fonts/OpenSans-Bold.ttf")); // @QUESTION: put '@' before string?
    }

}