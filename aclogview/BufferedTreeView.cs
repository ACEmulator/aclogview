using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class BufferedTreeView : TreeView
{
    protected override void OnHandleCreated(EventArgs e)
    {
        SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
        base.OnHandleCreated(e);
    }

    // Override BeginUpdate and EndUpdate to lock the treeview so that the scrollbar doesn't jump around during an update.
    public new void BeginUpdate()
    {
        SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        LockWindowUpdate(this.Handle);
    }
    public new void EndUpdate()
    {
        LockWindowUpdate(IntPtr.Zero);
        SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
    }

    // Pinvoke:
    private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
    private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
    private const int TVS_EX_DOUBLEBUFFER = 0x0004;
    private const int WM_SETREDRAW = 0xb;
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    [DllImport("user32.dll")]
    private static extern bool LockWindowUpdate(IntPtr hWndLock);
}
