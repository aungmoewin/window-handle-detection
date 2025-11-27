using System.Runtime.InteropServices;
using System.Text;

namespace window_handle_detection
{
    public partial class Form1 : Form
    {
        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private IntPtr m_hook = IntPtr.Zero;
        private WinEventDelegate m_dele = null;
        private bool m_isWaitingForMessageBox = false;
        private IntPtr m_messageBoxHandle = IntPtr.Zero;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventHook, WinEventDelegate lpfnWinEventHook, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint EVENT_OBJECT_CREATE = 1;
        private const uint EVENT_SYSTEM_DIALOG = 16;
        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint WM_CLOSE = 0x0010;

        public Form1()
        {
            InitializeComponent();
            SetupHook();
        }

        private void SetupHook()
        {
            m_dele = new WinEventDelegate(WinEventHook);
            m_hook = SetWinEventHook(EVENT_SYSTEM_DIALOG, EVENT_SYSTEM_DIALOG, IntPtr.Zero, m_dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        private void CleanupHook()
        {
            if (m_hook != IntPtr.Zero)
            {
                UnhookWinEvent(m_hook);
                m_hook = IntPtr.Zero;
            }
        }

        public void WinEventHook(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (!m_isWaitingForMessageBox)
                return;

            if (!IsWindow(hwnd))
                return;

            if (!IsWindowVisible(hwnd))
                return;

            StringBuilder sbClassName = new StringBuilder(256);
            GetClassName(hwnd, sbClassName, sbClassName.Capacity);

            string className = sbClassName.ToString();

            // Detect message box windows specifically (class name is #32770)
            if (className == "#32770")
            {
                StringBuilder sbWindowText = new StringBuilder(256);
                GetWindowText(hwnd, sbWindowText, sbWindowText.Capacity);
                string windowText = sbWindowText.ToString();

                m_messageBoxHandle = hwnd;
                DetectedDialog(hwnd, windowText, className);
            }
        }

        private void DetectedDialog(IntPtr hwnd, string windowText, string className)
        {
            string message = $"Window Handle: 0x{hwnd:X8}\n" +
                           $"Window Text: {windowText}\n" +
                           $"Class Name: {className}";

            // Update the label with the window handle information
            if (InvokeRequired)
            {
                Invoke(new Action(() => 
                {
                    lblWindowHandle.Text = message;
                }));
            }
            else
            {
                lblWindowHandle.Text = message;
            }
        }

        private void BtnDetectDialogs_Click(object? sender, EventArgs e)
        {
            // Set flag to indicate we're waiting for a message box
            m_isWaitingForMessageBox = true;
            
            // Show a test dialog to trigger the detection
            MessageBox.Show("Dialog detected!", "Dialog Detection", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCloseDialogs_Click(object? sender, EventArgs e)
        {
            // Close the message box if it's still open
            if (m_messageBoxHandle != IntPtr.Zero && IsWindow(m_messageBoxHandle))
            {
                PostMessage(m_messageBoxHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                m_messageBoxHandle = IntPtr.Zero;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Close the message box if it's still open
            if (m_messageBoxHandle != IntPtr.Zero && IsWindow(m_messageBoxHandle))
            {
                PostMessage(m_messageBoxHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }

            base.OnFormClosing(e);
        }
    }
}
