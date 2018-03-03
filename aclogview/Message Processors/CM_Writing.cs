using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Writing : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.BOOK_DATA_RESPONSE_EVENT:
                {
                    BookDataResponse message = BookDataResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.BOOK_MODIFY_PAGE_RESPONSE_EVENT:
                {
                    BookModifyPageResponse message = BookModifyPageResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.BOOK_ADD_PAGE_RESPONSE_EVENT:
                {
                    BookAddPageResponse message = BookAddPageResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.BOOK_DELETE_PAGE_RESPONSE_EVENT:
                {
                    BookDeletePageResponse message = BookDeletePageResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.BOOK_PAGE_DATA_RESPONSE_EVENT:
                {
                    BookPageDataResponse message = BookPageDataResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Writing__BookData_ID:
                {
                    BookData message = BookData.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Writing__BookModifyPage_ID: {
                    BookModifyPage message = BookModifyPage.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Writing__BookAddPage_ID: {
                    BookAddPage message = BookAddPage.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Writing__BookDeletePage_ID: {
                    BookDeletePage message = BookDeletePage.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Writing__BookPageData_ID: {
                    BookPageData message = BookPageData.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: Evt_Writing__GetInscription_ID
            case PacketOpcode.Evt_Writing__SetInscription_ID: {
                    SetInscription message = SetInscription.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

    public class BookDataResponse : Message
    {
        public uint i_bookID;
        public int i_maxNumPages;
        public uint numPages;
        public uint maxNumCharsPerPage;
        public PList<PageData> pageData = new PList<PageData>();
        public PStringChar inscription;
        public uint authorId;
        public PStringChar authorName;

        public static BookDataResponse read(BinaryReader binaryReader)
        {
            BookDataResponse newObj = new BookDataResponse();
            newObj.i_bookID = binaryReader.ReadUInt32();
            newObj.i_maxNumPages = binaryReader.ReadInt32();
            newObj.numPages = binaryReader.ReadUInt32();
            newObj.maxNumCharsPerPage = binaryReader.ReadUInt32();
            newObj.pageData = PList<PageData>.read(binaryReader);
            newObj.inscription = PStringChar.read(binaryReader);
            newObj.authorId = binaryReader.ReadUInt32();
            newObj.authorName = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_bookID = " + Utility.FormatHex(i_bookID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_maxNumPages = " + i_maxNumPages);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("numPages = " + numPages);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("maxNumCharsPerPage = " + maxNumCharsPerPage);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode pageDataNode = rootNode.Nodes.Add("pageData = ");
            ContextInfo.AddToList(new ContextInfo { Length = pageData.Length }, updateDataIndex: false);
            // Skip PList count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < pageData.list.Count; i++)
            {
                pageData.list[i].contributeToTreeNode(pageDataNode);
            }
            rootNode.Nodes.Add("inscription = " + inscription);
            ContextInfo.AddToList(new ContextInfo { Length = inscription.Length, DataType = DataType.Serialized_AsciiString });
            rootNode.Nodes.Add("authorId = " + Utility.FormatHex(authorId));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("authorName = " + authorName);
            ContextInfo.AddToList(new ContextInfo { Length = authorName.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }


    public class PageData
    {
        public uint authorID;
        public PStringChar authorName;
        public PStringChar authorAccount;
        public uint flags;
        public uint textIncluded;
        public uint ignoreAuthor;
        public PStringChar pageText;
        public int Length;

        public static PageData read(BinaryReader binaryReader)
        {
            PageData newObj = new PageData();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.authorID = binaryReader.ReadUInt32();
            newObj.authorName = PStringChar.read(binaryReader);
            newObj.authorAccount = PStringChar.read(binaryReader);
            newObj.flags = binaryReader.ReadUInt32();
            if ((newObj.flags >> 16) == 0xFFFF)
            {
                if ((ushort)newObj.flags == 2)
                {
                    newObj.textIncluded = binaryReader.ReadUInt32();
                    newObj.ignoreAuthor = binaryReader.ReadUInt32();
                }
                else
                {
                    newObj.textIncluded = newObj.flags;
                    newObj.ignoreAuthor = 0;
                }
            }
            if (newObj.textIncluded != 0)
                newObj.pageText = PStringChar.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode rootNode = new TreeNode("PageData");
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { Length = this.Length }, updateDataIndex: false);
            rootNode.Nodes.Add("authorID = " + Utility.FormatHex(authorID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("authorName = " + authorName);
            ContextInfo.AddToList(new ContextInfo { Length = authorName.Length, DataType = DataType.Serialized_AsciiString });
            rootNode.Nodes.Add("authorAccount = " + authorAccount);
            ContextInfo.AddToList(new ContextInfo { Length = authorAccount.Length, DataType = DataType.Serialized_AsciiString });
            rootNode.Nodes.Add("flags = " + Utility.FormatHex(flags));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("textIncluded = " + textIncluded);
            rootNode.Nodes.Add("ignoreAuthor = " + ignoreAuthor);
            if((flags >> 16) == 0xFFFF && (ushort)flags == 2) {
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            else
            {
                // Skip textIncluded and ignoreAuthor nodes
                ContextInfo.AddToList(new ContextInfo { });
                ContextInfo.AddToList(new ContextInfo { });
            }
            if (textIncluded != 0)
            {
                rootNode.Nodes.Add("pageText = " + pageText);
                ContextInfo.AddToList(new ContextInfo { Length = pageText.Length, DataType = DataType.Serialized_AsciiString });
            }
            node.Nodes.Add(rootNode);
        }
    }

    public class PageResponse
    {
        public uint i_bookID;
        public int i_pageNum;
        public int i_success;

        public static PageResponse read(BinaryReader binaryReader)
        {
            PageResponse newObj = new PageResponse();
            newObj.i_bookID = binaryReader.ReadUInt32();
            newObj.i_pageNum = binaryReader.ReadInt32();
            newObj.i_success = binaryReader.ReadInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("i_bookID = " + Utility.FormatHex(i_bookID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            node.Nodes.Add("i_pageNum = " + i_pageNum);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("i_success = " + i_success);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }

    public class BookModifyPageResponse : Message
    {
        public PageResponse pageResponse;

        public static BookModifyPageResponse read(BinaryReader binaryReader)
        {
            BookModifyPageResponse newObj = new BookModifyPageResponse();
            newObj.pageResponse = PageResponse.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            pageResponse.contributeToTreeNode(rootNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookAddPageResponse : Message
    {
        public PageResponse pageResponse;

        public static BookAddPageResponse read(BinaryReader binaryReader)
        {
            BookAddPageResponse newObj = new BookAddPageResponse();
            newObj.pageResponse = PageResponse.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            pageResponse.contributeToTreeNode(rootNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookDeletePageResponse : Message
    {
        public PageResponse pageResponse;

        public static BookDeletePageResponse read(BinaryReader binaryReader)
        {
            BookDeletePageResponse newObj = new BookDeletePageResponse();
            newObj.pageResponse = PageResponse.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            pageResponse.contributeToTreeNode(rootNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookPageDataResponse : Message
    {
        public uint bookID;
        public uint page;
        public PageData pageData;

        public static BookPageDataResponse read(BinaryReader binaryReader)
        {
            BookPageDataResponse newObj = new BookPageDataResponse();
            newObj.bookID = binaryReader.ReadUInt32();
            newObj.page = binaryReader.ReadUInt32();
            newObj.pageData = PageData.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("bookID = " + Utility.FormatHex(bookID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("page = " + page);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            pageData.contributeToTreeNode(rootNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    // This message is sent by the client if the add page response fails (i_success = 0) or
    // the page number in the response is not the page number expected by the client;
    public class BookData : Message {
        public uint i_objectid;

        public static BookData read(BinaryReader binaryReader) {
            BookData newObj = new BookData();
            newObj.i_objectid = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_objectid = " + Utility.FormatHex(i_objectid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookModifyPage : Message {
        public uint i_objectid;
        public int i_pageNum;
        public PStringChar i_pageText;

        public static BookModifyPage read(BinaryReader binaryReader) {
            BookModifyPage newObj = new BookModifyPage();
            newObj.i_objectid = binaryReader.ReadUInt32();
            newObj.i_pageNum = binaryReader.ReadInt32();
            newObj.i_pageText = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_objectid = " + Utility.FormatHex(i_objectid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_pageNum = " + i_pageNum);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_pageText = " + i_pageText);
            ContextInfo.AddToList(new ContextInfo { Length = i_pageText.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookAddPage : Message {
        public uint i_objectid;

        public static BookAddPage read(BinaryReader binaryReader) {
            BookAddPage newObj = new BookAddPage();
            newObj.i_objectid = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_objectid = " + Utility.FormatHex(i_objectid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookDeletePage : Message {
        public uint i_objectid;
        public int i_pageNum;

        public static BookDeletePage read(BinaryReader binaryReader) {
            BookDeletePage newObj = new BookDeletePage();
            newObj.i_objectid = binaryReader.ReadUInt32();
            newObj.i_pageNum = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_objectid = " + Utility.FormatHex(i_objectid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_pageNum = " + i_pageNum);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BookPageData : Message {
        public uint i_objectid;
        public int i_pageNum;

        public static BookPageData read(BinaryReader binaryReader) {
            BookPageData newObj = new BookPageData();
            newObj.i_objectid = binaryReader.ReadUInt32();
            newObj.i_pageNum = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_objectid = " + Utility.FormatHex(i_objectid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_pageNum = " + i_pageNum);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetInscription : Message {
        public uint i_objectid;
        public PStringChar i_inscription;

        public static SetInscription read(BinaryReader binaryReader) {
            SetInscription newObj = new SetInscription();
            newObj.i_objectid = binaryReader.ReadUInt32();
            newObj.i_inscription = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_objectid = " + Utility.FormatHex(i_objectid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_inscription = " + i_inscription);
            ContextInfo.AddToList(new ContextInfo { Length = i_inscription.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }
}
