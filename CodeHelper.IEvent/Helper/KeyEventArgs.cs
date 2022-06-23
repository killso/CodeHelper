using System;

namespace Helper
{
    public class KeyEventArgs : EventArgs
    {
        private readonly Keys keyData;

        private bool handled;

        private bool suppressKeyPress;

        //
        // Сводка:
        //     Возвращает значение, указывающее, была ли нажата клавиша ALT.
        //
        // Возврат:
        //     Значение true, если клавиша ALT была нажата, в противном случае — значение false.
        public virtual bool Alt => (keyData & Keys.Alt) == Keys.Alt;

        //
        // Сводка:
        //     Возвращает значение, указывающее, была ли нажата клавиша CTRL.
        //
        // Возврат:
        //     Значение true, если клавиша CTRL была нажата, в противном случае — значение false.
        public bool Control => (keyData & Keys.Control) == Keys.Control;

        //
        // Сводка:
        //     Возвращает или задает значение, указывающее, было ли обработано событие.
        //
        // Возврат:
        //     Значение true для обхода обработки элемента управления по умолчанию; значение
        //     false для передачи события обработчику элементов управления по умолчанию.
        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

        //
        // Сводка:
        //     Получает код клавиатуры для события System.Windows.Forms.Control.KeyDown или
        //     события System.Windows.Forms.Control.KeyUp.
        //
        // Возврат:
        //     Значение System.Windows.Forms.Keys, являющееся кодом клавиши для события.
        public Keys KeyCode
        {
            get
            {
                Keys keys = keyData & Keys.KeyCode;
                if (!Enum.IsDefined(typeof(Keys), (int)keys))
                {
                    return Keys.None;
                }

                return keys;
            }
        }

        //
        // Сводка:
        //     Получает значение клавиатуры для события System.Windows.Forms.Control.KeyDown
        //     или System.Windows.Forms.Control.KeyUp.
        //
        // Возврат:
        //     Представление целого числа для свойства System.Windows.Forms.KeyEventArgs.KeyCode.
        public int KeyValue => (int)(keyData & Keys.KeyCode);

        //
        // Сводка:
        //     Получает данные, касающиеся клавиши, для события System.Windows.Forms.Control.KeyDown
        //     или System.Windows.Forms.Control.KeyUp.
        //
        // Возврат:
        //     Значение System.Windows.Forms.Keys, представляющее код нажатой клавиши вместе
        //     с любыми флагами, показывающими, какие из клавиш CTRL, SHIFT и ALT были нажаты
        //     одновременно.
        public Keys KeyData => keyData;

        //
        // Сводка:
        //     Получает флаги модификаторов для события System.Windows.Forms.Control.KeyDown
        //     или события System.Windows.Forms.Control.KeyUp. Флаги указывают, какая комбинация
        //     клавиш CTRL, SHIFT и ALT была нажата.
        //
        // Возврат:
        //     Значение System.Windows.Forms.Keys, представляющее один или несколько флагов
        //     модификаторов.
        public Keys Modifiers => keyData & Keys.Modifiers;

        //
        // Сводка:
        //     Возвращает значение, указывающее, была ли нажата клавиша SHIFT.
        //
        // Возврат:
        //     Значение true, если клавиша SHIFT была нажата, и значение false — в противном
        //     случае.
        public virtual bool Shift => (keyData & Keys.Shift) == Keys.Shift;

        //
        // Сводка:
        //     Возвращает или задает значение, указывающее, следует ли передавать события нажатия
        //     клавиши базовому элементу управления.
        //
        // Возврат:
        //     Значение true, событие нажатия клавиши должно передаваться элементу управления;
        //     в противном случае — false.
        public bool SuppressKeyPress
        {
            get
            {
                return suppressKeyPress;
            }
            set
            {
                suppressKeyPress = value;
                handled = value;
            }
        }

        //
        // Сводка:
        //     Инициализирует новый экземпляр класса System.Windows.Forms.KeyEventArgs.
        //
        // Параметры:
        //   keyData:
        //     Значение System.Windows.Forms.Keys, представляющее нажатую клавишу вместе с любыми
        //     флагами, показывающими, какие из клавиш CTRL, SHIFT и ALT были нажаты одновременно.
        //     Возможные значения получаются путем применения побитовой операции ИЛИ (|) к константам,
        //     определенным в перечислении System.Windows.Forms.Keys.
        public KeyEventArgs(Keys keyData)
        {
            this.keyData = keyData;
        }
    }
}