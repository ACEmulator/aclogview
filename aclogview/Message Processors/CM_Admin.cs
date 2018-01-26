using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

public class CM_Admin : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Admin__ChatServerData_ID: // 0xF7DE
                {
                    var message = ChatServerData.read(messageDataReader);
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

    public class Request
    {
        public AsyncMethodID m_blobDispatchType;
        public uint cbSize;
        public uint m_contextID;
        public uint dwRequestID;
        public uint m_methodID; 
        public string m_roomName; // By Name variable only
        public uint m_roomID; // By ID variable only
        public string m_text;
        public TurbineChatBlob extraInfoBlob;
        public int Length;

        public static Request read(BinaryReader binaryReader, AsyncMethodID m_blobDispatchType)
        {
            Request newObj = new Request();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.m_blobDispatchType = m_blobDispatchType;
            newObj.cbSize = binaryReader.ReadUInt32();
            newObj.m_contextID = binaryReader.ReadUInt32(); // Seems to be incremented every time the client makes a request.
            newObj.dwRequestID = binaryReader.ReadUInt32();
            newObj.m_methodID = binaryReader.ReadUInt32();
            if (m_blobDispatchType == AsyncMethodID.ASYNCMETHOD_SENDTOROOMBYNAME)
            {
                newObj.m_roomName = Util.readUnicodeString(binaryReader);
            }
            else if (m_blobDispatchType == AsyncMethodID.ASYNCMETHOD_SENDTOROOMBYID)
            {
                newObj.m_roomID = binaryReader.ReadUInt32();
            }
            newObj.m_text = Util.readUnicodeString(binaryReader);
            newObj.extraInfoBlob = TurbineChatBlob.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("cbSize = " + cbSize);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("m_contextID = " + m_contextID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("dwRequestID = " + dwRequestID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("m_methodID = " + m_methodID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            if (m_blobDispatchType == AsyncMethodID.ASYNCMETHOD_SENDTOROOMBYNAME)
            {
                node.Nodes.Add("m_roomName = " + m_roomName);
                if (m_roomName.Length >= 128)
                    ContextInfo.AddToList(new ContextInfo { length = 2 + (m_roomName.Length * 2), dataType = DataType.LongSerialized_UnicodeString });
                else
                    ContextInfo.AddToList(new ContextInfo { length = 1 + (m_roomName.Length * 2), dataType = DataType.ShortSerialized_UnicodeString });
            }
            else if (m_blobDispatchType == AsyncMethodID.ASYNCMETHOD_SENDTOROOMBYID)
            {
                node.Nodes.Add("m_roomID = " + m_roomID);
                ContextInfo.AddToList(new ContextInfo { length = 4 });
            }
            node.Nodes.Add("m_text = " + m_text);
            if (m_text.Length >= 128)
                ContextInfo.AddToList(new ContextInfo { length = 2 + (m_text.Length * 2), dataType = DataType.LongSerialized_UnicodeString });
            else
                ContextInfo.AddToList(new ContextInfo { length = 1 + (m_text.Length * 2), dataType = DataType.ShortSerialized_UnicodeString });
            TreeNode extraInfoNode = node.Nodes.Add("extraInfoBlob = ");
            ContextInfo.AddToList(new ContextInfo { length = extraInfoBlob.Length }, updateDataIndex: false);
            extraInfoBlob.contributeToTreeNode(extraInfoNode);
        }
    }

    // Note: Technically the ByID and ByName methods have different response classes in the DLL but the structure returned remains the same.
    public class Response
    {
        public uint cbSize;
        public uint m_contextID;
        public uint dwResponseID;
        public uint m_methodID;
        public uint m_hResult;
        public int Length;

        public static Response read(BinaryReader binaryReader)
        {
            Response newObj = new Response();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.cbSize = binaryReader.ReadUInt32();
            newObj.m_contextID = binaryReader.ReadUInt32();
            newObj.dwResponseID = binaryReader.ReadUInt32();
            newObj.m_methodID = binaryReader.ReadUInt32();
            newObj.m_hResult = binaryReader.ReadUInt32();
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("cbSize = " + cbSize);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("m_contextID = " + m_contextID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("dwResponseID = " + dwResponseID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("m_methodID = " + m_methodID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("m_hResult = " + Utility.FormatHex(m_hResult));
            ContextInfo.AddToList(new ContextInfo { length = 4 });
        }
    }

    public class TurbineChatBlob
    {
        public uint cbSize;
        public uint speakerID;
        public uint hResult;
        public uint chatType;
        public int Length;

        public static TurbineChatBlob read(BinaryReader binaryReader)
        {
            TurbineChatBlob newObj = new TurbineChatBlob();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.cbSize = binaryReader.ReadUInt32();
            newObj.speakerID = binaryReader.ReadUInt32();
            newObj.hResult = binaryReader.ReadUInt32();
            newObj.chatType = binaryReader.ReadUInt32();
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("cbSize = " + cbSize);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("speakerID = " + Utility.FormatHex(speakerID));
            ContextInfo.AddToList(new ContextInfo { length = 4, dataType = DataType.ObjectID });
            node.Nodes.Add("hResult = " + Utility.FormatHex(hResult));
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("chatType = " + (ChatTypeEnum)chatType);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
        }
    }

    // See gmCCommunicationSystem::uiChatInterfaceProvider::OnSendToRoom
    public class SendToRoomChatEvent
    {
        public uint cbSize;
        public uint dwRoomID;
        public string pwszDisplayName;
        public string pwszText;
        public TurbineChatBlob extraInfoBlob;
        public int Length;

        public static SendToRoomChatEvent read(BinaryReader binaryReader)
        {
            SendToRoomChatEvent newObj = new SendToRoomChatEvent();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.cbSize = binaryReader.ReadUInt32();
            newObj.dwRoomID = binaryReader.ReadUInt32();
            newObj.pwszDisplayName = Util.readUnicodeString(binaryReader);
            newObj.pwszText = Util.readUnicodeString(binaryReader);
            newObj.extraInfoBlob = TurbineChatBlob.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("cbSize = " + cbSize);
            ContextInfo.AddToList(new ContextInfo { length =  4 });
            node.Nodes.Add("dwRoomID = " + dwRoomID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            node.Nodes.Add("pwszDisplayName = " + pwszDisplayName);
            if (pwszDisplayName.Length >= 128)
                ContextInfo.AddToList(new ContextInfo { length = 2 + (pwszDisplayName.Length * 2), dataType = DataType.LongSerialized_UnicodeString });
            else
                ContextInfo.AddToList(new ContextInfo { length = 1 + (pwszDisplayName.Length * 2), dataType = DataType.ShortSerialized_UnicodeString });
            node.Nodes.Add("pwszText = " + pwszText);
            if (pwszText.Length >= 128)
                ContextInfo.AddToList(new ContextInfo { length = 2 + (pwszText.Length * 2), dataType = DataType.LongSerialized_UnicodeString });
            else
                ContextInfo.AddToList(new ContextInfo { length = 1 + (pwszText.Length * 2), dataType = DataType.ShortSerialized_UnicodeString });
            TreeNode extraInfoNode = node.Nodes.Add("extraInfoBlob = ");
            ContextInfo.AddToList(new ContextInfo { length = extraInfoBlob.Length }, updateDataIndex: false);
            extraInfoBlob.contributeToTreeNode(extraInfoNode);
        }
    }

    public class ChatServerData : Message
    {
        public uint cbSize;
        public ChatNetworkBlobType m_blobType;
        public AsyncMethodID m_blobDispatchType;
        public uint m_targetType;
        public uint m_targetID;
        public uint m_transportType;
        public uint m_transportID;
        public uint m_cookie;
        public SendToRoomChatEvent sendToRoomChatEvent;
        public Request request;
        public Response response;

        public static ChatServerData read(BinaryReader binaryReader)
        {
            var newObj = new ChatServerData();
            newObj.cbSize = binaryReader.ReadUInt32();
            newObj.m_blobType = (ChatNetworkBlobType)binaryReader.ReadUInt32();
            newObj.m_blobDispatchType = (AsyncMethodID)binaryReader.ReadUInt32();
            newObj.m_targetType = binaryReader.ReadUInt32();        // Always 1 in pcaps
            newObj.m_targetID = binaryReader.ReadUInt32();
            newObj.m_transportType = binaryReader.ReadUInt32();     // Pcaps contain either a 0 or a 1 (Perhaps ChatEventType?)
            newObj.m_transportID = binaryReader.ReadUInt32();
            newObj.m_cookie = binaryReader.ReadUInt32();            // Always 0 in pcaps

            if (newObj.m_blobType == ChatNetworkBlobType.NETBLOB_EVENT_BINARY) // Server to client
            {
                newObj.sendToRoomChatEvent = SendToRoomChatEvent.read(binaryReader);
            }
            else if (newObj.m_blobType == ChatNetworkBlobType.NETBLOB_REQUEST_BINARY) // Client to server
            {
                newObj.request = Request.read(binaryReader, newObj.m_blobDispatchType);
            }
            else if (newObj.m_blobType == ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY) // Server to client acknowledgement
            {
                newObj.response = Response.read(binaryReader);
            }

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { length = 4, dataType = DataType.Opcode });
            rootNode.Nodes.Add("cbSize = " + cbSize);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_blobType = " + m_blobType);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_blobDispatchType = " + m_blobDispatchType);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_targetType = " + m_targetType);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_targetID = " + m_targetID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_transportType = " + m_transportType);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_transportID = " + m_transportID);
            ContextInfo.AddToList(new ContextInfo { length = 4 });
            rootNode.Nodes.Add("m_cookie = " + m_cookie);
            ContextInfo.AddToList(new ContextInfo { length = 4 });

            if (m_blobType == ChatNetworkBlobType.NETBLOB_EVENT_BINARY)
            {
                TreeNode chatEventNode = rootNode.Nodes.Add("sendToRoomChatEvent = ");
                ContextInfo.AddToList(new ContextInfo { length = sendToRoomChatEvent.Length }, updateDataIndex: false); 
                sendToRoomChatEvent.contributeToTreeNode(chatEventNode);
            }
            else if (m_blobType == ChatNetworkBlobType.NETBLOB_REQUEST_BINARY)
            {
                TreeNode requestNode = rootNode.Nodes.Add("request = ");
                ContextInfo.AddToList(new ContextInfo { length = request.Length }, updateDataIndex: false);
                request.contributeToTreeNode(requestNode);
            }
            else if (m_blobType == ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY)
            {
                TreeNode requestNode = rootNode.Nodes.Add("response = ");
                ContextInfo.AddToList(new ContextInfo { length = response.Length }, updateDataIndex: false);
                response.contributeToTreeNode(requestNode);
            }
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }
}
