using System.Windows.Forms;

class CustomListView : ListView
{
    private ContextMenuStrip _contextMenu;

    /// <summary>
    /// Pass the ContextMenuStrip that you want to display on the ListView headers/columns.
    /// </summary>
    /// <param name="cms"></param>
    public void InitializeHeaderContextMenu(ContextMenuStrip cms)
    {
        _contextMenu = cms;
    }

    protected override void WndProc(ref System.Windows.Forms.Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == 0x7b) //WM_CONTEXTMENU
        {
            if (m.WParam != this.Handle) _contextMenu.Show(Cursor.Position);
        }
    }
}





