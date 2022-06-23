using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public enum MouseButtons
    {
        //
        // Сводка:
        //     Была нажата левая кнопка мыши.
        Left = 0x100000,
        //
        // Сводка:
        //     Никакая кнопка мыши не была нажата.
        None = 0x0,
        //
        // Сводка:
        //     Была нажата правая кнопка мыши.
        Right = 0x200000,
        //
        // Сводка:
        //     Была нажата средняя кнопка мыши.
        Middle = 0x400000,
        //
        // Сводка:
        //     Была нажата первая кнопка XButton.
        XButton1 = 0x800000,
        //
        // Сводка:
        //     Была нажата вторая кнопка XButton.
        XButton2 = 0x1000000
    }
}
