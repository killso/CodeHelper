using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class MouseEventArgs : EventArgs
    {
        private readonly MouseButtons button;

        private readonly int clicks;

        private readonly int x;

        private readonly int y;

        private readonly int delta;

        //
        // Сводка:
        //     Возвращает значение, указывающее, какая кнопка мыши была нажата.
        //
        // Возврат:
        //     Одно из значений System.Windows.Forms.MouseButtons.
        public MouseButtons Button => button;

        //
        // Сводка:
        //     Возвращает значение, указывающее, сколько раз была нажата и отпущена кнопка мыши.
        //
        // Возврат:
        //     Значение типа System.Int32, представляющее число нажатий и отпусканий кнопки
        //     мыши.
        public int Clicks => clicks;

        //
        // Сводка:
        //     Возвращает координату X указателя мыши в момент создания события мыши.
        //
        // Возврат:
        //     Координата X указателя мыши в пикселях.
        public int X => x;

        //
        // Сводка:
        //     Возвращает координату Y указателя мыши в момент создания события мыши.
        //
        // Возврат:
        //     Координата Y указателя мыши в пикселях.
        public int Y => y;

        //
        // Сводка:
        //     Получает значение со знаком, указывающее количество делений, на которое повернулось
        //     колесико мыши, умноженное на константу WHEEL_DELTA. Делением называется один
        //     зубец колесика мыши.
        //
        // Возврат:
        //     Значение со знаком, указывающее количество делений, на которое повернулось колесико
        //     мыши, умноженное на константу WHEEL_DELTA.
        public int Delta => delta;

        //
        // Сводка:
        //     Возвращает расположение указателя мыши в момент создания события мыши.
        //
        // Возврат:
        //     Объект System.Drawing.Point, содержащий координаты x и y указателя мыши (в пикселях)
        //     относительно левого верхнего угла формы.
        public Point Location => new Point(x, y);

        //
        // Сводка:
        //     Инициализирует новый экземпляр класса System.Windows.Forms.MouseEventArgs.
        //
        // Параметры:
        //   button:
        //     Одно из значений System.Windows.Forms.MouseButtons, с помощью которого можно
        //     определить, какая кнопка мыши была нажата.
        //
        //   clicks:
        //     Количество нажатий кнопки мыши.
        //
        //   x:
        //     Координата x (в пикселях) места, где была нажата кнопка мыши.
        //
        //   y:
        //     Координата y (в пикселях) места, где была нажата кнопка мыши.
        //
        //   delta:
        //     Число со знаком, указывающее количество делений, на которое повернулось колесико
        //     мыши.
        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
        {
            this.button = button;
            this.clicks = clicks;
            this.x = x;
            this.y = y;
            this.delta = delta;
        }
    }
}
