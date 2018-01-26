using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aclogview
{
    /// <summary>
    /// This class is used to store context info about fields in messages.
    /// </summary>
    public class ContextInfo
    {
        public int startPosition;
        public int length;
        public DataType dataType;

        public ContextInfo()
        {
            startPosition = Form1.dataIndex;
        }
        
        /// <summary>
        /// This method adds context info to the main list and by default
        /// updates the node index and data index variables.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="updateDataIndex"></param>
        public static void AddToList(ContextInfo c, bool updateDataIndex = true) {
            Form1.contextList.Add(Form1.nodeIndex, c);
            if (updateDataIndex)
            {
                Form1.dataIndex += c.length;
            }
            Form1.nodeIndex++;
        }
    }

    public enum DataType
    {
        Undefined,
        Header12Bytes,  // Client to Server
        Header16Bytes,  // Server to Client
        SpellID_uint,        
        EnchantmentID,  // uint divided into two ushorts: the spell ID and the layer
        SpellID_ushort,
        SpellLayer,     
        ObjectID,
        Opcode,
        ShortSerialized_UnicodeString, // String length < 128 (+ 1 byte length header)
        LongSerialized_UnicodeString,   // String length >= 128 (+ 2 byte length header)
        UnicodeString,                  // UTF-16 string with no length header
        Serialized_AsciiString,         // AKA PStringChar; Has a 2-4 byte length header
        CellID
    }
}
