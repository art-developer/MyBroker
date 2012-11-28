using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Win32;
using System.Windows.Forms;
using System.Threading;

namespace MyBroker.WinApi
{
    public class StartFx
    {
        private const string MAIN_CAPTION = "StartFx";
        private const string ORDER_CAPTION = "Заключить сделку";
        private const string OPEN_CONFIRM_CAPTION = "Подтверждение сделки";
        private const string CLOSE_CONFIRM_CAPTION = "Подтверждение закрытия сделки";
        private const string CLOSE_ORDER_CAPTION = "Закрытие сделки";
        //private const string MAIN_CAPTION = "StartFX ИГРОВОЙ РЕЖИМ! Торговый счет не подключен";
        //private const string ORDER_CAPTION = "ИГРОВОЙ РЕЖИМ! Заключить сделку";
        //private const string CLOSE_ORDER_CAPTION = "ИГРОВОЙ РЕЖИМ! Закрытие сделки";
        //private const string OPEN_CONFIRM_CAPTION = "ИГРОВОЙ РЕЖИМ! Подтверждение сделки";
        //private const string CLOSE_CONFIRM_CAPTION = "ИГРОВОЙ РЕЖИМ! Подтверждение закрытия сделки";

        public static bool OpenOrder(string rateName, int direction, decimal summ)
        {
            
            IntPtr mainWnd = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, MAIN_CAPTION);
            if (mainWnd == IntPtr.Zero)
            {
                return false;
            }

            User32.SetForegroundWindow(mainWnd);
            User32.SetActiveWindow(mainWnd);
            
            IntPtr first = User32.GetWindow(mainWnd, GetWindow_Cmd.GW_CHILD);

            if (first == IntPtr.Zero)
            {
                return false;
            }

            IntPtr toolbar = User32.GetWindow(first, GetWindow_Cmd.GW_HWNDNEXT);
            if (toolbar == IntPtr.Zero)
            {
                return false;
            }

            User32.SendMouseLeftClick(toolbar, 115, 30);

            IntPtr orderWnd = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, ORDER_CAPTION);
            if (orderWnd == IntPtr.Zero)
            {
                return false;
            }

            User32.SetForegroundWindow(orderWnd);
            
            SendKeys.SendWait("+{TAB}");
            Thread.Sleep(500);
            SendKeys.SendWait(summ.ToString());
            SendKeys.SendWait("+{TAB}");
            SendKeys.SendWait("+{TAB}");
            Thread.Sleep(500);
            if (direction>0)
            {
                SendKeys.SendWait("{UP}");
                SendKeys.SendWait("{UP}");
            }
            else
            {
                SendKeys.SendWait("{DOWN}");
                SendKeys.SendWait("{DOWN}");
            }
            
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(2000);
            for(int i=0;i<20;i++)
            {
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(2000);
                if (IsOpenConfirmed())
                {
                    SendKeys.SendWait("{ESC}");
                    return true;
                }
            }
            SendKeys.SendWait("{ESC}");
            return false;
        }

        public static bool CloseOrder(string rateName)
        {
            IntPtr closeWindow = IntPtr.Zero;
            for (int j = 0; j < 10;j++)
            {
                IntPtr mainWnd = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, MAIN_CAPTION);
                User32.SetForegroundWindow(mainWnd);
                User32.SetActiveWindow(mainWnd);
                User32.EnumChildWindows(mainWnd,FocusOrderList,IntPtr.Zero);
                //return true;
                SendKeys.SendWait("{DELETE}");
                closeWindow = User32.FindWindow("QWidget", CLOSE_ORDER_CAPTION);
                if (closeWindow == IntPtr.Zero)
                {
                    Thread.Sleep(10000);
                    continue;
                }

                Thread.Sleep(1000);
                User32.SetForegroundWindow(closeWindow);
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(2000);
                for (int i = 0; i < 20; i++)
                {
                    SendKeys.SendWait("{ENTER}");
                    Thread.Sleep(2000);
                    if (IsCloseConfirmed())
                    {
                        SendKeys.SendWait("{ESC}");
                        return true;
                    }
                }
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(30000);
            }
            return false;
        }

        public static bool CheckTerminal()
        {
            IntPtr mainWnd = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, MAIN_CAPTION);
            User32.SetForegroundWindow(mainWnd);
            return mainWnd != IntPtr.Zero;
        }


        /*---------------ищем окно подтверждения открытия сделки------------*/
        private static bool IsOpenConfirmed()
        {
            IntPtr confirmWindow = User32.FindWindow("QWidget", OPEN_CONFIRM_CAPTION);
            return confirmWindow != IntPtr.Zero;
        }

        /*---------------ищем окно подтверждения закрытия сделки------------*/
        private static bool IsCloseConfirmed()
        {
            IntPtr confirmWindow = User32.FindWindow("QWidget", CLOSE_CONFIRM_CAPTION);
            bool result=confirmWindow != IntPtr.Zero;
            return result;
        }

        private static int summCounter = 0;
        public static int FindSummTextBox(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder windowText = new StringBuilder(256);
            User32.GetWindowText(hWnd, windowText, 256);
            if (windowText.ToString().Contains("qt_spinbox_lineedit"))
            {
                if (summCounter == 1)
                {
                    
                    User32.SendMouseLeftClick(hWnd, 0, 0);
                    summCounter = 0;
                    return 0;
                }
                summCounter +=1;
                return 1;
            }

            return 1;
        }

        public static int FocusOrderList(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder windowText = new StringBuilder(256);
            User32.GetWindowText(hWnd, windowText, 256);
            if (windowText.ToString().Contains("qt_scrollarea_viewport"))
            {
                //User32.SetForegroundWindow(hWnd);
                //User32.PostMessage(hWnd, (uint)WindowsMessages.WM_MOUSEMOVE, IntPtr.Zero, IntPtr.Zero);
                User32.SendMouseLeftClick(hWnd, 10, 10);
                //User32.SetFocus(hWnd);
                

                return 1;
            }

            return 1;
        }
    }

     
}
